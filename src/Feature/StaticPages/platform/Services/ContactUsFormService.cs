using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using static Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model.Airports;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Services
{
    public class ContactUsFormService : IContactUsFormService
    {
        private readonly IHelper _helper;
        public ContactUsFormService(IHelper helper)
        {
            this._helper = helper;
        }

        /// <summary>
        /// To get payment options
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        public ContactUsForm GetContactUsForm(Item dataSourceItem)
        {
            ContactUsForm contactUsForm = new ContactUsForm();

            if (dataSourceItem != null)
            {
                contactUsForm = new ContactUsForm()
                {
                    FormTitle = dataSourceItem.Fields[Template.ContactUsForm.FormTitleFieldId].ToString(),
                    FormSubtitle = dataSourceItem.Fields[Template.ContactUsForm.FormSubtitleFieldId].ToString(),
                    BannerTitle = dataSourceItem.Fields[Template.ContactUsForm.BannerTitleFieldId].ToString(),
                    BannerImage = _helper.GetImageURLByFieldId(dataSourceItem, Template.ContactUsForm.BannerImageFieldId),
                    MobileBannerImage = _helper.GetImageURLByFieldId(dataSourceItem, Template.ContactUsForm.MobileBannerImageFieldId),
                    ReachOutText = dataSourceItem.Fields[Template.ContactUsForm.ReachOutTextFieldId].ToString(),
                    SendQueryText = dataSourceItem.Fields[Template.ContactUsForm.SendQueryTextFieldId].ToString(),
                    FirstNameLabel = dataSourceItem.Fields[Template.ContactUsForm.FirstNameLabelFieldId].ToString(),
                    FirstNameRequired = dataSourceItem.Fields[Template.ContactUsForm.FirstNameRequiredFieldId].ToString(),
                    FirstNameIncorrect = dataSourceItem.Fields[Template.ContactUsForm.FirstNameIncorrectFieldId].ToString(),
                    LastNameLabel = dataSourceItem.Fields[Template.ContactUsForm.LastNameLabelFieldId].ToString(),
                    LastNameRequired = dataSourceItem.Fields[Template.ContactUsForm.LastNameRequiredFieldId].ToString(),
                    LastNameIncorrect = dataSourceItem.Fields[Template.ContactUsForm.LastNameIncorrectFieldId].ToString(),
                    MobileNoLabel = dataSourceItem.Fields[Template.ContactUsForm.MobileNoLabelFieldId].ToString(),
                    MobileNoIncorrect = dataSourceItem.Fields[Template.ContactUsForm.MobileNoIncorrectFieldId].ToString(),
                    MobileNoRequired = dataSourceItem.Fields[Template.ContactUsForm.MobileNoRequiredFieldId].ToString(),
                    EmailIdLabel = dataSourceItem.Fields[Template.ContactUsForm.EmailIdLabelFieldId].ToString(),
                    EmailIdRequired = dataSourceItem.Fields[Template.ContactUsForm.EmailIdRequiredFieldId].ToString(),
                    EmailIdIncorrect = dataSourceItem.Fields[Template.ContactUsForm.EmailIdIncorrectFieldId].ToString(),
                    HelpTextLabel = dataSourceItem.Fields[Template.ContactUsForm.HelpTextLabelFieldId].ToString(),
                    HelpTextRequired = dataSourceItem.Fields[Template.ContactUsForm.HelpTextRequiredFieldId].ToString(),
                    HelpTextMaxCharacter = dataSourceItem.Fields[Template.ContactUsForm.HelpTextMaxCharacterFieldId].ToString(),
                    HelpTextMaxCharacterMsg = dataSourceItem.Fields[Template.ContactUsForm.HelpTextMaxMsgFieldId].ToString(),
                    AirportRequired = dataSourceItem.Fields[Template.ContactUsForm.AirportRequiredFieldId].ToString(),
                    SelectAirportLabel = dataSourceItem.Fields[Template.ContactUsForm.SelectAirportLabelFieldId].ToString(),
                    FlightNumberLabel = dataSourceItem.Fields[Template.ContactUsForm.FlightNumberLabelFieldId].ToString(),
                    FlightDateLabel = dataSourceItem.Fields[Template.ContactUsForm.FlightDateLabelFieldId].ToString(),
                    IssueTypeLabel = dataSourceItem.Fields[Template.ContactUsForm.IssueTypeLabelFieldId].ToString(),
                    IssueTypeRequired = dataSourceItem.Fields[Template.ContactUsForm.IssueTypeRequiredFieldId].ToString(),
                    AirportDetails = GetAirportDetails(dataSourceItem),
                    IssueTypeList = GetIssueList(dataSourceItem),
                    TermsLabel = dataSourceItem.Fields[Template.ContactUsForm.TermsLabelFieldId].ToString(),
                    TermsRequired = dataSourceItem.Fields[Template.ContactUsForm.TermsRequiredFieldId].ToString(),
                    WritePlacholderLabel = dataSourceItem.Fields[Template.ContactUsForm.WritePlacholderLabelFieldId].ToString(),
                    SubmitButtonLabel = dataSourceItem.Fields[Template.ContactUsForm.SubmitBtnLabelFieldId].ToString()

                };
            }

            return contactUsForm;
        }



        #region Private Methods

        /// <summary>
        /// To get the Issue Type List
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private List<IssueType> GetIssueList(Item dataSourceItem)
        {

            ReferenceField referenceField = dataSourceItem.Fields[Template.ContactUsForm.IssueTypeListFieldId];

            List<IssueType> issueTypeList = new List<IssueType>();

            if (referenceField != null && referenceField.TargetItem != null)
            {
                ChildList childListItem = referenceField.TargetItem.GetChildren();

                foreach (Item option in childListItem)
                {
                    IssueType issueType = new IssueType()
                    {
                        IssueText = option.Fields[Template.ContactUsForm.IssueTextFieldId].ToString(),
                        IssueValue = option.Fields[Template.ContactUsForm.IssueValueFieldId].ToString(),
                        EmailFulfillmentMsg = option.Fields[Template.ContactUsForm.EmailFulfillmentFieldId].ToString(),
                        EmailSubmissionMsg = option.Fields[Template.ContactUsForm.EmailSubmissionFieldId].ToString(),
                        SMSFulfillmentMsg = option.Fields[Template.ContactUsForm.SMSFulfillmentFieldId].ToString(),
                        SMSSubmissionMsg = option.Fields[Template.ContactUsForm.SMSSubmissionFieldId].ToString(),
                        PopUpReOpenMsg = option.Fields[Template.ContactUsForm.PopUpReOpenFieldId].ToString(),
                        PopUpSuccessMsg = option.Fields[Template.ContactUsForm.PopUpSuccessFieldId].ToString(),
                        PopUpTitle = option.Fields[Template.ContactUsForm.PopUpTitleFieldId].ToString(),
                    };

                    issueTypeList.Add(issueType);
                }
            }

            return issueTypeList;
        }

        /// <summary>
        /// To get the details of Airport List
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private List<Airports> GetAirportDetails(Item dataSourceItem)
        {
            ReferenceField referenceField = dataSourceItem.Fields[Template.ContactUsForm.AirportListFieldId];

            List<Airports> airportList = new List<Airports>();

            if (referenceField != null && referenceField.TargetItem != null)
            {
                ChildList childListItem = referenceField.TargetItem.GetChildren();

                foreach (Sitecore.Data.Items.Item option in childListItem)
                {

                    Airports airportInfo = new Airports()
                    {


                        AirportCode = option.Fields[Template.ContactUsForm.AirportCodeFieldId].ToString(),
                        AirportName = option.Fields[Template.ContactUsForm.AirportNameFieldId].ToString(),
                        AirportAddress = option.Fields[Template.ContactUsForm.AirportAddressFieldId].ToString(),
                        AirportMobile = option.Fields[Template.ContactUsForm.AirportMobileFieldId].ToString(),
                        AirportEmail = option.Fields[Template.ContactUsForm.AirportEmailFieldId].ToString(),
                        SupportEmail = option.Fields[Template.ContactUsForm.SupportEmailFieldId].ToString(),
                        EmailText = option.Fields[Template.ContactUsForm.EmailTextFieldId].ToString(),
                        NodalName = option.Fields[Template.ContactUsForm.NodalNameFieldId].ToString(),
                        NodalMobile = option.Fields[Template.ContactUsForm.NodalMobileFieldId].ToString(),
                        NodalEmail = option.Fields[Template.ContactUsForm.NodalEmailFieldId].ToString(),
                        NodalEmail1 = option.Fields[Template.ContactUsForm.NodalEmail1FieldId].ToString(),
                        AppellateName = option.Fields[Template.ContactUsForm.AppellateNameFieldId].ToString(),
                        AppellateMobile = option.Fields[Template.ContactUsForm.AppellateMobileFieldId].ToString(),
                        AppellateEmail = option.Fields[Template.ContactUsForm.AppellateEmailFieldId].ToString(),
                        TerminalContent = option.Fields[Template.ContactUsForm.TerminalContentFieldId].ToString(),
                        FAQImage = _helper.GetImageURLByFieldId(option, Template.ContactUsForm.FAQImageFieldId),
                        FAQContent = option.Fields[Template.ContactUsForm.FAQContentFieldId].ToString(),
                        FAQTitle = option.Fields[Template.ContactUsForm.FAQTitleFieldId].ToString(),
                        FAQLink = _helper.LinkUrl(option.Fields[Template.ContactUsForm.FAQLinksFieldId]),
                        FAQLinkText = option.Fields[Template.ContactUsForm.FAQLinkTextFieldId].ToString(),
                        AuthoritiesContacts = GetAuthoritiesContacts(option),
                        TerminalContentTitle = option.Fields[Template.ContactUsForm.TerminalContentTitle].ToString(),
                        TerminalContentMain = option.Fields[Template.ContactUsForm.TerminalContentMain].ToString(),
                        MetaTitle = option.Fields[Template.ContactUsForm.MetaTitle].ToString(),
                        MetaDescription = option.Fields[Template.ContactUsForm.MetaDescription].ToString(),
                        Keywords = option.Fields[Template.ContactUsForm.Keywords].ToString(),
                        Canonical = _helper.LinkUrl(option.Fields[Template.ContactUsForm.Canonical]),
                        Viewport = option.Fields[Template.ContactUsForm.Viewport].ToString(),
                        Robots = option.Fields[Template.ContactUsForm.Robots].ToString(),
                        OG_Title = option.Fields[Template.ContactUsForm.OG_Title].ToString(),
                        OG_Image = option.Fields[Template.ContactUsForm.OG_Image].ToString(),
                        OG_Description = option.Fields[Template.ContactUsForm.OG_Description].ToString(),

                };

                    IEnumerable<Item> childListTerminal = option.Children.Where(x => x.TemplateID.ToString() == Template.ContactUsForm.TerminalHeadlineTemplateId).ToList();
                    if(childListTerminal != null)
                    {
                        List<Terminal> terminalList = new List<Terminal>();
                        foreach (Item terminalItem in childListTerminal)
                        {
                                Terminal terminal = new Terminal()
                                {
                                    TerminalName = terminalItem.Fields[Template.ContactUsForm.TerminalNameFieldId].ToString(),
                                    ContactList = GetContactList(terminalItem, Template.ContactUsForm.TerminalContactListFieldId),
                                    ImmigrationTitle = terminalItem.Fields[Template.ContactUsForm.TerminalImmTitleFieldId].ToString(),
                                    Immigration = GetContactList(terminalItem, Template.ContactUsForm.TerminalImmigrationFieldId),
                                    MinistryCivilTitle = terminalItem.Fields[Template.ContactUsForm.TerminalCivilTitleFieldId].ToString(),
                                    MinistryCivil = GetContactList(terminalItem, Template.ContactUsForm.TerminalCivilFieldId)
                                };
                                terminalList.Add(terminal);
                        }
                        airportInfo.TerminalDetails = new List<Terminal>();
                        airportInfo.TerminalDetails.AddRange(terminalList);
                        airportList.Add(airportInfo);
                    }

                }
            }

            return airportList;
        }


        /// <summary>
        /// To Get contact List
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private List<TerminalItem> GetContactList(Item dataSourceItem, string fieldId)
        {
            List<TerminalItem> terminalItems = new List<TerminalItem>();

            MultilistField contactLists = dataSourceItem.Fields[fieldId];

            if (contactLists != null && contactLists.GetItems() != null)
            {
                foreach (Item contactItem in contactLists.GetItems())
                {
                    TerminalItem terminalItem = new TerminalItem()
                    {
                        TerminalContactName = contactItem.Fields[Template.ContactUsForm.TerminalContactNameFieldId].ToString(),
                        DepartureContactNo = contactItem.Fields[Template.ContactUsForm.DepartureContactNoFieldId].ToString(),
                        ArrivalContactNo = contactItem.Fields[Template.ContactUsForm.ArrivalContactNoField].ToString(),
                    };

                    terminalItems.Add(terminalItem);
                }
            }

            return terminalItems;
        }

        private List<AuthoritiesContacts> GetAuthoritiesContacts(Item dataSourceItem)
        {
            IEnumerable<Item> authoritiesContacts = dataSourceItem.Children.Where(x => x.TemplateID.ToString() == Template.ContactUsForm.AuthoritiesContactsTemplateID).ToList();
            List<AuthoritiesContacts> authoritiesContact = new List<AuthoritiesContacts>();
            if (authoritiesContacts != null)
            {
                foreach (Item item in authoritiesContacts)
                {
                    AuthoritiesContacts contacts = new AuthoritiesContacts
                    {
                        Title = item.Fields[Template.ContactUsForm.AuthorityTitle].ToString(),
                        Name = item.Fields[Template.ContactUsForm.AuthorityName].ToString(),
                        Mobile = item.Fields[Template.ContactUsForm.AuthorityMobile].ToString(),
                        Email = item.Fields[Template.ContactUsForm.AuthorityEmail].ToString()
                    };
                    authoritiesContact.Add(contacts);
                }
            }

            return authoritiesContact;
            #endregion Private Methods
        }
    }
}