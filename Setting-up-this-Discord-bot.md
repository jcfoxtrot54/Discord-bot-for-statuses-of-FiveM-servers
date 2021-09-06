# How to setup the Discord bot
## Prerequisites
- A Discord account
- A Discord guild (server) that you have administrative access on
- A Windows installation
- https://aka.ms/dotnet-core-applaunch?framework=Microsoft.NETCore.App&framework_version=3.1.0&arch=x64&rid=win10-x64

## Step 1 - register your bot
- go to https://discord.com/developers/applications
- login/make a Discord account
- create an application (press "New Application" button)
- enter name of bot (it's shown to everyone, choose wisely)
- open bot tab on left, then press "Add Bot"

## Step 2 - add bot to your server
- copy your bot's client ID into this string and go to it in your web browser - you can get your client ID from the bot's "General Information" tab (https://discord.com/oauth2/authorize?client_id=CLIENTIDGOESHERE&scope=bot)

## Step 3 - configure the bot
- enable Developer Mode in Discord (https://www.discordtips.com/how-to-enable-developer-mode-in-discord/)
- set `botToken` to your bot's token (you can get this from the bot's "Bot" tab by clicking "Copy" under "Token")
- copy the ID of the guild into `guildID` (right click guild icon and press `Copy ID`)
- copy the ID of the channel into `channelID` (right click channel tab and press `Copy ID`)
- change the title and subtitle of the embed using `embedTitle` and `embedSubtitle`
- if you need to, you can change `embedUpdate` (this value is in seconds!). This changes the delay in which each server is checked. I found 10 seconds is a good enough value but change at your own peril.

- copying the template provided in `configuration.json`, fill in the details of each server in the JSON array

- `externalIP` is used so people can opt to direct connect to the servers. Change it to your server's public IP
- `internalIP` is used to check the individual servers. Please use localhost or 127.0.0.1 here if you an.
- `port` is the port in which the FiveM server is hosted on
- `displayName` is the name the server will be displayed as on the embed

## NOTES
- all links in this document are accurate as of date of last update.
- feel free to submit a pr for anything if you think it could do with being improved
- I am by far an expert on C#, but I figured the best way to learn is to put things into practice
