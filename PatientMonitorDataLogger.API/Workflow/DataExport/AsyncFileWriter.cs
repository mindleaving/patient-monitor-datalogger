using System.Collections.Concurrent;
using System.Text;

namespace PatientMonitorDataLogger.API.Workflow.DataExport;

public abstract class AsyncFileWriter<T> : IDisposable
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

            var fileStream = File.OpenWrite(outputFilePath);
            fileStream.Seek(0, SeekOrigin.End); // Append
            streamWriter = new StreamWriter(fileStream);
            streamWriter.AutoFlush = true;
            writeTask = Task.Factory.StartNew(
                WriteToFile,
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
            IsRunning = true;
        }
    }

    public virtual void Write(
        T item)
    {
        if(dataQueue.IsAddingCompleted)
            return;
        dataQueue.Add(item);
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
}