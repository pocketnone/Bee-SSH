using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using static BeeSSH.Core.API.Cache;
using static BeeSSH.Core.Crypter.String;

namespace BeeSSH.Core.API
{
    internal class Request
    {
        internal static string Login(string username, string password, string totp = "000000")
        {
            var requestOptions = new Dictionary<string, string>();
            requestOptions.Add("tool", ClientAuthKey);
            requestOptions.Add("email", username);
            requestOptions.Add("password", password);
            requestOptions.Add("otp", totp);
            using (var client = new HttpClient())
            {
                try
                {
                    var req = new HttpRequestMessage(HttpMethod.Post, APIEndPoint.Login)
                        { Content = new FormUrlEncodedContent(requestOptions) }; // Request
                    var res_raw = client.SendAsync(req).Result; // Response from the API
                    var res = res_raw.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var datastuff = JsonConvert.DeserializeObject<LoginDeserilizeModel>(res);
                        // if Error
                        if (!string.IsNullOrEmpty(datastuff.Error)) return datastuff.Error;
                        AuthCookieForAPI = datastuff.AuthCookie;
                        _Username = datastuff.Username;
                        foreach (var item in datastuff.ServerRes)
                        {
                            ServerList.Add(new ServerListModel
                            {
                                ServerName = Decrypt(item.Name, EncryptionMasterPass),
                                ServerIP = Decrypt(item.ServerIP, EncryptionMasterPass),
                                ServerUserName = Decrypt(item.ServeruserName, EncryptionMasterPass),
                                ServerPassword = Decrypt(item.serverpass, EncryptionMasterPass),
                                PassPharse = Decrypt(item.PassPharseData, EncryptionMasterPass),
                                ServerPort = Decrypt(item.port, EncryptionMasterPass),
                                RSAKEY = Boolean.Parse(item.isKey),
                                RSAKeyText = Decrypt(item.cryptRSA, EncryptionMasterPass),
                                FingerPrint = item.Fingerprint,
                                ServerUID = item.ServerUID
                            });
                        }
                    }
                    catch(Exception ex)
                    {
                        string m = ex.ToString();
                        var datastuff = JsonConvert.DeserializeObject<LoginNoServerDeserilizeModel>(res);
                        // if Error
                        if (!string.IsNullOrEmpty(datastuff.Error)) return datastuff.Error;
                        
                        AuthCookieForAPI = datastuff.AuthCookie;
                        _Username = datastuff.Username;
                        return "ok";
                    }

                    return "ok";
                }
                catch
                {
                    return "Error";
                }
            }
        }

