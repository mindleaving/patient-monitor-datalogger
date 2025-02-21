using System.Text;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

public interface ISerialPort
{
    Encoding Encoding { get; set; }

    void Open();

    int Read(
        byte[] buffer,
        int offset,
        int count);

    int Read(
        byte[] buffer);

    void Close();

    void Write(
        byte[] buffer,
        int offset,
        int count);
    void Write(
        byte[] buffer);
}