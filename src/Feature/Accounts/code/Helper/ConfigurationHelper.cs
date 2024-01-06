using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Feature.Accounts.Helper
{
    public static class ConfigurationHelper
    {
        public static string ServiceBaseUrl_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/ServiceBaseUrl_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV");
        public static string ServiceUserName_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/ServiceUserName_field", "UMC_SRV_USR1");
        public static string ServicePassword_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/ServicePassword_field", "Adani@123");
        public static string oDataGetDocumentListMethodName_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/oDataGetDocumentListMethodName_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/Doc_listSet");
        public static string UploadFileLocalPath_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/UploadFileLocalPath_field", "/AdaniGasERegistraion/Upload_Files");
        public static string DocumentSaveMethodName_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/DocumentSaveMethodName_field", "http://13.235.191.185:8081/service1.asmx?op=UploadFileFTP");

        public static string oDataGetCityRegionMethodName_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/oDataGetCityRegionMethodNam_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/RSA_MappingSet");
        public static string oDataGetCityMethodName_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/oDataGetCityMethodName_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/ConnObjSet?$filter=RegioGroup eq '{0}' and Plant eq '{1}'");
        public static string oDataSaveInquiry_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/oDataSaveInquiry_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL002_SRV//InquirySet");
        public static string oDataGetOTP_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/oDataGetOTP_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/SendOtpSet('{0}')");
        public static string oDataGetInquiry_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/oDataGetInquiry_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL002_SRV/InquiryDetailsSet('{0}')");
        public static string oDataCreateuser_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/oDataCreateuser_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/CreateUserSet");
        public static string oDataVerifyUser_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/oDataVerifyUser_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/CheckUserSet('{0}')");
        public static string VerifyOTP_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/oDataVerifyUser_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/ValidateOtpSet(MobileNo='{0}',ValidOtp='{1}')");
        public static string oDataSaveInqDocument_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/oDataSaveInqDocument_field", "https://13.235.191.185/Odata.svc/sap_inq");
        public static string Schemes_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/Schemes_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/SchemeMasterSet?$filter=Plant eq '1004' and Bpkind eq '9004'");
        public static string NewEnquirySecond_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/NewEnquirySecond_field", "https://13.235.191.185/Odata.svc/new_inq");

        public static string DomesticAdditionDetailsSave_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/DomesticAdditionDetailsSave_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL002_SRV/DomesticAddDataSet");
        public static string SetActualBPSet_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/SetActualBPSet_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL002_SRV/ProspectBpSet('{0}')");
        public static string GetActualBPSet_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/GetActualBPSet_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL002_SRV/ActualBpSet(Prospect='{0}',Scheme='{1}')");
        public static string SendSMS_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/SendSMS_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/FSO_SmsSet(MobileNo='{0}',Text='{1}')");
        public static string NewEnquirySecondDetails_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/NewEnquirySecondDetails_field", "https://13.235.191.185/Odata.svc/new_inq?$filter=SAPINQID eq '{0}'");
        public static string UpdateInquiryStatus_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/UpdateInquiryStatus_field", "https://13.235.191.185/Odata.svc/new_inq({0})");
        public static string GetInquiryStatus_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/GetInquiryStatus_field", "https://13.235.191.185/Odata.svc/inqstatus?$filter=SAPINQID eq '{0}'");
        public static string GetUploadedDocuments_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/GetUploadedDocuments_field", "https://13.235.191.185/Odata.svc/sap_inq('{0}')/sap_inq_attch");
        public static string AddDocumentInExistingList_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/AddDocumentInExistingList_field", "https://13.235.191.185/Odata.svc/sap_inq_update");
        public static string AddInqStatus_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/AddInqStatus_field", "https://13.235.191.185/Odata.svc/inqstatus");
        public static string GetInquiryPaidStatus_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/GetInquiryPaidStatus_field", "https://13.235.191.185/Odata.svc/inqstatus?$filter=SAPINQID eq '{0}' and PAID_STATUS eq 'PAID'");
        public static string InquiryStatusSet_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/InquiryStatusSet_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/InquiryStatusSet");
        public static string InquiryStatusSetGet_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/InquiryStatusSetGet_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL001_SRV/InquiryStatusSet('{0}')");
        public static string AdditionDataSave_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/AdditionDataSave_field", "https://13.235.191.185/Odata.svc/sap_inq_dtl");
        public static string AdditionDataGet_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/AdditionDataGet_field", "https://13.235.191.185/Odata.svc/sap_inq_dtl('{0}')");
        public static string AdditionDataUpdate_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/AdditionDataUpdate_field", "https://13.235.191.185/Odata.svc/sap_inq_dtl_update");
        public static string GetVehicleType_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/GetVehicleType_field", "https://13.235.191.185/Odata.svc/vehical_type?$select=Vehical_Type1");
        public static string GetVehicleMake_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/GetVehicleMake_field", "https://13.235.191.185/Odata.svc/vehical_type?$filter= Vehical_Type1 eq '{0}'");
        public static string GetVehicleModel_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/GetVehicleModel_field", "https://13.235.191.185/Odata.svc/vehical_type?$filter= Vehical_Type1 eq '{0}' and Make eq '{1}'");
        public static string CNGInquirySave_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/CNGInquirySave_field", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMYAGL005_SRV/CNGInquirySet");
        public static string VAPTServiceUserName_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/VAPTServiceUserName_field", "umc_srv_usr1");
        public static string VAPTServiceUrl_field = DictionaryPhraseRepository.Current.Get("/AccountServices/Enquiry Service URLs/VAPTServiceUrl_field", "https://13.235.191.185/Odata.svc/");

        public static string ServiceBaseUrl
        {
            get
            {
                return ServiceBaseUrl_field;// ConfigurationManager.AppSettings["ServiceBaseUrl"];
            }
        }

        public static string ServiceUserName
        {
            get
            {
                return ServiceUserName_field;// ConfigurationManager.AppSettings["ServiceUserName"];
            }
        }

        public static string ServicePassword
        {
            get
            {
                return ServicePassword_field;// ConfigurationManager.AppSettings["ServicePassword"];
            }
        }

        public static string oDataGetDocumentListMethodName
        {
            get
            {
                return oDataGetDocumentListMethodName_field;// ConfigurationManager.AppSettings["oDataGetDocumentListMethodName"];
            }
        }

        public static string UploadFileLocalPath
        {
            get
            {
                return UploadFileLocalPath_field;// ConfigurationManager.AppSettings["UploadFileLocalPath"];
            }
        }

        public static string DocumentSaveMethodName
        {
            get
            {
                return DocumentSaveMethodName_field;// ConfigurationManager.AppSettings["DocumentSaveMethodName"];
            }
        }

        public static string VAPTServiceUrl
        {
            get
            {
                return VAPTServiceUrl_field;//ConfigurationManager.AppSettings["VAPTServiceUrl"];
            }
        }

        public static string VAPTServiceUserName
        {
            get
            {
                return VAPTServiceUserName_field;// ConfigurationManager.AppSettings["VAPTServiceUserName"];
            }
        }

        public static string oDataGetCityRegionMethodName
        {
            get
            {
                return oDataGetCityRegionMethodName_field;// ConfigurationManager.AppSettings["oDataGetCityRegionMethodName"];
            }
        }



        public static string oDataGetCityMethodName
        {
            get
            {
                return oDataGetCityMethodName_field;// ConfigurationManager.AppSettings["oDataGetCityMethodName"];
            }
        }



        public static string oDataSaveInquiry
        {
            get
            {
                return oDataSaveInquiry_field;// ConfigurationManager.AppSettings["oDataSaveInquiry"];
            }
        }

        public static string oDataGetOTP
        {
            get
            {
                return oDataGetOTP_field;// ConfigurationManager.AppSettings["oDataGetOTP"];
            }
        }



        public static string oDataGetInquiry
        {
            get
            {
                return oDataGetInquiry_field;// ConfigurationManager.AppSettings["oDataGetInquiry"];
            }
        }


        public static string oDataCreateuser
        {
            get
            {
                return oDataCreateuser_field;// ConfigurationManager.AppSettings["oDataCreateuser"];
            }
        }

        public static string oDataVerifyUser
        {
            get
            {
                return oDataVerifyUser_field;// ConfigurationManager.AppSettings["oDataVerifyUser"];
            }
        }

        public static string VerifyOTP
        {
            get
            {
                return VerifyOTP_field; //ConfigurationManager.AppSettings["VerifyOTP"];
            }
        }


        public static string oDataSaveInqDocument
        {
            get
            {
                return oDataSaveInqDocument_field;//return ConfigurationManager.AppSettings["oDataSaveInqDocument"];
            }
        }

        public static string Schemes
        {
            get
            {
                return Schemes_field;// ConfigurationManager.AppSettings["Schemes"];
            }
        }

        public static string NewEnquirySecond
        {
            get
            {
                return NewEnquirySecond_field;// ConfigurationManager.AppSettings["NewEnquirySecond"];
            }
        }

        public static string DomesticAdditionDetailsSave
        {
            get
            {
                return DomesticAdditionDetailsSave_field;// ConfigurationManager.AppSettings["DomesticAdditionDetailsSave"];
            }
        }
        public static string SetActualBPSet
        {
            get
            {
                return SetActualBPSet_field;// ConfigurationManager.AppSettings["SetActualBPSet"];
            }
        }

        public static string GetActualBPSet
        {
            get
            {
                return GetActualBPSet_field;// ConfigurationManager.AppSettings["GetActualBPSet"];
            }

        }
        public static string SendSMS
        {
            get
            {
                return SendSMS_field;// ConfigurationManager.AppSettings["SendSMS"];
            }
        }

        public static string NewEnquirySecondDetails
        {
            get
            {
                return NewEnquirySecondDetails_field;// ConfigurationManager.AppSettings["NewEnquirySecondDetails"];
            }
        }

        public static string UpdateInquiryStatus
        {
            get
            {
                return UpdateInquiryStatus_field;// ConfigurationManager.AppSettings["UpdateInquiryStatus"];
            }
        }

        public static string GetInquiryStatus
        {
            get
            {
                return GetInquiryStatus_field;// ConfigurationManager.AppSettings["GetInquiryStatus"];
            }
        }

        public static string GetUploadedDocuments
        {
            get
            {
                return GetUploadedDocuments_field;// ConfigurationManager.AppSettings["GetUploadedDocuments"];
            }
        }
        public static string AddDocumentInExistingList
        {
            get
            {
                return AddDocumentInExistingList_field;// ConfigurationManager.AppSettings["AddDocumentInExistingList"];
            }
        }


        public static string AddInqStatus
        {
            get
            {
                return AddInqStatus_field;// ConfigurationManager.AppSettings["AddInqStatus"];
            }
        }

        public static string GetInquiryPaidStatus
        {
            get
            {
                return GetInquiryPaidStatus_field;// ConfigurationManager.AppSettings["GetInquiryPaidStatus"];
            }
        }

        public static string InquiryStatusSet
        {
            get
            {
                return InquiryStatusSet_field;// ConfigurationManager.AppSettings["InquiryStatusSet"];
            }
        }

        public static string InquiryStatusSetGet
        {
            get
            {
                return InquiryStatusSetGet_field;// ConfigurationManager.AppSettings["InquiryStatusSetGet"];
            }
        }

        //
        public static string AdditionDataSave
        {
            get
            {
                return AdditionDataSave_field;// ConfigurationManager.AppSettings["AdditionDataSave"];
            }
        }

        public static string AdditionDataGet
        {
            get
            {
                return AdditionDataGet_field;// ConfigurationManager.AppSettings["AdditionDataGet"];
            }
        }

        public static string AdditionDataUpdate
        {
            get
            {
                return AdditionDataUpdate_field;// ConfigurationManager.AppSettings["AdditionDataUpdate"];
            }
        }

        public static string GetVehicleType
        {
            get
            {
                return GetVehicleType_field;// ConfigurationManager.AppSettings["GetVehicleType"];
            }
        }

        public static string GetVehicleMake
        {
            get
            {
                return GetVehicleMake_field;// ConfigurationManager.AppSettings["GetVehicleMake"];
            }
        }

        public static string GetVehicleModel
        {
            get
            {
                return GetVehicleModel_field;// ConfigurationManager.AppSettings["GetVehicleModel"];
            }
        }

        public static string CNGInquirySave
        {
            get
            {
                return CNGInquirySave_field;// ConfigurationManager.AppSettings["CNGInquirySave"];
            }
        }
    }
}