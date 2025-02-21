using PatientMonitorDataLogger.PhilipsIntellivue.Helpers;

namespace PatientMonitorDataLogger.PhilipsIntellivue.Models;

public class PollProfileSupport : ISerializable
{
    public const uint MaxAllowedLanMtu = 1364;
    public const uint MaxAllowedRs232Mtu = 1000;

    public PollProfileSupport(
        PollProfileRevision profileRevision,
        RelativeTime minimumPollPeriod,
        uint maxMtuRx,
        uint maxMtuTx,
        uint maxTransmitBandwidth,
        PollProfileOptions options,
        List<AttributeValueAssertion> optionalPackages)
    {
        ProfileRevision = profileRevision;
        MinimumPollPeriod = minimumPollPeriod;
        MaxMtuRx = maxMtuRx;
        MaxMtuTx = maxMtuTx;
        MaxTransmitBandwidth = maxTransmitBandwidth;
        Options = options;
        OptionalPackages = optionalPackages;
    }

    public PollProfileRevision ProfileRevision { get; }
    public RelativeTime MinimumPollPeriod { get; }
    public uint MaxMtuRx { get; }
    public uint MaxMtuTx { get; }
    public uint MaxTransmitBandwidth { get; }
    public PollProfileOptions Options { get; }
    public List<AttributeValueAssertion> OptionalPackages { get; }

    public static PollProfileSupport Read(
        BigEndianBinaryReader binaryReader)
    {
        var profileRevision = (PollProfileRevision)binaryReader.ReadUInt32();
        var minimumPollPeriod = RelativeTime.Read(binaryReader);
        var maxMtuRx = binaryReader.ReadUInt32();
        var maxMtuTx = binaryReader.ReadUInt32();
        var maxTransmitBandwidth = binaryReader.ReadUInt32();
        var options = (PollProfileOptions)binaryReader.ReadUInt32();
        var optionalPackages = List<AttributeValueAssertion>.Read(binaryReader, AttributeValueAssertion.Read);
        return new(profileRevision, minimumPollPeriod, maxMtuRx, maxMtuTx, maxTransmitBandwidth, options, optionalPackages);
    }

    public byte[] Serialize()
    {
        return
        [
            ..BigEndianBitConverter.GetBytes((uint)ProfileRevision),
            ..MinimumPollPeriod.Serialize(),
            ..BigEndianBitConverter.GetBytes(MaxMtuRx),
            ..BigEndianBitConverter.GetBytes(MaxMtuTx),
            ..BigEndianBitConverter.GetBytes(MaxTransmitBandwidth),
            ..BigEndianBitConverter.GetBytes((uint)Options),
            ..OptionalPackages.Serialize()
        ];
    }
}