using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CymmetriLoginSDKLibrary21.apicalls
{
    public class ValidateUser
    {

        /*
         * "{\"success\":true,\"data\":{\"token\":\"eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJhZG1pbiIsInJvbGVzIjpbIlZBTElEQVRFX0lERU5USVRZIl0sInRlbmFudElkIjoiZGVtbzE5MSIsImV4cCI6MTY0NzgxNjcxNywidXNlcklkIjoiNjFjMDNmMTVmYTc5OGYwMTIxZWRmNGYyIiwiaWF0IjoxNjQ3ODE2NDE3LCJpc1Bhc3N3b3JkTGVzc0VuYWJsZWQiOnRydWV9.L-9MhvF2QCxxDvKuPFRwXWAfzi9YqpU4LtMvBk-gbYo\",\"url\":null},\"timestamp\":\"20-Mar-2022 10:46:57\",\"message\":null,\"errorCode\":null}"             
     * Success Response 
     * {"success":true,"data":{"tenantId":"demo191","domain":"demo191.cymmetri.in"},"timestamp":"20-Mar-2022 07:20:47","message":null,"errorCode":null}
     * 
     * Failed Response
     *  "{\"success\":false,\"data\":null,\"timestamp\":\"20-Mar-2022 10:46:57\",\"message\":null,\"errorCode\":\"AUTHSRVC.ACCESS_DENIED\"}"            
     */
        public class ValidateUserResponse
        {
            public bool success { get; set; }
            public ValidateUserResponseData data { get; set; }
            public string timestamp { get; set; }
            public string message { get; set; }
            public string errorCode { get; set; }
        }

        public class ValidateUserResponseData
        {
            public string token { get; set; }
            public string url { get; set; }

        }
        public ValidateUser(HttpClient httpClient, string cookie)
        {
            this.cookie = cookie;
            this.httpClient = httpClient;
            this.baseAddress = httpClient.BaseAddress;
        }

        public string cookie { get; set; }
        public HttpClient httpClient { get; set; }
        public Uri baseAddress { get; set; }

        private void SetClientHeaders()
        {
            var headerReferer = baseAddress.AbsoluteUri;
            var headerOrigin = headerReferer.Remove(headerReferer.Length - 1, 1);
            // Clear Headers
            httpClient.DefaultRequestHeaders.Clear();

            // Add appropriate Headers
            httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "Windows");
            httpClient.DefaultRequestHeaders.Add("Origin", headerOrigin);
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            httpClient.DefaultRequestHeaders.Add("Referer", headerReferer);

        }

        public async Task<string> MakeRequest(string username)
        {
            SetClientHeaders();
            var response = await httpClient.GetAsync(String.Format("authsrvc/pll/pub/validateIdentity/{0}", username));

            
            return await response.Content.ReadAsStringAsync();
        }

    }
}
