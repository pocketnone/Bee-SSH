using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeSSH.Core.API
{
    internal class Cache
    {
        internal static string AuthCookieForAPI { get; set; }
        internal static string EncryptionMasterPass { get; set; }                           // Master Password for encryption
        internal static List<ServerListModel> ServerList = new List<ServerListModel>();     // All Servers in a list Uncrypted

        // API Endpoints 
        internal static string LoginAPIURL = "https://as.mba/api/client_login";             // Request a Login
                
        internal static string AddServerAPIURL = "https://as.mba/api/client_new";           // Add a new server to the Webapp

        internal static string FetchScriptsAPIURL = "https://as.mba/api/fetch_userscripte"; 

        internal static string AddScriptAPIURL = "https://as.mba/api/add_userscript";

        // Some stuff
        internal static string ClientAuthKey = "vnsjrgshvb48sbvrbiwbkb_kbkkbhkgberibg";
    }

    internal class ServerListModel
    {
        internal string ServerName           { get; set; }
        internal string ServerIP             { get; set; }
        internal string ServerPassword       { get; set; }
        internal string ServerPort           { get; set; }
        internal string ServerUserName       { get; set; }
        internal byte[] RSAKEY               { get; set; }
    }
}
