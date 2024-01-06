using System;
using System.Collections.Generic;
using System.Linq;
using Feature.FormsExtensions.XDb;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExM.Framework.Diagnostics;
using Sitecore.XConnect;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Feature.FormsExtensions.SubmitActions.SendEmail
{
    public class FieldValueContactIdentierHandler : IExtractSendToContactIdentierHandler
    {
        private readonly ILogger logger;
        private readonly IXDbService xDbService;
        private readonly IXDbContactFactory contactFactory;

        public FieldValueContactIdentierHandler(ILogger logger, IXDbService xDbService, IXDbContactFactory contactFactory)
        {
            this.logger = logger;
            this.xDbService = xDbService;
            this.contactFactory = contactFactory;
        }

        public IList<ContactIdentifier> GetContacts(SendEmailExtendedData data, FormSubmitContext formSubmitContext)
        {
            var field = GetFieldById(data.FieldEmailAddressId.Value, formSubmitContext.Fields);
            if (field is null)
            {
                logger.LogWarn($"Could not find field with id {data.FieldEmailAddressId}");
            }
            var toAddresses = GetValue(field);
            if (string.IsNullOrEmpty(toAddresses))
            {
                logger.LogWarn("To address from field is empty");
                return new List<ContactIdentifier>();
            }
            return toAddresses.Split(';').Select(x => GetOrCreateContact(x, data.UpdateCurrentContact)).ToList();
        }

        private ContactIdentifier GetOrCreateContact(string toAddress, bool updateCurrentContact)
        {
            return updateCurrentContact ? IdentifyAndUpdateEmailContact(toAddress) : GetServiceContactIdentifier(toAddress);
        }

        private ContactIdentifier IdentifyAndUpdateEmailContact(string toAddress)
        {
            var basicContact = contactFactory.CreateContactWithEmail(toAddress);
            xDbService.IdentifyCurrent(basicContact);
            xDbService.UpdateEmail(basicContact);
            return new ContactIdentifier(basicContact.IdentifierSource, basicContact.IdentifierValue, ContactIdentifierType.Known);
        }

        protected virtual ContactIdentifier GetServiceContactIdentifier(string address)
        {
            var serviceContact = contactFactory.CreateContactWithEmail(address);
            xDbService.UpdateOrCreateServiceContact(serviceContact);
            return new ContactIdentifier(serviceContact.IdentifierSource, serviceContact.IdentifierValue, ContactIdentifierType.Known);
        }

        private static IViewModel GetFieldById(Guid id, IList<IViewModel> fields)
        {
            return fields.FirstOrDefault(f => Guid.Parse(f.ItemId) == id);
        }

        private static string GetValue(object field)
        {
            if (field.GetType().Name.ToString() == "ListViewModel" && ((Sitecore.ExperienceForms.Mvc.Models.Fields.ListViewModel)field).Items.Count > 0)
            {
                string dropvalue = ((Sitecore.ExperienceForms.Mvc.Models.Fields.ListViewModel)field).Items.Where(x => x.Selected == true).FirstOrDefault().Value;
                if (dropvalue.ToLower() == DictionaryPhraseRepository.Current.Get("/Contact Query/Rail", "Rail").ToLower())
                {
                    return DictionaryPhraseRepository.Current.Get("/Contact Query/Rail Email", "carmichael.project@adani.com.au");
                }

                else if (dropvalue.ToLower() == DictionaryPhraseRepository.Current.Get("/Contact Query/Mine", "Mine").ToLower())
                {
                    return DictionaryPhraseRepository.Current.Get("/Contact Query/Mine Email", "carmichael.project@adani.com.au");
                }

                else if (dropvalue.ToLower() == DictionaryPhraseRepository.Current.Get("/Contact Query/Port", "Port").ToLower())
                {
                    return DictionaryPhraseRepository.Current.Get("/Contact Query/Port Email", "abbotpoint.community@apt1.com.au");
                }

                else if (dropvalue.ToLower() == DictionaryPhraseRepository.Current.Get("/Contact Query/Renewables", "Renewables").ToLower())
                {
                    return DictionaryPhraseRepository.Current.Get("/Contact Query/Renewables Email", "Admin.Renewables@adani.com.au");
                }

                return ((Sitecore.ExperienceForms.Mvc.Models.Fields.ListViewModel)field).Items.Where(x => x.Selected == true).FirstOrDefault().Value;
            }
            else
            {
                return field?.GetType().GetProperty("Value")?.GetValue(field, null)?.ToString() ?? string.Empty;
            }
        }
    }
}