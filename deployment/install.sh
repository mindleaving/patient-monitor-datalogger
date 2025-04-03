#!/bin/bash

sudo apt update
sudo apt upgrade -y

sudo useradd -d /home/tech tech
sudo passwd -d tech
sudo adduser tech sudo
sudo mkdir -p /home/tech/.ssh
sudo cp ~/.ssh/authorized_keys /home/tech/.ssh/
sudo cp ~/.bashrc /home/tech/
sudo cp ~/.profile /home/tech/

unzip -q config.zip
rm config.zip
sudo cp -R config/* /
rm -rf config

# Fix permissions
sudo chmod +x deploy_patient-monitor-datalogger.sh
sudo chown tech:tech -R /home/tech
sudo chmod 700 -R /home/tech
sudo chmod +x /root/update.sh
sudo chmod +x /root/deploy_patient-monitor-datalogger.sh

sudo nmtui
sudo apt install -y ufw nginx unzip rsyslog
sudo ufw allow 22/tcp
sudo ufw logging off
sudo ufw enable

cd /etc/nginx/sites-enabled
sudo ln -s /etc/nginx/sites-available/patient-monitor-datalogger.conf
sudo rm default
cd ~
sudo usermod -a -G dialout datalogger
sudo usermod -a -G plugdev datalogger
gsettings set org.gnome.desktop.media-handling automount-open false

sudo mkdir /mnt/usb # Used for /root/update.sh
sudo mkdir -p /data/patient-monitor-datalogger
sudo chown datalogger:datalogger -R /data/patient-monitor-datalogger
sudo chmod 777 -R /data/patient-monitor-datalogger
sudo mkdir -p /var/www/PatientMonitorDataLogger.API
sudo mkdir -p /var/www/PatientMonitorDataLogger.Frontend
sudo chown datalogger:datalogger -R /var/www/*

curl -sSL https://dot.net/v1/dotnet-install.sh | sudo bash /dev/stdin --channel LTS --install-dir /usr/local/share/dotnet
sudo ln -s /usr/local/share/dotnet/dotnet /usr/bin/dotnet
sudo systemctl enable patient-monitor-datalogger-api.service
sudo systemctl enable update-patient-monitor-datalogger.service

./deploy_patient-monitor-datalogger.sh
sudo nginx -s reload

cd ~
sudo mv deploy_patient-monitor-datalogger.sh /home/tech/deploy_patient-monitor-datalogger.sh
rm install.sh
rm -f patient-monitor-datalogger-deployment-pack.zip
sudo deluser datalogger sudo
sudo reboot
