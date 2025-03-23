# Technical Documentation for Medical Device Data Logger
## The Product
Medical Device Data Logger, hereafter called The Product, consists of

- a small computer (Raspberry Pi Model 4B) with a 4.3 inch touch screen enclosed in a protective plastic case
- a 27 W USB Type-C power supply
- a USB Type A-to-RS232-D-Sub 9-pin male Adapter 30cm long
- a 2.5 m custom cable with D-sub 9-pin female to RJ45 connectors, wired accoring to specifications in Philips Intellivue Patient Monitor Data Export Interface Programming Guide (ID 453665127791, Published in Germany 09/2024).

Below image shows all involved components

![Data logger with accesories](images/datalogger-with-accesories.jpg)

### Functionality
The Product runs a  software with a web API and web frontend which enables the user to record data from supported medical devices like patient monitors and infusion pump systems. These data are recorded on the device, until deleted by the End User, and can be copied to a USB Drive for further use. The device is solely operated using the built-in touch screen. It powers on when connected to main electricity grid using the USB Type-C power supply.

## Manufacturer
Instructions for building the Product are available on https://github.com/mindleaving/patient-monitor-datalogger under a MIT License. Below manufacturer is not liable for any Products build by third parties using these instructions.

The Product can be ordered from the Manufacturer, in which case usual product liability laws apply.

The Product is developed and manufactured by

````
Jan Scholtyssek
Poststr. 28
69115 Heidelberg
Germany
````


## Bill Of Material

| Component            | Product Number | Manufacturer      | Standards |
| ---------------------|----------------|-------------------|-----------|
| Raspberry Pi Model 4 | PI4 MODEL B/4GB | Raspberry Pi Inc. | EMC, RED, IEC, RoHS |
| Pi 27 W USB Type-C Power Supply | SC1157 | Raspberry Pi Inc. | IEC, RoHS |
| 4.3" DSI Touch Display | 4.3inch DSI LCD (with case) | Waveshare | |
| Protective Case | 4.3inch DSI LCD (with case) | Waveshare | |
| Micro-SD Card 128 GB | SDSQUAB-128G-GN6MA | SanDisk | |
| USB-to-RS232 Serial Adapter UC232A | Aten | RoHS |


