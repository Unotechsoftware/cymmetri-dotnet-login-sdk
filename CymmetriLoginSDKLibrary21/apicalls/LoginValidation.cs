using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CymmetriLoginSDKLibrary21.apicalls
{
    class DomainValidationRequest
    {
        public DomainValidationRequest(string domain)
        {
            this.domain = domain;
        }
        public string domain { get; set; }
    }

    /*
     * Success Response 
     * {"success":true,"data":{"tenantId":"demo191","domain":"demo191.cymmetri.in"},"timestamp":"20-Mar-2022 07:20:47","message":null,"errorCode":null}
     * 
     * Failed Response
     *  {"success":false,"data":null,"timestamp":"20-Mar-2022 07:21:24","message":null,"errorCode":"REGSRVC.UNAUTHORIZED"}
     */
    public class DomainValidationResponse
    {
        public bool success { get; set; }
        public DomainValidationResponseData data { get; set; }
        public string timestamp { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
    }

    public class DomainValidationResponseData
    {
        public string tenantId { get; set; }
        public string domain { get; set; }

    }
    class DomainValidation
    {
        public DomainValidation(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            this.baseAddress = httpClient.BaseAddress;
        }

        public HttpClient httpClient { get; set; }
        public Uri baseAddress { get; set; }
        private StringContent GetJsonRequest()
        {
            var domain = baseAddress.Host;
            var requestData = new DomainValidationRequest(domain);

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

        public async Task<string> MakeRequest()
        {
            var data = GetJsonRequest();
            SetClientHeaders();
            var response = await httpClient.PostAsync("regsrvc/custom-domain/validate", data);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
