using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeSSH.DiscordRPC
{
    internal static class DiscordRPC_Manager
    {
        private static DiscordRPC_Loader drpcl = new DiscordRPC_Loader();
        internal static void RunDiscordRPC()
        {
            drpcl._Main();
        }

        internal static void Close()
        {
            drpcl._Discpose();
        }
    }
}
