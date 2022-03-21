using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CymmetriLoginSDKLibrary21.apicalls
{
    public class GetRegisteredFactorsResponse
    {
        public bool success { get; set; }
        public List<string> data { get; set; }
        public string timestamp { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
    }

    class GetRegisteredFactors
    {
        public GetRegisteredFactors(HttpClient httpClient, string currentToken)
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

        public async Task<string> MakeRequest()
        {
            SetClientHeaders();
            var response = await httpClient.GetAsync("authsrvc/auth/registeredFactorsByRule");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
