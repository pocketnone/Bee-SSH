using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static BeeSSH.Core.Crypter.String;

namespace BeeSSH.Core.Autosave
{
    public class AutoLogin
    {
        private static string HardCodetPassword = "superSecretPassword";    // Need to be changed!

        private static string GetLocalFolder => GetPath();

        private static string GetPath()
        {
            string thePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "/.beeshh/");
            if (!Directory.Exists(thePath))
                Directory.CreateDirectory(thePath);

            if (!File.Exists(thePath + "config.json"))
                File.Create(thePath + "config.json");

            return thePath + "config.json";
        }
        
        internal static void CreateAutologin(string Username, string Password, string Masterpassword, bool totfa)
        {
            string json = GetLocalFolder;
            JObject rss = new JObject(
                new JProperty("username", Encrypt(Username, HardCodetPassword)),
                new JProperty("password", Encrypt(Password, HardCodetPassword)),
                new JProperty("masterpassword", Encrypt(Masterpassword, HardCodetPassword)),
                new JProperty("totf", "totfa"));
            File.WriteAllText(json, rss.ToString());
        }

        internal static string[] GiveLoginData()
        {
            List<string> res = new List<string>();
            var data = File.ReadAllText(GetLocalFolder);
            var datastuff = JsonConvert.DeserializeObject<AutoLoginModel>(data);
            res.Add(Decrypt(datastuff.Username, HardCodetPassword));
            res.Add(Decrypt(datastuff.Password, HardCodetPassword));
            res.Add(Decrypt(datastuff.MasterPassword, HardCodetPassword));
            return res.ToArray();
        }
    }

    internal class AutoLoginModel
    {
        [JsonProperty("username")]
        internal string Username;
        [JsonProperty("password")]
        internal string Password;
        [JsonProperty("masterpassword")]
        internal string MasterPassword;
        [JsonProperty("totf")]
        internal string totfa;
    }
}