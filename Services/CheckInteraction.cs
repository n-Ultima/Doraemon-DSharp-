using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Doraemon.Services
{
    public class CheckInteraction
    {
        public static bool CanModerate(DiscordMember Moderator, DiscordMember user)
        {
            return Moderator.Hierarchy > user.Hierarchy;
        }
    }
}
