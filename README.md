# Connectivity Checker
A tool to check connectivity from your machine to several providers. 

# Getting Started
The image is available directly in dockerhub preconfigured to work on Azure Web Apps: https://hub.docker.com/r/cjaliaga/connectivity-checker/

To use the image in a Web App for containers:

- Create a [Web App for containers](https://portal.azure.com/#create/microsoft.appsvclinux)  
- Configure your web app to custom image: cjaliaga/connectivity-checker:latest
- Configure the App Settings and Connection Strings to check the connectivity
- Browse your site

