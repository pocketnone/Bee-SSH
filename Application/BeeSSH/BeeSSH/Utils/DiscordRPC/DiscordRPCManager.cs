namespace BeeSSH.Utils.DiscordRPC
{
    internal class DiscordRPCManager
    {
        private static BeeSSH.Utils.DiscordRPC.DiscordRPCLoader _DiscordRPCLoader = new DiscordRPCLoader();


        internal static void LoginView()
        {
            _DiscordRPCLoader.UpdateDetails("In Login");
            _DiscordRPCLoader.UpdateState("Is logging in");
        }

        #region Dashboard
        internal static void MainView()
        {
            _DiscordRPCLoader.UpdateDetails("In Dashboard");
            _DiscordRPCLoader.UpdateState("Look at the Dashboard");
        }
        internal static void AddServer_MainView()
        {
            _DiscordRPCLoader.UpdateState("Is adding a Server");
        }

        internal static void RSAKE_MainView()
        {
            _DiscordRPCLoader.UpdateState("Is generating a RSA Key");
        }

        internal static void Settings_MainView()
        {
            _DiscordRPCLoader.UpdateState("Changing some Settings");
        }

        internal static void Terminal_MainView()
        {
            _DiscordRPCLoader.UpdateState("In the Terminal");
        }

        #endregion Dashboard


    }
}
