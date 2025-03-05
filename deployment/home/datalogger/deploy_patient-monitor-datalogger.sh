#!/bin/bash

unzip -q PatientMonitorDataLogger.API.zip
sudo cp /var/www/PatientMonitorDataLogger.API/appsettings.json /var/www/PatientMonitorDataLogger.API/appsettings.json.bak
sudo cp -R publish/* /var/www/PatientMonitorDataLogger.API/
sudo cp /var/www/PatientMonitorDataLogger.API/appsettings.json.bak /var/www/PatientMonitorDataLogger.API/appsettings.json
sudo rm -rf publish

unzip -q PatientMonitorDataLogger.Frontend.zip
sudo cp -R dist/* /var/www/PatientMonitorDataLogger.Frontend/
sudo rm -rf dist
rm -rf PatientMonitorDataLogger*.zip
sudo chmod 777 -R /var/www/PatientMonitorDataLogger*
sudo systemctl restart patient-monitor-datalogger-api.service


