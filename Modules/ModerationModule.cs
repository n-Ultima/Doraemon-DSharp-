using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus;
using DSharpPlus.Entities;
using Doraemon.Services;
using bot;
using static bot.serverdata;
using Doraemon.Objects;

namespace Doraemon.Modules
{
    public class ModerationModule : BaseCommandModule
    {
        public async Task<string> produceID(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            return finalString;
        }
        [Command("kick")]
        [RequireUserPermissions(Permissions.ManageMessages)]
        public async Task KickUserAsync(CommandContext ctx, DiscordMember user, [RemainingText]string reason)
        {
            if(!CheckInteraction.CanModerate(ctx.Member, user))
            {
                await ctx.Message.DeleteAsync().ConfigureAwait(false);
                return;
            }
            var caseId = await produceID(20);
            var e = new DiscordEmbedBuilder()
                .WithDescription($"{user.Mention} was **kicked** from the server with ID: {caseId}")
                .WithColor(DiscordColor.Green);
            await ctx.Channel.SendMessageAsync(embed: e.Build());
            await user.RemoveAsync(reason).ConfigureAwait(false);
        }
        [Command("warn")]
        [RequireUserPermissions(Permissions.ManageMessages)]
        public async Task WarnUserAsync(CommandContext ctx, DiscordMember user, [RemainingText]string reason)
        {
            if (!CheckInteraction.CanModerate(ctx.Member, user))
            {
                await ctx.Message.DeleteAsync();
                return;
            }
            var caseId = await produceID(20);
            await user.CreateDmChannelAsync();
            var l = new DiscordEmbedBuilder()
                .WithTitle("You were warned.")
                .WithColor(DiscordColor.Gold)
                .WithDescription($"You were warned in **{ctx.Guild.Name}**\n\nYou were warned for reason: {reason}\nID `{caseId}`.\n**Moderator** | Hidden");
            try
            {
                await user.CreateDmChannelAsync();
                await user.SendMessageAsync(embed: l.Build());
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to Dm user.");
            }
            var k = new DiscordEmbedBuilder()
                .WithDescription($"{user.Mention} was **warned** with ID: `{caseId}`.\n**{user.Mention}**, check your DM's for information.")
                .WithColor(DiscordColor.Green);
            await ctx.Channel.SendMessageAsync(embed: k.Build());
            // If the user has no warns, we make a new user.
            if (data.serverdata.warnedusers is null)
            {
                data.serverdata.warnedusers = new warneduser[1] { new warneduser { userId = user.Id, reasons = new string[] { reason }, caseId = new string[] { caseId } } };
            }
            // If the user exists in the file, then we execute the command in the if statement.
            else
            {
                // Set the done variable, which defines if the user has been put into the file.
                bool done = false;
                // For each warned usr, we set their old reasons.
                foreach (warneduser u in data.serverdata.warnedusers)
                {
                    // If the warned user is equal to the warned user's ID
                    if (u.userId == user.Id)
                    {
                        // Set the old reasons
                        string[] oldReasons = u.reasons;
                        u.reasons = new string[oldReasons.Length + 1];
                        // Set the reasons to be old reasons
                        for (int i = 0; i < oldReasons.Length; i++)
                        {
                            u.reasons[i] = oldReasons[i];
                        }
                        // Do the same thing, but with case id's
                        string[] oldcaseid = u.caseId;
                        u.caseId = new string[oldcaseid.Length + 1];
                        for (int i = 0; i < oldcaseid.Length; i++)
                        {
                            u.caseId[i] = oldcaseid[i];
                        }
                        u.caseId[oldcaseid.Length] = caseId;
                        u.reasons[oldReasons.Length] = reason;
                        // After adding them, we call this done.
                        done = true;
                        break;
                    }
                }
                // If done is false
                if (!done)
                {
                    // We get the previously warned users.
                    warneduser[] prevUsers = data.serverdata.warnedusers;
                    data.serverdata.warnedusers = new warneduser[prevUsers.Length + 1];
                    for (int i = 0; i < prevUsers.Length; i++)
                    {
                        data.serverdata.warnedusers[i] = prevUsers[i];
                    }
                    data.serverdata.warnedusers[prevUsers.Length] = new warneduser { userId = user.Id, reasons = new string[] { reason }, caseId = new string[] { caseId } };
                }
            }
            warneduser user2 = null;
            // Make sure that the previously warned user is not the same user
            foreach (warneduser user3 in data.serverdata.warnedusers)
            {
                if (user3.userId == user.Id)
                {
                    user2 = user3;
                }
            }
        }
        [Command("tempmute")]
        [RequireUserPermissions(Permissions.MuteMembers)]
        public async Task TempMuteMember(CommandContext ctx, DiscordMember user, string duration, [RemainingText]string reason)
        {
            if (user == ctx.Member)
            {
                return;
            }
            if (!CheckInteraction.CanModerate(ctx.Member, user))
            {
                await ctx.Message.DeleteAsync();
                return;
            }
            var role = ctx.Guild.GetRole(826243826946408498);
            var textChannels = await ctx.Guild.GetChannelsAsync();
            foreach (var channel in textChannels.Where(x => x.Type == ChannelType.Text))
            {
                if (!channel.PermissionOverwrites.Any(x => x.CheckPermission(Permissions.SendMessages) == PermissionLevel.Denied))
                {
                    await channel.AddOverwriteAsync(role, deny: Permissions.SendMessages);
                }
            }
            var voiceChannels = await ctx.Guild.GetChannelsAsync();
            foreach (var channel in voiceChannels.Where(x => x.Type == ChannelType.Voice))
            {
                if(!channel.PermissionOverwrites.Any(x => x.CheckPermission(Permissions.SendMessages) == PermissionLevel.Denied))
                {
                    await channel.AddOverwriteAsync(role, deny: Permissions.Speak);
                }
            }
            var caseId = await produceID(20);
            char minute = 'm';
            char day = 'd';
            char hour = 'h';
            char second = 's';
            char week = 'w';
            var MuteTimer = new string(duration.Where(char.IsDigit).ToArray());
            if (minute.ToString().Any(duration.Contains) && day.ToString().Any(duration.Contains) && hour.ToString().Any(duration.Contains) && second.ToString().Any(duration.Contains))
            {
                await ctx.Channel.SendMessageAsync("You cannot pass multiple time formats.");
                return;
            }
            if (MuteTimer.Length == 0)
            {
                return;
            }
            var Timer = Convert.ToInt32(MuteTimer);
            if (minute.ToString().Any(duration.ToLower().Contains))
            {
                Bot.Mutes.Add(new Mute { Guild = ctx.Guild, User = user, End = DateTime.Now + TimeSpan.FromMinutes(Timer), Role = role });
            }
            else if (day.ToString().Any(duration.ToLower().Contains))
            {
                Bot.Mutes.Add(new Mute { Guild = ctx.Guild, User = user, End = DateTime.Now + TimeSpan.FromDays(Timer), Role = role });
            }
            else if (second.ToString().Any(duration.ToLower().Contains))
            {
                Bot.Mutes.Add(new Mute { Guild = ctx.Guild, User = user, End = DateTime.Now + TimeSpan.FromSeconds(Timer), Role = role });
            }
            else if (hour.ToString().Any(duration.ToLower().Contains))
            {
                Bot.Mutes.Add(new Mute { Guild = ctx.Guild, User = user, End = DateTime.Now + TimeSpan.FromHours(Timer), Role = role });
            }
            else if (week.ToString().Any(duration.ToLower().Contains))
            {
                Bot.Mutes.Add(new Mute { Guild = ctx.Guild, User = user, End = DateTime.Now + TimeSpan.FromDays(Timer * 7), Role = role });
            }
            else
            {
                Console.WriteLine("");
            }
            await user.GrantRoleAsync(role);
            var e = new DiscordEmbedBuilder()
                .WithDescription($"{user.Username} was muted with ID: `{caseId}`\n**{user.Username}**, check your DM's for information.")
                .WithColor(DiscordColor.Green);
            var message = ctx.Channel.SendMessageAsync(embed: e.Build()).Result;
            await Task.Delay(5000);
            await message.DeleteAsync();
            var userEmbed = new DiscordEmbedBuilder()
                .WithTitle("You were muted")
                .WithColor(DiscordColor.Red)
                .WithDescription($"You were muted in {ctx.Guild.Name}\n\nYou were muted for reason: {reason}.\nCaseID: `{caseId}`\n\nMute Timer: {duration}");
            try
            {
                await user.CreateDmChannelAsync();
                await user.SendMessageAsync(embed: userEmbed.Build());
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to DM user.");
            }
        }
        [Command("tempban")]
        [RequireUserPermissions(Permissions.ManageMessages)]
        public async Task BanUserTempAsync(CommandContext ctx, DiscordMember user, string duration, [RemainingText] string reason)
        {

        }
    }
}
