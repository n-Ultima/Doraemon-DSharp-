using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;

namespace Doraemon.Objects
{
    public class Mute
    {
        public DiscordGuild Guild;
        public DiscordMember User;
        public DiscordRole Role;
        public DateTime End;
    }
}