## Risk Assessment and Mitigation
### Risks due to Use of Product
- **Risk of electrical shock**: When operated with the intended power supply, no electrical shock is to be expected. The User Manual instructs the End User only to use the power supply shipped with the Product. Furthermore the User Manual specifies that the End User must not deassemble the Product, insert any items into the opening of the case, except appropriate cables into the exposed connectors. When connecting to medical devices, only the cables specified in the User Manual must be used, to ensure, no ground loops occur, especially when connecting to more than one medical device. When connecting to more than one medical device at a time, the End User must either use medical isolators between the Product and the medical devices or consult with a local electrician to ensure that no devices with different ground potentials are connected. The Product must be operated in a suitable environment, i.e. ambient room temperature at humidities between 30 and 85% humidity.
- **Risk of burns**: The Product contains no components that are intended to heat up excessively. The hottest part is expected to the processor the the Raspberry Pi, which contains circuits for throttling the processor, when it becomes too hot, which is the case at 80°C, with further throttling at 85°C, making it very unlikely for the product to become warmer than 90°C at any  point in time. This End User is not able to get in contact with the processor or any component to which the heat is conducted. The heat is dissipated through air convection and radiated through the walls of the protective case. The effect of operating the Raspberry Pi under maximum CPU-load for long periods of time on case temperature was explored during [General Product Safety Testing](#general-product-safety).
- **Risk of cuts and mechanical injuries**: Design of the Product has been evaluated with regards to any sharp edges, corners, protrusions or openings. No sharp edges, corners or protrusions where identified, that could injury the End User. The protective case has several openings at the bottom, none of which are large enough for the End User to get a finger lodged inside. All edges of these openings are smooth and pose no risk of cuts. In case the protective case gets damaged, it may have sharp edges. In that case the User Manual instructs the End User to stop using the product and either discuss repair with the Manufacturer or dispose the Product.
- **Risk of tripping**: Cables in general may pose a tripping hazard. The User Manual instructs the End User to place the Product at a location, that is sufficiently far away from the patient and any medical gas outlets and run power and data cables such, that they do not interfere with treatment of the patient and do not pose a tripping hazard.
- **Risk due to material wear**: Over time, or when improperly treated with chemicals, materials can wear, become brittle, get sticky or in other ways loose their intended properties. All materials used in the Product have been evaluated to that regard. Raspberry Pi, touch screen and micro-SD-card are unlikely to show wear over time. Data loss may occur, due to wear of the storage medium. This is discussed separately. The protective case may show mechanical wear in form of scratches. Cables may wear due to the insulations becoming brittle or mechanical stresses (kinks, pinching), exposing the conductors, potentially causing short circuits. The User Manual instructs the End User to check all cables for damages before each use and dispose any damaged cable immediately.
- **Risk of data loss**: The Product is intended for recording data from medical devices for up to 3 days and has been tested for that time period. It is not intended to store the recorded data in the long term. Due to software errors or the micro-SD storage card becoming faulty, data may get lost. The User Manual instructs the End User to copy recorded data to a USB drive using the web interface, move the data to a different storage medium and check that all data has been secured. This should be done after every recording session.

### Risks due to Effects on Environment
The following risks and their mitigation strategies have been identified:
- **Risk of fire**: Similar to the evaluation for the "Risk of burns", the limited heating of the components, with an expected maximum temperature of 90°C, makes it unlikely that the Product will catch fire, melt or begin to smoke. The more likely scenario is a ground loop, when connecting the Product to another device with an electrical fault. This risk is mitigated by following the same strategies as oulined in "Risk of electrical shock", i.e. using medical isolators or consulting with a local electrician before connecting to more than one medical device.
- **Risk of fire or explosion when operated near an oxygen-outlet**: The Product must not be operated within 1.5 m of any outlet of medical gases, especially oxygen or any environment with increased oxygen saturation. This is clearly stated in the User Manual and End Users verbally informed of this restriction upon delivery of the Product.
- **Risk of electric interference with medical devices**: All radio equipment of the Product is turned off during production. The End User is informed to keep all radio equipment turned off. This is also stated in the User Manual. The User Manual also outlines strategies for identifying, whether the Product is the cause of the interference. If the Product is identified as the source of interference, it is to be shut down, disconnected and removed from the vicinity of the effected medical devices.

### Safety Critical Components
### Independent Conformity Assessment

## Testing procedures

## Harmonized Standards Compliance Evaluation
### Relevant Standards, Regulations and Directives
The following regulations and harmonized standards frameworks are considered:
- General Product Safety Regulation (GPSR)
  - EN 62368-1 (PSU, Raspberry Pi 4B)
  - EN 60950-1:2006 (Raspberry Pi 4B)
  - BS EN 62311: 2008 (Raspberry Pi 4B)
- Electromagnetic Compatibility (EMC)
  - EN 55011:2009
  - EN 55011:2016
  - EN 55022:2010 + AC:2011 (PSU)
  - EN 55024:2010 (PSU)
  - EN 55032:2015 (PSU)
  - EN 55035:2017
  - EN 61000-3-2:2014
  - EN 61000-3-3:2013
  - EN 61204-3:2000
  - ETSI EN 301 489-1 V2.2.3: 2019 (Raspberry Pi 4B)
  - ETSI EN 301 489-17 V3.1.1: 2017 (Raspberry Pi 4B)
- Low Voltage Directive (LVD)
  - EN 62368-1:2014+A11:2017 (PSU)
- Radio Equipment Directive (RED)
  - ETSI EN 300 328 V2.2.2: 2019 (Raspberry Pi 4B)
  - ETSI EN 301 893 V2.1.1: 2017 (Raspberry Pi 4B)
- Restriction of the use of certain hazardous substances (RoHS)
  - IEC EN 63000: 2018 (PSU, Raspberry Pi 4B)

All parts of the Product are subject to GPSR. 

Due to the low operating voltage of 5V, the Raspberry Pi, touch screen and USB-to-RS232-Adpater are not subject ot the Low Voltage Directive, which covers devices with an input or output voltage of at least 50 V AC or 75 V DC. The power supply is subject to the LVD.

Raspberry Pi Model 4B contains circuits for wireless communication over WiFi and Bluetooth and is therefore subject to RED. Both Raspberry Pi and touch screen are subject to EMC regulation. 

Finally, all components are subject to RoHS regulation.

### General Product Safety
#### Intended User
End User of the Product is intended to be research staff at hospitals or medical research facilities.

#### Design and Composition
The Product is designed to be simple and easy to use, with only the most necessary interfaces for interaction, to reduce the risk of wrong usage. The DC connector of the power supply only fits into one of the connectors on the Raspberry Pi, which eliminates the risk of End Users connecting the power supply to any wrong connector.

The protective case has no sharp edges or corners, making injuries unlikely, as long as the protective case is assembled and intact. Due to the protective case, it is also not possible for the End User to come into contact with any parts of the Raspberry Pi, that mustn't been touched during operation. The exposed connectors are safe to be touched by the End User during operation.

#### Features
#### Effect on other Products
#### Information to End User
#### Enclosure and Markings
#### Cybersecurity
#### Continuous Product Safety Monitoring and Recall Procedures
##### Monitoring Recall of Materials Used
##### Safety Business Gateway Notification
##### Product User Registry
##### Accident Registration
##### Recall Procedure

### Electrical Safety
#### Interactions of Components
- TODO: Argument why conformity with regulations, standards and directives is maintained after assembling Raspberry Pi, touch screen and protective case.

### Environmental Safety
#### RoHS Conformity


### Quality Assurance Procedures for each produced datalogger
Each Product

## Declaration of Conformity

## References

### General Product Safety Regulation

Regulation (EU) 2023/988 of the European Parliament and of the Council of 10 May 2023 on general product safety, amending Regulation (EU) No 1025/2012 of the European Parliament and of the Council and Directive (EU) 2020/1828 of the European Parliament and the Council, and repealing Directive 2001/95/EC of the European Parliament and of the Council and Council Directive 87/357/EEC (Text with EEA relevance)

PE/79/2022/REV/1

OJ L 135, 23.5.2023, p. 1–51

ELI: http://data.europa.eu/eli/reg/2023/988/oj


### Blue Guide

Commission Notice — The ‘Blue Guide’ on the implementation of EU products rules 2016 (Text with EEA relevance)

C/2016/1958

OJ C 272, 26.7.2016, p. 1–149

https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=CELEX:52016XC0726(02)

### Low Voltage Directive

Directive 2014/35/EU of the European Parliament and of the Council of 26 February 2014 on the harmonisation of the laws of the Member States relating to the making available on the market of electrical equipment designed for use within certain voltage limits (recast) Text with EEA relevance

OJ L 96, 29.3.2014, p. 357–374

ELI: http://data.europa.eu/eli/dir/2014/35/oj

### Electromagnetic Compatibility

Directive 2014/30/EU of the European Parliament and of the Council of 26 February 2014 on the harmonisation of the laws of the Member States relating to electromagnetic compatibility (recast) Text with EEA relevance

OJ L 96, 29.3.2014, p. 79–106

ELI: http://data.europa.eu/eli/dir/2014/30/oj


### RoHS

Directive 2011/65/EU of the European Parliament and of the Council of 8 June 2011 on the restriction of the use of certain hazardous substances in electrical and electronic equipment (recast) Text with EEA relevance

OJ L 174, 1.7.2011, p. 88–110

ELI: http://data.europa.eu/eli/dir/2011/65/oj


### Radio Equipment Directive

Directive 2014/53/EU of the European Parliament and of the Council of 16 April 2014 on the harmonisation of the laws of the Member States relating to the making available on the market of radio equipment and repealing Directive 1999/5/EC Text with EEA relevance

OJ L 153, 22.5.2014, p. 62–106

ELI: http://data.europa.eu/eli/dir/2014/53/oj


## Annex
