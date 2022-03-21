using CymmetriLoginSDKLibrary21.apicalls;
using CymmetriLoginSDKLibrary21.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using static CymmetriLoginSDKLibrary21.apicalls.ValidateUser;

namespace CymmetriLoginSDKLibrary21
{
    public class LoginInstance
    {
        static HttpClient httpClient = new HttpClient();

        public string correlationCookie { get; set; }
        public string baseUrl { get; set; }
        public string tenant { get; set; }
        public string scenarioStage { get; set; }
        public bool isError { get; set; }

        public bool isFinalStep { get; set; }
        public bool isComplete { get; set; }
        public string username { get; set; }

        public string currentToken { get; set; }
        //Example: baseUrl = https://box.cymmetri.in/
        //Example: tenant = box
        public LoginInstance(string baseUrl)
        {
            this.isComplete = false;
            this.isFinalStep = false;
            this.isError = false;
            this.baseUrl = baseUrl;
            // initializing as "", because we will get this directly from the domain validation API.
            this.tenant = "";
            this.correlationCookie = "";
            this.scenarioStage = "Load";
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
            if (response.success)
            {
                tenant = response.data.tenantId;
            }
            return response;
        }

        /*
         * 
         * curl 'https://demo191.cymmetri.in/utilsrvc/correlation/pub' \
             -H 'Referer: https://demo191.cymmetri.in/' 
            --compressed
         * 
         */
        public string SetCorrelationCookie()
        {
            var correlationCookie = new CorrelationCookie(httpClient);
            var result = correlationCookie.MakeRequest();
            result.Wait();
            var z = result.Result;
            this.correlationCookie = z;
            return z;
        }

        /*
         * 
         * 
         * curl 'https://demo191.cymmetri.in/authsrvc/pll/pub/validateIdentity/admin' \
  -H 'Connection: keep-alive' \
  -H 'sec-ch-ua: " Not A;Brand";v="99", "Chromium";v="99", "Microsoft Edge";v="99"' \
  -H 'Accept: application/json' \
  -H 'user: null' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.74 Safari/537.36 Edg/99.0.1150.46' \
  -H 'tenant: demo191' \
  -H 'sec-ch-ua-platform: "Windows"' \
  -H 'Sec-Fetch-Site: same-origin' \
  -H 'Sec-Fetch-Mode: cors' \
  -H 'Sec-Fetch-Dest: empty' \
  -H 'Referer: https://demo191.cymmetri.in/' \
  -H 'Accept-Language: en-US,en;q=0.9' \
  -H 'Cookie: deviceId=d6b007e0-3a72-4955-b765-52d428a22527; Correlation=988BBC52825B4E21A9D5FB377C5F87C5; app_73e5c5f8-276b-47bb-a6a5-b6f82a779d79=e689a8da-faa7-46f5-9c7a-2800abdd206a; device=a9869200-a86f-11ec-ab71-d196abb3a0e7' \
  --compressed
         * 
         */
        public ValidateUserResponse ValidateUserIdentity(string username)
        {
            var validateUser = new ValidateUser(httpClient, this.correlationCookie);
            var result = validateUser.MakeRequest(username);
            result.Wait();
            ValidateUserResponse response = JsonConvert.DeserializeObject<ValidateUserResponse>(result.Result);
            this.username = username;
            return response;
        }

