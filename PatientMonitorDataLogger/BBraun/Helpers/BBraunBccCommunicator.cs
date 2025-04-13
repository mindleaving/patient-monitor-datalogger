using System.Collections.Concurrent;
using System.Data;
using PatientMonitorDataLogger.Shared.Helpers;
using PatientMonitorDataLogger.Shared.Models;

namespace PatientMonitorDataLogger.BBraun.Helpers;

public class BBraunBccCommunicator : IDisposable
{
    private readonly IODevice ioDevice;
    private readonly BBraunBccClientSettings settings;
    private readonly BBraunBccFrameReader frameReader;
    private readonly BBraunBccMessageCreator messageCreator;
    private readonly AwaitableTimeCappedCollection<BBraunBccFrame> messageCollection;
    private readonly object startStopLock = new();
    private BlockingCollection<byte[]> outgoingMessages = new();
    private Task? sendTask;
    private readonly string logName;

    public BBraunBccCommunicator(
        IODevice ioDevice,
        BBraunBccClientSettings settings,
        string logName)
    {
        this.ioDevice = ioDevice;
        this.settings = settings;
        this.logName = logName;
        frameReader = new BBraunBccFrameReader(ioDevice, settings);
        messageCreator = new BBraunBccMessageCreator(settings);
        messageCollection = new(settings.MessageRetentionPeriod);
        frameReader.FrameAvailable += QueueMessage;
        frameReader.IoDeviceFaulted += OnIoDeviceFaulted;
    }

    public bool IsListening => frameReader.IsListening;
    public bool IsSending { get; private set; }
    public event EventHandler<BBraunBccFrame>? NewMessage;
    public event EventHandler<ConnectionState>? ConnectionStatusChanged;

    private void QueueMessage(
        object? sender,
        BBraunBccFrame frame)
    {
#if DEBUG
        Log($"Received {frame}");
#endif
        messageCollection.Add(frame);
        NewMessage?.Invoke(this, frame);
        if(settings.Role == BccParticipantRole.Client)
            Enqueue(messageCreator.CreateAcknowledgeMessage());
    }

    private void OnIoDeviceFaulted(
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
            try
            {
                ioDevice.Write(message);
#if DEBUG
                Log($"Sent {StringMessageHelpers.RawControlCharactersToHumanFriendly(message)}");
#endif
            }
            catch
            {
                OnIoDeviceFaulted(this, EventArgs.Empty);
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
                outgoingMessages.CompleteAdding();
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
        WaitForRequest<BBraunBccFrame> waitRequest)
    {
        messageCollection.WaitFor(waitRequest);
    }

    public void Enqueue(
        byte[] message)
    {
        if(outgoingMessages.IsAddingCompleted)
            return;
        outgoingMessages.Add(message);
    }

    public void Dispose()
    {
        frameReader.Dispose();
        messageCollection.Dispose();
        outgoingMessages?.Dispose();
        sendTask?.Dispose();
    }

    private void Log(string message) => Console.WriteLine($"{logName} - {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}: {message}");
}