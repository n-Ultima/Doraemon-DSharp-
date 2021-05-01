using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Doraemon.Modules
{
    public class FunCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Shows the latency between the client and the gateway.")]
        public async Task Ping(CommandContext ctx)
        {
            var latency = ctx.Client.Ping;
            await ctx.Channel.SendMessageAsync($"Pong!, Your ping is {latency} milliseconds!.").ConfigureAwait(false);
        }
    }
}
