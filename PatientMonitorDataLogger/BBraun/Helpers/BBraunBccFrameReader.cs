using System.Text;
using System.Text.RegularExpressions;
using PatientMonitorDataLogger.SharedModels;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public class BBraunBccFrameReader : IDisposable
{
    public const byte StartOfHeaderCharacter = 0x01;
    public const byte StartOfTextCharacter = 0x02;
    public const byte EndOfTextCharacter = 0x03;
    public const byte EndOfTextBlockCharacter = 0x17;
    public const byte EndOfTransmissionCharacter = 0x04;

    private readonly Stream stream;
    private readonly BBraunBccClientSettings settings;
    private readonly object startStopLock = new();
    private CancellationTokenSource? cancellationTokenSource;
    private Task? readTask;

    public BBraunBccFrameReader(
        Stream stream,
        BBraunBccClientSettings settings)
    {
        this.stream = stream;
        this.settings = settings;
    }

    public event EventHandler<BBraunBccFrame>? FrameAvailable;
    public event EventHandler? SerialPortFaulted; 
    public bool IsListening { get; private set; }

    public void Start()
    {
        if(IsListening)
            return;
        lock (startStopLock)
        {
            if(IsListening)
                return;

            cancellationTokenSource = new CancellationTokenSource();
            readTask = Task.Factory.StartNew(
                () => Read(cancellationTokenSource.Token),
                cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            IsListening = true;
        }
    }

    private async Task Read(
        CancellationToken cancellationToken)
    {
        var buffer = new byte[1024];
        var frameData = new List<byte>();
        var isFrameInProgress = false;
        var useCharacterStuffing = settings.UseCharacterStuffing;
        var destuffNextByte = false;
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var bytesRead = stream.Read(buffer);
                if (bytesRead == 0)
                {
                    await Task.Delay(100, cancellationToken);
                    continue;
                }

                foreach (var b in buffer.Take(bytesRead))
                {
                    if (!isFrameInProgress && b != StartOfHeaderCharacter)
                        continue;

                    var destuffedByte = destuffNextByte ? Destuff(b) : b;
                    var isDestuffedByte = destuffNextByte;
                    destuffNextByte = false;
                    if (b == StartOfHeaderCharacter)
                    {
                        if (isFrameInProgress)
                        {
                            frameData.Clear();
                            Console.WriteLine("Frame aborted");
                        }

                        isFrameInProgress = true;
                        frameData.Add(b);
                        continue;
                    }

                    if (b == EndOfTransmissionCharacter)
                    {
                        frameData.Add(b);
                        try
                        {
                            var frame = BBraunBccFrame.Parse(frameData.ToArray());
                            FrameAvailable?.Invoke(this, frame);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Could not parse frame: " + e.Message);
                            Console.WriteLine($"Faulty data: {Convert.ToBase64String(frameData.ToArray())}");
                        }
                    }

                    if (useCharacterStuffing && !isDestuffedByte && (b == 'e' || b == 'E'))
                    {
                        destuffNextByte = true;
                        continue;
                    }

                    frameData.Add(destuffedByte);
                }
            }
        }
        catch
        {
            SerialPortFaulted?.Invoke(this, EventArgs.Empty);
        }
        finally
        {
            IsListening = false;
        }
    }

    private byte Destuff(
        byte b)
    {
        return b switch
        {
            (byte)'x' => (byte)'d',
            (byte)'e' => (byte)'e',
            (byte)'X' => (byte)'D',
            (byte)'E' => (byte)'E',
            _ => throw new FormatException($"Expected stuffed character (x,e,X,E), but found {(char)b}")
        };
    }


    public void Stop()
    {
        if(!IsListening)
            return;
        lock (startStopLock)
        {
            if(!IsListening)
                return;
            IsListening = true;

            cancellationTokenSource?.Cancel();
            try
            {
                readTask?.Wait();
            }
            catch (Exception e)
            {
                // Ignore
            }
        }
    }

    public void Dispose()
    {
        Stop();
    }
}

public class BBraunBccFrame : ISerializable
{
    public BBraunBccFrame(
        int length,
        string bedId,
        IBBraunBccMessage userData,
        int checksum)
    {
        Length = length;
        BedId = bedId;
        UserData = userData;
        Checksum = checksum;
    }

    public int Length { get; }
    public string BedId { get; }
    public IBBraunBccMessage UserData { get; }
    public int Checksum { get; set; }

    public static BBraunBccFrame Parse(
        byte[] buffer)
    {
        if(buffer[0] != BBraunBccFrameReader.StartOfHeaderCharacter)
            throw new FormatException("Didn't find start of head character");
        if (buffer[6] != BBraunBccFrameReader.StartOfTextCharacter)
            throw new FormatException("Didn't find start of text character");
        var lengthString = Encoding.ASCII.GetString(buffer[1..6]);
        var length = int.Parse(lengthString);
        if (buffer.Length != length)
            throw new ArgumentException("Length of buffer and length in message header do not agree");
        if (buffer[length - 1] != BBraunBccFrameReader.EndOfTransmissionCharacter)
            throw new FormatException("Didn't find end of transmission character");
        if (buffer[length - 7] != BBraunBccFrameReader.EndOfTextCharacter && buffer[length - 7] != BBraunBccFrameReader.EndOfTextBlockCharacter)
            throw new FormatException("Didn't find end of text/text block character");
        var checksumString = Encoding.ASCII.GetString(buffer[^6..^1]);
        var checksum = int.Parse(checksumString);
        if (!BccChecksumCalculator.IsChecksumCorrect(buffer[..^6], checksum))
            throw new FormatException("Corrupt data. Checksum doesn't match");
        var bedId = ExtractBedId(Encoding.ASCII.GetString(buffer[7..26]));
        var userDataBytes = buffer[(7+bedId.Length+5)..^7];
        var userData = ParseUserData(userDataBytes);
        return new BBraunBccFrame(
            length,
            bedId,
            userData,
            checksum);
    }

    private static IBBraunBccMessage ParseUserData(
        byte[] bytes)
    {
        return new GenericIbBraunBccMessage(Encoding.UTF8.GetString(bytes));
    }

    private static string ExtractBedId(
        string str)
    {
        var bedIdMatch = Regex.Match(str, "^(?<bedID>[\x2F-\x39\x41-\x7A]{1,15})/1/1>");
        if (!bedIdMatch.Success)
            throw new FormatException("Couldn't find bed-ID");
        return bedIdMatch.Groups["bedID"].Value;
    }

    /// <summary>
    /// Serialize frame WITHOUT character stuffing
    /// </summary>
    public byte[] Serialize()
    {
        return [
            BBraunBccFrameReader.StartOfHeaderCharacter,
            ..Encoding.ASCII.GetBytes(Length.ToString("00000")),
            BBraunBccFrameReader.StartOfTextCharacter,
            ..Encoding.ASCII.GetBytes($"{BedId}/1/1>"),
            ..UserData.Serialize(),
            BBraunBccFrameReader.EndOfTextCharacter,
            ..Encoding.ASCII.GetBytes(Checksum.ToString("00000")),
            BBraunBccFrameReader.EndOfTransmissionCharacter,
        ];
    }
}

public interface IBBraunBccMessage : ISerializable
{
}

public class GenericIbBraunBccMessage : IBBraunBccMessage
{
    public GenericIbBraunBccMessage(
        string data)
    {
        Data = data;
    }

    public string Data { get; }

    public byte[] Serialize()
    {
        return Encoding.UTF8.GetBytes(Data);
    }
}