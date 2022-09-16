using static BeeSSH.Core.Autosave.ConfigLoader;

namespace BeeSSH.Utils.DiscordRPC
{
    internal class DiscordRPCManager
    {
        private static DiscordRPCLoader _DiscordRPCLoader = new DiscordRPCLoader();


        internal static void LoginView()
        {
            if (Discord)
            {
                _DiscordRPCLoader.UpdateDetails("In Login");
                _DiscordRPCLoader.UpdateState("logging in");
            }
        }

        #region Dashboard

        internal static void MainView()
        {
            if (Discord)
            {
                _DiscordRPCLoader.UpdateDetails("BEESSH");
                _DiscordRPCLoader.UpdateState("Welcome Screen");
            }
        }

        internal static void AddServer_MainView()
        {
            if (Discord)
                _DiscordRPCLoader.UpdateState("Adding a Server");
        }

        internal static void RSAKE_MainView()
        {
            if (Discord)
                _DiscordRPCLoader.UpdateState("Generating a RSA Key");
        }

        internal static void Settings_MainView()
        {
            if (Discord)
                _DiscordRPCLoader.UpdateState("Changing some Settings");
        }

        internal static void Terminal_MainView()
        {
            if (Discord)
                _DiscordRPCLoader.UpdateState("In the Terminal");
        }

        internal static void Connections()
        {
            if (Discord)
                _DiscordRPCLoader.UpdateState("Looking at my Connections");
        }

        internal static void PandleServer(string Servername)
        {
            if (Discord)
                _DiscordRPCLoader.UpdateState($"Connected with {Servername}");
        }

        #endregion Dashboard
    }
}