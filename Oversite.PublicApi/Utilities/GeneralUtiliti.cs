using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OversitePublic.Utilities
{
    public class GeneralUtiliti
    {
        private HttpClient httpClient = new HttpClient();
        public T InvokePostHttpClient<T, F>(F obj, string url)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = JsonConvert.SerializeObject(obj);
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            //using (var httpClient = new HttpClient())
            //{
             //   httpClient.Timeout = TimeSpan.FromMinutes(5);
                cts.CancelAfter(httpClient.Timeout);
                httpClient.DefaultRequestHeaders.Clear();
                response = httpClient.PostAsync(url, new StringContent(contents, Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {

                    jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    ob = JsonConvert.DeserializeObject<T>(jsonResponse);

                }

            //}

            return (T)ob;
        }
        public async Task<T> InvokePostHttpClientasyc<T, F>(F obj, string url, string authToken = null)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = JsonConvert.SerializeObject(obj);
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            //using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                cts.CancelAfter(httpClient.Timeout);
                if (authToken != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken);
                }
                response = httpClient.PostAsync(url, new StringContent(contents, Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {

                    jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    ob = JsonConvert.DeserializeObject<T>(jsonResponse);

                }

            }
            return await Task.FromResult<T>((T)ob);
        }


        public T InvokePostHttpClientWithoutRequest<T>(string url)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            var cts = new CancellationTokenSource();
            var jsonResponse = string.Empty;
            Object ob = new object();
            //using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMinutes(5);
                cts.CancelAfter(httpClient.Timeout);
                httpClient.DefaultRequestHeaders.Clear();
                response = httpClient.PostAsync(url, null).Result;
                if (response.IsSuccessStatusCode)
                {

                    jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                    ob = JsonConvert.DeserializeObject<T>(jsonResponse);

                }

            }

            return (T)ob;
        }

        public string BuildACEOnPremiseRequestUrl(string ip, string port, string url = "")
        {
            string urlFormat = "http://{0}:{1}";
            string requestUrl = "";
            if (!string.IsNullOrEmpty(url))
            {
                requestUrl = url;
            }
            else if (!string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(port))
            {
                requestUrl = ip;
            }
            else
            {
                requestUrl = string.Format(urlFormat, ip, port);
            }
            return requestUrl;
        }


        public T InvokeGetHttpClientWithoutRequest<T>(string url, string authToken)
        {
           
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            var cts = new CancellationTokenSource();

            var jsonResponse = string.Empty;
            Object ob = new object();
            //try
            //{
                //using (var httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }))
                {
                    httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                    httpClient.Timeout = TimeSpan.FromMinutes(5);
                    cts.CancelAfter(httpClient.Timeout);

                    if (authToken != null)
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken);
                    }
                    response = httpClient.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {

                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        ob = JsonConvert.DeserializeObject<T>(jsonResponse);

                    }

                }
            //}
            //catch (Exception ex)
            //{
            //    T Obj = Activator.CreateInstance<T>();
            //    if (cts.IsCancellationRequested)
            //    {
            //       // Obj = Activator.CreateInstance<T>();
            //        jsonResponse = "Api call failed because the Api did not properly respond after a period of time";
            //    }
            //    else
            //    {

            //        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
            //      //  Obj = JsonConvert.DeserializeObject<G>(jsonResponse);
            //    }
               
            //}
            return (T)ob;
        }

        private static void AddHeaders(HttpRequestHeaders httpRequestHeaders, bool isPostOperation = false, string sampleUserContext = "")
        {
            httpRequestHeaders.Accept.Clear();
            //string sampleUserContext = string.Empty;
            httpRequestHeaders.Add("Authorization", sampleUserContext);
            if (isPostOperation)
                httpRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //public GeneralGetResponse GeneralGet(string optFlag, long firmID, long branchId, string itemID, string pledgeNo, string barCode, string parameter, string fromDate, string toDate, string authToken = null)
        //{
        //    GeneralGetResponse obj = new GeneralGetResponse();
        //    try
        //    {
        //        string baseUrl = configuration.GetSection("ServiceUrlSettings").GetSection("baseUrl").Value;
        //        string generalUrl = $"/glgeneralAPI/api/v1/general?optFlag={optFlag}&firmID={firmID}&bracnchID={branchId}&itemID={itemID}&pledgeNo={pledgeNo}&barCode={barCode}&parameter={parameter}&fromDate={fromDate}&toDate={toDate}";
        //        string Url = baseUrl + generalUrl;
        //        var generalGetUrl = Url.ToString();
        //        obj = GlobalMethods.InvokeGetHttpClientWithoutRequest<GeneralGetResponse>(generalGetUrl, authToken);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return obj;

        //}

    }
}
