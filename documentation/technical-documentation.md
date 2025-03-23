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
- **Risk due to material wear**: Over time, or when improperly treated with chemicals, e.g. cleaning agents, materials can wear, become brittle, get sticky or in other ways loose their intended properties. All materials used in the Product have been evaluated to that regard. Raspberry Pi and touch screen are unlikely to show wear over time. Data loss may occur, due to wear of the micro-SD-card. This is discussed separately below. The protective case may show mechanical wear in form of scratches, which do not pose a risk to End User or interfere with the function of the Product. Should the protective case become chipped, thereby exposing sharp edges, the User Manual instructs the End User to return the Product for repair or dispose of it. Cables may wear due to the insulations becoming brittle or mechanical stresses (kinks, pinching), exposing the conductors, potentially causing short circuits. The User Manual instructs the End User to check all cables for damages before each use and dispose any damaged cable immediately.
- **Risk of data loss**: The Product is intended for recording data from medical devices for up to 3 days and has been tested for that time period. It is not intended to store the recorded data in the long term. Due to software errors or the micro-SD storage card becoming faulty, data may get lost. The User Manual instructs the End User to copy recorded data to a USB drive using the web interface, move the data to a different storage medium and check that all data has been secured. This should be done after every recording session.
- **Risk of microbial contamination of patient environment**: The User Manual warns the End User not to place the Product inside the patients perimeter, i.e. keep the Product outside of 1.5 m of the patient bed and any surfaces that are used by any patient or used for storing or preparing materials meant for entering the patient perimeter. The Product is not suitable to be disinfected. It must be kept clean of any visible contamination using a dry cloth.

### Risks due to Effects on Environment
The following risks and their mitigation strategies have been identified:
- **Risk of fire**: Similar to the evaluation for the "Risk of burns", the limited heating of the components, with an expected maximum temperature of 90°C, makes it unlikely that the Product will catch fire, melt or begin to smoke. The more likely scenario is a ground loop, when connecting the Product to another device with an electrical fault. This risk is mitigated by following the same strategies as oulined in "Risk of electrical shock", i.e. using medical isolators or consulting with a local electrician before connecting to more than one medical device.
- **Risk of fire or explosion when operated near an oxygen-outlet**: The Product must not be operated within 1.5 m of any outlet of medical gases, especially oxygen or any environment with increased oxygen saturation. This is clearly stated in the User Manual and End Users verbally informed of this restriction upon delivery of the Product.
- **Risk of electric interference with medical devices**: All radio equipment of the Product is turned off during production. The End User is informed to keep all radio equipment turned off. This is also stated in the User Manual. The User Manual also outlines strategies for identifying, whether the Product is the cause of the interference. If the Product is identified as the source of interference, it is to be shut down, disconnected and removed from the vicinity of the effected medical devices.
- **Risk of interference with medical device function**: The data interfaces used to communicate with the supported medical devices are designed by the medical device manufacturers for the exact purpose of data export. Only interfaces are used, that are specifically designed for data export and only if they are NOT capable of altering or in any other way interfere with the function of the medical device. The communication is strictly done as outlined in the respective data interface programming manuals. Furthermore data communication has been tested in an isolated test environment to ensure correct implementation of the communication protocols and detecting any interference with medical device function.
  - **Philips Intellivue**: The "Data Export Interface Programmning Guide" published in Germany 09/2024 (ID: 453665127791) from Philips has been obtained and communication protocol implemented fully and strictly adhering to all restrictions, notes and warnings outlined in the guide, especially with regard to not overload the monitor with requests and workload. A simulated Philips Intellivue patient monitor has been implemented and communication tested during development with that simulated patient monitor. Next, the communication has been tested on a test Philips Intellivue MX800 patient monitor with software revision R, borrowed from University Hospital Heidelberg. During testing the monitor was observed for any signs of interference or slow down. No interference or slow down was detected. Finally data communication was tested on a Philips Intellivue MX800 connected to a stable patient and the monitor observed for 30 min. for any sign of interference. No interference was detected and navigation and usage of the monitor was not impacted in any perceiavable way.
  - **B. Braun Space Infusion System**: The "Technical Interface Specification BCC" Version 6.0, Document No. I0022_000030, Date 2022-03-03 was obtained from B. Braun Meisungen AG Hospital Care Division. The BCC protocol was implemented to a degree where the parameters could be extracted as quadruples with string values. No interpretation of the values was implemented. All restrictions, notes and warnings stated in the BCC Specification were respected and taken into account. A simulated B. Braun Infusion System was implemented and used during development to test the communication protocol. Next, communication was tested using a single SpaceStation rack frame with a SpaceCom communication module, a SpaceCover and 2 Perfusor Space infusion pumps, updated to software version 688N, borrowed from University Hospital Heidelberg, were used for testing communication. The infusion pumps and the alarm indicators of the SpaceCover were observed for 30 min. during continuous use of the infusion pumps. No impact on the infusion pumps or SpaceCover were identified. To reduce the workload on the pumps further, a poll interval of 10 seconds was selected. The pumps support polling with 500 ms between the end of the last result and the next poll request.
