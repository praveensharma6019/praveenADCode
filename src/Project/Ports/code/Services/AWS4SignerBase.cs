using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Sitecore.Ports.Website.Services
{
    public abstract class AWS4SignerBase
    {
        // SHA256 hash of an empty request body
        public const string EMPTY_BODY_SHA256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

        public const string SCHEME = "AWS4";
        public const string ALGORITHM = "HMAC-SHA256";
        public const string TERMINATOR = "aws4_request";

        // format strings for the date/time and date stamps required during signing
        public const string ISO8601BasicFormat = "yyyyMMddTHHmmssZ";
        public const string DateStringFormat = "yyyyMMdd";

        // some common x-amz-* parameters
        public const string X_Amz_Algorithm = "X-Amz-Algorithm";
        public const string X_Amz_Credential = "X-Amz-Credential";
        public const string X_Amz_SignedHeaders = "X-Amz-SignedHeaders";
        public const string X_Amz_Date = "X-Amz-Date";
        public const string X_Amz_Signature = "X-Amz-Signature";
        public const string X_Amz_Expires = "X-Amz-Expires";
        public const string X_Amz_Content_SHA256 = "X-Amz-Content-SHA256";
        public const string X_Amz_Decoded_Content_Length = "X-Amz-Decoded-Content-Length";
        public const string X_Amz_Meta_UUID = "X-Amz-Meta-UUID";

        // the name of the keyed hash algorithm used in signing
        public const string HMACSHA256 = "HMACSHA256";

        // request canonicalization requires multiple whitespace compression
        protected static readonly Regex CompressWhitespaceRegex = new Regex("\\s+");

        // algorithm used to hash the canonical request that is supplied to
        // the signature computation
        public static HashAlgorithm CanonicalRequestHashAlgorithm = HashAlgorithm.Create("SHA-256");

        /// <summary>
        /// The service endpoint, including the path to any resource.
        /// </summary>
        public Uri EndpointUri { get; set; }

        /// <summary>
        /// The HTTP verb for the request, e.g. GET.
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// The signing name of the service, e.g. 's3'.
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// The system name of the AWS region associated with the endpoint, e.g. us-east-1.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Returns the canonical collection of header names that will be included in
        /// the signature. For AWS4, all header names must be included in the process 
        /// in sorted canonicalized order.
        /// </summary>
        /// <param name="headers">
        /// The set of header names and values that will be sent with the request
        /// </param>
        /// <returns>
        /// The set of header names canonicalized to a flattened, ;-delimited string
        /// </returns>
        protected string CanonicalizeHeaderNames(IDictionary<string, string> headers)
        {
            var headersToSign = new List<string>(headers.Keys);
            headersToSign.Sort(StringComparer.OrdinalIgnoreCase);

            var sb = new StringBuilder();
            foreach (var header in headersToSign)
            {
                if (sb.Length > 0)
                    sb.Append(";");
                sb.Append(header.ToLower());
            }
            return sb.ToString();
        }

        /// <summary>
        /// Computes the canonical headers with values for the request. 
        /// For AWS4, all headers must be included in the signing process.
        /// </summary>
        /// <param name="headers">The set of headers to be encoded</param>
        /// <returns>Canonicalized string of headers with values</returns>
        protected virtual string CanonicalizeHeaders(IDictionary<string, string> headers)
        {
            if (headers == null || headers.Count == 0)
                return string.Empty;

            // step1: sort the headers into lower-case format; we create a new
            // map to ensure we can do a subsequent key lookup using a lower-case
            // key regardless of how 'headers' was created.
            var sortedHeaderMap = new SortedDictionary<string, string>();
            foreach (var header in headers.Keys)
            {
                sortedHeaderMap.Add(header.ToLower(), headers[header]);
            }

            // step2: form the canonical header:value entries in sorted order. 
            // Multiple white spaces in the values should be compressed to a single 
            // space.
            var sb = new StringBuilder();
            foreach (var header in sortedHeaderMap.Keys)
            {
                var headerValue = CompressWhitespaceRegex.Replace(sortedHeaderMap[header], " ");
                sb.AppendFormat("{0}:{1}\n", header, headerValue.Trim());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the canonical request string to go into the signer process; this 
        /// consists of several canonical sub-parts.
        /// </summary>
        /// <param name="endpointUri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="queryParameters"></param>
        /// <param name="canonicalizedHeaderNames">
        /// The set of header names to be included in the signature, formatted as a flattened, ;-delimited string
        /// </param>
        /// <param name="canonicalizedHeaders">
        /// </param>
        /// <param name="bodyHash">
        /// Precomputed SHA256 hash of the request body content. For chunked encoding this
        /// should be the fixed string ''.
        /// </param>
        /// <returns>String representing the canonicalized request for signing</returns>
        protected string CanonicalizeRequest(Uri endpointUri,
                                             string httpMethod,
                                             string queryParameters,
                                             string canonicalizedHeaderNames,
                                             string canonicalizedHeaders,
                                             string bodyHash)
        {
            var canonicalRequest = new StringBuilder();

            canonicalRequest.AppendFormat("{0}\n", httpMethod);
            canonicalRequest.AppendFormat("{0}\n", CanonicalResourcePath(endpointUri));
            canonicalRequest.AppendFormat("{0}\n", queryParameters);

            canonicalRequest.AppendFormat("{0}\n", canonicalizedHeaders);
            canonicalRequest.AppendFormat("{0}\n", canonicalizedHeaderNames);

            canonicalRequest.Append(bodyHash);

            return canonicalRequest.ToString();
        }

        /// <summary>
        /// Returns the canonicalized resource path for the service endpoint
        /// </summary>
        /// <param name="endpointUri">Endpoint to the service/resource</param>
        /// <returns>Canonicalized resource path for the endpoint</returns>
        protected string CanonicalResourcePath(Uri endpointUri)
        {
            if (string.IsNullOrEmpty(endpointUri.AbsolutePath))
                return "/";

            // encode the path per RFC3986
            return HttpHelpers.UrlEncode(endpointUri.AbsolutePath, true);
        }

        /// <summary>
        /// Compute and return the multi-stage signing key for the request.
        /// </summary>
        /// <param name="algorithm">Hashing algorithm to use</param>
        /// <param name="awsSecretAccessKey">The clear-text AWS secret key</param>
        /// <param name="region">The region in which the service request will be processed</param>
        /// <param name="date">Date of the request, in yyyyMMdd format</param>
        /// <param name="service">The name of the service being called by the request</param>
        /// <returns>Computed signing key</returns>
        protected byte[] DeriveSigningKey(string algorithm, string awsSecretAccessKey, string region, string date, string service)
        {
            const string ksecretPrefix = SCHEME;
            char[] ksecret = null;

            ksecret = (ksecretPrefix + awsSecretAccessKey).ToCharArray();

            byte[] hashDate = ComputeKeyedHash(algorithm, Encoding.UTF8.GetBytes(ksecret), Encoding.UTF8.GetBytes(date));
            byte[] hashRegion = ComputeKeyedHash(algorithm, hashDate, Encoding.UTF8.GetBytes(region));
            byte[] hashService = ComputeKeyedHash(algorithm, hashRegion, Encoding.UTF8.GetBytes(service));
            return ComputeKeyedHash(algorithm, hashService, Encoding.UTF8.GetBytes(TERMINATOR));
        }

        /// <summary>
        /// Compute and return the hash of a data blob using the specified algorithm
        /// and key
        /// </summary>
        /// <param name="algorithm">Algorithm to use for hashing</param>
        /// <param name="key">Hash key</param>
        /// <param name="data">Data blob</param>
        /// <returns>Hash of the data</returns>
        protected byte[] ComputeKeyedHash(string algorithm, byte[] key, byte[] data)
        {
            var kha = KeyedHashAlgorithm.Create(algorithm);
            kha.Key = key;
            return kha.ComputeHash(data);
        }

        /// <summary>
        /// Helper to format a byte array into string
        /// </summary>
        /// <param name="data">The data blob to process</param>
        /// <param name="lowercase">If true, returns hex digits in lower case form</param>
        /// <returns>String version of the data</returns>
        public static string ToHexString(byte[] data, bool lowercase)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString(lowercase ? "x2" : "X2"));
            }
            return sb.ToString();
        }
    }

    public static class HttpHelpers
    {
        /// <summary>
        /// Makes a http request to the specified endpoint
        /// </summary>
        /// <param name="endpointUri"></param>
        /// <param name="httpMethod"></param>
        /// <param name="headers"></param>
        /// <param name="requestBody"></param>
        public static void InvokeHttpRequest(Uri endpointUri,
                                             string httpMethod,
                                             IDictionary<string, string> headers,
                                             string requestBody)
        {
            try
            {
                var request = ConstructWebRequest(endpointUri, httpMethod, headers);

                if (!string.IsNullOrEmpty(requestBody))
                {
                    var buffer = new byte[8192]; // arbitrary buffer size                        
                    var requestStream = request.GetRequestStream();
                    using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(requestBody)))
                    {
                        var bytesRead = 0;
                        while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            requestStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }

                CheckResponse(request);
            }
            catch (WebException ex)
            {
                using (var response = ex.Response as HttpWebResponse)
                {
                    if (response != null)
                    {
                        var errorMsg = ReadResponseBody(response);
                        Console.WriteLine("\n-- HTTP call failed with exception '{0}', status code '{1}'", errorMsg, response.StatusCode);
                    }
                }
            }
        }

        /// <summary>
        /// Construct a HttpWebRequest onto the specified endpoint and populate
        /// the headers.
        /// </summary>
        /// <param name="endpointUri">The endpoint to call</param>
        /// <param name="httpMethod">GET, PUT etc</param>
        /// <param name="headers">The set of headers to apply to the request</param>
        /// <returns>Initialized HttpWebRequest instance</returns>
        public static HttpWebRequest ConstructWebRequest(Uri endpointUri,
                                                         string httpMethod,
                                                         IDictionary<string, string> headers)
        {
            var request = (HttpWebRequest)WebRequest.Create(endpointUri);
            request.Method = httpMethod;

            foreach (var header in headers.Keys)
            {
                // not all headers can be set via the dictionary
                if (header.Equals("host", StringComparison.OrdinalIgnoreCase))
                    request.Host = headers[header];
                else if (header.Equals("content-length", StringComparison.OrdinalIgnoreCase))
                    request.ContentLength = long.Parse(headers[header]);
                else if (header.Equals("content-type", StringComparison.OrdinalIgnoreCase))
                    request.ContentType = headers[header];
                else
                    request.Headers.Add(header, headers[header]);
            }

            return request;
        }

        public static void CheckResponse(HttpWebRequest request)
        {
            // Get the response and read any body into a string, then display.
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("\n-- HTTP call succeeded");
                    var responseBody = ReadResponseBody(response);
                    if (!string.IsNullOrEmpty(responseBody))
                    {
                        Console.WriteLine("\n-- Response body:");
                        Console.WriteLine(responseBody);
                    }
                }
                else
                    Console.WriteLine("\n-- HTTP call failed, status code: {0}", response.StatusCode);
            }
        }

        /// <summary>
        /// Reads the response data from the service call, if any
        /// </summary>
        /// <param name="response">
        /// The response instance obtained from the previous request
        /// </param>
        /// <returns>The body content of the response</returns>
        public static string ReadResponseBody(HttpWebResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response", "Value cannot be null");

            // Then, open up a reader to the response and read the contents to a string
            // and return that to the caller.
            string responseBody = string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            return responseBody;
        }


        /// <summary>
        /// Helper routine to url encode canonicalized header names and values for safe
        /// inclusion in the presigned url.
        /// </summary>
        /// <param name="data">The string to encode</param>
        /// <param name="isPath">Whether the string is a URL path or not</param>
        /// <returns>The encoded string</returns>
        public static string UrlEncode(string data, bool isPath = false)
        {
            // The Set of accepted and valid Url characters per RFC3986. Characters outside of this set will be encoded.
            const string validUrlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            var encoded = new StringBuilder(data.Length * 2);
            string unreservedChars = String.Concat(validUrlCharacters, (isPath ? "/:" : ""));

            foreach (char symbol in System.Text.Encoding.UTF8.GetBytes(data))
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                    encoded.Append(symbol);
                else
                    encoded.Append("%").Append(String.Format("{0:X2}", (int)symbol));
            }

            return encoded.ToString();
        }
    }
}