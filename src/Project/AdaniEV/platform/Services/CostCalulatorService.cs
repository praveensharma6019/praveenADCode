using Adani.EV.Project.Helper;
using Adani.EV.Project.Models;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Services
{
    public class CostCalulatorService : ICostCalculatorService
    {
        public ContactusSectionsModel GetcontactusSectionsModel(Rendering rendering)
        {
            ContactusSectionsModel contactusSectionsModel = new ContactusSectionsModel();   
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
                if (datasource == null)
                {
                    Sitecore.Diagnostics.Log.Info("GetcontactusSectionsModel : Datasource is empty", this);
                    return default;
                }

                contactusSectionsModel.banner.title = datasource.Fields[Templates.ContactusBanner.Fields.Title] != null ? datasource.Fields[Templates.ContactusBanner.Fields.Title].Value : "";
                contactusSectionsModel.banner.imageSrc = Utils.GetImageURLByFieldId(datasource, Templates.ContactusBanner.Fields.image);

                contactusSectionsModel.reachout.title = datasource.Fields[Templates.ContactusReachout.Fields.Title] != null ? datasource.Fields[Templates.ContactusReachout.Fields.Title].Value : "";
             
                contactusSectionsModel.reachout.addressdetails.iconsrc = Utils.GetImageURLByFieldId(datasource, Templates.ContactusReachout.Fields.Addressicon);
                contactusSectionsModel.reachout.addressdetails.address = datasource.Fields[Templates.ContactusReachout.Fields.Address] != null ? datasource.Fields[Templates.ContactusReachout.Fields.Address].Value : "";

                contactusSectionsModel.reachout.contactdetails.iconsrc = Utils.GetImageURLByFieldId(datasource, Templates.ContactusReachout.Fields.contacticon);
                contactusSectionsModel.reachout.contactdetails.phone = datasource.Fields[Templates.ContactusReachout.Fields.contactPhone] != null ? datasource.Fields[Templates.ContactusReachout.Fields.contactPhone].Value : "";


                contactusSectionsModel.reachout.emaildetails.iconsrc = Utils.GetImageURLByFieldId(datasource, Templates.ContactusReachout.Fields.emailicon);
                contactusSectionsModel.reachout.emaildetails.email = datasource.Fields[Templates.ContactusReachout.Fields.email] != null ? datasource.Fields[Templates.ContactusReachout.Fields.email].Value : "";

                contactusSectionsModel.contactusform.title = datasource.Fields[Templates.Contactusformdetails.Fields.title] != null ? datasource.Fields[Templates.Contactusformdetails.Fields.title].Value : "";
                contactusSectionsModel.contactusform.details = datasource.Fields[Templates.Contactusformdetails.Fields.Details] != null ? datasource.Fields[Templates.Contactusformdetails.Fields.Details].Value : "";
                contactusSectionsModel.contactusform.checkboxtext = datasource.Fields[Templates.Contactusformdetails.Fields.AgreementInformation] != null ? datasource.Fields[Templates.Contactusformdetails.Fields.AgreementInformation].Value : "";
                contactusSectionsModel.contactusform.buttonname = datasource.Fields[Templates.Contactusformdetails.Fields.ButtonName] != null ? datasource.Fields[Templates.Contactusformdetails.Fields.ButtonName].Value : "";
                contactusSectionsModel.contactusform.flagSrc = Utils.GetImageURLByFieldId(datasource, Templates.Contactusformdetails.Fields.Flagsrc);

                contactusSectionsModel.faq.title = datasource.Fields[Templates.Contactusfaq.Fields.FaqTitle] != null ? datasource.Fields[Templates.Contactusfaq.Fields.FaqTitle].Value : "";
                contactusSectionsModel.faq.details = datasource.Fields[Templates.Contactusfaq.Fields.Faqdetails] != null ? datasource.Fields[Templates.Contactusfaq.Fields.Faqdetails].Value : "";
                contactusSectionsModel.faq.link = Utils.GetLinkURL(datasource?.Fields[Templates.Contactusfaq.Fields.FaqLink]);
                contactusSectionsModel.faq.imageSrc = Utils.GetImageURLByFieldId(datasource, Templates.Contactusfaq.Fields.FaqImage);


                return contactusSectionsModel;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
            }
        }

        public CostCalulatorSectionsModel GetCostCalulatorSectionsModel(Rendering rendering)
        {
            CostCalulatorSectionsModel costCalulatorSectionsModel = new CostCalulatorSectionsModel();   

            try
			{
                var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
                if (datasource == null)
                {
                    Sitecore.Diagnostics.Log.Info("GetCostCalulatorSectionsModel : Datasource is empty", this);
                    return default;
                }

                MultilistField widgetsMultilistField = datasource.Fields[Templates.Calculator.Fields.widgets];

                if (widgetsMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in widgetsMultilistField.GetItems())
                    {
                        BenefitItemModel ItemsData = new BenefitItemModel();                    
                        ItemsData.title = galleryItem.Fields[Templates.CalculatorItem.Fields.Title].Value;
                        ItemsData.imgsrc = Utils.GetImageURLByFieldId(galleryItem, Templates.CalculatorItem.Fields.Image);                      
                        costCalulatorSectionsModel.calculator.widget.fields.Add(ItemsData);
                    }
                }

                costCalulatorSectionsModel.calculator.widget.notes.note1 = datasource.Fields[Templates.BannerNoteDetails.Fields.Note1] != null ? datasource.Fields[Templates.BannerNoteDetails.Fields.Note1].Value : "";
                costCalulatorSectionsModel.calculator.widget.notes.note2 = datasource.Fields[Templates.BannerNoteDetails.Fields.Note2] != null ? datasource.Fields[Templates.BannerNoteDetails.Fields.Note2].Value : "";


                costCalulatorSectionsModel.benefits.title= datasource.Fields[Templates.Benefits.Fields.BenefitsTitle] != null ? datasource.Fields[Templates.Benefits.Fields.BenefitsTitle].Value : "";
                costCalulatorSectionsModel.benefits.detail= datasource.Fields[Templates.Benefits.Fields.BenefitsDetail] != null ? datasource.Fields[Templates.Benefits.Fields.BenefitsDetail].Value : "";
                MultilistField galleryMultilistField = datasource.Fields[Templates.Benefits.Fields.BenefitsGroupOfBenefits];

                if (galleryMultilistField.Count > 0)
                {
                    foreach (Item galleryItem in galleryMultilistField.GetItems())
                    {
                        BenefitItemModel ItemsData = new BenefitItemModel();
                        ItemsData.id = galleryItem.Fields[Templates.GroupOfBenefitItem.Fields.Id].Value;
                        ItemsData.title = galleryItem.Fields[Templates.GroupOfBenefitItem.Fields.Title].Value;
                        ItemsData.detail = galleryItem.Fields[Templates.GroupOfBenefitItem.Fields.Detail].Value; 
                        ItemsData.imgsrc = Utils.GetImageURLByFieldId(galleryItem, Templates.GroupOfBenefitItem.Fields.Image);
                        costCalulatorSectionsModel.benefits.fields.Add(ItemsData);
                    }
                }

                return costCalulatorSectionsModel;

            }
			catch (Exception ex)
			{
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                return default;
			}
        }
    }
}