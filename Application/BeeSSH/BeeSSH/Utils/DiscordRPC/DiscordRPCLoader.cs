using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;

namespace BeeSSH.Utils.DiscordRPC
{
    internal class DiscordRPCLoader : IDisposable
    {
        internal DiscordRpcClient _client = new DiscordRpcClient(_clientID);
        internal static string _clientID { get; set; }

        public DiscordRPCLoader(string _ClientID)
        {
            _clientID = _ClientID;
            _client.Initialize();
            _client.SetPresence(new RichPresence()
            {
                Details = "Bee-SSH Client",
                State = "https://as.mba",
                Timestamps = Timestamps.Now,
                Assets = new Assets()
                {
                    LargeImageKey = "",
                    LargeImageText = "Bee-SSH Client",
                    SmallImageKey = "",
                    SmallImageText = ""
                },
                Buttons = new Button[]
                {
                    new Button(){ Label = "Download BeeSSH", Url = "https://as.mba/" },
                    new Button(){ Label = "Discord", Url = "" }
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

     

        ~DiscordRPCLoader() {
            Dispose();
        }

        public void Dispose()
        {
            _client.Dispose();
            GC.Collect();
        }
    }
}
