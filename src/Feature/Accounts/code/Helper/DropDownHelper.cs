using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Sitecore.Feature.Accounts.Helper
{
    public static class DropDownHelper
    {
        public static List<Models.Society> GetSociety(string RegionArea, string PlantId)
        {
            List<Models.Society> SocietyList = new List<Models.Society>();
            var response = oDataHelper.GetDatabyUrl(string.Format(ConfigurationHelper.oDataGetCityMethodName, RegionArea, PlantId));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
            XmlNodeList list = doc.GetElementsByTagName("entry");
            foreach (XmlNode dr in list)
            {
                Models.Society society = new Models.Society();
                society.SocietyId = dr.ChildNodes[5].ChildNodes[0].ChildNodes[0].InnerText;
                society.SocietyName = dr.ChildNodes[5].ChildNodes[0].ChildNodes[3].InnerText;

                SocietyList.Add(society);
            }

            return SocietyList;
        }

        public static bool VerifyOTP(string MobileNo, string OTP)
        {
            //string otp = "";
            var response = oDataHelper.GetDatabyUrl(string.Format(ConfigurationHelper.VerifyOTP, MobileNo, OTP));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
            XmlNodeList list = doc.GetElementsByTagName("entry");
            if (list[0].ChildNodes[5].ChildNodes[0].ChildNodes[2].InnerText == "S")
                return true;
            else return false;
        }

        public static string SendSMS(string MobileNo, string Type, string InquiryNo)
        {
            string otp = "";
            var response = oDataHelper.GetDatabyUrl(string.Format(ConfigurationHelper.SendSMS, MobileNo, Type, InquiryNo));
            return otp;
        }

        public static List<Models.CityRegion> GetCityRegion()
        {
            List<Models.CityRegion> CityRegionList = new List<Models.CityRegion>();
            var response = oDataHelper.GetDatabyUrl(Helper.ConfigurationHelper.oDataGetCityRegionMethodName);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
            XmlNodeList list = doc.GetElementsByTagName("entry");
            foreach (XmlNode dr in list)
            {
                Models.CityRegion CityRegion = new Models.CityRegion();
                CityRegion.PlantId = dr.ChildNodes[5].ChildNodes[0].ChildNodes[0].InnerText;
                CityRegion.CityId = dr.ChildNodes[5].ChildNodes[0].ChildNodes[1].InnerText;
                CityRegion.CityName = dr.ChildNodes[5].ChildNodes[0].ChildNodes[2].InnerText;
                CityRegion.RegionId = dr.ChildNodes[5].ChildNodes[0].ChildNodes[3].InnerText;
                CityRegion.RegionName = dr.ChildNodes[5].ChildNodes[0].ChildNodes[4].InnerText;

                CityRegionList.Add(CityRegion);
            }

            return CityRegionList;
        }

        public static string GetOTP(string MobileNo)
        {
            string otp = "";
            var response = oDataHelper.GetDatabyUrl(string.Format(ConfigurationHelper.oDataGetOTP, MobileNo));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
            XmlNodeList list = doc.GetElementsByTagName("entry");
            foreach (XmlNode dr in list)
            {
                return dr.ChildNodes[5].ChildNodes[0].ChildNodes[1].InnerText;
               
            }

            return otp;
        }
        public static string SendSMS(string MobileNo, string Message)
        {
            string otp = "";
            var response = oDataHelper.GetDatabyUrl(string.Format(ConfigurationHelper.SendSMS, MobileNo, Message));
            return otp;
        }
    }
}