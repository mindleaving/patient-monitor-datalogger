cd PatientMonitorDataLogger.API
dotnet clean -c Release
dotnet publish -c Release -r linux-arm64 --no-self-contained
Compress-Archive -Path bin/Release/net8.0/linux-arm64/publish -DestinationPath ../deployment/PatientMonitorDataLogger.API.zip -Force
cd ../patient-monitor-datalogger-frontend
npm run build
Compress-Archive -Path dist -DestinationPath ../deployment/PatientMonitorDataLogger.Frontend.zip -Force
cd ..

#scp F:\Projects\patient-monitor-datalogger\deployment\PatientMonitorDataLogger.*.zip datalogger-3bplus:~
#ssh datalogger-3bplus "./deploy_patient-monitor-datalogger.sh"

scp F:\Projects\patient-monitor-datalogger\deployment\PatientMonitorDataLogger.*.zip datalogger01:~
ssh datalogger01 "./deploy_patient-monitor-datalogger.sh"