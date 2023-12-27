using System.Collections.Specialized;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class PromotionOffer: AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");

            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
            {
                return null;
            }

            if (!item.TemplateID.Equals(Templates.OfferCollection.PromoOfferTemplateID))
            {
                return null;
            }
            string OfferTitle =!string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionTitleFieldID].Value)? 
                                item.Fields[Templates.OfferCollection.PromotionTitleFieldID].Value:
                                string.Empty;
            string PromotionType= !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionTypeFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionTypeFieldID].Value :
                                string.Empty;
            string PromotionCode = !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionCodeFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionCodeFieldID].Value :
                                string.Empty;
            string OfferType = !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionOfferTypeFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionOfferTypeFieldID].Value :
                                string.Empty;
            string DisplayText = !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionDisplayTextFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionDisplayTextFieldID].Value :
                                string.Empty;
            string TerminalStoreType = !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionTerminalStoreTypeFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionTerminalStoreTypeFieldID].Value :
                                string.Empty;
            string OfferDescription = !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionOfferDescriptionFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionOfferDescriptionFieldID].Value :
                                string.Empty;
            string Termandcondition = !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionOfferTermAndConditionFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionOfferTermAndConditionFieldID].Value :
                                string.Empty;
            string PromotionTypeLabel= !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionOfferLabelTypeFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionOfferLabelTypeFieldID].Value :
                                string.Empty;
            string PromotionTabTitle = !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionOfferTabTitleFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionOfferTabTitleFieldID].Value :
                                string.Empty;


            StringCollection OfferFullTextSearch = new StringCollection
            {
                OfferTitle?.ToLower(),
                PromotionType?.ToLower(),
                PromotionCode?.ToLower(),
                OfferType?.ToLower(),
                DisplayText?.ToLower(),
                TerminalStoreType?.ToLower(),
                OfferDescription?.ToLower(),
                Termandcondition?.ToLower(),
                PromotionTypeLabel?.ToLower(),
                PromotionTabTitle?.ToLower()
            };
            string.Join(" ", OfferFullTextSearch);

            return OfferFullTextSearch;
        }
    }
}