using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BeeSSH.API_Utils.API_Cache;

namespace BeeSSH.DiscordRPC
{
    internal class DiscordRPC_Loader
    {
        public DiscordRpcClient dcClient = new DiscordRpcClient(DiscordRPC_Config.DiscordID); // Create Discord RPC

        // Construktor
        public DiscordRPC_Loader()
        {
            dcClient.Initialize(); // Init this 
        }

        // Main Function
        public void _Main()
        {
            dcClient.SetPresence(new RichPresence()
            {
                Details = "BeeSSH",
                State = $"Bees are good for SSH?",
                Timestamps = Timestamps.Now,
                Assets = new Assets()
                {
                    LargeImageKey = "Open Source Project",
                    LargeImageText = "https://as.mba/"
                },
                Buttons = new Button[]
                {
                    new Button(){ Label = "BeeSSH on Github", Url = "https://github.com/sysfaker/Bee-SSH" },
                    new Button(){ Label = "BeeSSH Webpage", Url = "https://as.mba/" }
                }
            });
        }


        public void _Invoke()
        {
            dcClient.Invoke();
        }

        public void _Discpose()
        {
            dcClient.Dispose();
        }
    }
}
