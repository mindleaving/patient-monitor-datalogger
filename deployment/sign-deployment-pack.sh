#!/bin/bash

if [ $# -eq 0 ]
then
	echo "No private key supplied"
	echo "Usage: sign-deployment-pack.sh <signing RSA private key>"
	exit 1
fi

openssl dgst -sha256 -sign $1 -out deployment-pack-signature.sig  patient-monitor-datalogger-deployment-pack.zip
