using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Sitecore.Feature.Accounts.Models;

namespace Sitecore.Feature.Accounts.Services
{
    public class AdaniGasNewConnectionServices
    {
        public void StoreNewConnectionDataAdaniGas(NewConnectionModel model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StoreNewConnectionDataAdaniGas Store request in Database EnquiryNo : - " + model.EnquiryNo, this);
                        using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                        {
                            NewConnectionData objdata = new NewConnectionData();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            objdata.EnquiryNo = model.EnquiryNo;
                            objdata.Name = model.FullName ?? string.Empty;
                            objdata.ContactNo = model.Mobile ?? string.Empty;
                            objdata.ConnectionTypeCode = model.Partner_Type ?? string.Empty;
                            objdata.ConnectionType = model.PartnerTypeList.Where(x=>x.Value==model.Partner_Type).Select(x=>x.Text).FirstOrDefault() ?? string.Empty;
                            objdata.CityName = model.CityList.Where(x=>x.Value==model.City).Select(x=>x.Text).FirstOrDefault() ?? string.Empty;
                            objdata.CityCode = model.City?? string.Empty;
                            objdata.AreaCode = model.Area ?? string.Empty;
                            objdata.AreaName = model.AreaList.Where(x=>x.Value==model.Area).Select(x=>x.Text).FirstOrDefault() ?? string.Empty;
                            objdata.TypeOfHouse = model.HouseType ?? string.Empty;
                            objdata.ApartmentComplex = model.ApartmentComplex ?? string.Empty;
                            objdata.OtherApartmentOrSociety = model.OtherApartmentComplex ?? string.Empty;
                            objdata.HouseNo_BuildingNo = model.HouseNo ?? string.Empty;
                            objdata.TypeOfCustomer = model.TypeOfCustomer ?? string.Empty;
                            objdata.OtherTypeOfCustomer = model.OtherTypeOfCustomer ?? string.Empty;
                            objdata.Application = model.Application ?? string.Empty;
                            objdata.OtherApplication = model.OtherApplication ?? string.Empty;
                            objdata.TypeOfIndustry = model.TypeOfIndustry ?? string.Empty;
                            objdata.OtherTypeOfIndustry = model.OtherTypeOfIndustry ?? string.Empty;
                            objdata.CurrentFuelUsing = model.CurrentFuelUse ?? string.Empty;
                            objdata.OtherFuelUsing = model.OtherCurrentFuelUse ?? string.Empty;
                            objdata.MonthlyConsumption = model.MonthlyConsumption ?? string.Empty;
                            objdata.AddressLine1 = model.AddressLine1 ?? string.Empty;
                            objdata.AddressLine2 = model.AddressLine2 ?? string.Empty;
                            objdata.Pincode = model.Pincode ?? string.Empty;
                            objdata.ReferenceSource = model.ReferenceSource;
                            objdata.CampaignID = model.CampaignID;
                            objdata.FormURL = model.FormURL;
                            objdata.SubmitOn = DateTime.Now;
                            objdata.SubmittedBy = model.FullName;

                            dbcontext.NewConnectionDatas.InsertOnSubmit(objdata);
                            dbcontext.SubmitChanges();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at submitting form in db for AdaniGas NewConnectionEnquiry " + model.Mobile + ": " + ex.Message, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at submitting form in db for AdaniGas NewConnectionEnquiry " + model.Mobile + ":"+ ex.Message, this);
            }
        }

        public void StoreNewConnectionDataShortformAdaniGas(NewConnectionModel model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StoreNewConnectionDataAdaniGas Store request in Database EnquiryNo : - " + model.EnquiryNo, this);
                        using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                        {
                            NewConnectionData objdata = new NewConnectionData();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            objdata.EnquiryNo = model.EnquiryNo;
                            objdata.Name = model.FullName ?? string.Empty;
                            objdata.ContactNo = model.Mobile ?? string.Empty;
                            objdata.ConnectionTypeCode = "9004";
                            objdata.ConnectionType = "Residential";
                            objdata.CityName = model.CityList.Where(x => x.Value == model.City).Select(x => x.Text).FirstOrDefault() ?? string.Empty;
                            objdata.CityCode = model.City ?? string.Empty;
                            objdata.AreaCode = model.Area ?? string.Empty;
                            objdata.AreaName = model.Area;
                            
                            //objdata.ReferenceSource = model.ReferenceSource;
                            objdata.CampaignID = model.CampaignID;
                            objdata.FormURL = model.FormURL;
                            objdata.SubmitOn = DateTime.Now;
                            objdata.SubmittedBy = model.FullName;

                            dbcontext.NewConnectionDatas.InsertOnSubmit(objdata);
                            dbcontext.SubmitChanges();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at submitting form in db for AdaniGas NewConnectionEnquiry " + model.Mobile + ": " + ex.Message, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at submitting form in db for AdaniGas NewConnectionEnquiry " + model.Mobile + ":" + ex.Message, this);
            }
        }

        public string GetUniqueRegNo()
        {
            int num = 6;
            char[] charArray = new char[62];
            charArray = "1234567890".ToCharArray();
            int num1 = num;
            byte[] numArray = new byte[1];
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            rNGCryptoServiceProvider.GetNonZeroBytes(numArray);
            num1 = num;
            numArray = new byte[num1];
            rNGCryptoServiceProvider.GetNonZeroBytes(numArray);
            StringBuilder stringBuilder = new StringBuilder(num1);
            byte[] numArray1 = numArray;
            for (int i = 0; i < (int)numArray1.Length; i++)
            {
                byte num2 = numArray1[i];
                stringBuilder.Append(charArray[num2 % ((int)charArray.Length - 1)]);
            }
            string UniqueNo =stringBuilder.ToString();
            return UniqueNo;
        }
    }
}