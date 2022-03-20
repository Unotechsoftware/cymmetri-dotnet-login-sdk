using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CymmetriLoginSDKLibrary21.apicalls
{
    class CorrelationCookie
    {
        public CorrelationCookie(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.baseAddress = httpClient.BaseAddress;
        }

        public HttpClient httpClient { get; set; }
        public Uri baseAddress { get; set; }

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

        public async Task<string> MakeRequest()
        {
            SetClientHeaders();
            var response = await httpClient.GetAsync("utilsrvc/correlation/pub");
            
            var cookies =  response.Headers.GetValues("Set-Cookie");
            List<string> cookieList = new List<string>();
            foreach (var item in cookies)
            {
                cookieList.Add(item);
            }
            return cookieList[0];
            //return await response.Content.ReadAsStringAsync();
        }
    }
}
