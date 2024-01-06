extern alias itextsharp;
using itextsharp::iTextSharp.text;
using itextsharp::iTextSharp.text.pdf;
using CaptchaMvc.HtmlHelpers;
using DotNetIntegrationKit;
using Newtonsoft.Json;
using paytm;
using RestSharp;
using SapPiService.Domain;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.Repositories;
using Sitecore.Feature.Accounts.Services;
using Sitecore.Foundation.Alerts.Models;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Attributes;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;
using System.Xml.Linq;
using Sitecore.Feature.Accounts.SessionHelper;
using System.Data;
using System.Diagnostics;
using static Sitecore.Feature.Accounts.Controllers.AccountsController;
using System.Drawing.Imaging;
using Sitecore.Feature.Accounts.UploadDocuments.AEMLDocuments;

namespace Sitecore.Feature.Accounts.UploadDocuments.AEMLDocuments
{
    public class AEMLNewConnectionLibrary
    {
        public static void GetApplicationDetail(NewConnectionService newconnectionservice, NewConnectionApplication model)
        {
            GetSuburbList(model.SelectedPincode, model);
            GetConnectionTypeList(newconnectionservice, model, model.TotalLoad);
            GetVoltageLevel(newconnectionservice, model, model.TotalLoad);
            GetMeterType(newconnectionservice, model);
            GetPurposeOfSupply(newconnectionservice, model);
        }
        public static void GetSuburbList(string pincode, NewConnectionApplication model)
        {
            NewConnectionService newconnectionservice = new NewConnectionService();
            var listAllArea = newconnectionservice.ListAreaPinWorkcenterMapping();

            if (!string.IsNullOrEmpty(pincode) && listAllArea.Any(a => a.Pincode == pincode))
            {
                var cityList = listAllArea.Where(a => a.Pincode == pincode).Select(c => c.Area).Distinct().ToList();
                model.SuburbSelectList = new List<SelectListItem>();
                if (cityList != null && cityList.Any())
                {
                    if (cityList.Count == 1)
                    {
                        model.SelectedSuburb = cityList[0].ToString();

                    }
                    foreach (var item in cityList)
                    {
                        model.SuburbSelectList.Add(new SelectListItem
                        {
                            Text = item,
                            Value = item
                        });
                    }
                }


            }
        }
        public static void GetLDPList(string pincode, NewConnectionApplication model)
        {
            NewConnectionService newconnectionservice = new NewConnectionService();
            var listAllArea = newconnectionservice.ListAreaPinWorkcenterMapping();
            if (!string.IsNullOrEmpty(pincode) && listAllArea.Any(a => a.Pincode == pincode))
            {
                var dummyldp = listAllArea.Where(a => a.Pincode == pincode).Select(c => c.DummyLDP).Distinct().ToList();
                model.SuburbSelectList = new List<SelectListItem>();
                if (dummyldp != null && dummyldp.Any())
                {

                    model.LDPOrderNumber = dummyldp[0].ToString();

                }

            }
        }
        public static void GetApplicationSubtypeList(NewConnectionApplication model)
        {
            NewConnectionService newconnectionservice = new NewConnectionService();
            var ApplicationSubType = newconnectionservice.ApplicationSubTypeList();
            model.ApplicationsubtypeList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(model.ApplicationType))
            {

                if (ApplicationSubType != null && ApplicationSubType.Any())
                {
                    if (ApplicationSubType.Count == 1)
                    {
                        model.SelectedConnectionType = ApplicationSubType[0].ApplicationSubtype.ToString();
                    }
                    foreach (var item in ApplicationSubType.OrderBy(a => a.ApplicationSubtype).Distinct().ToList())
                    {
                        model.ApplicationsubtypeList.Add(new SelectListItem
                        {
                            Text = item.ApplicationSubtypeDesc,
                            Value = item.ApplicationSubtype.ToString()
                        });
                    }
                }
            }
            else
            {
                model.ApplicationsubtypeList = new List<SelectListItem>();
            }
        }
        public static double CalculateTotalLoad(NewConnectionApplication model)
        {
            if (!string.IsNullOrEmpty(model.ConnectedLoadHP) && ((!string.IsNullOrEmpty(model.ConnectedLoadKW))))
            {
                model.TotalLoad = System.Convert.ToDouble(model.ConnectedLoadKW.Trim()) + (System.Convert.ToDouble(model.ConnectedLoadHP.Trim()) * 0.746);
            }
            else if (!string.IsNullOrEmpty(model.ConnectedLoadHP))
            {
                model.TotalLoad = 0 + (System.Convert.ToDouble(model.ConnectedLoadHP.Trim()) * 0.746);

            }
            else if (!string.IsNullOrEmpty(model.ConnectedLoadKW))
            {
                model.TotalLoad = System.Convert.ToDouble(model.ConnectedLoadKW.Trim()) + (0 * 0.746);

            }
            double TotalLoad = Math.Round(model.TotalLoad, 3);

            return TotalLoad;
        }
        public static double CalculateNormativeCharge(NewConnectionApplication model)
        {
            NewConnectionService newConnectionservice = new NewConnectionService();
            var lsttypeofconnection = newConnectionservice.ListNormativeChargeMapping();
            foreach (var item in lsttypeofconnection)
            {
                if (item.Phase == "3PH" && model.TotalLoad > 33.5 && model.TotalLoad <= 67 && item.LoadUnit == "HP")
                {

                    double NormativeCharge = (System.Double.Parse(item.ServiceLineRate) - System.Double.Parse(item.ApplicationCharge));
                    model.DedicatedDistributionfacilityRs = Math.Round(NormativeCharge, 2);
                }
                else if (item.LoadUnit == "HP" && item.Phase == "3PH" && model.TotalLoad > 67.01 && model.TotalLoad <= 201)
                {

                    double NormativeCharge = (System.Double.Parse(item.ServiceLineRate) - System.Double.Parse(item.ApplicationCharge));
                    model.DedicatedDistributionfacilityRs = Math.Round(NormativeCharge, 2);
                }
                else if (item.LoadUnit == "HP" && item.Phase == "3PH" && model.TotalLoad > 201.01 && model.TotalLoad <= 99999.00)
                {

                    double NormativeCharge = (System.Double.Parse(item.ServiceLineRate) - System.Double.Parse(item.ApplicationCharge));
                    model.DedicatedDistributionfacilityRs = Math.Round(NormativeCharge, 2);
                }
                else if (item.LoadUnit == "KVA" && item.Phase == "HT" && model.TotalLoad > 0.01 && model.TotalLoad <= 500)
                {

                    double NormativeCharge = (System.Double.Parse(item.ServiceLineRate) - System.Double.Parse(item.ApplicationCharge));
                    model.DedicatedDistributionfacilityRs = Math.Round(NormativeCharge, 2);
                }
                else if (item.LoadUnit == "KVA" && item.Phase == "HT" && model.TotalLoad > 500.01 && model.TotalLoad <= 99999.00)
                {

                    double NormativeCharge = (System.Double.Parse(item.ServiceLineRate) - System.Double.Parse(item.ApplicationCharge));
                    model.DedicatedDistributionfacilityRs = Math.Round(NormativeCharge, 2);
                }
                else if (item.LoadUnit == "KW" && item.Phase == "3PH" && model.TotalLoad > 25.01 && model.TotalLoad <= 50)
                {

                    double NormativeCharge = (System.Double.Parse(item.ServiceLineRate) - System.Double.Parse(item.ApplicationCharge));
                    model.DedicatedDistributionfacilityRs = Math.Round(NormativeCharge, 2);
                }
                else if (item.LoadUnit == "KW" && item.Phase == "3PH" && model.TotalLoad > 50.01 && model.TotalLoad <= 160)
                {

                    double NormativeCharge = (System.Double.Parse(item.ServiceLineRate) - System.Double.Parse(item.ApplicationCharge));
                    model.DedicatedDistributionfacilityRs = Math.Round(NormativeCharge, 2);
                }
                else if (item.LoadUnit == "KW" && item.Phase == "3PH" && model.TotalLoad > 160.01 && model.TotalLoad <= 99999.00)
                {

                    double NormativeCharge = (System.Double.Parse(item.ServiceLineRate) - System.Double.Parse(item.ApplicationCharge));
                    model.DedicatedDistributionfacilityRs = Math.Round(NormativeCharge, 2);
                }
            }
            double TotalLoads = Math.Round(model.DedicatedDistributionfacilityRs, 2);


            return TotalLoads;

        }








