#!/bin/bash

cd ~
wget https://github.com/mindleaving/patient-monitor-datalogger/releases/latest/patient-monitor-datalogger-deployment-pack.zip
unzip -q patient-monitor-datalogger-deployment-pack.zip
rm config.zip
rm install.sh
./deploy_patient-monitor-datalogger.sh
