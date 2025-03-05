BASE_PATH=$PWD
rm PatientMonitorDataLogger.*.zip

cd PatientMonitorDataLogger.API
dotnet clean -c Release
dotnet publish -c Release -r linux-arm64 --no-self-contained
cd bin/Release/net8.0/linux-arm64
zip -q -r $BASE_PATH/deployment/PatientMonitorDataLogger.API.zip publish

cd $BASE_PATH/patient-monitor-datalogger-frontend
npm run build
zip -q -r $BASE_PATH/deployment/PatientMonitorDataLogger.Frontend.zip dist
cd $BASE_PATH

#scp ~/git/patient-monitor-datalogger/deployment/PatientMonitorDataLogger.*.zip datalogger-3bplus:~
#ssh datalogger-3bplus "./deploy_patient-monitor-datalogger.sh"

scp ~/git/patient-monitor-datalogger/deployment/PatientMonitorDataLogger.*.zip datalogger-4b:~
ssh datalogger-4b "./deploy_patient-monitor-datalogger.sh"
