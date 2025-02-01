
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Oversite.Helpers
{
	public class ApiManager
	{
        public Tuple<T, string> InvokePostHttpClient<T, F>(F obj, string url)
        {
            string baseUrl = url;
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = JsonConvert.SerializeObject(obj);
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            try
            {
                using (var httpClient = new HttpClient())
                {


                    httpClient.Timeout = TimeSpan.FromMinutes(10);

                    cts.CancelAfter(httpClient.Timeout);
                    //httpClient.Timeout = TimeSpan.FromSeconds(60);;
                    response = httpClient.PostAsync(baseUrl, new StringContent(contents, Encoding.UTF8, "application/json"), cts.Token).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        ob = JsonConvert.DeserializeObject<T>(jsonResponse);
                        return Tuple.Create((T)ob, jsonResponse);
                    }
                    else
                    {
                        T Obj = Activator.CreateInstance<T>();
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        return Tuple.Create(Obj, jsonResponse);
                    }


                }

            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    // a real cancellation, triggered by the caller
                }
                else
                {
                    // a web request timeout (possibly other things!?)
                }
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);
            }
            catch (Exception ex)
            {
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);
            }

        }
        public Tuple<T, string> SMSInvokePostHttpClient<T, F>(F obj, string url)
        {
            string baseUrl = url;
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = JsonConvert.SerializeObject(obj);
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            try
            {
                using (var httpClient = new HttpClient())
                {


                    httpClient.Timeout = TimeSpan.FromMinutes(10);

                    cts.CancelAfter(httpClient.Timeout);
                    //httpClient.Timeout = TimeSpan.FromSeconds(60);;
                    response = httpClient.GetAsync(baseUrl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        ob = JsonConvert.DeserializeObject<T>(jsonResponse);
                        return Tuple.Create((T)ob, jsonResponse);
                    }
                    else
                    {
                        T Obj = Activator.CreateInstance<T>();
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        return Tuple.Create(Obj, jsonResponse);
                    }


                }

            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    // a real cancellation, triggered by the caller
                }
                else
                {
                    // a web request timeout (possibly other things!?)
                }
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);
            }
            catch (Exception ex)
            {
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);
            }

        }
        public Tuple<string> InvokePostHttpClientXML(string obj, string url)
        {
            string baseUrl = url;
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            string contents = obj;
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            try
            {
                using (var httpClient = new HttpClient())
                {
                     

                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    cts.CancelAfter(httpClient.Timeout);
                    //httpClient.Timeout = TimeSpan.FromSeconds(60);;
                    response = httpClient.PostAsync(baseUrl, new StringContent(contents, Encoding.UTF8, "text/xml"), cts.Token).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        
                        return Tuple.Create(jsonResponse);
                    }
                    else
                    {
                       
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        return Tuple.Create(jsonResponse);
                    }


                }

            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    // a real cancellation, triggered by the caller
                }
                else
                {
                    // a web request timeout (possibly other things!?)
                }
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(jsonResponse);
            }
            catch (Exception ex)
            {
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(jsonResponse);
            }

        }
        public Tuple<T, string> InvokeOKYCPostHttpClient<T, F>(F obj, string url)
        {
            string baseUrl = url;
            PropertyInfo pi = obj.GetType().GetProperty("aadhar");
            String aadhar = (String)(pi.GetValue(obj, null));
            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    IList<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>> {
                    {
                            new KeyValuePair<string, string>("aadhar",aadhar) },
                    };
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    cts.CancelAfter(httpClient.Timeout);
                   
                    response = httpClient.PostAsync(url, new FormUrlEncodedContent(nameValueCollection)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        ob = JsonConvert.DeserializeObject<T>(jsonResponse);
                        return Tuple.Create((T)ob, jsonResponse);
                    }
                    else
                    {
                        T Obj = Activator.CreateInstance<T>();
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        return Tuple.Create(Obj, jsonResponse);
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    // a real cancellation, triggered by the caller
                }
                else
                {
                    // a web request timeout (possibly other things!?)
                }
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);
            }
            catch (Exception ex)
            {
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);
            }

        }
        public Tuple<T, string> InvokeOKYCValidatePostHttpClient<T, F>(F obj, string url)
        {
            PropertyInfo potp = obj.GetType().GetProperty("otp");
            String otp = (String)(potp.GetValue(obj, null)); 

            PropertyInfo paadharNo = obj.GetType().GetProperty("aadharNo");
            String aadharNo = (String)(paadharNo.GetValue(obj, null));

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            var jsonResponse = string.Empty;
            Object ob = new object();
            var cts = new CancellationTokenSource();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    IList<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>>
                    {
                           { new KeyValuePair<string, string>("otp", otp) },
                           { new KeyValuePair<string, string>("aid", aadharNo) },
                    };
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    cts.CancelAfter(httpClient.Timeout);
                    response = httpClient.PostAsync(url, new FormUrlEncodedContent(nameValueCollection)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        ob = JsonConvert.DeserializeObject<T>("{\"isOptVerified\":\"" + jsonResponse + "\"}");
                        return Tuple.Create((T)ob, jsonResponse);
                    }
                    else
                    {
                        T Obj = Activator.CreateInstance<T>();
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        return Tuple.Create(Obj, jsonResponse);
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    // a real cancellation, triggered by the caller
                }
                else
                {
                    // a web request timeout (possibly other things!?)
                }
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);
            }
            catch (Exception ex)
            {
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);
            }

        }

        public Tuple<T, string> InvokeGetHttpClient<T>(string url)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            var cts = new CancellationTokenSource();
            var jsonResponse = string.Empty;
            Object ob = new object();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(10);

                    cts.CancelAfter(httpClient.Timeout);
                    response = httpClient.GetAsync(url, cts.Token).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                        ob = JsonConvert.DeserializeObject<T>(jsonResponse);
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    // a real cancellation, triggered by the caller
                }
                else
                {
                    // a web request timeout (possibly other things!?)
                }
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);
            }
            catch (Exception ex)
            {
                T Obj = Activator.CreateInstance<T>();
                jsonResponse = response.Content.ReadAsStringAsync().Result.ToString();
                return Tuple.Create(Obj, jsonResponse);
            }

            return Tuple.Create((T)ob, jsonResponse);
        }

        public T InvokePostHttpClientWithoutRequest<T>(string url)
        {

            HttpResponseMessage response = new HttpResponseMessage() { StatusCode = HttpStatusCode.BadRequest };
            var cts = new CancellationTokenSource();
            var jsonResponse = string.Empty;
            Object ob = new object();
            using (var httpClient = new HttpClient())
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
                var httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                httpClient.Timeout = TimeSpan.FromMinutes(30);
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

    }
}
