using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Doraemon.Objects
{
    public class Ban
    {
        public DiscordGuild Guild;
        public DiscordUser User;
        public DateTime End;
    }
}
