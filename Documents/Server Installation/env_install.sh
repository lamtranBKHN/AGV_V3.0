#!/usr/bin/sh

# Update system
echo "Updating system"
sudo apt update
sudo apt install wget curl git g++ gcc

# Install nodejs
echo "Installing nodejs"
curl -sL https://deb.nodesource.com/setup_14.x | sudo bash -
cat /etc/apt/sources.list.d/nodesource.list
sudo apt -y install nodejs
node -v
npm -v
sudo npm install -g --unsafe-perm node-red

# Install mariadb
echo "Installing mariadb"
sudo apt update
sudo apt install mariadb-server
sudo mysql_secure_installation