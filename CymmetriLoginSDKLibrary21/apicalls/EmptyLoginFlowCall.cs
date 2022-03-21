using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CymmetriLoginSDKLibrary21.apicalls
{
    public class EmptyLoginFlowResponse
    {
        public bool success { get; set; }
        public EmptyLoginFlowResponseData data { get; set; }
        public string timestamp { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
    }

    public class EmptyLoginFlowResponseData
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public string sessionId { get; set; }
        public string refreshTokenExpiry { get; set; }

    }
    class EmptyLoginFlowCall
    {
        public EmptyLoginFlowCall(HttpClient httpClient, string currentToken)
        {
            this.httpClient = httpClient;
            this.currentToken = currentToken;
            this.baseAddress = httpClient.BaseAddress;
        }

        

        public Uri baseAddress { get; set; }
        private StringContent GetJsonRequest()
        {
            var data = new StringContent("{}", Encoding.UTF8, "application/json");
            return data;
        }
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

        public async Task<string> MakeRequest()
        {
            var data = GetJsonRequest();
            SetClientHeaders();
            var response = await httpClient.PostAsync("authsrvc/auth/loginFlow", data);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