        /*
         * 
         * 
         * curl 'https://demo191.cymmetri.in/authsrvc/auth/token' \
  -H 'Connection: keep-alive' \
  -H 'sec-ch-ua: " Not A;Brand";v="99", "Chromium";v="99", "Microsoft Edge";v="99"' \
  -H 'sec-ch-ua-mobile: ?0' \
  -H 'User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.74 Safari/537.36 Edg/99.0.1150.46' \
  -H 'tenant: demo191' \
  -H 'content-type: application/json' \
  -H 'Accept: application/json' \
  -H 'user: admin' \
  -H 'sec-ch-ua-platform: "Windows"' \
  -H 'Origin: https://demo191.cymmetri.in' \
  -H 'Sec-Fetch-Site: same-origin' \
  -H 'Sec-Fetch-Mode: cors' \
  -H 'Sec-Fetch-Dest: empty' \
  -H 'Referer: https://demo191.cymmetri.in/' \
  -H 'Accept-Language: en-US,en;q=0.9' \
  -H 'Cookie: deviceId=d6b007e0-3a72-4955-b765-52d428a22527; app_73e5c5f8-276b-47bb-a6a5-b6f82a779d79=e689a8da-faa7-46f5-9c7a-2800abdd206a; device=a9869200-a86f-11ec-ab71-d196abb3a0e7; Correlation=E2A85E5CBFA7469091E77E763CA08390' \
  --data-raw '{"login":"admin","password":"U2FsdGVkX1/3znoNq5bbvkguYw+/9LPkjkuy5ORlhhI="}' \
  --compressed
         * 
         */
        public PasswordValidationResponse ValidatePassword(string password)
        {
            var enc_pass = GetEncryptedUser(username, password);
            var validatePassword = new ValidatePassword(httpClient);
            var result = validatePassword.MakeRequest(username, enc_pass);
            result.Wait();
            PasswordValidationResponse response = JsonConvert.DeserializeObject<PasswordValidationResponse>(result.Result);
            if (response.data.refreshToken != null)
            {
                isFinalStep = true;
                isComplete = true;
            } else
            {
                if (response.success)
                {
                    this.currentToken = response.data.token;
                } else
                {
                    isFinalStep = true;
                    this.isError = true;
                }
            }
            
            return response;
        }

        // Two APIs must be called
        // API 1 - authsrvc/auth/loginFlow
        // API 2 - authsrvc/auth/registeredFactorsByRule
        public List<string> GetRegisteredMFAFactors()
        {
            var emptyLoginFlowRequest = new EmptyLoginFlowCall(httpClient, currentToken);
            var result = emptyLoginFlowRequest.MakeRequest();
            result.Wait();
            EmptyLoginFlowResponse response = JsonConvert.DeserializeObject<EmptyLoginFlowResponse>(result.Result);
            if (response.success)
            {
                
                    this.currentToken = response.data.token;
                    //return response;

                    var mfaRegisteredFactors = new GetRegisteredFactors(httpClient, currentToken);
                    var result_mfa = mfaRegisteredFactors.MakeRequest();
                    result_mfa.Wait();
                    GetRegisteredFactorsResponse response_mfa = JsonConvert.DeserializeObject<GetRegisteredFactorsResponse>(result_mfa.Result);
                    return response_mfa.data;
                
            } else
            {
                isError = true;
                isFinalStep = true;
                return null;
            }
            
        }

        // Currently Only handles OTP type MFA
        public LoginFlowResponse SendMFAAsLoginFlowBody(string mfaType, string otp)
        {
            var loginFlowWithBody = new LoginFlowWithBody(httpClient, currentToken);
            var result = loginFlowWithBody.MakeRequest(mfaType, otp);
            result.Wait();
            LoginFlowResponse response = JsonConvert.DeserializeObject<LoginFlowResponse>(result.Result);
            if (response.success)
            {
                this.currentToken = response.data.token;
                return response;
            }
            else
            {
                isError = true;
                isFinalStep = true;
                return response;
            }
        }

        // Final MFA Flow Step ideally
        public MfaFlowResponse CheckMFAFlow()
        {
            var mfaFlow = new MfaFlow(httpClient, currentToken);
            var result = mfaFlow.MakeRequest();
            result.Wait();
            MfaFlowResponse response = JsonConvert.DeserializeObject<MfaFlowResponse>(result.Result);
            if (response.success)
            {
                isComplete = true;   
            }
            else
            {
                isError = true;
            }
            isFinalStep = true;
            return response;
        }

