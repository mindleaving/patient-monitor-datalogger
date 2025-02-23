cd PatientMonitorDataLogger.API
dotnet clean -c Release
dotnet publish -c Release -r linux-arm64 --no-self-contained
Compress-Archive -Path bin/Release/net8.0/linux-arm64/publish -DestinationPath ../PatientMonitorDataLogger.API.zip -Force
cd ../patient-monitor-datalogger-frontend
npm run build
Compress-Archive -Path dist -DestinationPath ../PatientMonitorDataLogger.Frontend.zip -Force
cd ..

scp F:\Projects\patient-monitor-datalogger\PatientMonitorDataLogger.*.zip datalogger-4b:~
ssh datalogger-4b "./deploy_patient-monitor-datalogger.sh"