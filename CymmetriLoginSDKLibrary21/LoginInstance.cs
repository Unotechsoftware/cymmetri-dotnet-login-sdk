using CymmetriLoginSDKLibrary21.apicalls;
using CymmetriLoginSDKLibrary21.utils;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace CymmetriLoginSDKLibrary21
{
    public class LoginInstance
    {
        static HttpClient httpClient = new HttpClient();
        public string baseUrl { get; set; }
        public string tenant { get; set; }

        //Example: baseUrl = https://box.cymmetri.in/
        //Example: tenant = box
        public LoginInstance(string baseUrl)
        {
            this.baseUrl = baseUrl;
            // initializing as "", because we will get this directly from the domain validation API.
            this.tenant = "";
            httpClient.BaseAddress = new Uri(baseUrl);
        }

        //Gives encrypted Password that is analogous to crypto-js
        public string GetEncryptedUser(string username, string password)
        {
            Protection p = new Protection();
            var encryptedPassword = p.OpenSSLEncrypt(password, username);

            return encryptedPassword;
        }

        // We already have the base URL, we should allow for calling this API as part of flow or independently
        // We won't add this to the constructor, because the Internet may or may not be available during constructor call.
        // CURL Request Example - 
        /*
         * curl 'https://demo191.cymmetri.in/regsrvc/custom-domain/validate' \
            -H 'Accept: application/json' \
            -H 'content-type: application/json' \
            -H 'sec-ch-ua-platform: "Windows"' \
            -H 'Origin: https://demo191.cymmetri.in' \
            -H 'Sec-Fetch-Site: same-origin' \
            -H 'Sec-Fetch-Mode: cors' \
            -H 'Sec-Fetch-Dest: empty' \
            -H 'Referer: https://demo191.cymmetri.in/' \
            --data-raw '{"domain":"demo191.cymmetri.in"}' \
            --compressed
         */
        public DomainValidationResponse ValidateDomain()
        {
            var domainValidation = new DomainValidation(httpClient);
            var result = domainValidation.MakeRequest();
            result.Wait();
            DomainValidationResponse response = JsonConvert.DeserializeObject<DomainValidationResponse>(result.Result);
            return response;
        }
    }
}
