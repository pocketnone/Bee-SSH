
# Bee-SSH SSH Client

<p align="center">
  <img src="https://github.com/sysfaker/Bee-SSH/blob/Indev/Branding/b3.png" style="max-height: 50%">
</p>

----

## Project Infos


<p align="center">
  <img alt="GitHub code size in bytes" src="https://img.shields.io/github/languages/code-size/sysfaker/Bee-SSH?style=for-the-badge">
  <img alt="GitHub release (latest by date including pre-releases)" src="https://img.shields.io/github/v/release/sysfaker/Bee-SSH?include_prereleases&style=for-the-badge">
  <img alt="GitHub all releases" src="https://img.shields.io/github/downloads/sysfaker/Bee-SSH/total?color=%230099cc&style=for-the-badge">
</p>

## About


Bee SSH is a Open-Source SSH Client similar like [Putty](https://www.putty.org/) and [FileZilla](https://filezilla-project.org/). <br>
Bee SSH can be used over the Internet to store its servers and custom user scripts on a central platform and work independently of the computer. <br>
Since Bee SSH is open source you can easily host your own system. <br>


### Features

- SSH Connection
- sFTP
- FTP
- User System to store Server and custom Userscripts

## Webpage Security

- Password Hash, Salt and Pepper (Random 55 long string behind every password)
- 2FA Added
- hCaptcha
- Ratelimit on API (Fetch Cloudflare IP. Need to be removed if you dont use Cloudflare)
- [Helmet](https://helmetjs.github.io/)

## Donations


You are welcome to support BeeSSH public website and service. The donations will be spent on the server and the domain, everything else will be donated to animal welfare organizations (With focus on bees and insects).  


## Installation Webpage


#### Preparation

1. Install Git 

```bash
$ sudo apt install git wget -y
```
2. Install NodeJS

- LTS Node Version

```bash
$ sudo apt install nodejs npm -y
```
- Other Versions: [Install Guide](https://techviewleo.com/how-to-install-node-js-18-lts-on-ubuntu/)
    
3Install MongoDB (If you want to host it locally)

- [Mongo DB Install Guide](https://www.mongodb.com/docs/manual/installation/)

#### Install Webpage

```bash
$ mkdir /opt/BeeSSH
$ cd /opt/BeeSSH
$ git clone https://github.com/sysfaker/Bee-SSH/tree/main
```

Rename .env.Example to .env and past any missing values

#### Start Webpage

```bash
$ npm app.js
```

- You can use `pm2` to let the App run forever.

#### API Documentation

Coming Soon...
