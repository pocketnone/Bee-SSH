using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeSSH.Core.API
{
    internal class Cache
    {
        internal static string UserNameFromAPI { get; set; } 
        internal static string AuthCookieForAPI { get; set; }
        internal static Dictionary<string, Dictionary<string, string>> APICacheServers { get; set; }

    }
}
