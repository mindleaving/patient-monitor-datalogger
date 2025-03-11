using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class PhilipsIntellivueFrameReader : IDisposable
{
    private readonly IODevice ioDevice;
    private readonly object startStopLock = new();
    private CancellationTokenSource? cancellationTokenSource;
    private Task? listeningTask;

    public PhilipsIntellivueFrameReader(
        IODevice ioDevice)
    {
        this.ioDevice = ioDevice;
    }

    public event EventHandler<PhilipsIntellivueFrame>? FrameAvailable;
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
            listeningTask = Task.Factory.StartNew(
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
        var frameData = new System.Collections.Generic.List<byte>();
        var isFrameInProgress = false;
        var unescapeNextByte = false;
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
                    if (!isFrameInProgress && b != TransparencyByteUnescapedStream.FrameStartCharacter)
                        continue;

                    var unescapedByte = unescapeNextByte ? Unescape(b) : b;
                    unescapeNextByte = false;
                    if (b == TransparencyByteUnescapedStream.FrameStartCharacter)
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

                    if (b == TransparencyByteUnescapedStream.FrameEndCharacter)
                    {
                        if (unescapeNextByte)
                        {
                            Console.WriteLine("Frame aborted");
                        }
                        else
                        {
                            frameData.Add(b);
                            try
                            {
                                var frame = PhilipsIntellivueFrame.Parse(frameData.ToArray());
                                FrameAvailable?.Invoke(this, frame);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Could not parse frame: " + e.Message);
                                Console.WriteLine($"Faulty data: {Convert.ToBase64String(frameData.ToArray())}");
                            }
                        }

                        frameData.Clear();
                        isFrameInProgress = false;
                        continue;
                    }

                    if (b == TransparencyByteUnescapedStream.EscapeCharacter)
                    {
                        unescapeNextByte = true;
                        continue;
                    }

                    frameData.Add(unescapedByte);
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

    private byte Unescape(byte b) => (byte)(b ^ 0x20);

    public void Stop()
    {
        if(!IsListening)
            return;
        lock (startStopLock)
        {
            if(!IsListening)
                return;

            IsListening = false;
            try
            {
                cancellationTokenSource?.Cancel();
                listeningTask?.Wait(TimeSpan.FromSeconds(10));
            }
            catch
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