using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeSSH.DiscordRPC
{
    internal static class DiscordRPC_Config
    {
        // Useable stuff
        internal static string DiscordID { get => GetDiscordIDFromAPI(); }
        internal static string Titles { get => GetTitle(); }
        internal static string Images_Large { get => GetImage(); }
        internal static string Images_Small { get => GetSmallImage(); }


        private static string GetImage() => "beesshlarge";

        private static string GetSmallImage() => "beesshsmall";

        private static string GetTitle() => "Bee SSH Client"; 
        private static string GetDiscordIDFromAPI() => "987291263852240916";
        
    }
}
