using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CymmetriLoginSDKLibrary21.apicalls
{
    class PasswordValidationRequest
    {
        public PasswordValidationRequest(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
        public string login { get; set; }
        public string password { get; set; }
    }

    /*
     * Success Response 
     * {"success":true,"data":{"tenantId":"demo191","domain":"demo191.cymmetri.in"},"timestamp":"20-Mar-2022 07:20:47","message":null,"errorCode":null}
     * 
     * Failed Response
     *  {"success":false,"data":null,"timestamp":"20-Mar-2022 07:21:24","message":null,"errorCode":"REGSRVC.UNAUTHORIZED"}
     */
    public class PasswordValidationResponse
    {
        public bool success { get; set; }
        public PasswordValidationResponseData data { get; set; }
        public string timestamp { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
    }

    public class PasswordValidationResponseData
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public string sessionId { get; set; }
        public string refreshTokenExpiry { get; set; }

    }
    class ValidatePassword
    {
        public ValidatePassword(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.baseAddress = httpClient.BaseAddress;
        }

        public HttpClient httpClient { get; set; }
        public Uri baseAddress { get; set; }
        private StringContent GetJsonRequest(string username, string encryptedPass)
        {
            var requestData = new PasswordValidationRequest(username, encryptedPass);

            var json = JsonConvert.SerializeObject(requestData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return data;
        }

        private void SetClientHeaders()
        {
            var headerReferer = baseAddress.AbsoluteUri;
            var headerOrigin = headerReferer.Remove(headerReferer.Length - 1, 1);
            // Clear Headers
            httpClient.DefaultRequestHeaders.Clear();

            // Add appropriate Headers
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
            httpClient.DefaultRequestHeaders.Add("Origin", headerOrigin);
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            httpClient.DefaultRequestHeaders.Add("Referer", headerReferer);

        }

        public async Task<string> MakeRequest(string username, string encryptedPass)
        {
            var data = GetJsonRequest(username, encryptedPass);
            SetClientHeaders();
            var response = await httpClient.PostAsync("authsrvc/auth/token", data);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
