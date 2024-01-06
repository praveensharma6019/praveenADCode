using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Ports.Website.Models;

namespace Sitecore.Ports.Website.Helper
{
    public class PortsIncomeTaxReturnHelper
    {
        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");

        public List<string> GetIsPanLinkedWithAadharList()
        {
            try
            {
                List<string> PanLinkedWithAadharList = new List<string>();
                PanLinkedWithAadharList.Add("Yes");
                PanLinkedWithAadharList.Add("No");

                PanLinkedWithAadharList.Add("NA");
                return PanLinkedWithAadharList;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetMSME: " + ex.Message, this);
            }
            return null;

        }
        public List<string> GetIsFilledItReturnFillingListt()
        {
            try
            {
                List<string> FilledItReturnFillingListt = new List<string>();
                FilledItReturnFillingListt.Add("Yes");
                FilledItReturnFillingListt.Add("No");

                FilledItReturnFillingListt.Add("NA");
                return FilledItReturnFillingListt;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetMSME: " + ex.Message, this);
            }
            return null;

        }
    }
}