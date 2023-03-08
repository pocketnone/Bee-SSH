using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using static BeeSSH.Utils.DiscordRPC.DiscordRPCConfig;

namespace BeeSSH.Utils.DiscordRPC
{
    internal class DiscordRPCLoader : IDisposable
    {
        internal DiscordRpcClient _client = new DiscordRpcClient(DiscordID);
        internal static string _clientID { get; set; }

        public DiscordRPCLoader()
        {
            _client.Initialize();
            _client.SetPresence(new RichPresence()
            {
                Details = "Bee-SSH Client",
                State = "https://res.yt/",
                Timestamps = Timestamps.Now,
                Assets = new Assets()
                {
                    LargeImageKey = "logo",
                    LargeImageText = "Sum Sum SSH"
                },
                Buttons = new Button[]
                {
                    new Button() { Label = "Download BEESSH", Url = "https://res.yt/" },
                    new Button() { Label = "Discord", Url = "https://discord.gg/ChhDyjCQWK" }
                }
            });
        }

        //Update the Current Details from the SSH
        public void UpdateDetails(string State)
        {
            _client.UpdateDetails(State);
        }

        //Update the current State from the State
        public void UpdateState(string State)
        {
            _client.UpdateDetails(State);
        }


        ~DiscordRPCLoader()
        {
            Dispose();
        }

        public void Dispose()
        {
            _client.Dispose();
            GC.Collect();
        }
    }
}