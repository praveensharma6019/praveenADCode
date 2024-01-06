using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Realty.Website.Model
{
    [Serializable]
    public class PropertyCollection
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PropertyCollection))]
        [RegularExpression(@"^\d{12}$", ErrorMessageResourceName = nameof(InvalidAadhaar), ErrorMessageResourceType = typeof(PropertyCollection))]
        public string AadharNumber { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PropertyCollection))]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}", ErrorMessageResourceName = nameof(InvalidPAN), ErrorMessageResourceType = typeof(PropertyCollection))]
        public string PanNumber { get; set; }
        
        public string PropertyCode { get; set; }
        public string CreatedBy { set; get; }
        public DateTime CreatedDate { set; get; }
        public string OrderId { set; get; }
        public string PaymentStatus { set; get; }
        public string PaymentType { set; get; }
        public string CurrencyType { set; get; }
        public string PaymentGateway { set; get; }
        public string msg { set; get; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PropertyCollection))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(PropertyCollection))]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PropertyCollection))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(PropertyCollection))]
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Remarks { get; set; }
        public string PropertyType { get; set; }
        public string PropertyName { get; set; }
        public string ParentProject { get; set; }
        public string PaymentAmount { get; set; }
        public string FormName { set; get; }
        public string PageInfo { get; set; }
        public List<Property> PropertyCollectionLists { get; set; }
        public PropertyCollection()
        {
            PropertyCollectionLists = new List<Property>();
        }

        public string TransactionId { get; set; }
        public string Responsecode { get; set; }
        public string ResponseMessage { get; set; }
        public string PaymentRef { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionDate { get; set; }
        public string FullName { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Realty/Booknow/Required", "Please enter value for {0}");

        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Realty/Booknow/Invalid Email Address", "Please enter a valid email address");
        public static string InvalidPAN => DictionaryPhraseRepository.Current.Get("/Realty/Booknow/Invalid Pan details", "Please enter a valid PAN details");
        public static string InvalidAadhaar => DictionaryPhraseRepository.Current.Get("/Realty/Booknow/Invalid Aadhaar", "Please enter a valid Aadhaar Number");
        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Realty/Booknow/Invalid Mobile", "Please enter a valid Mobile Number");
    }





    [Serializable]
    public class Property
    {
        public string PropertyName { get; set; }
        public string PropertyAddress { get; set; }
        public List<PropertyType> PropertyTypes { get; set; }
        public string PropertyConfiguration { get; set; }
        public string PropertyPossession { get; set; }
        public string PropertyLogo { get; set; }
        public string PropertyCode { get; set; }
        public string ParentProject { get; set; }
        public Property()
        {
            PropertyTypes = new List<PropertyType>();
        }

    }


    [Serializable]
    public class PropertyType
    {
        public string PropertySelection { get; set; }
        public string CarpetArea { get; set; }
        public string BookingPrice { get; set; }
        public string BookingAmount { get; set; }
    }

    [Serializable]
    public class PropertyTypeSelect
    {
        public string PropertySelection { get; set; }
    }

    [Serializable]
    public class PropertyTypeDetails
    {
        public List<PropertyTypeSelect> PropertiesTypeList { get; set; }
        public string CarpetArea { get; set; }
        public string BookingPrice { get; set; }
        public string BookingAmount { get; set; }
        public string PropertyCode { get; set; }
        public PropertyTypeDetails()
        {
            PropertiesTypeList = new List<PropertyTypeSelect>();
        }
    }

    public class RealtyHelper
    {
        public static string GetPropertyName(string PropCode)
        {
            Item Properties = Context.Database.GetItem(Templates.Pages.PropertyList);
            string PropertyName = string.Empty;
            if(Properties != null)
            {
                if(Properties.HasChildren)
                {
                    foreach(Item p in Properties.GetChildren())
                    {
                        if(p.Fields["PropertyCode"].Value.Trim()==PropCode.Trim())
                        {
                            PropertyName = p.Fields["Property Name"].Value;
                        }
                    }
                }
            }
            return PropertyName;
        }
        public static string GetParentProject(string PropCode)
        {
            Item Properties = Context.Database.GetItem(Templates.Pages.PropertyList);
            string ParentProjectName = string.Empty;
            if (Properties != null)
            {
                if (Properties.HasChildren)
                {
                    foreach (Item p in Properties.GetChildren())
                    {
                        if (p.Fields["PropertyCode"].Value.Trim() == PropCode.Trim())
                        {
                            ReferenceField field = p.Fields["ParentProject"];
                            if (field == null || field.TargetItem == null)
                            {
                                return ParentProjectName;
                            }
                            else
                            {
                                if(!string.IsNullOrWhiteSpace(field.TargetItem.Fields["Text"].Value))
                                {
                                    ParentProjectName = field.TargetItem.Fields["Text"].Value.ToUpper();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return ParentProjectName;
        }
        public static PropertyCollection GetPropertyList(string PropCode)
        {
            PropertyCollection propertyCollection = new PropertyCollection();
            Item Properties = Context.Database.GetItem(Templates.Pages.PropertyList);
            if (Properties != null)
            {
                if (Properties.HasChildren)
                {
                    foreach (Item p in Properties.GetChildren())
                    {
                        Property PropertyInfo = new Property();
                        PropertyInfo.PropertyName = p["Property Name"];
                        Sitecore.Data.Fields.ImageField imgField = ((Sitecore.Data.Fields.ImageField)p.Fields["Property Logo"]);
                        string url = Sitecore.Resources.Media.MediaManager.GetMediaUrl(imgField.MediaItem);
                        PropertyInfo.PropertyLogo = url;
                        PropertyInfo.PropertyAddress = p["Property Address"];
                        PropertyInfo.PropertyConfiguration = p["Property Configuration"];
                        PropertyInfo.PropertyPossession = p["Property Possession"];
                        PropertyInfo.PropertyCode = p["PropertyCode"];
                        if(p["PropertyCode"].ToString() == PropCode)
                        {
                            foreach (var items in p.Children.ToList())
                            {
                                PropertyType PropertyTypess = new PropertyType();
                                PropertyTypess.CarpetArea = items["Carpet Area"];
                                PropertyTypess.PropertySelection = items["Property Type"];
                                PropertyTypess.BookingPrice = items["Starting Price"];
                                PropertyTypess.BookingAmount = items["Booking Amount"];
                                PropertyInfo.PropertyTypes.Add(PropertyTypess);

                            }
                        }                        
                        propertyCollection.PropertyCollectionLists.Add(PropertyInfo);
                    }
                }
            }
            return propertyCollection;
        }
        public static string GetPropertyBookingAmount(string PropCode,string PropType)
        {
            string Amt = string.Empty;
            try
            {
                Item Properties = Context.Database.GetItem(Templates.Pages.PropertyList);
                if (Properties != null)
                {
                    if (Properties.HasChildren)
                    {
                        foreach (Item p in Properties.GetChildren())
                        {
                            if (p.Fields["PropertyCode"].Value.Trim() == PropCode.Trim())
                            {
                                if (p.HasChildren)
                                {
                                    foreach (Item ptype in p.GetChildren())
                                    {
                                        if (!string.IsNullOrEmpty(ptype.Fields["Booking Amount"].Value) && !string.IsNullOrWhiteSpace(ptype.Fields["Booking Amount"].Value))
                                        {
                                            Amt = ptype.Fields["Booking Amount"].Value;
                                        }
                                        break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                return Amt;
            }
            catch(Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Error at Fetching Amount to be paid for booking  : " + e.Message, e.Message);
                return Amt;
            }
        }
    }


}
