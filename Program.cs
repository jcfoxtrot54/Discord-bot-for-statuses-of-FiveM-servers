using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace StatusSharp
{
    class Program
    {
        Configuration config = JsonConvert.DeserializeObject<Configuration>(System.IO.File.ReadAllText("configuration.json"));
        private readonly DiscordSocketClient _client;
        private SocketGuild guildObject;
        private ITextChannel channelObject;
        private IUserMessage messageObject;
        static void Main()
        {
            new Program().primaryThread().GetAwaiter().GetResult();
        }
        public Program()
        {
            _client = new DiscordSocketClient();
            _client.Log += onLog;
            _client.Ready += onReady;
        }
        public async Task primaryThread()
        {
            await _client.LoginAsync(TokenType.Bot, config.botToken);
            await _client.StartAsync();
            await Task.Delay(Timeout.Infinite);
        }

        public Task onLog(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        public Task onReady()
        {
            Console.WriteLine($"Connected as: {_client.CurrentUser.ToString()}");
            serverStatus();
            return Task.CompletedTask;
        }
        public async Task serverStatus()
        {
            guildObject = _client.GetGuild(config.guildID);
            channelObject = guildObject.GetTextChannel(config.channelID);
            if (channelObject is null)
            {
                Console.WriteLine("Error: channel object null!");
                return;
            }
            await purgeChannel(channelObject);
            messageObject = await channelObject.SendMessageAsync("Initialising...");
            await Task.Delay(500);

            

            while (true)
            {
                Console.WriteLine("Editing...");
                await messageObject.ModifyAsync(async x => x.Embed = await GenerateEmbed());
                await messageObject.ModifyAsync(x => x.Content="");
                await Task.Delay(config.embedUpdate* 1000);
            };
        }
            
        public Task<Embed> GenerateEmbed()
        {
            int itemCounter = 0;
            bool needsSpacer = false;
            EmbedBuilder embed = new EmbedBuilder();
            embed.Title = config.embedTitle;
            embed.Color = new Discord.Color((uint)config.embedColour);
            embed.Description = $"**This status was last updated {DateTime.Now.ToString("F") + " " + DateTime.Now.ToString("UTC K")}**\n"+ config.embedSubtitle;
            embed.Footer = new EmbedFooterBuilder() { Text= "https://github.com/jcfoxtrot54/FiveM-Discord-Bot (statuses updates every 10 seconds)", IconUrl="http://aftermathgw.com/aftermathstripe.png" };
            HttpClient webClient = new HttpClient() { Timeout=TimeSpan.FromSeconds(2)};
            config.serverList.ForEach(serverObj =>
            {

                if (needsSpacer)
                {
                    needsSpacer = false;
                    embed.AddField(
                        "\u200b",
                        "\u200b",
                        false
                    );
                }
                itemCounter++;
                string playerIndexResult="";
                string infoResult = "";
                try
                {
                    playerIndexResult = webClient.GetStringAsync($"http://{serverObj.internalIP}:{serverObj.port}/players.json").Result;
                    infoResult = webClient.GetStringAsync($"http://{serverObj.internalIP}:{serverObj.port}/info.json").Result;
                }
                catch (Exception)
                {
                    
                }
                
                if (playerIndexResult == "")
                {
                    embed.AddField(
                        serverObj.displayName,
                        ":red_circle: Offline",
                        true
                    );
                    Console.WriteLine($"{serverObj.displayName}: offline");
                }
                else
                {
                    Newtonsoft.Json.Linq.JArray playerCount = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(playerIndexResult);
                    JObject info = JObject.Parse(infoResult);

                    embed.AddField(
                        serverObj.displayName,
                        $":green_circle: {playerCount.Count}/{info["vars"]["sv_maxClients"]} players\n\nDirect connect via console (F8) \n`connect {serverObj.externalIP}:{serverObj.port}`",
                        true
                    );
                    Console.WriteLine($"{serverObj.displayName}: online");

                    
                }
                if (itemCounter == 2)
                {
                    itemCounter = 0;
                    needsSpacer = true;
                }
            });
            return Task.FromResult(embed.Build());
        }
        public async Task purgeChannel(ITextChannel channelObj)
        {
            var channelMessages = await channelObj.GetMessagesAsync().FlattenAsync();
            await channelObj.DeleteMessagesAsync(channelMessages);
        }
    }

    class Server { 
        public string externalIP;
        public string internalIP;
        public string port;
        public string displayName;
        public Server(string externalIP, string internalIP, string port, string displayName)
        {
            this.externalIP = externalIP;
            this.internalIP = internalIP;
            this.port = port;
            this.displayName = displayName;
        }
    }
}