﻿using System.Text;
using PatientMonitorDataLogger.BBraun;
using PatientMonitorDataLogger.BBraun.Helpers;
using PatientMonitorDataLogger.Shared.Simulation;

namespace PatientMonitorDataLoggerTest.BBraun;

public class BBraunBccCommunicatorTest
{
    [Test]
    public void MultiPartMessageIsAcknowledged()
    {
        var part1 = StringMessageHelpers.HumanFriendlyControlCharactersToRaw("<SOH>01152<STX>ComSystem/1/1>214,50100,FMNOR,1<RS>214,50100,FMSTAT,00051<RS>214,50100,FMDONGLE,T<RS>214,50100,FMBANY,_NV<RS>214,50100,FMRT,FMS1NSNS<RS>214,50100,FMRSTAT,00051<RS>214,50111,GNNEW,688F030006<RS>214,50111,INRT,0<RS>214,50111,INSOL,_NV<RS>214,50111,INSOLSN,_NV<RS>214,50111,INSOLID,_NV<RS>214,50111,INVOLACT,0<RS>214,50111,INRMT,0<RS>214,50111,INDOCAL,0<RS>214,50111,INDCON,0<RS>214,50111,INDCONU,23<RS>214,50111,INDORT,0<RS>214,50111,INDORTU,23<RS>214,50111,INVI,0<RS>214,50111,INTIME,0<RS>214,50111,INBORT,800.00<RS>214,50111,INNR,60008D98<RS>214,50111,INP1,4<RS>214,50111,INP2,0<RS>214,50111,INSYR,0<RS>214,50111,INSTBY,0<RS>214,50111,INSERNUM,36248<RS>214,50111,INDTNR,6144<RS>214,50111,DSSTATUS,00050<RS>214,50111,INBOVAL,0.0<RS>214,50111,INBOCNT,0<RS>214,50111,INM1,1<RS>214,50111,INM2,0<RS>214,50111,INM3,0<RS>214,50111,INM4,_NV<RS>214,50111,INM5,0<RS>214,50111,INM6,0<RS>214,50111,INM7,0<RS>214,50111,INM8,1<RS>214,50111,INM9,_NV<RS>214,50111,INM10,0<RS>214,50111,INM11,0<RS>214,50111,INM12,0<RS>214,50111,INA1,0<RS>214,50111,INA2,0<RS>214,50111,INA3,0<RS>214,50111,INA4,0<RS>214,50111,INA5,0<RS>214,50111,INA6,_NV<RS>214,50111,INA7,0<RS>214,50111,INA8,0<RS>214,50111,INA9,0<RS>214,50111,INA10,_NV<RS>214,50111,INA11,0<RS>214,50111,INA12,0<RS>214,50111,INA13,_NV<RS>214,50111,INA14,0<RS>214,50111,INA15,0<ETB>00002<EOT>");
        var part2 = StringMessageHelpers.HumanFriendlyControlCharactersToRaw("<SOH>01073<STX>ComSystem/1/1>215,50113,GNNEW,688F030006<RS>215,50113,INRT,0.30<RS>215,50113,INSOL,Arterenol4mg/50<RS>215,50113,INSOLSN,Arteren<EE><80><A1><RS>215,50113,INSOLID,33~4mg/5002<RS>215,50113,INVOLACT,47.64<RS>215,50113,INRMT,65535<RS>215,50113,INDOCAL,0<RS>215,50113,INDCON,0<RS>215,50113,INDCONU,32<RS>215,50113,INDORT,0<RS>215,50113,INDORTU,50<RS>215,50113,INVI,1.4<RS>215,50113,INTIME,97<RS>215,50113,INBORT,800.00<RS>215,50113,INNR,60009FA0<RS>215,50113,INP1,4<RS>215,50113,INP2,35<RS>215,50113,INSYR,50<RS>215,50113,INSTBY,0<RS>215,50113,INSERNUM,40864<RS>215,50113,INDTNR,6144<RS>215,50113,DSSTATUS,00051<RS>215,50113,INBOVAL,0.0<RS>215,50113,INBOCNT,0<RS>215,50113,INM1,0<RS>215,50113,INM2,0<RS>215,50113,INM3,1<RS>215,50113,INM4,_NV<RS>215,50113,INM5,1<RS>215,50113,INM6,0<RS>215,50113,INM7,0<RS>215,50113,INM8,1<RS>215,50113,INM9,_NV<RS>215,50113,INM10,0<RS>215,50113,INM11,0<RS>215,50113,INM12,0<RS>215,50113,INA1,0<RS>215,50113,INA2,0<RS>215,50113,INA3,0<RS>215,50113,INA4,0<RS>215,50113,INA5,0<RS>215,50113,INA6,_NV<RS>215,50113,INA7,0<RS>215,50113,INA8,0<RS>215,50113,INA9,0<RS>215,50113,INA10,_NV<RS>215,50113,INA11,0<RS>215,50113,INA12,0<RS>215,50113,INA13,_NV<RS>215,50113,INA14,0<RS>215,50113,INA15,0<ETB>00025<EOT>");
        var part3 = StringMessageHelpers.HumanFriendlyControlCharactersToRaw("<SOH>01067<STX>ComSystem/1/1>215,50114,GNNEW,688F030006<RS>215,50114,INRT,0.10<RS>215,50114,INSOL,Beloc25mg/50<RS>215,50114,INSOLSN,Beloc25<RS>215,50114,INSOLID,43~25mg/5003<RS>215,50114,INVOLACT,20.39<RS>215,50114,INRMT,65535<RS>215,50114,INDOCAL,0<RS>215,50114,INDCON,0<RS>215,50114,INDCONU,32<RS>215,50114,INDORT,0<RS>215,50114,INDORTU,50<RS>215,50114,INVI,19.0<RS>215,50114,INTIME,6000<RS>215,50114,INBORT,800.00<RS>215,50114,INNR,60000717<RS>215,50114,INP1,4<RS>215,50114,INP2,0<RS>215,50114,INSYR,60<RS>215,50114,INSTBY,1434<RS>215,50114,INSERNUM,1815<RS>215,50114,INDTNR,6144<RS>215,50114,DSSTATUS,00051<RS>215,50114,INBOVAL,0.0<RS>215,50114,INBOCNT,0<RS>215,50114,INM1,0<RS>215,50114,INM2,0<RS>215,50114,INM3,1<RS>215,50114,INM4,_NV<RS>215,50114,INM5,1<RS>215,50114,INM6,0<RS>215,50114,INM7,0<RS>215,50114,INM8,1<RS>215,50114,INM9,_NV<RS>215,50114,INM10,0<RS>215,50114,INM11,0<RS>215,50114,INM12,0<RS>215,50114,INA1,0<RS>215,50114,INA2,0<RS>215,50114,INA3,0<RS>215,50114,INA4,0<RS>215,50114,INA5,0<RS>215,50114,INA6,_NV<RS>215,50114,INA7,0<RS>215,50114,INA8,0<RS>215,50114,INA9,0<RS>215,50114,INA10,_NV<RS>215,50114,INA11,0<RS>215,50114,INA12,0<RS>215,50114,INA13,_NV<RS>215,50114,INA14,0<RS>215,50114,INA15,0<ETX>00127<EOT>");
        var simulatedIoDevice = new SimulatedIoDevice();
        var settings = BBraunBccClientSettings.CreateForSimulatedConnection(BccParticipantRole.Client, false, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), simulatedIoDevice);
        var frames = new List<BBraunBccFrame>();
        var sut = new BBraunBccCommunicator(simulatedIoDevice, settings, nameof(BBraunBccCommunicator));
        sut.NewMessage += (_,frame) => frames.Add(frame);
        sut.Start();

        simulatedIoDevice.Receive(Encoding.UTF8.GetBytes(part1));
        Assert.That(() => simulatedIoDevice.OutgoingData.Count(c => c == BBraunBccMessageCreator.AcknowledgeCharacter), Is.EqualTo(1).After(500));
        Assert.That(frames.Count, Is.EqualTo(1));
        Assert.That(frames[0].BedId, Is.EqualTo("ComSystem"));

        simulatedIoDevice.Receive(Encoding.UTF8.GetBytes(part2));
        Assert.That(() => simulatedIoDevice.OutgoingData.Count(c => c == BBraunBccMessageCreator.AcknowledgeCharacter), Is.EqualTo(2).After(500));
        Assert.That(frames.Count, Is.EqualTo(2));
    }
}