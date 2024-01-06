using Microsoft.Extensions.DependencyInjection;
using Sitecore.Configuration;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.EDS.Core.Dispatch;
using Sitecore.EmailCampaign.Cm.Pipelines.SendEmail;
using Sitecore.EmailCampaign.Model.Web.Exceptions;
using Sitecore.EmailCampaign.Model.Web.Settings;
using Sitecore.ExM.Framework.Diagnostics;
using Sitecore.Modules.EmailCampaign;
using Sitecore.Pipelines;
using Sitecore.StringExtensions;
using System;
using System.Threading;
using Sitecore.Modules.EmailCampaign.Core.Dispatch;
using Feature.FormsExtensions.ApplicationSettings;
using Sitecore.Modules.EmailCampaign.Messages;
using Newtonsoft.Json.Linq;
using Feature.FormsExtensions.Business;
using System.Collections.Generic;
using System.Linq;

namespace Feature.FormsExtensions.Pipelines.SendEmail
{
    public class SendEmail
    {
        private readonly ILogger _logger;
        private readonly IDispatchManager _dispatchManager;
        private readonly EcmSettings _exmSettings;
        private readonly Random _rand = new Random();
        public SendEmail(ILogger logger) : this(logger, Factory.CreateObject("exm/eds/dispatchManager", true) as IDispatchManager, ServiceLocator.ServiceProvider.GetService<EcmSettings>())
        {
        }
        public SendEmail(ILogger logger, IDispatchManager dispatchManager, EcmSettings exmSettings)
        {
            Assert.ArgumentNotNull(logger, "logger");
            Assert.ArgumentNotNull(dispatchManager, "dispatchManager");
            Assert.ArgumentNotNull(exmSettings, "exmSettings");
            this._logger = logger;
            this._dispatchManager = dispatchManager;
            this._exmSettings = exmSettings;
        }
        public void Process(SendMessageArgs args)
        {
            if (!(args.EcmMessage is MessageItem message))
                return;

            if (!args.IsTestSend)
            {
                if (!message.CustomPersonTokens.ContainsKey(Constants.CustomTokensFormKey))
                    return;
                var formFields = message.CustomPersonTokens[Constants.CustomTokensFormKey];
                if (formFields == null)
                    return;
                message.CustomPersonTokens[Constants.CustomTokensFormKey] = ConvertToPlainText(formFields);
            }


            Assert.ArgumentNotNull(args, "args");
            EmailMessage emailMessage = args.CustomData["EmailMessage"] as EmailMessage;
            if (emailMessage == null)
            {
                args.AddMessage("Missing EmailMessage from arguments.");
                return;
            }
            if (emailMessage.Recipients.Count < 1)
            {
                args.AddMessage("Missing Recipients from EmailMessage argument.");
                return;
            }
            if (!this._dispatchManager.IsConfigured)
            {
                args.AddMessage("Dispatch manager is not configured");
                return;
            }
            this._logger.TraceInfo("Message dispatch has started. Subject: '{0}'. Recipient:  '{1}'.".FormatWith(new object[]
            {
                emailMessage.Subject,
                this._exmSettings.IncludePIIinLogFiles ? emailMessage.Recipients[0] : "PII removed"
            }));
            args.StartSendTime = DateTime.UtcNow;
            if (GlobalSettings.MtaEmulation || args.Task.Message.Emulation)
            {
                Thread.Sleep(this._rand.Next(GlobalSettings.EmulationMinSendTime, GlobalSettings.EmulationMaxSendTime));
                if ((double)this._rand.Next(0, 100000) <= GlobalSettings.EmulationFailProbability * 1000.0)
                {
                    throw new NonCriticalException("Emulation failed");
                }
            }
            else
            {
                string replyToEmail = string.Empty;
                replyToEmail = message.CustomPersonTokens.ContainsKey("form_Email") && message.CustomPersonTokens["form_Email"] != null ? message.CustomPersonTokens["form_Email"].ToString() : string.Empty;

                if (string.IsNullOrEmpty(replyToEmail))
                    replyToEmail = message.CustomPersonTokens.ContainsKey("form_FromEmail") && message.CustomPersonTokens["form_FromEmail"] != null ? message.CustomPersonTokens["form_FromEmail"].ToString() : string.Empty;
                if (string.IsNullOrEmpty(replyToEmail))
                    replyToEmail = message.CustomPersonTokens.ContainsKey("form_ContactEmail") && message.CustomPersonTokens["form_ContactEmail"] != null ? message.CustomPersonTokens["form_ContactEmail"].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(replyToEmail))
                {
                    emailMessage.Recipients.Remove(replyToEmail);
                    emailMessage.Recipients.Add(emailMessage.FromAddress);
                    emailMessage.ReplyTo = replyToEmail;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Sitecore.Context.User.Profile.Email))
                    {
                        emailMessage.Recipients.Remove(Sitecore.Context.User.Profile.Email);
                        emailMessage.Recipients.Add(emailMessage.FromAddress);
                        emailMessage.ReplyTo = Sitecore.Context.User.Profile.Email;
                    }
                }

                DispatchResult result = this._dispatchManager.SendAsync(emailMessage).Result;
            }
            args.AddMessage("ok", PipelineMessageType.Information);
            this._logger.TraceInfo("Message dispatch has been completed. Subject: '{0}'. Recipient: '{1}'.".FormatWith(new object[]
            {
                emailMessage.Subject,
                this._exmSettings.IncludePIIinLogFiles ? emailMessage.Recipients[0] : "PII removed"
            }));
            args.SendingTime = Util.GetTimeDiff(args.StartSendTime, DateTime.UtcNow);
        }


        private static string ConvertToPlainText(object formFieldsObject)
        {
            var plainTextString = "";
            if (!(formFieldsObject is JArray json))
                return plainTextString;

            var formFields = json.ToObject<IList<FormField>>();
            foreach (var formField in formFields)
            {
                plainTextString += formField.Name + " : ";
                if (formField.Value != null)
                {
                    plainTextString += formField.Value.Name;
                }
                else
                {
                    plainTextString += formField.ValueList.Aggregate("", (current, value) => current + ", " + value.Name);
                }
                plainTextString += Environment.NewLine;
            }
            return plainTextString;
        }


    }
}