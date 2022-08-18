using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static BeeSSH.Core.Crypter.String;
using static BeeSSH.Core.API.Cache;

namespace BeeSSH.Core.API
{
    internal class Request
    {   

        internal static string Login(string username,string password,string totp = "000000")
        {
            var requestOptions = new Dictionary<string, string>();
            requestOptions.Add("tool", ClientAuthKey);
            requestOptions.Add("email", username);
            requestOptions.Add("password", password);
            requestOptions.Add("otp", totp);
            using(var client = new HttpClient())
            {
                try
                {

                    var req = new HttpRequestMessage(HttpMethod.Post, LoginAPIURL) { Content = new FormUrlEncodedContent(requestOptions) };     // Request
                    var res_raw = client.SendAsync(req).Result;                                                                                 // Response from the API
                    string res = res_raw.Content.ReadAsStringAsync().Result;                                                                    // Convert to String
                    var datastuff = JsonConvert.DeserializeObject<LoginDeserilizeModel>(res);                                                   // Convert to responseapi

                    // if Error
                    if(!System.String.IsNullOrEmpty(datastuff.Error))
                    {
                        return datastuff.Error;
                    }


                    AuthCookieForAPI = datastuff.AuthCookie;
                    foreach (var item in datastuff.ServerRes)
                    {
                        if(!Boolean.Parse(item.isKey))
                        {
                            ServerList.Add(new ServerListModel
                            {
                                ServerName = Decrypt(item.Name, EncryptionMasterPass),
                                ServerIP = Decrypt(item.ServerIP, EncryptionMasterPass),
                                ServerUserName = Decrypt(item.ServeruserName, EncryptionMasterPass),
                                ServerPassword = Decrypt(item.serverpass, EncryptionMasterPass),
                                ServerPort = Decrypt(item.port, EncryptionMasterPass)
                            });
                        } else
                        {
                            ServerList.Add(new ServerListModel
                            {
                                ServerName = Decrypt(item.Name, EncryptionMasterPass),
                                ServerIP = Decrypt(item.ServerIP, EncryptionMasterPass),
                                ServerUserName = Decrypt(item.ServeruserName, EncryptionMasterPass),
                                ServerPort = Decrypt(item.port, EncryptionMasterPass),
                                RSAKEY = Encoding.UTF8.GetBytes(Decrypt(item.serverpass, EncryptionMasterPass))
                            });
                        }
                    }
                    return "ok";
                } catch
                {
                    return "Error";
                }
            }
        }

        // Add a Server to the Backend from the Webpage
        internal static string AddServer(string servername_crypted, string port_crypted, bool isKey, string ipadress_crypted, string PasswordOrKey_crypted)
        {
            var requestOptions = new Dictionary<string, string>();
            requestOptions.Add("tool", ClientAuthKey);
            requestOptions.Add("authkey", AuthCookieForAPI);
            requestOptions.Add("servername", servername_crypted);
            requestOptions.Add("port", port_crypted);
            requestOptions.Add("isKEY", isKey.ToString());
            requestOptions.Add("ipadress", ipadress_crypted);
            requestOptions.Add("PasswordKey", PasswordOrKey_crypted);
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, AddServerAPIURL) { Content = new FormUrlEncodedContent(requestOptions) };     // Request
                var res_raw = client.SendAsync(req).Result;                                                                                 // Response from the API
                string res = res_raw.Content.ReadAsStringAsync().Result;                                                                    // Convert to String
                var datastuff = JsonConvert.DeserializeObject<OtherResponse>(res);
                return datastuff.DataRes;
            }
        }

        internal static string AddScripts(string Scriptname, string ScriptContent)
        {
            var requestOptions = new Dictionary<string, string>();
            requestOptions.Add("tool", ClientAuthKey);
            requestOptions.Add("authkey", AuthCookieForAPI);
            requestOptions.Add("userscript", ScriptContent);
            requestOptions.Add("scriptName", Scriptname);
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, AddServerAPIURL) { Content = new FormUrlEncodedContent(requestOptions) };                 // Request
                var res_raw = client.SendAsync(req).Result;                                                                                 // Response from the API
                string res = res_raw.Content.ReadAsStringAsync().Result;                                                                                    // Convert to String
                var datastuff = JsonConvert.DeserializeObject<OtherResponse>(res);
                return datastuff.DataRes;
            }
        }
        
        internal static string FetchShortCutsScripts()
        {
            var requestOptions = new Dictionary<string, string>();
            requestOptions.Add("tool", ClientAuthKey);
            requestOptions.Add("authkey", AuthCookieForAPI);
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, AddServerAPIURL) { Content = new FormUrlEncodedContent(requestOptions) };     // Request
                var res_raw = client.SendAsync(req).Result;                                                                                 // Response from the API
                string res = res_raw.Content.ReadAsStringAsync().Result;                                                                    // Convert to String
                var datastuff = JsonConvert.DeserializeObject<ScriptsModel>(res);
                
                if(datastuff.InfoRes != "Success")
                {
                    return datastuff.InfoRes;
                }
                
                
                foreach (var resListArr in datastuff.ListRes)
                {
                    Scriptlist.Add(new ScriptModel
                        {
                            name = resListArr.Name,
                            script = resListArr.scriptdata
                        }
                    );
                }

                return "ok";
            }
        }
    }

    //=================================================================================================================
    // Data Models
    //=================================================================================================================

    #region DataModels

    // Login
    internal class LoginDeserilizeModel
    {
        [JsonProperty("Info")]
        public string Error { get; set; }
        [JsonProperty("AuthKey")]
        public string AuthCookie { get; set; }
        [JsonProperty("AuthKey")]
        public List<DataLoginModel> ServerRes { get; set; }
    }
    // Login
    internal class DataLoginModel {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("crpyt_ServerUser")]
        public string ServeruserName { get; set; }

        [JsonProperty("crpyt_ip")]
        public string ServerIP { get; set; }

        [JsonProperty("crpyt_password")]
        public string serverpass { get; set; }

        [JsonProperty("crpyt_port")]
        public string port { get; set; }

        [JsonProperty("isKey")]
        public string isKey { get; set; }
    }

    // ETC
    internal class OtherResponse
    {
        [JsonProperty("Info")]
        public string DataRes { get; set; }
    }

    internal class ScriptsModel
    {
        [JsonProperty("Info")]
        public string InfoRes { get; set; }
        
        [JsonProperty("data")]
        public List<ScriptsExtensionModel> ListRes { get; set; }
    }

    internal class ScriptsExtensionModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("Script")]
        public string scriptdata { get; set; }

    }

    #endregion DataModels
}