        public static void GetConnectionTypeList(NewConnectionService newconnectionservice, NewConnectionApplication model, double TotalLoad)
        {
            var listConnectionType = newconnectionservice.ListConnectionTypeMapping();
            model.ConnectionTypeSelectList = new List<SelectListItem>();
            var ConnectionTypeList = new List<NEWCON_MST_CONNECTION_TYPE>();
            if (TotalLoad > 0 && TotalLoad <= 8)
                ConnectionTypeList = listConnectionType.Where(a => a.CONNECTION_TYPE_DESC == "LT:Low Tension").Distinct().ToList();
            else if (TotalLoad >= 8.1 && TotalLoad <= 160)
                ConnectionTypeList = listConnectionType.Where(a => a.CONNECTION_TYPE_DESC == "LT:Low Tension").Distinct().ToList();
            else if (TotalLoad >= 160.001 && TotalLoad <= 480)
                ConnectionTypeList = listConnectionType.Where(a => a.CONNECTION_TYPE_DESC == "HT:HIGH Tension" || a.CONNECTION_TYPE_DESC == "LT:Low Tension").Distinct().ToList();
            else if (TotalLoad >= 480.001 && TotalLoad <= 99999)
                ConnectionTypeList = listConnectionType.Where(a => a.CONNECTION_TYPE_DESC == "HT:HIGH Tension").Distinct().ToList();
            if (ConnectionTypeList != null && ConnectionTypeList.Any())
            {
                if (ConnectionTypeList.Count == 1)
                {
                    model.SelectedConnectionType = ConnectionTypeList[0].CONNECTION_TYPE.ToString();
                }
                foreach (var item in ConnectionTypeList)
                {
                    model.ConnectionTypeSelectList.Add(new SelectListItem
                    {
                        Text = item.CONNECTION_TYPE_DESC,
                        Value = item.CONNECTION_TYPE.ToString()
                    });
                }
            }
        }
        public static void GetVoltageLevel(NewConnectionService newconnectionservice, NewConnectionApplication model, double TotalLoad)
        {
            var listVoltageLevel = newconnectionservice.ListVoltageLevelMapping();
            model.VoltageLevelSelectList = new List<SelectListItem>();
            var VoltageTypeList = new List<NEWCON_MST_VOLTAGE_LEVEL>();

            model.ConnectionTypeCode = (model.SelectedConnectionType == "1" ? "HT" : "LT");
            if (TotalLoad > 0 && TotalLoad <= 8 && model.ConnectionTypeSelectList.Count == 1 && !string.IsNullOrEmpty(model.SelectedConnectionType))
                VoltageTypeList = listVoltageLevel.Where(a => a.CONNECTION_TYPE == model.SelectedConnectionType && a.DESCRIPTION == "440 V" || a.DESCRIPTION == "230 V").ToList();
            else if (TotalLoad >= 8.1 && TotalLoad <= 160 && model.ConnectionTypeSelectList.Count == 1 && !string.IsNullOrEmpty(model.SelectedConnectionType))
                VoltageTypeList = listVoltageLevel.Where(a => a.CONNECTION_TYPE == model.SelectedConnectionType && a.DESCRIPTION == "440 V").ToList();
            else if (TotalLoad >= 160.001 && TotalLoad <= 480 && model.SelectedConnectionType == "0" && !string.IsNullOrEmpty(model.SelectedConnectionType))
                VoltageTypeList = listVoltageLevel.Where(a => a.CONNECTION_TYPE == model.SelectedConnectionType && a.DESCRIPTION == "440 V").ToList();
            else if (TotalLoad >= 160.001 && TotalLoad <= 480 && model.SelectedConnectionType == "1" && !string.IsNullOrEmpty(model.SelectedConnectionType))
                VoltageTypeList = listVoltageLevel.Where(a => a.CONNECTION_TYPE == model.SelectedConnectionType && a.DESCRIPTION == "11 KV" || a.DESCRIPTION == "22 KV" || a.DESCRIPTION == "33 KV").ToList();
            else if (TotalLoad >= 480.001 && TotalLoad <= 99999 && model.ConnectionTypeSelectList.Count == 1 && !string.IsNullOrEmpty(model.SelectedConnectionType))
                VoltageTypeList = listVoltageLevel.Where(a => a.CONNECTION_TYPE == model.SelectedConnectionType && a.DESCRIPTION == "11 KV" || a.DESCRIPTION == "22 KV" || a.DESCRIPTION == "33 KV").ToList();
            if (VoltageTypeList != null && VoltageTypeList.Any())
            {
                if (VoltageTypeList.Count == 1)
                {
                    model.VoltageLevel = VoltageTypeList[0].VOLTAGE_LEVEL;
                }
                foreach (var item in VoltageTypeList)
                {
                    model.VoltageLevelSelectList.Add(new SelectListItem
                    {
                        Text = item.DESCRIPTION,
                        Value = item.VOLTAGE_LEVEL
                    });
                }
            }
        }








