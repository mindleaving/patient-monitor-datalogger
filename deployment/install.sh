#!/bin/bash

sudo apt update
sudo apt upgrade -y

unzip -q config.zip -d config
sudo cp -R config/* /
rm -rf config

sudo nmtui
sudo apt install -y ufw nginx unzip rsyslog usbmount
sudo ufw allow 22/tcp
sudo ufw allow 80/tcp
sudo ufw logging off
sudo ufw enable

cd /etc/nginx/sites-enabled
sudo ln -s /etc/nginx/sites-available/patient-monitor-datalogger.conf
sudo rm default
cd ~
sudo usermod -a -G dialout www-data
sudo usermod -a -G plugdev www-data

sudo mkdir -p /data/patient-monitor-datalogger
sudo chmod 777 -R /data/patient-monitor-datalogger
sudo mkdir -p /var/www/PatientMonitorDataLogger.API
sudo mkdir -p /var/www/PatientMonitorDataLogger.Frontend
sudo chown www-data:www-data -R /var/www/*

curl -sSL https://dot.net/v1/dotnet-install.sh | sudo bash /dev/stdin --channel LTS --install-dir /usr/local/share/dotnet
sudo ln -s /usr/local/share/dotnet/dotnet /usr/bin/dotnet
sudo systemctl enable patient-monitor-datalogger-api.service

cd ~
rm install.sh
rm config.sh
