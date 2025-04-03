#!/bin/bash

cd /root

if [ $1 -eq "web" ]
then
	
	if wget -q https://github.com/mindleaving/patient-monitor-datalogger/releases/latest/download/patient-monitor-datalogger-deployment-pack.zip
	then
		echo "Could not download latest patient-monitor-datalogger-deployment-pack.zip"
		exit 1
	fi
	if wget -q https://github.com/mindleaving/patient-monitor-datalogger/releases/latest/download/deployment-pack-signature.sig
	then
		echo "Could not download latest deployment-pack-signature.sig"
		exit 1
	fi
elif [ $1 -eq "usb" ]
then
	if [ ! -f /dev/sdb1 ]
	then
		echo "No USB drive found. Skipping update."
		exit 0
	fi
	mount /dev/sdb1 /mnt/usb
	if [ $? -ne 0 ]
	then
		echo "Could not mount USB drive. Skipping update."
		exit 0
	fi
	if [ ! -f /mnt/usb/patient-monitor-datalogger-deployment-pack.zip || ! -f /mnt/usb/deployment-pack-signature.sig ]
	then
		echo "USB drive doesn't contain patient-monitor-datalogger-deployment-pack.zip and deployment-pack-signature.sig. Skipping update."
		umount /mnt/usb
		exit 0
	fi
	cp /mnt/usb/patient-monitor-datalogger-deployment-pack.zip .
	cp /mnt/usb/deployment-pack-signature.sig .
	umount /mnt/usb
else
	echo "Usage: update.sh <web|usb>"
	exit 1;
fi

if openssl dgst -sha256 -verify update-public-key.pem -signature deployment-pack-signature.sig patient-monitor-datalogger-deployment-pack.zip
then
	echo "Medical Device Datalogger update: Invalid signature for patient-monitor-datalogger-deployment-pack.zip"
	rm patient-monitor-datalogger-deployment-pack.zip
	rm deployment-pack-signature.sig
	exit 1
fi

# Unpack deployment pack
unzip -q patient-monitor-datalogger-deployment-pack.zip
rm install.sh

# Update configuration files
unzip -q config.zip -d config
cp -R config/* /
chown datalogger:datalogger -R /home/datalogger
chmod 770 -R /home/datalogger
nginx -s reload
systemctl daemon-reload
rm config.zip
rm -rf config

# Deploy API and web frontend
./deploy_patient-monitor-datalogger.sh

# Clean up
rm patient-monitor-datalogger-deployment-pack.zip
rm deployment-pack-signature.sig
