using System.Collections.Concurrent;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public abstract class AsyncFileWriter<T> : IDisposable, IAsyncDisposable
{
    private readonly string outputFilePath;
    private StreamWriter? streamWriter;
    protected BlockingCollection<T> dataQueue = new();
    private readonly object startStopLock = new();
    private Task? writeTask;

    protected AsyncFileWriter(
        string outputFilePath)
    {
        this.outputFilePath = outputFilePath;
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
            
            streamWriter = new StreamWriter(File.OpenWrite(outputFilePath));
            streamWriter.AutoFlush = true;
            writeTask = Task.Factory.StartNew(
                WriteToFile,
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            IsRunning = true;
        }
    }

    private void WriteToFile()
    {
        foreach (var data in dataQueue.GetConsumingEnumerable())
        {
            var lines = Serialize(data);
            foreach (var line in lines)
            {
                streamWriter!.WriteLine(line);
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

            IsRunning = false;
            dataQueue.CompleteAdding();
            try
            {
                writeTask?.Wait();
                streamWriter?.Dispose();
            }
            catch (AggregateException aggregateException)
            {
                if (aggregateException.InnerException is not (TaskCanceledException or OperationCanceledException))
                    throw;
            }
            dataQueue = new BlockingCollection<T>();
        }
    }

    protected abstract IEnumerable<string> Serialize(
        T data);

    public void Dispose()
    {
        Stop();
    }

    public async ValueTask DisposeAsync()
    {
        Stop();
    }
}