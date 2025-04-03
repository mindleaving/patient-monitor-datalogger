#!/bin/bash

unzip -q PatientMonitorDataLogger.API.zip
cp /var/www/PatientMonitorDataLogger.API/appsettings.json /var/www/PatientMonitorDataLogger.API/appsettings.json.bak
cp -R publish/* /var/www/PatientMonitorDataLogger.API/
cp /var/www/PatientMonitorDataLogger.API/appsettings.json.bak /var/www/PatientMonitorDataLogger.API/appsettings.json
rm -rf publish

unzip -q PatientMonitorDataLogger.Frontend.zip
cp -R dist/* /var/www/PatientMonitorDataLogger.Frontend/
rm -rf dist
rm -rf PatientMonitorDataLogger*.zip
chmod 755 -R /var/www/PatientMonitorDataLogger*
systemctl restart patient-monitor-datalogger-api.service

