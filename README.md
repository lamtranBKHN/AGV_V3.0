# AGV_V3.0 Warehouse system Solution

<br />

## Introduction

AGV_V3.0 is a warehouse solution and logistics. It provides applications and tools for warehouse transportation and supervision.

## AGV_V3.0 system structure
<!-- ![system_structure](https://i.imgur.com/muqNsgX.pnghttps://i.imgur.com/muqNsgX.png) -->

<p align="center">
  <img src="https://i.imgur.com/wmu5PqV.png" />
</p>
<p align="center">
    <em>System sequence diagram</em>
</p>

### 1. Dashboard

* **Live data dashboard**
* **User interface** which is interactions between users and AGV application
* **Monitor application**

![Alt Text](https://lh3.googleusercontent.com/RLRewLKiSdSvGHaeaIwW7mcLCzi1ScSVIRKC8ZzjMiucbOYyRy8EByWT9b-QZ3siB5_Rmq3Feyao954bDppguCMAOMQH4ss7i7rD7rpRHQB6jdkdVuiWLICCM0SVCuybOdw_hh9PrbAtsllnP1dG-nBKuNEXHC7WwrINjKidRt2qDDL-pOsrHZOfrUHioDqwQAziCEeZcDT8cX47xSOkmtO6LvT4t01t8Mik2sjKLunMt_B8kND8Gdwh6lH1F47oLjD1Zdj03uKalJNn2CVKlnnR4tqpRpBz3jp0PqSXkJtTuNnPDIoRF5Jzx8V3ooDLP8jKyikGSTWcoFPk8AfK0VvWfstMrDgd0oi0ZaE37ssBY-VvdwTOhdrkJtmGGG4rvQR3_og6p-pWQJ-q7S2aamxzBqBHWuh4FSx1pfiE6w7JgX6Y_vjWqKkgR-YSK-YejlAGTcYFa8hEMKgwZcydoC-MtNW9TzOlhFFocr9kYDOfTaclo47haCe3ZXfQ1EliNobJUxHxb-RnOPUcAQNu85633wwZtzovG7RUmLvcZK48ffKvwFbZxzaFYM-8Bp71iK3f48EFoU-2HUK4EH-H9FnG5x1eN6I5KfCBo4Ik9k8VgX75WMjR8gzz9XxxM26enhOO6YcNH_hd8Uz-JSBaZ_mUonhBR09P66SLdbzuy61nZEZYSvWqI5jXehOYTO2PPpC9I7Lr7G95EnBtJhzomc7fHA=w640-h480-no?authuser=0)

[![Watch the video](https://i.imgur.com/VYz3cjG.png)](https://youtu.be/d8McS9fE4w4)

### 2. Controller and planning module

* **Center application controller** - Handle: Cargo, goods moving request from user; received AGV status; update dashboard; reading and update database, ...
* **Logging** - Write log to file for tracing information
* **Nearest AGV finding algorithm** - Find nearest available AGV
* **Best route finding algorithm** - Find best AGV moving way
* **Reading and update database module**
* **AGV received update status command handler**
* **Dashboard update module**
* **MQTT connector**
* **HEX command with parity check bit creating module**

<p align="center">
  <img src="https://i.imgur.com/EpHNAb7.png" />
</p>
<p align="center">
    <em>Controlling and planning sequence diagram</em>
</p>

<br/>

<p align="center">
  <img src="https://i.imgur.com/MS0Opxu.png" />
</p>
<p align="center">
    <em>Planning process</em>
</p>

<br/>

<p align="center">
  <img src="https://i.imgur.com/yvP3bC2.png" />
</p>
<p align="center">
    <em>Planning console application</em>
</p>

### 3. AGV

**AGV** is based on a 2 wheel drive platform with:

* **Stepper** motor for moving and rotating accurately
* **RFID tag** for positioning
* **STM32** with **FreeRTOS**
* **ESP8266** for wireless communication
* **ESP32Cam** for live view and object tracking
* **Sonar and proximity sensor** for checking safety

<br/>

<p align="center">
  <img src="https://i.imgur.com/0dLrHpk.png" />
</p>
<p align="center">
    <em>AGV</em>
</p>

[AGV video here](https://youtu.be/pZySXu92PTg)

#### 3.1. FreeRTOS in AGV

<p align="center">
  <img src="https://i.imgur.com/poCubnd.png" />
</p>
<p align="center">
    <em>RTOS on AGV</em>
</p>


#### 3.2. Hardware

* STM32F407VET6 ARM Cortex-M4 | FreeRTOS
* ESP32Cam
* ESP8266
* HC-SR04
* A4988 Stepper motor driver
* DC DC Buck converter
* Proximity sensors
* 3S lithium battery
* Human sensor

### 4. Object tracking system 

![Alt Text](https://media.giphy.com/media/vFKqnCdLPNOKc/giphy.gif)

### 5. MQTT broker

[MQTT broker](https://en.wikipedia.org/wiki/MQTT) is a **server that receives all messages from the clients and then routes the messages to the appropriate destination clients.**

<p align="center">
  <img src="https://smartfactoryvn.com/wp-content/uploads/2018/11/mqtt_broker.png" />
</p>
<p align="center">
    <em>MQTT broker</em>
</p>

## 6. Database

* [MariaDB](https://en.wikipedia.org/wiki/MariaDB) Offers Tighter **Security** Measures
* **Performance** Is Faster and More **Efficient**
* Get Access to **Better User Support**

<p align="center">
  <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQzDnzmrJgWJdRFUiSfoCleZeE4mtkmSsWb6APkMmkL8Nac_jCHVDBuw25sbvYnPnsSCIQ&usqp=CAU" />
</p>
<p align="center">
    <em>MariaDB</em>
</p>

### 7. Author

***

<div>
<h3 align="left">Languages and Tools:</h3>
<p align="left"> <a href="https://www.arduino.cc/" target="_blank"> <img src="https://cdn.worldvectorlogo.com/logos/arduino-1.svg" alt="arduino" width="40" height="40"/> </a> <a href="https://www.cprogramming.com/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/c/c-original.svg" alt="c" width="40" height="40"/> </a> <a href="https://www.w3schools.com/cpp/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/cplusplus/cplusplus-original.svg" alt="cplusplus" width="40" height="40"/> </a> <a href="https://www.w3schools.com/cs/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="csharp" width="40" height="40"/> </a> <a href="https://dotnet.microsoft.com/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dot-net/dot-net-original-wordmark.svg" alt="dotnet" width="40" height="40"/> </a> <a href="https://git-scm.com/" target="_blank"> <img src="https://www.vectorlogo.zone/logos/git-scm/git-scm-icon.svg" alt="git" width="40" height="40"/> </a> <a href="https://www.w3.org/html/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/html5/html5-original-wordmark.svg" alt="html5" width="40" height="40"/> </a> <a href="https://developer.mozilla.org/en-US/docs/Web/JavaScript" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/javascript/javascript-original.svg" alt="javascript" width="40" height="40"/> </a> <a href="https://www.mysql.com/" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/mysql/mysql-original-wordmark.svg" alt="mysql" width="40" height="40"/> </a> <a href="https://nodejs.org" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/nodejs/nodejs-original-wordmark.svg" alt="nodejs" width="40" height="40"/> </a> <a href="https://opencv.org/" target="_blank"> <img src="https://www.vectorlogo.zone/logos/opencv/opencv-icon.svg" alt="opencv" width="40" height="40"/> </a> <a href="https://www.python.org" target="_blank"> <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/python/python-original.svg" alt="python" width="40" height="40"/> </a> <a href="https://www.rabbitmq.com" target="_blank"> <img src="https://www.vectorlogo.zone/logos/rabbitmq/rabbitmq-icon.svg" alt="rabbitMQ" width="40" height="40"/> </a> </p>
</div>
