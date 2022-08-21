﻿using System.Collections.Generic;

namespace BeeSSH.Core.API
{
    internal class Cache
    {
        internal static string AuthCookieForAPI { get; set; }
        internal static string EncryptionMasterPass { get; set; }                           // Master Password for encryption
        internal static List<ServerListModel> ServerList = new List<ServerListModel>();     // All Servers in a list Uncrypted
        internal static List<ScriptModel> Scriptlist = new List<ScriptModel>();             // All Scripts

        
        internal struct APIEndPoint
        {

            #region Serverendpoints

            internal static string Login = "https://as.mba/api/client_login";             // Request a Login

            internal static string AddServer = "https://as.mba/api/client_new";           // Add a new server to the Webapp
            
            internal static string DeleteServer = "https://as.mba/api/client_delete";  
            
            internal static string UpdateServer = "https://as.mba/api/client_update";  

            #endregion
            

            #region ScriptEndpoints

            internal static string FetchScripts = "https://as.mba/api/fetch_userscripte";

            internal static string AddScript = "https://as.mba/api/add_userscript";

            internal static string DeleteScript = "https://as.mba/api/delete_userscripte";

            #endregion
        }
        // Some stuff
        internal static string ClientAuthKey = "vnsjrgshvb48sbvrbiwbkb_kbkkbhkgberibg";
    }

    internal class ServerListModel
    {
        internal string ServerName { get; set; }
        internal string ServerIP { get; set; }
        internal string ServerPassword { get; set; }
        internal string ServerPort { get; set; }
        internal string ServerUserName { get; set; }
        internal string PassPharse { get; set; }
        internal byte[] RSAKEY { get; set; }
    }

    internal class ScriptModel
    {
        internal string name { get; set; }

        internal string script { get; set; }
    }
}