        // Add a Server to the Backend from the Webpage
        internal static AddServerResponse AddServer(string servername_crypted, string port_crypted, bool isKey, string crypt_key,
            string ipadress_crypted, string Password_crypted, string serverusername_crypted,
            string passPharse = "null")
        {
            var requestOptions = new Dictionary<string, string>();
            requestOptions.Add("tool", ClientAuthKey);
            requestOptions.Add("authkey", AuthCookieForAPI);
            requestOptions.Add("servername", servername_crypted);
            requestOptions.Add("port", port_crypted);
            requestOptions.Add("isKEY", isKey.ToString());
            requestOptions.Add("rsakey", crypt_key);
            requestOptions.Add("ipadress", ipadress_crypted);
            requestOptions.Add("Password", Password_crypted);
            requestOptions.Add("ServerUsername", serverusername_crypted);
            requestOptions.Add("PassPharse", passPharse);
            requestOptions.Add("Fingerprint", "null");
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, APIEndPoint.AddServer)
                    { Content = new FormUrlEncodedContent(requestOptions) }; // Request
                var res_raw = client.SendAsync(req).Result; // Response from the API
                var res = res_raw.Content.ReadAsStringAsync().Result; // Convert to String
                var datastuff = JsonConvert.DeserializeObject<AddServerResponse>(res);
                return datastuff;
            }
        }

        internal static string AddFingerprint(string crypt_fingerprint, string serverUID)
        {
            var requestOptions = new Dictionary<string, string>();
            requestOptions.Add("tool", ClientAuthKey);
            requestOptions.Add("authkey", AuthCookieForAPI);
            requestOptions.Add("serverUID", serverUID);
            requestOptions.Add("Fingerprint", crypt_fingerprint);
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, APIEndPoint.FingerPrintAPI)
                    { Content = new FormUrlEncodedContent(requestOptions) }; // Request
                var res_raw = client.SendAsync(req).Result; // Response from the API
                var res = res_raw.Content.ReadAsStringAsync().Result; // Convert to String
                var datastuff = JsonConvert.DeserializeObject<OtherResponse>(res);
                return datastuff.DataRes;
            }
        }


        /// <summary>
        /// Update Server
        /// </summary>
        /// <param name="servername_crypted"></param>
        /// <param name="port_crypted"></param>
        /// <param name="isKey"></param>
        /// <param name="ipadress_crypted"></param>
        /// <param name="PasswordOrKey_crypted"></param>
        /// <param name="serverusername_crypted"></param>
        /// <param name="ScriptUID"></param>
        /// <param name="passPharse"></param>
        /// <returns></returns>
        internal static string UpdateServer(string servername_crypted, string port_crypted, bool isKey, string crypt_RSAKey,
            string ipadress_crypted,
            string PasswordOrKey_crypted, string serverusername_crypted, string ScriptUID, string passPharse = "null")
        {
            var requestOptions = new Dictionary<string, string>();
            requestOptions.Add("tool", ClientAuthKey);
            requestOptions.Add("authkey", AuthCookieForAPI);
            requestOptions.Add("servername", servername_crypted);
            requestOptions.Add("port", port_crypted);
            requestOptions.Add("isKEY", isKey.ToString().ToLower());
            requestOptions.Add("cryptRSAKEY", crypt_RSAKey);
            requestOptions.Add("ipadress", ipadress_crypted);
            requestOptions.Add("PasswordKey", PasswordOrKey_crypted);
            requestOptions.Add("ServerUsername", serverusername_crypted);
            requestOptions.Add("PassPharse", passPharse);
            requestOptions.Add("scriptUID", ScriptUID);
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, APIEndPoint.UpdateServer)
                    { Content = new FormUrlEncodedContent(requestOptions) }; // Request
                var res_raw = client.SendAsync(req).Result; // Response from the API
                var res = res_raw.Content.ReadAsStringAsync().Result; // Convert to String
                var datastuff = JsonConvert.DeserializeObject<OtherResponse>(res);
                return datastuff.DataRes;
            }
        }

        /// <summary>
        /// Delete Server
        /// </summary>
        /// <param name="ScriptUID"></param>
        /// <returns></returns>
        internal static string DeleteServer(string ScriptUID)
        {
            var requestOptions = new Dictionary<string, string>();
            requestOptions.Add("tool", ClientAuthKey);
            requestOptions.Add("authkey", AuthCookieForAPI);
            requestOptions.Add("scriptUID", ScriptUID);
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, APIEndPoint.DeleteServer)
                    { Content = new FormUrlEncodedContent(requestOptions) }; // Request
                var res_raw = client.SendAsync(req).Result; // Response from the API
                var res = res_raw.Content.ReadAsStringAsync().Result; // Convert to String
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
                var req = new HttpRequestMessage(HttpMethod.Post, APIEndPoint.AddScript)
                    { Content = new FormUrlEncodedContent(requestOptions) }; // Request
                var res_raw = client.SendAsync(req).Result; // Response from the API
                var res = res_raw.Content.ReadAsStringAsync().Result; // Convert to String
                var datastuff = JsonConvert.DeserializeObject<OtherResponse>(res);
                return datastuff.DataRes;
            }
        }

        internal static string DeleteScript(string ScriptID)
        {
            var requestOptions = new Dictionary<string, string>();
            requestOptions.Add("tool", ClientAuthKey);
            requestOptions.Add("authkey", AuthCookieForAPI);
            requestOptions.Add("sCUID", ScriptID);
            using (var client = new HttpClient())
            {
                var req = new HttpRequestMessage(HttpMethod.Post, APIEndPoint.DeleteScript)
                    { Content = new FormUrlEncodedContent(requestOptions) }; // Request
                var res_raw = client.SendAsync(req).Result; // Response from the API
                var res = res_raw.Content.ReadAsStringAsync().Result; // Convert to String
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
                var req = new HttpRequestMessage(HttpMethod.Post, APIEndPoint.FetchScripts)
                    { Content = new FormUrlEncodedContent(requestOptions) }; // Request
                var res_raw = client.SendAsync(req).Result; // Response from the API
                var res = res_raw.Content.ReadAsStringAsync().Result; // Convert to String

                try
                {
                    var datastuff = JsonConvert.DeserializeObject<ScriptsModel>(res);

                    if (datastuff.InfoRes != "Success") return datastuff.InfoRes;


                    foreach (var resListArr in datastuff.ListRes)
                        Scriptlist.Add(new ScriptModel
                            {
                                name = resListArr.Name,
                                script = resListArr.scriptdata
                            }
                        );
                }
                catch
                {
                    return "ok";
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
        [JsonProperty("Info")] public string Error { get; set; }
        [JsonProperty("AuthKey")] public string AuthCookie { get; set; }
        [JsonProperty("Username")] public string Username { get; set; }
        [JsonProperty("data")] public List<DataLoginModel> ServerRes { get; set; }
    }

    //Login if no Server
    internal class LoginNoServerDeserilizeModel
    {
        [JsonProperty("Info")] public string Error { get; set; }
        
        [JsonProperty("Username")] public string Username { get; set; }
        [JsonProperty("AuthKey")] public string AuthCookie { get; set; }
    }

    // Login
    internal class DataLoginModel
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("crpyt_ServerUser")] public string ServeruserName { get; set; }

        [JsonProperty("crpyt_ip")] public string ServerIP { get; set; }

        [JsonProperty("crpyt_password")] public string serverpass { get; set; }

        [JsonProperty("crpyt_port")] public string port { get; set; }

        [JsonProperty("isRSA")] public string isKey { get; set; }
        
        [JsonProperty("crypt_RSAKey")] public string cryptRSA { get; set; }
        [JsonProperty("crpyt_PassPharse")] public string PassPharseData { get; set; }

        [JsonProperty("fingerprint")] public string Fingerprint { get; set; }
        [JsonProperty("server_UID")] public string ServerUID { get; set; }
    }

    // ETC
    internal class OtherResponse
    {
        [JsonProperty("Info")] public string DataRes { get; set; }
    }
    
    internal class AddServerResponse
    {
        [JsonProperty("Info")] public string DataRes { get; set; }
        [JsonProperty("ServerUID")] public string ServerUID { get; set; }
    }

    internal class ScriptsModel
    {
        [JsonProperty("Info")] public string InfoRes { get; set; }

        [JsonProperty("data")] public List<ScriptsExtensionModel> ListRes { get; set; }
    }

    internal class ScriptsExtensionModel
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("Script")] public string scriptdata { get; set; }
    }

    #endregion DataModels
}