- **Risk of environmental poluation**: The Product poses a risk to the environment, if improperly disposed. The End User is notified by WEEE marking on the Product and instructions in the User Manual to responsibly dispose of the product at its end of life. The User Manual recommends the End User to return the Product to Manufacturer or hand it over to an organization specialized in disposing of electronic waste.

### Safety Critical Components
The power supply has been identified as a safety critical component, because it is connected to main power.

Other components are deemed low risk components, because they operate at low voltages and do not pose critical risks to the End User, environment or any connected equipment in terms or electrical, mechanical, thermal, chemical or radiation safety.

### Independent Conformity Assessment
TODO: Document, why a third-party conformity assessment is not necessary

## Testing procedures

## Harmonized Standards Compliance Evaluation
### Relevant Standards, Regulations and Directives
The following regulations and harmonized standards frameworks are considered:
- General Product Safety Regulation (GPSR)
  - EN 62368-1 (conformity declared for: PSU, Raspberry Pi 4B)
  - EN 60950-1:2006 (conformity declared for: Raspberry Pi 4B)
  - BS EN 62311: 2008 (conformity declared for: Raspberry Pi 4B)
- Electromagnetic Compatibility (EMC)
  - EN 55011:2009
  - EN 55011:2016
  - EN 55022:2010 + AC:2011 (conformity declared for: PSU)
  - EN 55024:2010 (conformity declared for: PSU)
  - EN 55032:2015 (conformity declared for: PSU)
  - EN 55035:2017
  - EN 61000-3-2:2014
  - EN 61000-3-3:2013
  - EN 61204-3:2000
  - ETSI EN 301 489-1 V2.2.3: 2019 (conformity declared for: Raspberry Pi 4B)
  - ETSI EN 301 489-17 V3.1.1: 2017 (conformity declared for: Raspberry Pi 4B)
- Low Voltage Directive (LVD)
  - EN 62368-1:2014+A11:2017 (conformity declared for: PSU)
- Radio Equipment Directive (RED)
  - ETSI EN 300 328 V2.2.2: 2019 (conformity declared for: Raspberry Pi 4B)
  - ETSI EN 301 893 V2.1.1: 2017 (conformity declared for: Raspberry Pi 4B)
- Restriction of the use of certain hazardous substances (RoHS)
  - IEC EN 63000: 2018 (conformity declared for: PSU, Raspberry Pi 4B)

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
The Product enabled the End User to record data from supported Medical Devices. Currently the following medical devices are supported:
- Philips Intellivue MX-series patient monitors
- B. Braun Space infusion pump system

The Product is using a general computer to offer these features. To prevent the End User from using any unintended features of that computer, the web interface, used to control the Product, is started in kiosk mode, which requires a physical keyboard to be connected to leave that mode. The User Manual warns the End User not to use any other features of the computer and not change any settings.

TODO: Create a new user account without sudo or any other administrator rights, which is logged in at startup.


#### Effect on other Products


#### Information to End User
End Users will receive a User Manual with the Product and will be instructed by a removable sticker on the Product to read the User Manual carefully before using the Product for the first time.

