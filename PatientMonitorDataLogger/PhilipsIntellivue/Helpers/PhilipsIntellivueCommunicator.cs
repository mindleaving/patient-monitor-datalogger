using System.Collections.Concurrent;
using System.Data;
using PatientMonitorDataLogger.PhilipsIntellivue.Models;
using PatientMonitorDataLogger.Shared.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public class PhilipsIntellivueCommunicator : IDisposable
{
    private readonly IODevice ioDevice;
    private readonly string logName;
    private readonly PhilipsIntellivueFrameReader frameReader;
    private readonly AwaitableTimeCappedCollection<ICommandMessage> messageCollection;
    private readonly object startStopLock = new();
    private BlockingCollection<ICommandMessage>? outgoingMessages = new();
    private Task? sendTask;

    public PhilipsIntellivueCommunicator(
        IODevice ioDevice,
        TimeSpan messageRetentionPeriod,
        string logName)
    {
        this.ioDevice = ioDevice;
        this.logName = logName;
        frameReader = new PhilipsIntellivueFrameReader(ioDevice);
        messageCollection = new(messageRetentionPeriod);
        frameReader.FrameAvailable += QueueMessage;
        frameReader.SerialPortFaulted += OnSerialPortFaulted;
    }

    public bool IsListening => frameReader.IsListening;
    public bool IsSending { get; private set; }
    public event EventHandler<ICommandMessage>? NewMessage;
    public event EventHandler<ConnectionState>? ConnectionStatusChanged;

    private void QueueMessage(
        object? sender,
        PhilipsIntellivueFrame frame)
    {
        if (IsAssociationAbort(frame.UserData))
        {
            ConnectionStatusChanged?.Invoke(this, ConnectionState.Closed);
            Stop();
        }
#if DEBUG
        Log($"Received {frame.UserData}");
#endif
        messageCollection.Add(frame.UserData);
        NewMessage?.Invoke(this, frame.UserData);
    }

    private void OnSerialPortFaulted(
        object? sender,
        EventArgs e)
    {
        ConnectionStatusChanged?.Invoke(this, ConnectionState.Broken);
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
                sendTask = Task.Factory.StartNew(
                    Send,
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
                IsSending = true;
            }
            ConnectionStatusChanged?.Invoke(this, ConnectionState.Open);
        }
    }

    private void Send()
    {
        foreach (var message in outgoingMessages!.GetConsumingEnumerable())
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
                ioDevice.Write(frameBytes);
#if DEBUG
                Log($"Sent {message}");
#endif
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
            messageCollection.Clear();
            ConnectionStatusChanged?.Invoke(this, ConnectionState.Closed);
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

    private static PhilipsIntellivueFrame BuildFrame(
        ICommandMessage message)
    {
        var userDataBytes = message.Serialize();
        var frameHeader = new PhilipsIntellivueFrameHeader(ProtocolId.DataExport, MessageType.AssociationControlOrDataExportCommand, (ushort)userDataBytes.Length);
        var frameHeaderBytes = frameHeader.Serialize();
        var checksum = CrcCcittFcsAlgorithm.CalculateFcs([ ..frameHeaderBytes, ..userDataBytes ]);
        return new PhilipsIntellivueFrame(
            frameHeader,
            message,
            checksum);
    }

    public void Dispose()
    {
        Stop();
        frameReader.FrameAvailable -= QueueMessage;
        frameReader.SerialPortFaulted -= OnSerialPortFaulted;
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