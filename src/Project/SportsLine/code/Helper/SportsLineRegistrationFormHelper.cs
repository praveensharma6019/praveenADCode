using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SportsLine.Website.Models;
using static Sitecore.SportsLine.Website.Models.SportsLineRegistrationFormModel;

namespace Sitecore.SportsLine.Website.Helper
{
    public class SportsLineRegistrationFormHelper
    {
        private Database db = Factory.GetDatabase("web");

        public SportsLineRegistrationFormHelper()
        {

        }
        public List<CheckBoxes> GetSectorServedList()
        {
            try
            {
                List<CheckBoxes> SSList = new List<CheckBoxes>()
                {
                    new CheckBoxes{Text="Cricket", Value="Cricket"},
                    new CheckBoxes{Text="Football", Value="Football"},
                    new CheckBoxes{Text="Tug Of War", Value="TugOfWar"}
                };
                return SSList;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSectorServed: " + ex.Message, this);
            }
            return null;
        }


        public string GetSelectedCheckboxValues(List<CheckBoxes> selectedBox)
        {
            string selectedValues = string.Empty;
            try
            {
                if (selectedBox != null && selectedBox.Count > 0)
                {
                    foreach (CheckBoxes cb in selectedBox)
                    {
                        if (cb.Checked)
                        {
                            selectedValues = selectedValues + cb.Value + "|";
                        }
                    }
                }
                return selectedValues.TrimEnd('|');
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSelectedCheckboxValues: " + ex.Message, this);
                return selectedValues;
            }
        }

        public string GetUniqueRegNo()
        {
            int num = 10;
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
            return stringBuilder.ToString();
        }


    }
}