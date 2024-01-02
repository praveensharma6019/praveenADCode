using Adani.Foundation.Messaging.Models;
using Newtonsoft.Json;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Adani.Foundation.Messaging.Services.SMS
{
    public class SMSService : ISMSService
    {
        public bool Send(string recipient, string messageBody, IEnumerable<KeyValuePair<string, string>> data)
        {
            try
            {
                Log.Info($"Ambuja SMS Service Recipient No received: {recipient}", recipient);
                Log.Info($"Ambuja SMS Service MessageBody received: {messageBody}", messageBody);
                Log.Info($"Ambuja SMS Service data received: {data}", data);
                var ambujaSMSServicePayloadModel = new AmbujaSMSServicePayloadModel();
                ambujaSMSServicePayloadModel.messages = new List<AmbujaSMSServicePayload>();

                var ambujaSMSServicePayload = new AmbujaSMSServicePayload();
                ambujaSMSServicePayload.from = "AMBUJA";

                ambujaSMSServicePayload.destinations = new List<Recepiants>();
                var recepiants = new Recepiants();
                recepiants.to =$"91{recipient}";
                ambujaSMSServicePayload.destinations.Add(recepiants);

                var formattedMessage = ReplaceFieldValues(messageBody, data);
                Log.Info($"Ambuja SMS Service data formattedMessage: {formattedMessage}", formattedMessage);
                ambujaSMSServicePayload.text = formattedMessage;

                var dltParameters = new DltParameters();
                dltParameters.principalEntityId = "1701159162138078568";
                dltParameters.contentTemplateId = "1707169882946025905";

                var dltParameterDetails = new DltParameterDetails();
                dltParameterDetails.indiaDlt = dltParameters;

                ambujaSMSServicePayload.regional = dltParameterDetails;
                ambujaSMSServicePayloadModel.messages.Add(ambujaSMSServicePayload);

                var trackingDetails = new TrackingDetails();
                trackingDetails.track = "URL";
                ambujaSMSServicePayloadModel.tracking = trackingDetails;

                var smsServiceUrl = Settings.GetSetting("SMSSender:ApiPath");

                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "App b173f6ba1de1516641427d2ddc52a85e-7532413e-15a5-490b-8758-016468927c65");
                var request = new HttpRequestMessage(HttpMethod.Post, smsServiceUrl);

                var serializedContent = JsonConvert.SerializeObject(ambujaSMSServicePayloadModel);
                Log.Info($"Ambuja SMS Service data serializedContent: {serializedContent}", serializedContent);
                request.Content = new StringContent(serializedContent,
                                    Encoding.UTF8,
                                    "application/json");

                var response = client.SendAsync(request).Result;

                Log.Info($"Ambuja, Response from Infobip API: {response}", response);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Log.Info($"Ambuja, OTP Sent failed due to Exception in SMSservice: {ex.Message}", ex.InnerException);
                return false;
            }
        }

        private static string ReplaceFieldValues(string content, IEnumerable<KeyValuePair<string, string>> formData)
        {
            string text = content;
            if (string.IsNullOrEmpty(text)) return string.Empty;

            foreach (var item in formData)
            {
                text = text.Replace($"[{item.Key}]", item.Value);
            }

            return text;
        }
    }
}
