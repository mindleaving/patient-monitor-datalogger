#!/bin/bash
set -e

if [ $# -eq 0 ]
then
	echo "No private key supplied"
	echo "Usage: build-deployment-pack.sh <signing RSA private key>"
	exit 1
fi

BASE_PATH=$PWD
rm -f $BASE_PATH/deployment/patient-monitor-datalogger-deployment-pack.zip
rm -f $BASE_PATH/deployment/deployment-pack-signature.sig
rm -f $BASE_PATH/deployment/PatientMonitorDataLogger.API.zip
rm -f $BASE_PATH/deployment/PatientMonitorDataLogger.Frontend.zip
rm -f $BASE_PATH/deployment/config.zip

if [ ! -f PatientMonitorDatalogger.sln ]
then
	echo "build-deployment-pack.sh must be run from repository root containing PatientMonitorDatalogger.sln"
	exit 1
fi
./publish_to_linux.sh
cd $BASE_PATH/deployment
if [ ! -f PatientMonitorDataLogger.API.zip ]
then
	echo "PatientMonitorDataLogger.API.zip missing"
	cd $BASE_PATH
	exit 1
fi
if [ ! -f PatientMonitorDataLogger.Frontend.zip ]
then
	echo "PatientMonitorDataLogger.Frontend.zip missing"
	cd $BASE_PATH
	exit 1
fi
zip -q -r config.zip config
if [ $? -ne 0 ]
then
	echo "config.zip missing"
	cd $BASE_PATH
	exit 1
fi

zip -q -r patient-monitor-datalogger-deployment-pack.zip config.zip install.sh PatientMonitorDataLogger.API.zip PatientMonitorDataLogger.Frontend.zip
if [ $? -ne 0 ]
then
	echo "Could not create patient-monitor-datalogger-deployment-pack.zip"
	cd $BASE_PATH
	exit 1
fi
./sign-deployment-pack.sh $1
cd $BASE_PATH
