using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using System.Net.Http.Headers;
using System.Threading;
using Sitecore.Diagnostics;

namespace Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services
{
    public class APIResponse : IAPIResponse
    {
        public string GetAPIResponse(string method, string URL, Dictionary<string, string> headers, Dictionary<string, string> parameters, object body)
        {
            string jsonResponse = string.Empty;
            using (var client = new WebClient())
            {
                try
                {
                    client.UseDefaultCredentials = true;
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            client.Headers.Add(header.Key, header.Value);
                        }
                    }
                    var uriBuilder = new UriBuilder(URL);
                    var paramValues = HttpUtility.ParseQueryString(uriBuilder.Query);
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            paramValues.Add(param.Key, param.Value);
                        }
                    }

                    uriBuilder.Query = paramValues.ToString();
                    ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => { return true; };
                    URL = method.ToUpper() == "GET" ? URL + uriBuilder.Query.ToString() : URL;
                    var uri = new Uri(URL);



                    if (method.ToUpper() == "GET")
                    {
                        client.Headers.Add("Content-Type:application/json");
                        client.Headers.Add("Accept:application/json");
                        jsonResponse = client.DownloadString(uri);
                    }
                    if (method.ToUpper() == "POST")
                    {
                        jsonResponse = client.UploadString(uriBuilder.Uri, method.ToUpper(), JsonConvert.SerializeObject(body));
                    }
                }
                catch (WebException ex)
                {
                    // Http Error
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        var webResponse = (HttpWebResponse)ex.Response;
                        var statusCode = (int)webResponse.StatusCode;
                        var msg = webResponse.StatusDescription;
                        throw new HttpException(statusCode, msg);
                    }
                    else
                    {
                        throw new HttpException(500, ex.Message);
                    }
                }
                return jsonResponse;
            }

        }


        public async Task<string> GetAPIResponseNew(string method, string URL, Dictionary<string, string> headers, Dictionary<string, string> parameters, object body)
        {
            string jsonResponse = string.Empty;
            using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                LogRepository _logger = new LogRepository();
                try
                {
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    var uriBuilder = new Uri(URL);

                    ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => { return true; };
                    client.BaseAddress = uriBuilder;
                    Log.Info("GetAPIResponseNew "+ method.ToUpper() +" Start", this);
                    if (method.ToUpper() == "GET")
                    {
                        // client.DefaultRequestHeaders.Add("Content-Type","application/json");
                        //  client.DefaultRequestHeaders.Add("Accept","application/json");                      
                        var responseTask = await client.GetAsync(URL);
                        Log.Info("GetAPIResponseNew GET StatusCode " + responseTask.StatusCode, this);
                        if (responseTask.IsSuccessStatusCode)
                        {
                            jsonResponse = await responseTask.Content.ReadAsStringAsync();                            
                        }
                    }
                    if (method.ToUpper() == "POST")
                    {
                        string query = await ParamsToStringAsync(parameters);
                        HttpContent _body = new StringContent(JsonConvert.SerializeObject(body));
                        _body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var responseTask = await client.PostAsync(uriBuilder + "?" + query, _body);
                        Log.Info("GetAPIResponseNew POST StatusCode " + responseTask.StatusCode, this);
                        if (responseTask.IsSuccessStatusCode)
                        {
                            jsonResponse = await responseTask.Content.ReadAsStringAsync();
                            _logger.Error(jsonResponse);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.Error(ex.Message);
                    throw new HttpException(500, ex.Message);
                }

                return jsonResponse;
            }

        }

        public async Task<string> GetDelayedAPIResponse(string method, string URL, Dictionary<string, string> headers, Dictionary<string, string> parameters, object body)
        {
            string jsonResponse = string.Empty;
            LogRepository _logger = new LogRepository();
            _logger.Info("Inside API call GetDelayedAPIResponse.");
            using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                try
                {
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }

                    var uriBuilder = new Uri(URL);

                    ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => { return true; };
                    client.BaseAddress = uriBuilder;

                    if (method.ToUpper() == "GET")
                    {
                        string query = await ParamsToStringAsync(parameters);
                        var responseMessage = new HttpResponseMessage();
                        var cancellationTokenSource = new CancellationTokenSource();
                        await Task.Run(async () =>
                        {
                            await Task.Delay(300);
                            responseMessage = await client.GetAsync(uriBuilder + "?" + query);
                        });

                        if (responseMessage.IsSuccessStatusCode == true)
                        {
                            _logger.Info("Received response from API for URI :" + String.Concat(uriBuilder, "?", query));
                            jsonResponse = await responseMessage.Content.ReadAsStringAsync();
                        }
                    }
                    if (method.ToUpper() == "POST")
                    {
                        string query = await ParamsToStringAsync(parameters);
                        HttpContent _body = new StringContent(JsonConvert.SerializeObject(body));
                        _body.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var responseTask = await client.PostAsync(uriBuilder + "?" + query, _body);
                        if (responseTask.IsSuccessStatusCode)
                        {
                            jsonResponse = await responseTask.Content.ReadAsStringAsync();
                            _logger.Error(jsonResponse);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.Error(ex.Message);
                    throw new HttpException(500, ex.Message);
                }

                return jsonResponse;
            }

        }

        private async Task<string> ParamsToStringAsync(Dictionary<string, string> urlParams)
        {
            using (HttpContent content = new FormUrlEncodedContent(urlParams))
                return await content.ReadAsStringAsync();
        }
    }
}