#### Enclosure and Markings
The Product is enclosed by a protective case from Waveshare International Limited made of plastic (TODO: Specify which plastic, when answer from Waveshare is received). It is specifically made for use with Raspberry Pi Model 4B and the 4.3" DSI touch display. It ensures exact alignment of the components with the case and minimal cracks between display and case. All openings are small enough that no fingers can be lodged inside them. No sharp edges, corners or protrusions are present, making mechanical injury unlikely. Because it is plastic all way through, the case itself has no surfaces that could become hot to the touch. It has no moving part, that could pinch the End User. The screws at the bottom, securing the electronics to the case, are countersunk in the case, such that the End User doesn't get in contact with them.

A label is attached to the left side of the case containing
- product name
- Revision number
- serial number
- manufacturer name and address
- Symbols
  - CE marking
  - WEEE marking

#### Cybersecurity
#### Continuous Product Safety Monitoring and Recall Procedures 
The Product is sold directly to End Users and a valid email address and permission to contact the End User while they use the Product is demanded before completing a sale. That email is used for follow-up with the End User, to get feedback and be made aware of any issues with the Product.

When any safety issues are identified, all End Users are notified by email of that issue and the required action that needs to be taken.

If an issue is deemed critical, i.e. poses a risk of injury or death to the End User, other persons or animals or damage to the environment, the manufacturer may recall the product, if the issue cannot be addressed by the End User. In that case, all Products, including the accessories (power supply and cables), must be returned to the manufacturer. If feasible, Manufacturer will pick up the Product at the End Users location. If a pick-up is not feasible, End User will receive a shipping label that they can use free of charge to return the Product. If the issue cannot be appropriately addressed, End Users will receive a refund for the full cost of the Product.

##### Monitoring Recall of Materials Used
To become aware of any recalls or known issues with the components used in the Product, the manufacturers websites will be visited once every 3 month and searched for any recalls. Furthermore, a search engine will be used once every 3 month to search for ```manufacturer + product + "recall"```.

If an issue or recall of any of the used components is found, the relevancy and impact or that issue is evaluated by the Manufacturer and as soon as possible, but at most after 7 days, decide the actions necessary.

##### Safety Business Gateway Notification
Because a direct communication is maintained with all End Users, no notification will be made to the Safety Business Alert Gateway.

##### Product User Registry
The Manufacturer will maintain a database with current email addresses to all End Users.


### Electrical Safety
#### Interactions of Components
- TODO: Argument why conformity with regulations, standards and directives is maintained after assembling Raspberry Pi, touch screen and protective case.

### Environmental Safety
#### RoHS Conformity
RoHS conformity declarations are available for all used components, except Waveshare 4.3" DSI touch display (TODO conformity declaration has been requested).

#### End of Life Procedures
End Users are informed by the WEEE marking on the Product and in the User Manual to dispose of the Product by returning it to the Manufacturer or handing it to organizations specialized in handling electronic waste.

The End User is warned in the User Manual to erase patient and measurement data from the Product before disposing it.


## Manufacturing process
The Product is assembled and setup by Manufacturer at his [private address](#manufacturer). Components are sourced from https://reichelt.de and Waveshare's Amazon shop. The assembly and installation process is specified in the [GitHub repository](https://github.com/mindleaving/patient-monitor-datalogger).

### Quality Assurance Procedures for each produced datalogger
Each Product is tested at the end of the assembly and installation process by
- Create a log session with
  - Device Type: Patient Monitor
  - Monitor Type: Simulated Philips Intellivue
  - Recorded parameters: All
  - CSV separator: Semicolon (;)
- Start log session
- Confirm that numerics data is shown and updated once per second
- Stop log session
- Confirm that no new numerics data is shown
- Insert USB drive
- Select "Copy to USB drive"
- Select USB drive and click "Copy"
- Eject USB drive
- Inspect data on USB drive on another computer and confirm that all expected data has been generated
- Reboot the Product
- Confirm that web interface is shown when startup has finished
- Delete log session

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
