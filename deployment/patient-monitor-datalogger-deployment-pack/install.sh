#!/bin/bash
set -e

# Update system
sudo apt update
sudo apt upgrade -y

# Install additional software
sudo apt install -y ufw nginx unzip rsyslog

# Create privileged user
sudo useradd -d /home/tech tech
sudo passwd -d tech
sudo adduser tech sudo
sudo mkdir -p /home/tech/.ssh
sudo cp ~/.ssh/authorized_keys /home/tech/.ssh/
sudo cp ~/.bashrc /home/tech/
sudo cp ~/.profile /home/tech/

# Deploy configuration files
unzip -q config.zip
rm config.zip
sudo cp -R config/* /
rm -rf config

# Create symbolic links
cd /etc/nginx/sites-enabled
sudo ln -s /etc/nginx/sites-available/patient-monitor-datalogger.conf
sudo rm default

# Fix permissions
cd ~
sudo chmod 440 /etc/sudoers.d/*
sudo chmod +x deploy_patient-monitor-datalogger.sh
sudo chown datalogger:datalogger -R /home/datalogger
sudo chmod 700 -R /home/datalogger
sudo chown tech:tech -R /home/tech
sudo chmod 700 -R /home/tech
sudo chmod +x /root/update.sh
sudo chmod +x /root/deploy_patient-monitor-datalogger.sh
sudo chmod +x ~/Desktop/medical-device-datalogger.desktop

# Configure network
sudo nmtui
sudo ufw allow 22/tcp
sudo ufw logging off
sudo ufw enable

# Allow datalogger user to access serial port
sudo usermod -a -G dialout datalogger
sudo usermod -a -G plugdev datalogger

# Disable services not needed
gsettings set org.gnome.desktop.media-handling automount-open false
sudo systemctl disable bluetooth.service

# Create directories
sudo mkdir /mnt/usb # Used for /root/update.sh
sudo mkdir -p /data/patient-monitor-datalogger
sudo chown datalogger:datalogger -R /data/patient-monitor-datalogger
sudo chmod 777 -R /data/patient-monitor-datalogger
sudo mkdir -p /var/www/PatientMonitorDataLogger.API
sudo mkdir -p /var/www/PatientMonitorDataLogger.Frontend
sudo chown datalogger:datalogger -R /var/www/*

# Install .NET
curl -sSL https://dot.net/v1/dotnet-install.sh | sudo bash /dev/stdin --channel LTS --install-dir /usr/local/share/dotnet
sudo ln -s /usr/local/share/dotnet/dotnet /usr/bin/dotnet

# Deploy API and frontend
./deploy_patient-monitor-datalogger.sh
sudo nginx -s reload
sudo systemctl enable patient-monitor-datalogger-api.service
sudo systemctl enable update-patient-monitor-datalogger.service

# Setup autostart of frontend
mkdir -p ~/.config/autostart
cd ~/.config/autostart
ln -s ~/Desktop/medical-device-datalogger.desktop

# Clean up
cd ~
rm install.sh
rm deploy_patient-monitor-datalogger.sh
rm -f patient-monitor-datalogger-deployment-pack.zip

# Restrict user permissions
sudo deluser datalogger sudo
sudo deluser datalogger adm
sudo rm -f /etc/sudoers.d/010_pi-nopasswd
sudo reboot
