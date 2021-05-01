using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
namespace Doraemon.Common
{
    public static class Embeds
    {
        public static async Task<DiscordMessage> SendModSuccessAsync(this DiscordChannel channel, string title, string description)//Sends the Success Message, but with the Mod Log footer.
        {
            var e = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Green)
                .WithTitle(title)
                .WithDescription(description)
                .WithFooter("Mod Log");
            var message = await channel.SendMessageAsync(embed: e.Build());
            return message;
        }
        public static async Task<DiscordMessage> SendNormalSuccessAsync(this DiscordChannel channel, string title, string description)
        {
            var e = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Green)
                .WithTitle(title)
                .WithDescription(description);
            var message = await channel.SendMessageAsync(embed: e.Build());
            return message;
        }
        public static async Task<DiscordMessage> SendErrorMessageAsync(this DiscordChannel channel, string title, string description)
        {
            var e = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Red)
                .WithTitle(title)
                .WithDescription(description);
            var message = await channel.SendMessageAsync(embed: e.Build());
            return message;
        }
        public static async Task<DiscordMessage> SendLogMessageAsync(this DiscordChannel channel, string title, string description)
        {
            var e = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Green)
                .WithTitle(title + " Log")
                .WithDescription(description);
            var message = await channel.SendMessageAsync(embed: e.Build());
            return message;

        }
        public static async Task<DiscordDmChannel> SendDMAsync(this DiscordDmChannel channel, string title, string description)
        {
            var e = new DiscordEmbedBuilder()
                .WithTitle(title)
                .WithColor(DiscordColor.Red)
                .WithDescription(description);
            var message = await channel.SendMessageAsync(embed: e.Build());
            return channel;
        }
        public static async Task<DiscordDmChannel> SendBanDMAsync(this DiscordDmChannel channel, string title, string description)
        {
            var e = new DiscordEmbedBuilder()
                .WithTitle(title)
                .WithColor(DiscordColor.Red)
                .WithDescription(description + " [here](https://forms.gle/PyxFAAKQ5W4GA8F5A)");
            var message = await channel.SendMessageAsync(embed: e.Build());
            return channel;
        }
        public static async Task<DiscordMessage> SendPublicLogMessageAsync(this DiscordChannel channel, string content)
        {
            var message = await channel.SendMessageAsync(content);
            return message;
        }
    }
}
