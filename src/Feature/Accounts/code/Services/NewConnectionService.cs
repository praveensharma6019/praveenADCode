using SapPiService.Domain;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Feature.Accounts;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.SessionHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Sitecore.Feature.Accounts.Models
{
    public class NewConnectionService
    {

       
        public List<NEW_CON_CITY_PINCODE_MST> ListAreaPinWorkcenterMapping()
        {
            List<NEW_CON_CITY_PINCODE_MST> lstArea = new List<NEW_CON_CITY_PINCODE_MST>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lstAreaGen = dataContext.NEW_CON_CITY_PINCODE_MSTs.OrderBy(o => o.Area).ToList();
                    foreach (var a in lstAreaGen)
                    {
                        lstArea.Add(
                            new NEW_CON_CITY_PINCODE_MST
                            {
                                Id = a.Id,
                                Area = a.Area.ToUpper(),
                                City = a.City.ToUpper(),
                                CreatedBy = a.CreatedBy,
                                CreatedDate = a.CreatedDate,
                                Pincode = a.Pincode.ToUpper(),
                                Division=a.Division,
                                Unit=a.Unit,
                                DummyLDP=a.DummyLDP,
                                AreaSentToSAP=a.AreaSentToSAP,
                                WorkStation = a.WorkStation.ToUpper()
                            });
                    }
                    return lstArea;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lstArea;

        }
        public List<NEW_CON_NormativeCharge> ListNormativeChargeMapping()
        {
            List<NEW_CON_NormativeCharge> lstnormativeype = new List<NEW_CON_NormativeCharge>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lstnormativeypes = dataContext.NEW_CON_NormativeCharges.ToList();
                    foreach (var a in lstnormativeypes)
                    {
                        lstnormativeype.Add(
                            new NEW_CON_NormativeCharge
                            {
                                ApplicationCharge = a.ApplicationCharge,
                        Phase=a.Phase,
                        ServiceLineRate=a.ServiceLineRate,
                        LoadHigh=a.LoadHigh,
                        LoadLow=a.LoadLow,
                        PMActivity=a.PMActivity,
                        LoadUnit=a.LoadUnit,
                            });
                    }
                    return lstnormativeype;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lstnormativeype;
        }

        public List<NEWCON_MST_CONNECTION_TYPE> ListConnectionTypeMapping()
        {
            List<NEWCON_MST_CONNECTION_TYPE> lstconnectiontype = new List<NEWCON_MST_CONNECTION_TYPE>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lsttypeofconnection = dataContext.NEWCON_MST_CONNECTION_TYPEs.ToList();
                    foreach (var a in lsttypeofconnection)
                    {
                        lstconnectiontype.Add(
                            new NEWCON_MST_CONNECTION_TYPE
                            {
                                CONNECTION_TYPE_DESC = a.CONNECTION_TYPE_DESC,
                                CONNECTION_TYPE = a.CONNECTION_TYPE,
                            });
                    }
                    return lstconnectiontype;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lstconnectiontype;
        }

        public List<NEWCON_MST_APPLICANT_TYPE> ListGovernmentApplicationTypeMapping()
        {
            List<NEWCON_MST_APPLICANT_TYPE> lstapplicationtypegovernment = new List<NEWCON_MST_APPLICANT_TYPE>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lstGovernmentType = dataContext.NEWCON_MST_APPLICANT_TYPEs.ToList();
                    foreach (var a in lstGovernmentType)
                    {
                        lstapplicationtypegovernment.Add(
                            new NEWCON_MST_APPLICANT_TYPE
                            {
                                APPLICANT_TYPE = a.APPLICANT_TYPE,
                                APPLICANT_TYPE_DESC = a.APPLICANT_TYPE_DESC,
                                APPLICANT_TYPE_GovernmentList = a.APPLICANT_TYPE_GovernmentList
                            });
                    }
                    return lstapplicationtypegovernment;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lstapplicationtypegovernment;
        }
        public List<NEWCON_MST_APPLICANT_TITLE> ListAPPLICANT_TITLE1Mapping()
        {
            List<NEWCON_MST_APPLICANT_TITLE> lstapplicationtitle = new List<NEWCON_MST_APPLICANT_TITLE>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lstapplicantstype = dataContext.NEWCON_MST_APPLICANT_TITLEs.ToList();
                    foreach (var a in lstapplicantstype)
                    {
                        lstapplicationtitle.Add(
                            new NEWCON_MST_APPLICANT_TITLE
                            {
                                APPLICANT_TITLE = a.APPLICANT_TITLE,
                                APPLICANT_TITLE_DESC = a.APPLICANT_TITLE_DESC,
                            });
                    }
                    return lstapplicationtitle;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lstapplicationtitle;
        }
        public List<NEWCON_MST_BILL_LANGUAGE> ListBillLanguageMapping()
        {
            List<NEWCON_MST_BILL_LANGUAGE> lstbilllanguage = new List<NEWCON_MST_BILL_LANGUAGE>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lstbillinglang = dataContext.NEWCON_MST_BILL_LANGUAGEs.ToList();
                    foreach (var a in lstbillinglang)
                    {
                        lstbilllanguage.Add(
                            new NEWCON_MST_BILL_LANGUAGE
                            {
                                DESCRIPTION = a.DESCRIPTION,
                                ID = a.ID,
                            });
                    }
                    return lstbilllanguage;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lstbilllanguage;
        }

        public bool IsLECRegistered(string registrationNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONLECRegistrationDetails.Any(a => a.LECRegistrationNumber == registrationNumber && a.LECIsActive == true))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsLECRegistered: " + ex.Message, this);
            }
            return false;
        }

        public List<NEWCON_MST_PURPOSE_CONNECTION> ListPurposeOfSupplyMapping()
        {
            List<NEWCON_MST_PURPOSE_CONNECTION> lstPurposeOfSupply = new List<NEWCON_MST_PURPOSE_CONNECTION>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lstPurposeOfSupplyGen = dataContext.NEWCON_MST_PURPOSE_CONNECTIONs.OrderBy(o => o.PURPOSE_OF_CONNECTION).ToList();
                    foreach (var a in lstPurposeOfSupplyGen)
                    {
                        lstPurposeOfSupply.Add(
                            new NEWCON_MST_PURPOSE_CONNECTION
                            {
                                PURPOSE_OF_CONNECTION = a.PURPOSE_OF_CONNECTION,
                                CONNECTION_DESCRIPTION = a.CONNECTION_DESCRIPTION,
                                APPLIED_TARIFF = a.APPLIED_TARIFF,
                                TARIFF_DESCRIPTION = a.TARIFF_DESCRIPTION,
                                CONNECTION_TYPE = a.CONNECTION_TYPE,
                                IsAbove20 = a.IsAbove20,
                                ENABLE_FLAG_SUB_APP1 = a.ENABLE_FLAG_SUB_APP1,
                                ENABLE_FLAG_SUB_APP2 = a.ENABLE_FLAG_SUB_APP2,
                                PremiseType = a.PremiseType,
                                APPLICANT_TYPE=a.APPLICANT_TYPE,
                                APPLICANT_SUB_TYPE=a.APPLICANT_SUB_TYPE
                            });
                    }
                    return lstPurposeOfSupply;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lstPurposeOfSupply;

        }




        public List<NEWCON_MST_VOLTAGE_LEVEL> ListVoltageLevelMapping()
        {
            List<NEWCON_MST_VOLTAGE_LEVEL> lsVoltageLevel = new List<NEWCON_MST_VOLTAGE_LEVEL>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lstVoltageLevelGen = dataContext.NEWCON_MST_VOLTAGE_LEVELs.OrderBy(o => o.VOLTAGE_LEVEL).ToList();
                    foreach (var a in lstVoltageLevelGen)
                    {
                        lsVoltageLevel.Add(
                            new NEWCON_MST_VOLTAGE_LEVEL
                            {
                                VOLTAGE_LEVEL = a.VOLTAGE_LEVEL,
                                DESCRIPTION = a.DESCRIPTION,
                                CONNECTION_TYPE = a.CONNECTION_TYPE,
                                OA = a.OA,
                                MeterType = a.MeterType


                            });
                    }
                    return lsVoltageLevel;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lsVoltageLevel;

        }
        public List<NEWCON_MST_METERLOAD_TYPE> ListOfMeterLoadMapping()
        {
            List<NEWCON_MST_METERLOAD_TYPE> lstMeterLoad = new List<NEWCON_MST_METERLOAD_TYPE>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lstMeterLoadGen = dataContext.NEWCON_MST_METERLOAD_TYPEs.OrderBy(o => o.PURPOSE_CONNECTION).ToList();
                    foreach (var a in lstMeterLoadGen)
                    {
                        lstMeterLoad.Add(
                            new NEWCON_MST_METERLOAD_TYPE
                            {
                                ZLTYP = a.ZLTYP,
                                APPLIED_TARRIF = a.APPLIED_TARRIF,
                                PURPOSE_CONNECTION = a.PURPOSE_CONNECTION,
                                ZUNIT_KW = a.ZUNIT_KW,
                                ZUNIT_HP = a.ZUNIT_HP,
                                KWMANDATORY = a.KWMANDATORY,
                                HPMANDATORY = a.HPMANDATORY,
                                DESCRIPTION = a.DESCRIPTION,

                            });
                    }
                    return lstMeterLoad;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lstMeterLoad;

        }


        public List<NEWCON_MST_PREMISE_TYPE> ListOfPremiseType()
        {
            List<NEWCON_MST_PREMISE_TYPE> lstpremisetytpe = new List<NEWCON_MST_PREMISE_TYPE>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lstpremisetypegen = dataContext.NEWCON_MST_PREMISE_TYPEs.OrderBy(o => o.PREMISE_TYPE).ToList();
                    foreach (var a in lstpremisetypegen)
                    {
                        lstpremisetytpe.Add(
                            new NEWCON_MST_PREMISE_TYPE
                            {
                                TEXT = a.TEXT,
                                PREMISE_TYPE = a.PREMISE_TYPE,
                                ENABLE_SUB_APP1 = a.ENABLE_SUB_APP1,
                                ENABLE_FLAG_SUB_APP2 = a.ENABLE_FLAG_SUB_APP2

                            });
                    }
                    return lstpremisetytpe;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lstpremisetytpe;

        }



        //Shekhar Gigras - Upload Document Methods - Start
        public List<NEWCON_DOCUMENTMASTER> GetDocuments(string LoadType, string titleValue, string IsJoint)
        {
            List<NEWCON_DOCUMENTMASTER> result = new List<NEWCON_DOCUMENTMASTER>();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                result = dataContext.NEWCON_DOCUMENTMASTERs.Where(D => D.APPLICATION_TYPE_CODE.Trim() == IsJoint.Trim() && D.DELETEFLAG == "1" && D.ZALSTYP == LoadType && D.DOCID != 20 && (D.TITLE.Trim() == titleValue.Trim() || D.TITLE.Trim() == "0".Trim())).ToList();
            }
            return result;
        }

        public List<NEW_CON_APPLICATION_FORM> GetApplicationConnectionList(string mobileno,string searchtext)
        {
            List<NEW_CON_APPLICATION_FORM> result = new List<NEW_CON_APPLICATION_FORM>();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                result = dataContext.NEW_CON_APPLICATION_FORMs.Where(D => D.MobileNumber.Trim() == mobileno).ToList();
            }
            return result;
        }
        public List<NEW_CON_APPLICATION_FORM> GetLECApplicationConnectionList(string lecregno, string searchtext)
        {
            List<NEW_CON_APPLICATION_FORM> result = new List<NEW_CON_APPLICATION_FORM>();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                var lecresult = dataContext.NEWCON_OTHER_DETAILs.Where(D => D.LECNumber.Trim() == lecregno).Select(D=>D.AccountNumber).ToList();
                var resultList = dataContext.NEW_CON_APPLICATION_FORMs.ToList();
                foreach(var lec in lecresult)
                {
                    var obj = resultList.Find(cv => cv.ApplicationNumber.ToLower().Equals(lec.ToLower()));
                    if (obj != null)
                    {
                        result.Add(obj);
                    }
                }
            }
            return result;
        }

        public NEW_CON_APPLICATION_FORM GetApplicationConnectionDetail(string id)
        {
            NEW_CON_APPLICATION_FORM result = new NEW_CON_APPLICATION_FORM();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                result = dataContext.NEW_CON_APPLICATION_FORMs.Where(D => D.Id.ToString()==id).FirstOrDefault();
            }
            return result;
        }
        public NEWCON_LOAD_DETAIL GetApplicationLoadDetailConnectionDetail(string ApplicationNumber)
        {
            NEWCON_LOAD_DETAIL result = new NEWCON_LOAD_DETAIL();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                result = dataContext.NEWCON_LOAD_DETAILs.Where(D => D.AccountNumber.ToString() == ApplicationNumber).FirstOrDefault();
            }
            return result;
        }

        public NEWCON_OTHER_DETAIL GetApplicationOtherDetailConnectionDetail(string ApplicationNumber)
        {
            NEWCON_OTHER_DETAIL result = new NEWCON_OTHER_DETAIL();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                result = dataContext.NEWCON_OTHER_DETAILs.Where(D => D.AccountNumber.ToString() == ApplicationNumber).FirstOrDefault();
            }
            return result;
        }
        //Shekhar Gigras - Upload Document Methods - End


        //To be used in New Connection
        public static string ValidateCANewConnection(CAValidateInfo accountDetails)
        {
            string message = string.Empty;
            
            if (accountDetails.VIGILANCEFLAG == "X")
            {
                message = "Change of name request cannot be processed for the entered Account no. Please visit the nearest Divisional office for further assistance.";
            }
            else if (accountDetails.OVERDUE_AMT == "X" || System.Convert.ToDecimal(accountDetails.OVERDUE_AMT) > 0)
            {
                message = "Your previous connection : Arrears was Found. Please clear your Arrears";
            }

           
            
            return message;
        }
        
        public List<NEW_CON_APPLICATIONTYPE> ApplicationTypeList()
        {
            List<NEW_CON_APPLICATIONTYPE> lstapplicationtype = new List<NEW_CON_APPLICATIONTYPE>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lst = dataContext.NEW_CON_APPLICATIONTYPEs.ToList();
                    foreach (var a in lst)
                    {
                        lstapplicationtype.Add(
                            new NEW_CON_APPLICATIONTYPE
                            {
                                ApplicationId = a.ApplicationId.Trim(),
                                ApplicationType = a.ApplicationType.Trim(),
                            });
                    }
                    return lstapplicationtype;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ApplicationTypeList: " + ex.Message, this);
            }
            return lstapplicationtype;
        }

        public List<NEW_CON_APPLICATIONSUBTYPE> ApplicationSubTypeList()
        {
            List<NEW_CON_APPLICATIONSUBTYPE> lstapplicationtype = new List<NEW_CON_APPLICATIONSUBTYPE>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lst = dataContext.NEW_CON_APPLICATIONSUBTYPEs.ToList();
                    foreach (var a in lst)
                    {
                        lstapplicationtype.Add(
                            new NEW_CON_APPLICATIONSUBTYPE
                            {
                                ApplicationSubtypeDesc = a.ApplicationSubtypeDesc.Trim(),
                                ApplicationSubtype = a.ApplicationSubtype.Trim(),
                                ApplicationType=a.ApplicationType.Trim()
                            });
                    }
                    return lstapplicationtype;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ApplicationTypeList: " + ex.Message, this);
            }
            return lstapplicationtype;
        }
    }
}