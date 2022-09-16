using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static BeeSSH.Core.Crypter.String;

namespace BeeSSH.Core.Autosave
{
    public class AutoLogin
    {
        private static string HardCodetPassword = "superSecretPassword"; // Need to be changed!

        private static string GetLocalFolder => GetPath();

        private static string GetPath()
        {
            var thePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                          "/beeshh/";
            if (!Directory.Exists(thePath))
                Directory.CreateDirectory(thePath);

            if (!File.Exists(thePath + "config.json"))
            {
                var json = thePath + "config.json";
                var rss = new JObject(
                    new JProperty("username", null),
                    new JProperty("password", null),
                    new JProperty("masterpassword", null),
                    new JProperty("totf", false),
                    new JProperty("autologin", false));
                File.WriteAllText(json, rss.ToString());
            }

            return thePath + "config.json";
        }

        internal static void CreateAutologin(string Username, string Password, string Masterpassword, bool totfa, bool autologin)
        {
            var json = GetLocalFolder;
            var rss = new JObject(
                new JProperty("username", Encrypt(Username, HardCodetPassword)),
                new JProperty("password", Encrypt(Password, HardCodetPassword)),
                new JProperty("masterpassword", Encrypt(Masterpassword, HardCodetPassword)),
                new JProperty("totf", totfa), 
                new JProperty("autologin", autologin));
            File.WriteAllText(json, rss.ToString());
        }

        internal static string[] GiveLoginData()
        {
            var res = new List<string>();
            var data = File.ReadAllText(GetLocalFolder);
            var datastuff = JsonConvert.DeserializeObject<AutoLoginModel>(data);
            res.Add(Decrypt(datastuff.Username, HardCodetPassword));
            res.Add(Decrypt(datastuff.Password, HardCodetPassword));
            res.Add(Decrypt(datastuff.MasterPassword, HardCodetPassword));
            res.Add(datastuff.totfa);
            res.Add(datastuff.autologin);
            return res.ToArray();
        }


        internal static bool GetAutoLogin()
        {
            var data = File.ReadAllText(GetLocalFolder);
            var datastuff = JsonConvert.DeserializeObject<AutoLoginModel>(data);
            bool res = false;
            try
            {
                res = bool.Parse(datastuff.autologin);
            }
            catch { }

            return res;
        }
    }

    internal class AutoLoginModel
    {
        [JsonProperty("username")] internal string Username;
        [JsonProperty("password")] internal string Password;
        [JsonProperty("masterpassword")] internal string MasterPassword;
        [JsonProperty("totf")] internal string totfa;
        [JsonProperty("autologin")] internal string autologin;
    }
}