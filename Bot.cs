using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Doraemon.Objects;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Doraemon.Modules;
using System.Linq;
using System.Collections.Generic;
using bot;
using Microsoft.Extensions.Configuration;
namespace Doraemon
{
    public class Bot
    {
        public static DiscordClient Client { get; set; }
        public static CommandsNextExtension Commands { get; set; }
        public static List<Mute> Mutes = new List<Mute>();
        public async Task RunAsync()
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("C:/Users/schsesports/source/repos/DSharpBot/DSharpBot/config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            var config = new DiscordConfiguration
            {
                Token = configJson.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
            };
            Client = new DiscordClient(config);
            Client.Ready += OnReady;
            Client.MessageCreated += OnMessageReceived;
            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                DmHelp = true,
            };
            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<FunCommands>();
            Commands.RegisterCommands<ModerationModule>();
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
        private static async Task OnMessageReceived(DiscordClient sender, MessageCreateEventArgs e)
        {
            string[] badWord = am();
            foreach (string word in am())
            {
                if (e.Message.Content.ToLower().Split(" ").Intersect(badWord).Any())
                {
                    await e.Message.DeleteAsync();
                }
            }
        }

        private async Task OnReady(DiscordClient sender, ReadyEventArgs e)
        {
            var guild = Client.GetGuildAsync(826243808710098954);
            if (File.Exists(data.save.system.path))
            {
                data.save.system.load();
            }
            Task.Run(async () => await MuteHandler().ConfigureAwait(false));
        }
        private async Task MuteHandler()
        {
            List<Mute> Remove = new List<Mute>();
            foreach(var mute in Mutes)
            {
                if(DateTime.Now < mute.End)
                {
                    continue;
                }
                var guild = await Client.GetGuildAsync(826243808710098954).ConfigureAwait(false);
                if(guild.GetRole(mute.Role.Id) is null)
                {
                    Remove.Add(mute);
                    continue;
                }
                if(guild.GetMemberAsync(mute.User.Id) is null)
                {
                    Remove.Add(mute);
                    continue;
                }
                var role = guild.GetRole(mute.Role.Id);
                var user = await guild.GetMemberAsync(mute.User.Id);
                if(role.Position > guild.CurrentMember.Hierarchy)
                {
                    Remove.Add(mute);
                    continue;
                }
                await user.RevokeRoleAsync(role);
                Remove.Add(mute);
            }
            Mutes = Mutes.Except(Remove).ToList();
            await Task.Delay(1000);
            await MuteHandler();
        }
        public static string[] am()
        {
            string[] returned = new string[26];
            returned[0] = "nigger";
            returned[1] = "nigga";
            returned[2] = "queer";
            returned[3] = "faggot";
            returned[4] = "cunt";
            returned[5] = "ni66er";
            returned[6] = "niqquer";
            returned[7] = "nigeria";
            returned[8] = "n-i-g-g-e-r";
            returned[9] = "negro";
            returned[10] = "fag";
            returned[11] = "fa66ot";
            returned[12] = "f@660t";
            returned[17] = "cum";
            returned[18] = "dick";
            returned[19] = "tits";
            returned[20] = "tit";
            returned[21] = "titties";
            returned[22] = "dicks";
            return returned;
        }
    }
}