        public int Scenario_PasswordBased(string Msg, string username, string flow,string password, string otpType, string otp, out string nextStage, out string errorMessage)
        {
            // Before Form Load - "Load"
            // 1. we will assume the baseAddress has already been passed.
            // 2. Validate Domain
            // 3. If domain is not validated. Show error
            // 4. if domain validated, set the correlationcookie
            // Form Load 1 - "Username"
            // 1. Read Username from form
            // 2. Validate username
            // 3. If username not validated. Show error
            // 4. If username validated proceed to Form 2
            // Form Load 2 - "Flow"
            // 1. Read Flow Name
            // 2. If Flow is not "password-based". Show error for now, stating that the flow is not yet supported.
            // 3. If flow is "password-based". Show Form 3
            // Form Load 3 - "Password"
            // 1. Read Password
            // 2. Validated Password.
            // 3. If not validated. Show password is wrong error.
            // 4. if Validated; Need to check if this is it; or MFA needed.
            // 5. if this is it; show submit form.
            // 6. if MFA needed; check if OTP factors are available.
            // 7. if OTP factors are not available; show error saying that these factors are not yet supported.
            // 8. if OTP factors are available; Show form 4.
            // Form Load 4 - "OTP Factor"
            // 1. Show drop down of OTP factors.
            // 2. On Select of OTP factors; Load Form 5
            // Form Load 5 - "OTP"
            // 1. Read OTP
            // 2. Validate OTP
            // 3. If OTP not validated; Show error saying that this OTP is incorrect.
            // 4. If OTP validated; Run CheckMFA()
            // 5. If CheckMFA not successful; SHow error saying something went wrong with MFA validation.
            // 6. If checkMFA successful; show submit form.
            // Submit Form Load - "Submit"
            // 1. Show Login submit button.
            switch (this.scenarioStage)
            {
                case "Load":
                    var dvr = this.ValidateDomain();
                    if (dvr.success)
                    {
                        this.SetCorrelationCookie();
                        nextStage = "Username";
                        errorMessage = "";
                        return 0;
                    } else
                    {
                        nextStage = "Error";
                        errorMessage = String.Format("Error in validating domain {0}", this.baseUrl);
                        return -1;
                    }
                case "Username":
                    var userResponse = this.ValidateUserIdentity(username);
                    if (userResponse.success)
                    {
                        nextStage = "Flow";
                        errorMessage = "";
                        return 0;
                    } else
                    {
                        nextStage = "Error";
                        errorMessage = String.Format("Error in validating user : {0} on domain {1}", username, this.baseUrl);
                        return -1;
                    }
                case "Flow":
                    if (flow == "passwordbased")
                    {
                        nextStage = "Password";
                        errorMessage = "";
                        return 0;

                    } else
                    {
                        nextStage = "Error";
                        errorMessage = String.Format("Selected Flow {0} is not supported yet in the SDK.", flow);
                        return -1;
                    }
                case "Password":
                    var pvr = this.ValidatePassword(password);
                    if (pvr.success)
                    {
                        if (pvr.data.refreshToken != null)
                        {
                            // Received refresh Token so close the flow.
                            nextStage = "Submit";
                            errorMessage = "";
                            return 0;
                        } else
                        {
                            nextStage = "OTP Factor";
                            errorMessage = "";
                            return 0;
                            // Not erroneous but should continue with flow.
                        }
                    }
                    else
                    {
                        // Password validation failed
                        nextStage = "Error";
                        errorMessage = "Password Validation failed for user";
                        return -1;
                    }
                case "OTP Factor":
                    if (otpType == "CymmetriAuthenticator" || otpType == "GoogleAuthenticator")
                    {
                        nextStage = "OTP";
                        errorMessage = "";
                        return 0;
                    }
                    else
                    {
                        nextStage = "Error";
                        errorMessage = String.Format("{0} is not supported MFA type yet. ", otpType);
                        return -1;
                    }
                case "OTP":
                    var mfaResp = this.SendMFAAsLoginFlowBody(otpType, otp);
                    if (mfaResp.success)
                    {
                        var mfaCheckResp = this.CheckMFAFlow();
                        if (mfaCheckResp.success)
                        {
                            nextStage = "Submit";
                            errorMessage = "";
                            return 0;
                        } else
                        {
                            nextStage = "Error";
                            errorMessage = "Error in check MFA Flow response";
                            return -1;
                        }
                    } else
                    {
                        nextStage = "Error";
                        errorMessage = String.Format("Error in validating mfa check with OTP as {0} for Type {1}", otp, otpType);
                        return -1;
                    }
                case "Error":
                    errorMessage = Msg;
                    nextStage = "Close";
                    return -1;
                default:
                    break;
            }
            errorMessage = "Error in something";
            nextStage = "Load";
            return -1;
        }
    }
}
