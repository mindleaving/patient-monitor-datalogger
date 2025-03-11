namespace PatientMonitorDataLogger.Shared.Models;

public interface IODevice : IDisposable
{
    void Open();

    int Read(
        byte[] buffer,
        int offset,
        int count);

    int Read(
        byte[] buffer);

    void Write(
        byte[] buffer,
        int offset,
        int count);
    void Write(
        byte[] buffer);

    void Close();
}