        public static void GetMeterType(NewConnectionService newconnectionservice, NewConnectionApplication model)
        {
            var listVoltageLevel = newconnectionservice.ListVoltageLevelMapping();
            if (model.VoltageLevelSelectList.Count >= 1 && !string.IsNullOrEmpty(model.VoltageLevel))
            {
                var VoltageLevelList = listVoltageLevel.Where(a => a.VOLTAGE_LEVEL == model.VoltageLevel).FirstOrDefault();
                model.MeterType = VoltageLevelList != null ? VoltageLevelList.MeterType : "";
            }
        }

        public static void GetPurposeOfSupply(NewConnectionService newconnectionservice, NewConnectionApplication model)
        {
            var listPurposeofSupply = newconnectionservice.ListPurposeOfSupplyMapping();
            model.PurposeOfSupplySelectList = new List<SelectListItem>();
            var PurposeOfSuppluList = new List<NEWCON_MST_PURPOSE_CONNECTION>();

            if (model.SelectedConnectionType == "0" && model.TotalLoad <= 20)
            {
                if (model.IsApplicantType == "0")
                {
                    PurposeOfSuppluList = listPurposeofSupply.Where(a => a.CONNECTION_TYPE == model.ConnectionTypeCode && a.ENABLE_FLAG_SUB_APP2 == "0" && a.APPLICANT_TYPE.ToLower().Trim() == "govt" && a.APPLICANT_SUB_TYPE.ToLower().Trim() == model.selectedGovernmentType.ToLower().Trim()).Distinct().ToList();
                }
                else
                {

                    PurposeOfSuppluList = listPurposeofSupply.Where(a => a.CONNECTION_TYPE == model.ConnectionTypeCode && a.ENABLE_FLAG_SUB_APP2 == "0" && a.APPLICANT_TYPE.ToLower().Trim() == "non-govt").Distinct().ToList();
                }
            }
            if (model.SelectedConnectionType == "0" && model.TotalLoad > 20)
            {
                if (model.IsApplicantType == "0")
                {
                    PurposeOfSuppluList = listPurposeofSupply.Where(a => a.CONNECTION_TYPE == model.ConnectionTypeCode &&
                        a.ENABLE_FLAG_SUB_APP1 == "0" && a.APPLICANT_TYPE.ToLower().Trim() == "govt" && a.APPLICANT_SUB_TYPE.ToLower().Trim() == model.selectedGovernmentType.ToLower().Trim()).Distinct().ToList();
                }
                else
                {
                    PurposeOfSuppluList = listPurposeofSupply.Where(a => a.CONNECTION_TYPE == model.ConnectionTypeCode &&
                   a.ENABLE_FLAG_SUB_APP1 == "0" && a.APPLICANT_TYPE.ToLower().Trim() == "non-govt").Distinct().ToList();
                }
            }
            if (model.SelectedConnectionType == "1")
            {
                if (model.IsApplicantType == "0")
                {
                    PurposeOfSuppluList = listPurposeofSupply.Where(a => a.CONNECTION_TYPE == model.ConnectionTypeCode && a.APPLICANT_TYPE.ToLower().Trim() == "govt" && a.APPLICANT_SUB_TYPE.ToLower().Trim() == model.selectedGovernmentType.ToLower().Trim()).Distinct().ToList();// && a.Govt_Non_Govt.Trim() == "Govt" && a.Government == model.selectedGovernmentType
                }
                else
                {
                    PurposeOfSuppluList = listPurposeofSupply.Where(a => a.CONNECTION_TYPE == model.ConnectionTypeCode && a.APPLICANT_TYPE.ToLower().Trim() == "non-govt").Distinct().ToList();// && a.Govt_Non_Govt.Trim() == "Non-Govt"
                }
            }
            if (PurposeOfSuppluList != null && PurposeOfSuppluList.Any())
            {
                if (PurposeOfSuppluList.Count == 1)
                {
                    model.PurposeOfSupply = PurposeOfSuppluList[0].PURPOSE_OF_CONNECTION;
                }
                foreach (var item in PurposeOfSuppluList)
                {
                    model.PurposeOfSupplySelectList.Add(new SelectListItem
                    {
                        Text = item.CONNECTION_DESCRIPTION,
                        Value = item.PURPOSE_OF_CONNECTION
                    });
                }
            }
            var AppliedTariffList = new NEWCON_MST_PURPOSE_CONNECTION();
            if (model.SelectedConnectionType == "0" && model.TotalLoad <= 20 && !string.IsNullOrEmpty(model.PurposeOfSupply))
                AppliedTariffList = listPurposeofSupply.Where(a => a.PURPOSE_OF_CONNECTION == model.PurposeOfSupply).FirstOrDefault();
            else if (model.SelectedConnectionType == "0" && model.TotalLoad > 20 && !string.IsNullOrEmpty(model.PurposeOfSupply))
                AppliedTariffList = listPurposeofSupply.Where(a => a.PURPOSE_OF_CONNECTION == model.PurposeOfSupply).FirstOrDefault();
            else if (model.SelectedConnectionType == "1" && !string.IsNullOrEmpty(model.PurposeOfSupply))
                AppliedTariffList = listPurposeofSupply.Where(a => a.PURPOSE_OF_CONNECTION == model.PurposeOfSupply).FirstOrDefault();
            if (AppliedTariffList != null)
                model.AppliedTariff = AppliedTariffList.APPLIED_TARIFF;


            if ((!string.IsNullOrEmpty(model.PurposeOfSupply)) && (!string.IsNullOrEmpty(model.AppliedTariff)))
            {
                var listMeterLoad = newconnectionservice.ListOfMeterLoadMapping();
                var MeterLoadList = listMeterLoad.Where(a => a.APPLIED_TARRIF == model.AppliedTariff && a.PURPOSE_CONNECTION == model.PurposeOfSupply || a.APPLIED_TARRIF == model.AppliedTariff && a.PURPOSE_CONNECTION == "0").Distinct().ToList();

                model.MeterLoadSelectList = new List<SelectListItem>();
                if (MeterLoadList != null && MeterLoadList.Any())
                {
                    if (MeterLoadList.Count == 1)
                    {
                        model.MeterLoad = MeterLoadList[0].PURPOSE_CONNECTION;
                    }
                    foreach (var item in MeterLoadList)
                    {
                        model.MeterLoadSelectList.Add(new SelectListItem
                        {
                            Text = item.DESCRIPTION,
                            Value = item.ZLTYP
                        });
                    }
                }

                if (model.IsSez == "0")
                {
                    var PremiseTypeList = newconnectionservice.ListOfPremiseType();

                    var PremiseList = listPurposeofSupply.Where(a => a.PURPOSE_OF_CONNECTION == model.PurposeOfSupply).FirstOrDefault();
                    var list = PremiseList.PremiseType;
                    if (list != "")
                    {
                        List<string> dropDownItems = new List<string>();
                        dropDownItems.AddRange(list.Split(',').ToList());
                        if (dropDownItems != null && dropDownItems.Any())
                        {
                            foreach (var item in dropDownItems)
                            {
                                var PremiseLists = new NEWCON_MST_PREMISE_TYPE();
                                if (model.TotalLoad > 20)
                                    PremiseLists = PremiseTypeList.Where(a => a.PREMISE_TYPE == item && a.ENABLE_SUB_APP1 == "1").FirstOrDefault();
                                else
                                    PremiseLists = PremiseTypeList.Where(a => a.PREMISE_TYPE == item && a.ENABLE_FLAG_SUB_APP2 == "1").FirstOrDefault();
                                if (PremiseLists != null)
                                {
                                    if (dropDownItems.Count == 1)
                                    {
                                        model.PremiseType = PremiseLists.PREMISE_TYPE;
                                    }
                                    model.PremiseTypeSelectList.Add(new SelectListItem
                                    {
                                        Text = PremiseLists.TEXT,
                                        Value = PremiseLists.PREMISE_TYPE
                                    });
                                }
                            }
                        }
                    }
                    else
                    {

                        if (model.TotalLoad > 20)
                        {

                            var PremiseTypeListss = PremiseTypeList.Where(a => a.TEXT != "TEMPORARY ILLUMINATION" && a.ENABLE_SUB_APP1 == "1").Distinct().ToList();
                            model.PremiseTypeSelectList = new List<SelectListItem>();
                            if (PremiseTypeListss != null && PremiseTypeListss.Any())
                            {
                                foreach (var item in PremiseTypeListss)
                                {
                                    model.PremiseTypeSelectList.Add(new SelectListItem
                                    {
                                        Text = item.TEXT,
                                        Value = item.TEXT
                                    });
                                }
                            }
                        }
                        else
                        {

                            var PremiseTypeListss = PremiseTypeList.Where(a => a.TEXT != "TEMPORARY ILLUMINATION" && a.ENABLE_FLAG_SUB_APP2 == "1").Distinct().ToList();
                            model.PremiseTypeSelectList = new List<SelectListItem>();
                            if (PremiseTypeListss != null && PremiseTypeListss.Any())
                            {
                                foreach (var item in PremiseTypeListss)
                                {
                                    model.PremiseTypeSelectList.Add(new SelectListItem
                                    {
                                        Text = item.TEXT,
                                        Value = item.TEXT
                                    });
                                }
                            }
                        }



                    }
                }
                else
                {
                    var PremiseTypeList = newconnectionservice.ListOfPremiseType();
                    var PremiseList = PremiseTypeList.Where(a => a.TEXT.ToLower() == "sez consumer").Distinct().ToList();
                    if (PremiseList != null && PremiseList.Any())
                    {
                        if (PremiseList.Count == 1)
                        {
                            model.PremiseType = PremiseList[0].PREMISE_TYPE;
                        }
                        foreach (var item in PremiseList)
                        {
                            model.PremiseTypeSelectList.Add(new SelectListItem
                            {
                                Text = item.TEXT,
                                Value = item.PREMISE_TYPE
                            });
                        }
                    }
                }
            }


        }


