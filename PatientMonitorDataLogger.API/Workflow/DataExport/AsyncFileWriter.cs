using System.Collections.Concurrent;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public abstract class AsyncFileWriter<T> : IDisposable, IAsyncDisposable
{
    private readonly string outputFilePath;
    private readonly StreamWriter streamWriter;
    protected readonly BlockingCollection<T> dataQueue = new();
    private readonly object startStopLock = new();
    private CancellationTokenSource? cancellationTokenSource;
    private Task? writeTask;

    protected AsyncFileWriter(
        string outputFilePath)
    {
        this.outputFilePath = outputFilePath;
        streamWriter = new StreamWriter(File.OpenWrite(outputFilePath));
    }

    public bool IsRunning { get; private set; }

    public void Start()
    {
        if(IsRunning)
            return;
        lock (startStopLock)
        {
            if(IsRunning)
                return;
            cancellationTokenSource = new CancellationTokenSource();
            writeTask = Task.Factory.StartNew(
                () => WriteToFile(cancellationTokenSource.Token),
                cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }
    }

    private void WriteToFile(
        CancellationToken cancellationToken)
    {
        foreach (var data in dataQueue.GetConsumingEnumerable(cancellationToken))
        {
            var lines = Serialize(data);
            foreach (var line in lines)
            {
                streamWriter.WriteLine(line);
            }
        }
    }

    public void Stop()
    {
        if(!IsRunning)
            return;
        lock (startStopLock)
        {
            if(!IsRunning)
                return;
            cancellationTokenSource?.Cancel();
            try
            {
                writeTask?.Wait();
            }
            catch (AggregateException aggregateException)
            {
                if (aggregateException.InnerException is not (TaskCanceledException or OperationCanceledException))
                    throw;
            }
        }
    }

    protected abstract IEnumerable<string> Serialize(
        T data);

    public void Dispose()
    {
        Stop();
        streamWriter.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        Stop();
        await streamWriter.DisposeAsync();
    }
}