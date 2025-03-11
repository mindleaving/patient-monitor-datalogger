using PatientMonitorDataLogger.BBraun.Models;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public class BBraunBccFrameReader : IDisposable
{
    public const byte StartOfHeaderCharacter = 0x01;
    public const byte StartOfTextCharacter = 0x02;
    public const byte EndOfTextCharacter = 0x03;
    public const byte EndOfTextBlockCharacter = 0x17;
    public const byte EndOfTransmissionCharacter = 0x04;

    private readonly IODevice ioDevice;
    private readonly BBraunBccClientSettings settings;
    private readonly object startStopLock = new();
    private CancellationTokenSource? cancellationTokenSource;
    private Task? readTask;

    public BBraunBccFrameReader(
        IODevice ioDevice,
        BBraunBccClientSettings settings)
    {
        this.ioDevice = ioDevice;
        this.settings = settings;
    }

    public event EventHandler<BBraunBccFrame>? FrameAvailable;
    public event EventHandler? IoDeviceFaulted; 
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
                var bytesRead = ioDevice.Read(buffer);
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
                            var frame = BBraunBccFrame.Parse(frameData.ToArray(), settings.Role);
                            FrameAvailable?.Invoke(this, frame);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Could not parse frame:");
                            Console.WriteLine(e);
                            Console.WriteLine($"Faulty data: {Convert.ToBase64String(frameData.ToArray())}");
                        }

                        frameData.Clear();
                        isFrameInProgress = false;
                        continue;
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
            IoDeviceFaulted?.Invoke(this, EventArgs.Empty);
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