namespace BeeSSH.Utils.DiscordRPC
{
    internal class DiscordRPCManager
    {
        private static BeeSSH.Utils.DiscordRPC.DiscordRPCLoader _DiscordRPCLoader = new DiscordRPCLoader();


        internal static void LoginView()
        {
            _DiscordRPCLoader.UpdateDetails("In Login");
            _DiscordRPCLoader.UpdateState("logging in");
        }

        #region Dashboard
        internal static void MainView()
        {
            _DiscordRPCLoader.UpdateDetails("In Welcome Screen");
            _DiscordRPCLoader.UpdateState("Looking at my Stats");
        }
        internal static void AddServer_MainView()
        {
            _DiscordRPCLoader.UpdateState("Adding a Server");
        }

        internal static void RSAKE_MainView()
        {
            _DiscordRPCLoader.UpdateState("Generating a RSA Key");
        }

        internal static void Settings_MainView()
        {
            _DiscordRPCLoader.UpdateState("Changing some Settings");
        }

        internal static void Terminal_MainView()
        {
            _DiscordRPCLoader.UpdateState("In the Terminal");
        }
        internal static void Connections()
        {
            _DiscordRPCLoader.UpdateState("Looking at my Connections");
        }

        #endregion Dashboard


    }
}