        public static bool SaveNewConnectionApplicationInfo(PaymentHistoryDataContext dataContext, NewConnectionApplication model)
        {
            bool isInsert = true;
            bool insertStatus = true;
            try
            {
                NEW_CON_APPLICATION_FORM obj = new NEW_CON_APPLICATION_FORM();
                var lstAreaGen = dataContext.NEW_CON_APPLICATION_FORMs.OrderBy(o => o.Id).Distinct().ToList();
                var c = lstAreaGen.Count;

                var tempnumber = "N" + c.ToString().PadLeft(9, '0');

                obj = dataContext.NEW_CON_APPLICATION_FORMs.SingleOrDefault(x => x.Id.ToString() == model.Id.ToString());

                if (obj == null)
                {
                    obj = new NEW_CON_APPLICATION_FORM();
                    obj.CreatedDate = DateTime.Now;
                    obj.FormName = "New Connection Application";
                }
                else
                {
                    isInsert = false;
                    obj.FormName = "New Connection Application Modified";
                }

                obj.AccountNumber = model.AccountNo;
                obj.Id = (!string.IsNullOrEmpty(model.Id.ToString()) && model.Id.ToString() == "00000000-0000-0000-0000-000000000000" ? Guid.NewGuid() : model.Id);

                obj.ApplicationNumber = (model.ApplicationNo == null && string.IsNullOrEmpty(model.ApplicationNo) ? tempnumber : model.ApplicationNo);

                obj.ApplicationType = model.ApplicationType;
                obj.ApplicationSubType = model.ApplicationSubType;
                obj.IsExistingCustomer = model.IsExistingCustomer;
                obj.CANumber = model.CANumber;
                obj.StartTempDate = model.TempStartDate;
                obj.EndTempTime = model.TempEndDate;
                obj.OrganizationName = model.OrganizationName;
                obj.DateOfBirthJoint1 = model.Name1JointDateofBirth;
                obj.Name1Joint = model.Name1Joint;
                obj.Name2Joint = model.Name2Joint;
                //obj.SectorType = model.IsApplicantType == "0" ? true : false; ;
                obj.GovernmentType = model.selectedGovernmentType;
                obj.ApplicantType = model.ApplicantType == "1" ? true : false;
                obj.IsJoint = model.ApplicanttType == "1" ? true : false;
                obj.ApplicationTitle = model.ApplicationTitle;
                obj.FirstName = model.FirstName;
                obj.MiddleName = model.MiddleName;
                obj.LastName = model.LastName;
                obj.DateOfBirth = model.DateofBirth;
                obj.AddressOfSupply = model.FlatNumber + "#" + model.BuildingName + "#" + model.Street + "#" + model.Landmark;
                obj.Pincode = model.SelectedPincode;
                obj.Surburb = model.SelectedSuburb;

                obj.BillingAddressDifferentFromSupply = model.billingdifferentthanAddresswheresupply;
                obj.BillingAddress = model.ApplicantCorrespondenceFlatNumber + "#" + model.ApplicantCorrespondenceBuildingName + "#" + model.ApplicantCorrespondenceStreet + "#" + model.ApplicantCorrespondenceLandmark;
                obj.BillingSuburb = model.ApplicantCorrespondenceSuburb;
                obj.BillingPincode = model.ApplicantCorrespondencePincode;


                obj.AddressIncaseOfRental = model.AddressInCaseOfRental;
                obj.NameOfRental = model.RentalNameoftheOwner;
                obj.RentalContactNo = model.RentalContactNumber;
                obj.RentalOwnerEmail = model.RentalOwnerEmail;
                obj.AddressOfRental = model.RentalFlatNumber + "#" + model.RentalBuildingName + "#" + model.RentalStreet;
                obj.RentalSuburb = model.RentalSuburb;
                obj.RentalPincode = model.RentalPincode;


                obj.MobileNumber = model.MobileNo;
                obj.Email = model.Email;
                obj.LandlineNumber = model.LandlineNo;
                obj.BillLanguage = model.BillLanguage;
                obj.BillingFormat = model.BillFormat;
                obj.IsGreenTariffApplied = (!string.IsNullOrEmpty(model.GreenTariff) ? true : false);
                obj.GreenTariff = model.GreenTariff;
                obj.IsSEZ = model.IsSez == "1" ? true : false;

                obj.MICR = model.MICR;
                obj.TypeOfAccount = model.BankAccountType;
                obj.Bank = model.Bank;
                obj.Branch = model.Branch;
                obj.BankAccountNumber = model.BankAccountNumber;
                obj.BankHolderName = model.BankHoldersName;

                obj.ModifiedDate = DateTime.Now;
                obj.ApplicationModel = model.ApplicationMode;
                obj.Status = System.Convert.ToInt32(model.Status);

                obj.IsTermConditionAccepted = model.IsTermConditionAccepted == "1" ? true : false;
                obj.IsCopiedSelfAttested = model.IsCopiedSelfAttested == "1" ? true : false;
                if (isInsert)
                {
                    dataContext.NEW_CON_APPLICATION_FORMs.InsertOnSubmit(obj);
                }
                dataContext.SubmitChanges();

                obj = dataContext.NEW_CON_APPLICATION_FORMs.SingleOrDefault(x => x.Id.ToString() == obj.Id.ToString());
                if (obj != null)
                {
                    model.ApplicationNo = obj.ApplicationNumber;
                    model.Id = obj.Id;
                    model.Status = obj.Status.ToString();
                    model.ApplicationMode = obj.ApplicationModel;
                    model.AccountNo = obj.AccountNumber;
                    model.ApplicationMode = obj.ApplicationModel;
                }
            }
            catch (Exception ex)
            {
                insertStatus = false;
            }

            return insertStatus;
        }
        public static bool SaveNewConnectionInfo(NewConnectionApplication model)
        {
            bool insertStatus = true;
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                insertStatus = SaveNewConnectionApplicationInfo(dataContext, model);
                if (insertStatus && (model.ApplicationMode == "SAD" || model.ApplicationMode == "SLD" || model.ApplicationMode == "SOD" || model.ApplicationMode == "SDD"))
                {
                    insertStatus = SaveNewConnectionLoadDetailInfo(dataContext, model);
                }
                if (insertStatus && (model.ApplicationMode == "SAD" || model.ApplicationMode == "SLD" || model.ApplicationMode == "SOD" || model.ApplicationMode == "SDD"))
                {
                    insertStatus = SaveNewConnectionOtherInfo(dataContext, model);
                }
            }
            return insertStatus;
        }

        public static bool SaveNewConnectionLoadDetailInfo(PaymentHistoryDataContext dataContext, NewConnectionApplication model)
        {
            bool isInsert = true;
            bool insertStatus = true;
            try
            {
                NEWCON_LOAD_DETAIL obj = new NEWCON_LOAD_DETAIL();
                obj = dataContext.NEWCON_LOAD_DETAILs.SingleOrDefault(x => x.AccountNumber.ToString() == model.ApplicationNo.ToString());
                if (obj == null)
                {
                    obj = new NEWCON_LOAD_DETAIL();
                    obj.CreateDate = DateTime.Now;
                    obj.id = Guid.NewGuid();
                }
                else
                {
                    isInsert = false;
                }
                obj.AccountNumber = model.ApplicationNo;
                obj.ConnectedLoadKW = model.ConnectedLoadKW;
                obj.ConnectedLoadHP = model.ConnectedLoadHP;
                obj.TotalLoad = model.TotalLoad.ToString();
                obj.ConnectionType = model.SelectedConnectionType;
                obj.VoltageLevel = model.VoltageLevel;
                obj.PurposeOfSupply = model.PurposeOfSupply;
                obj.MeterType = model.MeterType;
                obj.MeterLoad = model.MeterLoad;
                obj.PremiseType = model.PremiseType;
                obj.ContractDemand = model.ContractDemand;
                obj.ModifiedDate = DateTime.Now;
                obj.LDPOrderNumber = model.LDPOrderNumber == null ? "" : model.LDPOrderNumber;
                if (isInsert)
                {
                    dataContext.NEWCON_LOAD_DETAILs.InsertOnSubmit(obj);
                }
                dataContext.SubmitChanges();

                var objApp = dataContext.NEW_CON_APPLICATION_FORMs.SingleOrDefault(x => x.Id.ToString() == model.Id.ToString());
                if (objApp != null)
                {
                    objApp.ApplicationModel = model.ApplicationMode;
                }
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                insertStatus = false;
            }
            return insertStatus;
        }

        public static bool SaveNewConnectionOtherInfo(PaymentHistoryDataContext dataContext, NewConnectionApplication model)
        {
            bool isInsert = true;
            bool insertStatus = true;
            try
            {
                NEWCON_OTHER_DETAIL obj = new NEWCON_OTHER_DETAIL();
                obj = dataContext.NEWCON_OTHER_DETAILs.SingleOrDefault(x => x.AccountNumber.ToString() == model.ApplicationNo.ToString());
                if (obj == null)
                {
                    obj = new NEWCON_OTHER_DETAIL();
                    obj.CreateDate = DateTime.Now;
                    obj.id = Guid.NewGuid();
                }
                else
                {
                    isInsert = false;
                }
                obj.AccountNumber = model.ApplicationNo;
                obj.IsMeterConnectionCabinExist = model.Meterconnectioninexistingmetercabin;
                obj.NearestConsumerMeterNo = model.NearestConsumerMeterNo;
                obj.NearestConsumerAccountNo = model.NearestConsumerAccountNo;
                obj.ExistingConnection = model.ExistingConnection;
                obj.ConsumerNo = model.ConsumerNo;
                obj.Utility = model.Utility;
                obj.WiringCompleted = model.WiringCompleted;
                obj.LECNumber = model.LECNumber;
                obj.NameOnLEC = model.NameOnLEC;
                obj.LECEmail = model.LECEmail;
                obj.LECLandline = model.LECLandlineNo;
                obj.LECMobileNumber = model.LECMobileNo;
                obj.ModifiedDate = DateTime.Now;
                obj.TR_NO = model.TRNumber;
                obj.Is_DDF = model.DedicatedDistributionfacility == "1" ? true : false;
                obj.DDFRs = model.DedicatedDistributionfacilityRs.ToString();
                obj.IS_METER_SUPPLIER = model.metersupplier == "AEML" ? true : false;

                if (isInsert)
                {
                    dataContext.NEWCON_OTHER_DETAILs.InsertOnSubmit(obj);
                }
                dataContext.SubmitChanges();

                var objApp = dataContext.NEW_CON_APPLICATION_FORMs.SingleOrDefault(x => x.Id.ToString() == model.Id.ToString());
                if (objApp != null)
                {
                    objApp.ApplicationModel = model.ApplicationMode;
                }
                dataContext.SubmitChanges();

                var model1 = new NewConnectionApplication();
                model1.DocumentList = (model.DocumentList == null ? new List<NEWCON_DOCUMENTMASTER>() : model.DocumentList);
                AEMLUploadDocumentLibrary.UploadDocumentFile(dataContext, model1, "PH", "Installation test report from licensed electrical contractor", "20", model.TRNumber, model.ApplicationNo, model.AccountNo, "");
            }
            catch (Exception ex)
            {
                insertStatus = false;
            }
            return insertStatus;
        }

        public static string ValidateCANumber(NewConnectionApplication model)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(model.CANumber))
                {
                    var accountDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.CANumber);
                    var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.CANumber);

                    string iDate = consumerDetails.MOVEOUTFLAG;
                    DateTime oDate = DateTime.Parse(iDate);
                    DateTime currentDate = DateTime.Now;
                    TimeSpan ts = currentDate.Subtract(oDate);
                    //result.
                    var c = ts.Days;
                    if (consumerDetails.MOVEOUTFLAG == "9999-12-31")
                    {
                        message = "Mentioned account is active , Please enter Account No which is disconnected for more than 6 months";
                    }
                    else if (consumerDetails.MOVEOUTFLAG != "9999-12-31" && c <= 180)
                    {
                        message = "Please enter Account No which is disconnected for more than 6 months";
                    }
                    else if (consumerDetails.MOVEOUTFLAG != "9999-12-31" && c > 180)
                    {
                        string checkCA = NewConnectionService.ValidateCANewConnection(accountDetails);
                        if (!string.IsNullOrEmpty(checkCA))
                        {
                            message = checkCA;
                            model.CANumber = null;
                            model.IsCAValid = false;
                        }
                        else
                        {
                            model.IsCAValid = true;
                            model.FirstName = consumerDetails.Name;
                            model.Email = consumerDetails.Email;
                            model.FlatNumber = consumerDetails.HouseNumber;
                            model.Street = consumerDetails.Street;
                            model.SelectedPincode = consumerDetails.PinCode;
                            model.SelectedSuburb = consumerDetails.City;
                        }
                    }


                }

            }

            catch (Exception ex)
            {
                message = "Please enter a valid CA number";
            }
            return message;
        }

        public static string ValidateConsumerNumber(NewConnectionApplication model)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(model.ConsumerNo))
                {

                    var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.ConsumerNo);
                    if (string.IsNullOrEmpty(consumerDetails.Name))
                    {
                        Log.Info("Not a valid consumer number", new AEMLNewConnectionLibrary());
                        message = DictionaryPhraseRepository.Current.Get("/NewCON/MICR/Input not valid", "Not a valid consumer number");
                    }


                }

            }

            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }


        public static string GetBankDetail(NewConnectionApplication model)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(model.MICR))
                {
                    var BankDetails = SapPiService.Services.RequestHandler.CheckBankDetails(model.MICR);
                    if (BankDetails != null && (!string.IsNullOrEmpty(BankDetails.BANK_NAME)))
                    {
                        model.Bank = BankDetails.BANK_NAME;
                        model.Branch = BankDetails.BANK_BRANCH;
                    }
                    else
                    {
                        Log.Info("NewConnection MICR from SAP - not valid:" + BankDetails.Message, new AEMLNewConnectionLibrary());
                        message = DictionaryPhraseRepository.Current.Get("/NewCON/MICR/Input not valid", "Entered details not found associated with any Bank and Branch, please enter valid MICR");
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return message;
        }
        public static string GetLECDetails(NewConnectionApplication model)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(model.LECNumber))
                {
                    var LECDetails = SapPiService.Services.RequestHandler.ValidateAndFetchLECDetailsByLicenseNumber(model.LECNumber);

                    if (LECDetails != null && (!string.IsNullOrEmpty(LECDetails.LEC_Name)))
                    {
                        model.NameOnLEC = LECDetails.LEC_Name;
                        model.LECMobileNo = LECDetails.LEC_MOBILE_NO;
                        model.LECEmail = LECDetails.LEC_Email;
                    }
                    else
                    {
                        Log.Info("NewConnection LEC no from SAP - not valid:" + LECDetails.Message, new AEMLNewConnectionLibrary());
                        message = DictionaryPhraseRepository.Current.Get("/NewCON/MICR/Input not valid", "Entered details not found, please enter valid LEC number ");

                    }




                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return message;
        }
        public static string ValidateTRNumber(NewConnectionApplication model)
        {
            string message = string.Empty;
            string ZZLEC = "";
            try
            {
                var LECDetails = SapPiService.Services.RequestHandler.ValidateAndFetchLECDetailsByLicenseNumber(model.LECNumber);
                ZZLEC = LECDetails.LEC_License_NO;
                bool TRDeatils = SapPiService.Services.RequestHandler.ValidateTRNumber(model.TRNumber, ZZLEC);
                if (!TRDeatils)
                {
                    message = DictionaryPhraseRepository.Current.Get("/NewCON/TR Registration/Input not valid", "This is not a valid TR number");

                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return message;
        }
        public static string ValidateExistingNumber(NewConnectionApplication model)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(model.NearestConsumerAccountNo))
                {
                    var ConsumerMeterNumber = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.NearestConsumerAccountNo);
                    if (ConsumerMeterNumber != null && (!string.IsNullOrEmpty(ConsumerMeterNumber.MeterNumber)))
                    {
                        model.NearestConsumerMeterNo = ConsumerMeterNumber.MeterNumber.TrimStart(new Char[] { '0' });
                    }
                    else
                    {
                        Log.Info("NewConnection Meter Number from SAP - not valid:", new AEMLNewConnectionLibrary());
                        message = DictionaryPhraseRepository.Current.Get("/NewCON/MICR/Input not valid", "Not a valid Consumer Account Number");
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return message;
        }
        public static string ValidateMeterNumber(NewConnectionApplication model)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(model.NearestConsumerMeterNo))
                {
                    var ConsumerMeterNumber = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.NearestConsumerMeterNo);
                    if (ConsumerMeterNumber != null && (!string.IsNullOrEmpty(ConsumerMeterNumber.CANumber)))
                    {
                        model.NearestConsumerAccountNo = ConsumerMeterNumber.CANumber;
                    }
                    else
                    {
                        Log.Info("NewConnection Meter Number from SAP - not valid:", new AEMLNewConnectionLibrary());
                        message = DictionaryPhraseRepository.Current.Get("/NewCON/MICR/Input not valid", "Meter Number is not valid");
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return message;
        }
        public static string LPDNumber(NewConnectionApplication model)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(model.LDPOrderNumber))
                {
                    var LPDORderNumber = SapPiService.Services.RequestHandler.GetOrderIdForNewCon(model.LDPOrderNumber);
                    if (LPDORderNumber.OrderIdSAP != "")
                    {
                        Log.Info("NewConnection Meter Number from SAP - not valid:", new AEMLNewConnectionLibrary());

                    }
                    else
                    {
                        Log.Info("NewConnection Meter Number from SAP - not valid:", new AEMLNewConnectionLibrary());
                        message = DictionaryPhraseRepository.Current.Get("/NewCON/MICR/Input not valid", "Entered LDP Number is not correct");
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return message;
        }

        public static void PostDataToSAP(NewConnectionApplication model)
        {
            try
            {
                bool Success;
                NewConnectionService newconnectionservice = new NewConnectionService();

                PostDataWebUpdate requestObj = new PostDataWebUpdate

                {

                    TempRegistrationNumber = model.ApplicationNo,
                   
                     PurposeofSupply = model.PurposeOfSupply,
                    AppliedTariff = model.AppliedTariff,
                    PremiseType = model.PremiseType,
                   MeterLoad = model.MeterLoad,
                   VoltageLevel = model.VoltageLevel,
                   ApplicationTitle = model.ApplicationTitle,
                   
                    BuildingName = model.BuildingName,
                    LaneStreet = model.Street,
                    Pincode = model.SelectedPincode,
                    Suburb = model.SelectedSuburb,
                    MobileNumber = model.MobileNo,
                    Email = model.Email,
                  
                    


                };


 
                    if(model.IsApplicantType=="0")
                {
                    requestObj.SectorType = "1";
                }
                    else if (model.IsApplicantType == "1")
                {
                    requestObj.SectorType = "2";

                }
                    if ((!string.IsNullOrEmpty(model.FlatNumber)))
                {
                  
                    requestObj.HouseNumber = model.FlatNumber;
                }
                else
                {
                    requestObj.HouseNumber = "";
                }
               
                if (model.Meterconnectioninexistingmetercabin == "Yes")
                {
                    requestObj.MeterCabin = "2";
                }
                else if (model.Meterconnectioninexistingmetercabin == "No")
                {
                    requestObj.MeterCabin = "1";
                }
                if (model.IsGreenTariffApplied == "1")
                {
                    requestObj.IsGreenTariffApplied = "X";
                }
                else
                {
                    requestObj.IsGreenTariffApplied = "";
                }
                if (model.BillFormat == "EBill")
                {
                    requestObj.BillingFormatEBill = "X";
                }
                else
                {
                    requestObj.BillingFormatEBill = "";
                }
                if (model.BillFormat == "EPBill")
                {
                    requestObj.BillingFormatEPBill = "X";
                }
                else
                {
                    requestObj.BillingFormatEPBill = "";
                }
                if (model.metersupplier == "SELF")
                {
                    requestObj.SelfMeter = "X";
                }
                else
                {
                    requestObj.SelfMeter = "";
                }
                if (model.IsApplicantType == "0")
                {
                    requestObj.GovernmentSelected = "X";
                }
                else
                {
                    requestObj.GovernmentSelected = "";
                }
                
                if (model.TotalLoad <= 25)
                {
                    requestObj.TotalLoad = "2";
                }
                else if (model.TotalLoad > 25)
                {
                    requestObj.TotalLoad = "1";
                }
                if (model.SelectedConnectionType == "0")
                {
                    requestObj.ConnectionType = "LT";
                }
                else if (model.SelectedConnectionType == "1")
                {
                    requestObj.ConnectionType = "HT";
                }

                if (model.MeterType == "1PH")
                {
                    requestObj.MeterTypeCount1PH = "1";
                    if (model.ConnectedLoadKW != null)
                    {
                        requestObj.ConnectedLoadKW1PH = model.ConnectedLoadKW;
                    }
                    else
                    {
                        requestObj.ConnectedLoadKW1PH = "";
                    }
                    if (model.ConnectedLoadHP != null)
                    {
                        requestObj.ConnectedLoadHP1PH = model.ConnectedLoadHP;
                    }
                    else
                    {
                        requestObj.ConnectedLoadHP1PH = "";
                    }
                }
                
                if (model.MeterType == "3PH")
                {
                    requestObj.MeterTypeCount3PH = "1";
                    if (model.ConnectedLoadKW != null)
                    {
                        requestObj.ConnectedLoadKW3PH = model.ConnectedLoadKW;
                    }
                    else
                    {
                        requestObj.ConnectedLoadKW3PH = "";
                    }
                    if (model.ConnectedLoadHP != null)
                    {
                        requestObj.ConnectedLoadHP3PH = model.ConnectedLoadHP;
                    }
                    else
                    {
                        requestObj.ConnectedLoadHP3PH = "";
                    }
                }
               
                 if (model.MeterType == "HT")
                {
                    requestObj.MeterTypeCountHT = "1";
                    if (model.ConnectedLoadKW != null)
                    {
                        requestObj.ConnectedLoadKWHT = model.ConnectedLoadKW;
                    }
                    else
                    {
                        requestObj.ConnectedLoadKWHT = "";
                    }
                    if (model.ConnectedLoadHP != null)
                    {
                        requestObj.ConnectedLoadHT = model.ConnectedLoadHP;
                    }
                    else
                    {
                        requestObj.ConnectedLoadHT = "";
                    }
                }
                if (model.TempStartDate.HasValue)
                {
                    requestObj.TempStartDate = model.TempStartDate.ToString();
                }
                else
                {
                    requestObj.TempStartDate = "";
                }
                if (model.TempEndDate.HasValue)
                {
                    requestObj.TempEndDate = model.TempEndDate.ToString();
                }
                else
                {
                    requestObj.TempEndDate = "";
                }
                var listAllArea = newconnectionservice.ListAreaPinWorkcenterMapping();
                var AreaList = listAllArea.Where(a => a.Area == model.SelectedSuburb).FirstOrDefault();
                model.AreaTobeSent = AreaList.AreaSentToSAP;
                if ((string.IsNullOrEmpty(model.LDPOrderNumber)))
                {
                    var LDPorderNumber = listAllArea.Where(a => a.Pincode == model.SelectedPincode).FirstOrDefault();
                    model.LDPOrderNumber = LDPorderNumber.DummyLDP;
                    requestObj.LDPNumber = model.LDPOrderNumber;
                }
                else
                {
                    requestObj.LDPNumber = model.LDPOrderNumber;
                }
                var WorkCentre = listAllArea.Where(a => a.Pincode == model.SelectedPincode).FirstOrDefault();
                requestObj.Workcenter = WorkCentre.WorkStation;
                var city= listAllArea.Where(a => a.Pincode == model.SelectedPincode).FirstOrDefault();
                requestObj.City = city.City;
                if (model.billingdifferentthanAddresswheresupply == "Yes")
                {
                    requestObj.billingdifferentthanAddresswheresupply = "X";
                    requestObj.BillingHouseNumber = model.ApplicantCorrespondenceFlatNumber;
                    requestObj.BillingBuildingName = model.ApplicantCorrespondenceFlatNumber;
                    requestObj.BillingLandmark = model.ApplicantCorrespondenceLandmark;
                    requestObj.BillingLane = model.ApplicantCorrespondenceStreet;
                    requestObj.BillingPincode = model.ApplicantCorrespondencePincode;
                    requestObj.BillingSuburb = model.ApplicantCorrespondenceSuburb;
                    
                }
                else
                {
                    requestObj.billingdifferentthanAddresswheresupply = "";
                    requestObj.BillingHouseNumber = "";
                    requestObj.BillingBuildingName = "";
                    requestObj.BillingLandmark = "";
                    requestObj.BillingLane = "";
                    requestObj.BillingPincode = "";
                    requestObj.BillingSuburb = "";
                }
                if (model.AddressInCaseOfRental == "Yes")
                {
                    requestObj.RentalAddress = "X";
                    requestObj.RentalBuildingName = model.RentalBuildingName;
                    requestObj.RentalHouseNumber = model.RentalFlatNumber;
                    requestObj.NameofRentalOwner = model.RentalNameoftheOwner;
                    requestObj.RentalLane = model.RentalStreet;
                    requestObj.RentalPincode = model.RentalPincode;
                    requestObj.RentalSuburb = model.RentalSuburb;
                    requestObj.RentalMobileNumber = model.RentalContactNumber;
                    requestObj.RentalEmail = model.RentalOwnerEmail;
                    
                }
                else
                {
                    requestObj.RentalAddress = "";
                    requestObj.RentalBuildingName = "";
                    requestObj.RentalHouseNumber = "";
                    requestObj.NameofRentalOwner = "";
                    requestObj.RentalLane = "";
                    requestObj.RentalPincode = "";
                    requestObj.RentalSuburb = "";
                    requestObj.RentalMobileNumber = "";
                    requestObj.RentalEmail = "";
                }
                
                if (!string.IsNullOrEmpty(model.NearestConsumerAccountNo))
                {
                    requestObj.NearestCAnumber = model.NearestConsumerAccountNo;
                }
                else
                {
                    requestObj.NearestCAnumber = "";
                }
                if (!string.IsNullOrEmpty(model.LandlineNo))
                {
                    requestObj.LandlineNumber = model.LandlineNo;
                }
                else
                {
                    requestObj.LandlineNumber = "";
                }
                if (!string.IsNullOrEmpty(model.NearestConsumerMeterNo))
                {
                    requestObj.NearestMeternumber = model.NearestConsumerMeterNo;
                }
                else
                {
                    requestObj.NearestMeternumber = "";
                }
                if (!string.IsNullOrEmpty(model.LECNumber))
                {
                    requestObj.LECNumber = model.LECNumber;
                }
                else
                {
                    requestObj.LECNumber = "";
                }
                if (!string.IsNullOrEmpty(model.BankAccountNumber))
                {
                    requestObj.BankAccountNumber = model.BankAccountNumber;
                }
                else
                {
                    requestObj.BankAccountNumber = "";
                }
                if (!string.IsNullOrEmpty(model.MICR))
                {
                    requestObj.MICR = model.MICR;
                }
                else
                {
                    requestObj.MICR = "";
                }
                if (!string.IsNullOrEmpty(model.Bank))
                {
                    requestObj.Bank = model.Bank;
                }
                else
                {
                    requestObj.Bank = "";
                }
                if (!string.IsNullOrEmpty(model.Branch))
                {
                    requestObj.Branch = model.Branch;
                }
                else
                {
                    requestObj.Branch = "";
                }
                if (!string.IsNullOrEmpty(model.BillLanguage))
                {
                    requestObj.BillLangianguage = model.BillLanguage;
                }
                else
                {
                    requestObj.BillLangianguage = "";
                }
                if (!string.IsNullOrEmpty(model.LandlineNo ))
                {
                    requestObj.LandlineNumber = model.LandlineNo;
                }
                else
                {
                    requestObj.LandlineNumber = "";
                }
                if (!string.IsNullOrEmpty(model.ConsumerNo))
                {
                    requestObj.ConsumerNumber = model.ConsumerNo;
                }
                else
                {
                    requestObj.ConsumerNumber = "";
                }
                if (!string.IsNullOrEmpty(model.Utility))
                {
                    requestObj.Utility = model.Utility;
                }
                else
                {
                    requestObj.Utility = "";
                }
              
                    if (!string.IsNullOrEmpty(model.OrganizationName))
                    {
                        requestObj.OrganizationName = model.OrganizationName;
                    }
                    else
                    {
                        requestObj.OrganizationName = "";
                    }
                
                if (!string.IsNullOrEmpty(model.FirstName))
                {
                    requestObj.FirstName = model.FirstName;
                }
                else
                {
                    requestObj.FirstName = "";
                }
                if (!string.IsNullOrEmpty(model.MiddleName))
                {
                    requestObj.MiddleName = model.MiddleName;
                }
                else
                {
                    requestObj.MiddleName = "";
                }
                if (!string.IsNullOrEmpty(model.LastName))
                {
                    requestObj.LastName = model.LastName;
                }
                else
                {
                    requestObj.LastName = "";
                }
                if (model.WiringCompleted =="Yes")
                {
                    requestObj.WiringCompleted = "1";
                }
                else
                {
                    requestObj.WiringCompleted = "2";
                }
                
                if (!string.IsNullOrEmpty(model.ContractDemand))
                {
                    requestObj.ContractDemand = model.ContractDemand;
                }
                else
                {
                    requestObj.ContractDemand = "";
                }
                if (!string.IsNullOrEmpty(model.Landmark))
                {
                    requestObj.Landmark = model.Landmark;
                }
                else
                {
                    requestObj.Landmark = "";
                }
                
                model.GetExistingDocuments = AEMLUploadDocumentLibrary.GetExistingDocument(model.ApplicationNo);
                List<PostDocumentsWebUpdate> requestDocList = new List<PostDocumentsWebUpdate>();
                foreach(var document in model.GetExistingDocuments)
                {
                    if (document.DocumentTypeCode.ToLower() != "tr")
                    {
                        requestDocList.Add(new PostDocumentsWebUpdate() { DocumentDescription = document.DocumentDescription, DocumentSerialNumber = document.DocumentType, DocumentData = document.DocumentData.ToArray() });
                    }
                }
                var PostData= SapPiService.Services.RequestHandler.PostDataForNewCon(requestObj, requestDocList);
                if (PostData.IsSuccess == true)
                {
                    Success = true;
                    Log.Info("NewConnection LEC no from SAP - not valid:" ,new AEMLNewConnectionLibrary());
                    


                }
                else
                {
                    Success = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + " For Mobil number:" + model.MobileNo, "PostDataToSAP");
            }
        }
    }
}

