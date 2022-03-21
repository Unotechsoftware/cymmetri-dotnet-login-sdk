using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CymmetriLoginSDKLibrary21.apicalls
{
    public class LoginFlowOTPRequest
    {
        public LoginFlowOTPRequest(string mfaType, string otp)
        {
            this.mfaType = mfaType;
            this.otp = otp;
        }
        public string mfaType { get; set; }
        public string otp { get; set; }
    }
    public class LoginFlowResponse
    {
        public bool success { get; set; }
        public EmptyLoginFlowResponseData data { get; set; }
        public string timestamp { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
    }

    public class LoginFlowResponseData
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public string sessionId { get; set; }
        public string refreshTokenExpiry { get; set; }

    }
    class LoginFlowWithBody
    {
        private StringContent GetJsonRequest(string mfaType, string otp)
        {
            var requestData = new LoginFlowOTPRequest(mfaType, otp);

            var json = JsonConvert.SerializeObject(requestData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return data;
        }
        public LoginFlowWithBody(HttpClient httpClient, string currentToken)
        {
            this.httpClient = httpClient;
            this.currentToken = currentToken;
            this.baseAddress = httpClient.BaseAddress;
        }
        public Uri baseAddress { get; set; }
        public HttpClient httpClient { get; set; }

        public string currentToken { get; set; }

        private void SetClientHeaders()
        {
            var headerReferer = baseAddress.AbsoluteUri;
            var headerOrigin = headerReferer.Remove(headerReferer.Length - 1, 1);
            // Clear Headers
            httpClient.DefaultRequestHeaders.Clear();

            // Add appropriate Headers
            httpClient.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", this.currentToken));
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
            httpClient.DefaultRequestHeaders.Add("Origin", headerOrigin);
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            httpClient.DefaultRequestHeaders.Add("Referer", headerReferer);

        }

        public async Task<string> MakeRequest(string mfaType, string otp)
        {
            var data = GetJsonRequest(mfaType, otp);
            SetClientHeaders();
            var response = await httpClient.PostAsync("authsrvc/auth/loginFlow", data);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
