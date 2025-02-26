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
        frameReader.SerialPortFaulted += OnSerialPortFaulted;
    }

    public bool IsListening => frameReader.IsListening;
    public bool IsSending { get; private set; }
    public event EventHandler<ICommandMessage>? NewMessage;
    public event EventHandler<MonitorConnectionChangeEventType>? ConnectionStatusChanged;

    private void QueueMessage(
        object? sender,
        Rs232Frame frame)
    {
        if (IsAssociationAbort(frame.UserData))
        {
            ConnectionStatusChanged?.Invoke(this, MonitorConnectionChangeEventType.Aborted);
            Stop();
        }
        Log($"Received {frame.UserData}");
        messageCollection.Add(frame.UserData);
        NewMessage?.Invoke(this, frame.UserData);
    }

    private void OnSerialPortFaulted(
        object? sender,
        EventArgs e)
    {
        ConnectionStatusChanged?.Invoke(this, MonitorConnectionChangeEventType.Faulted);
        Stop();
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
            ConnectionStatusChanged?.Invoke(this, MonitorConnectionChangeEventType.Connected);
        }
    }

    private void Send(
        CancellationToken token)
    {
        foreach (var message in outgoingMessages!.GetConsumingEnumerable(token))
        {
            byte[] frameBytes;
            try
            {
                var frame = BuildFrame(message);
                frameBytes = frame.Serialize();
            }
            catch
            {
                Console.WriteLine($"Failed to create frame for message {message}. Message was discarded.");
                continue;
            }

            try
            {
                serialPort.Write(frameBytes);
                Log($"Sent {message}");
            }
            catch
            {
                OnSerialPortFaulted(this, EventArgs.Empty);
                IsSending = false;
                throw;
            }
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
                catch
                {
                    // Ignore
                }
                finally
                {
                    outgoingMessages = new();
                }
            }
            ConnectionStatusChanged?.Invoke(this, MonitorConnectionChangeEventType.Disconnected);
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
        if(outgoingMessages == null)
            return;
        if(outgoingMessages.IsAddingCompleted)
            return;
        outgoingMessages.Add(message);
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

    private bool IsAssociationAbort(
        ICommandMessage commandMessage)
    {
        if (commandMessage is not AssociationCommandMessage associationCommandMessage)
            return false;
        return associationCommandMessage.SessionHeader.Type == AssociationCommandType.Abort;
    }
}