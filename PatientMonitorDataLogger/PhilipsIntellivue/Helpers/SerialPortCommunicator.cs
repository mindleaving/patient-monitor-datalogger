using System.Collections.Concurrent;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class SerialPortCommunicator : IDisposable
{
    private readonly ISerialPort serialPort;
    private readonly string logName;
    private readonly PhilipsIntellivueSerialDataFrameReader frameReader;
    private readonly AwaitableTimeCappedCollection<ICommandMessage> messageCollection;
    private readonly object startStopLock = new();
    private BlockingCollection<ICommandMessage>? outgoingMessages = new();
    private CancellationTokenSource? sendCancellationTokenSource;
    private Task? sendTask;

    public SerialPortCommunicator(
        ISerialPort serialPort,
        TimeSpan messageRetentionPeriod,
        string logName)
    {
        this.serialPort = serialPort;
        this.logName = logName;
        frameReader = new PhilipsIntellivueSerialDataFrameReader(serialPort);
        messageCollection = new(messageRetentionPeriod);
        frameReader.FrameAvailable += QueueMessage;
    }

    public bool IsListening => frameReader.IsListening;
    public bool IsSending { get; private set; }
    public event EventHandler<ICommandMessage>? NewMessage;

    private void QueueMessage(
        object? sender,
        Rs232Frame frame)
    {
        Log($"Received {frame.UserData}");
        messageCollection.Add(frame.UserData);
        NewMessage?.Invoke(this, frame.UserData);
    }

    public void Start()
    {
        if(IsListening && IsSending)
            return;
        lock (startStopLock)
        {
            if(IsListening && IsSending)
                return;
            if(!IsListening)
                frameReader.Start();

            if (!IsSending)
            {
                sendCancellationTokenSource = new CancellationTokenSource();
                sendTask = Task.Factory.StartNew(
                    () => Send(sendCancellationTokenSource.Token),
                    sendCancellationTokenSource.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
                IsSending = true;
            }
        }
    }

    private void Send(
        CancellationToken token)
    {
        foreach (var message in outgoingMessages!.GetConsumingEnumerable(token))
        {
            var frame = BuildFrame(message);
            var frameBytes = frame.Serialize();
            serialPort.Write(frameBytes);
            Log($"Sent {message}");
        }
    }

    public void Stop()
    {
        if(!IsListening && !IsSending)
            return;
        lock (startStopLock)
        {
            if(!IsListening && !IsSending)
                return;
            if(IsListening)
                frameReader.Stop();
            if (IsSending)
            {
                IsSending = false;
                outgoingMessages?.CompleteAdding();
                sendCancellationTokenSource?.Cancel();
                try
                {
                    sendTask?.Wait();
                }
                catch (AggregateException aggregateException)
                {
                    if (aggregateException.InnerException != null && aggregateException.InnerException is not (TaskCanceledException or OperationCanceledException))
                        throw aggregateException.InnerException;
                }
                finally
                {
                    outgoingMessages = new();
                }
            }
        }
    }

    public void WaitForMessage(
        WaitForRequest<ICommandMessage> waitRequest)
    {
        messageCollection.WaitFor(waitRequest);
    }

    public void Enqueue(
        ICommandMessage message)
    {
        outgoingMessages!.Add(message);
    }

    private static Rs232Frame BuildFrame(
        ICommandMessage message)
    {
        var userDataBytes = message.Serialize();
        var frameHeader = new Rs232FrameHeader(ProtocolId.DataExport, MessageType.AssociationControlOrDataExportCommand, (ushort)userDataBytes.Length);
        var frameHeaderBytes = frameHeader.Serialize();
        var checksum = CrcCcittFcsAlgorithm.CalculateFcs([ ..frameHeaderBytes, ..userDataBytes ]);
        return new Rs232Frame(
            frameHeader,
            message,
            checksum);
    }

    public void Dispose()
    {
        Stop();
        frameReader.Dispose();
        messageCollection.Dispose();
    }

    private void Log(string message) => Console.WriteLine($"{logName} - {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}: {message}");
}