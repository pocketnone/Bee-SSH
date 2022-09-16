using System;
using System.IO;
using System.Net;
using BeeSSH.Core.Autosave.INI;

namespace BeeSSH.Core.Autosave
{
    public static class ConfigLoader
    {
        public static bool Discord { get; set; }
        public static bool AutoLogin { get; set; }
        public static bool DebugLogs { get; set; }


        public static void InizialConfig()
        {
            PathToINI();
        }

        private static string PathToINI()
        {
            var thePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                          "/beeshh/";
            if (!Directory.Exists(thePath))
                Directory.CreateDirectory(thePath);

            if (!File.Exists(thePath + "settings.ini"))
            {
                var ini = new IniFile(thePath + "settings.ini");
                ini.Write("AutoLogin", "false");
                ini.Write("DiscordRPC", "true");
                ini.Write("DebugLogs", "false");
            }

            var _ini = new IniFile(thePath + "settings.ini");
            Discord = bool.Parse(_ini.Read("DiscordRPC"));
            AutoLogin = bool.Parse(_ini.Read("AutoLogin"));
            DebugLogs = bool.Parse(_ini.Read("DebugLogs"));

            return thePath + "settings.ini";
        }
    }
}