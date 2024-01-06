using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Defence.Website.Models;

namespace Sitecore.Defence.Website.Helper
{
    public class Helper
    {
        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
        public List<string> GetMSME()
        {
            try
            {
                List<string> msme = new List<string>();
                msme.Add("Micro");
                msme.Add("Small");
                msme.Add("Medium");
                msme.Add("NA");
                return msme;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetMSME: " + ex.Message, this);
            }
            return null;

        }
        public List<string> GetTypeofOwnership()
        {
            try
            {
                Item listItem = db.GetItem("{BF324A15-9FDF-4C3D-B288-2F00DF9B461E}");
                if (listItem != null)
                {
                    if (listItem.HasChildren)
                    {
                        List<string> ToOwn = new List<string>();
                        {
                            foreach (Item ml in listItem.GetChildren())
                            {
                                var itm = ml.Fields["Value"].Value;
                                ToOwn.Add(itm);
                            }
                        };
                        return ToOwn;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetTypeofOwnership: " + ex.Message, this);
            }
            return null;
        }
        public List<string> GetFinancialYearsList()
        {
            try
            {
                Item listItem = db.GetItem("{75CFDB76-3BDE-435E-B02C-22E733BC2380}");
                if (listItem != null)
                {
                    if (listItem.HasChildren)
                    {
                        List<string> ToOwn = new List<string>();
                        {
                            foreach (Item ml in listItem.GetChildren())
                            {
                                var itm = ml.Fields["Value"].Value;
                                ToOwn.Add(itm);
                            }
                        };
                        return ToOwn;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetFinancialYearsList: " + ex.Message, this);
            }
            return null;
        }
        public List<CheckBoxes> GetSectorServedList()
        {
            try
            {
                List<CheckBoxes> SSList = new List<CheckBoxes>()
                {
                    new CheckBoxes{Text="Defence", Value="Defence" },
                    new CheckBoxes{Text="Aerospace", Value="Aerospace" },
                    new CheckBoxes{Text="Other", Value=""}
                };
                return SSList;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSectorServed: " + ex.Message, this);
            }
            return null;
        }

        public List<CheckBoxes> GetSupplierTypeList()
        {
            try
            {
                List<CheckBoxes> STList = new List<CheckBoxes>()
                {
                    new CheckBoxes{Text="Tier1", Value="Tier1" },
                    new CheckBoxes{Text="Tier2", Value="Tier2" },
                    new CheckBoxes{Text="Tier3", Value="Tier3"}
                };
                return STList;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSupplierTypeList: " + ex.Message, this);
            }
            return null;
        }

        public List<CheckBoxes> GetSegmentServedTypeList()
        {
            try
            {
                List<CheckBoxes> SSTList = new List<CheckBoxes>()
                {
                    new CheckBoxes{Text="Land Army", Value="Land Army" },
                    new CheckBoxes{Text="Airforce", Value="Airforce" },
                    new CheckBoxes{Text="Naval", Value="Naval"},
                    new CheckBoxes{Text="Space", Value="Space"},
                    new CheckBoxes{Text="UAV", Value="UAV"},
                    new CheckBoxes{Text="Other", Value=""}
                };
                return SSTList;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSegmentServedTypeList: " + ex.Message, this);
            }
            return null;
        }

        public List<CheckBoxes> GetDA_PlatformsServedList()
        {
            try
            {
                List<CheckBoxes> DAPSList = new List<CheckBoxes>()
                {
                    new CheckBoxes{Text="Small Arms", Value="Small Arms" },
                    new CheckBoxes{Text="Air Defence Guns", Value="Air Defence Guns" },
                    new CheckBoxes{Text="Radar", Value="Radar"},
                    new CheckBoxes{Text="Commerical AirCraft", Value="Commerical AirCraft" },
                    new CheckBoxes{Text="Civil Aviation", Value="Civil Aviation" },
                    new CheckBoxes{Text="MRO", Value="MRO"},
                    new CheckBoxes{Text="Fighter Aircraft", Value="Fighter Aircraft" },
                    new CheckBoxes{Text="Helicopter", Value="Helicopter" },
                    new CheckBoxes{Text="Missiles", Value="Missiles"},
                    new CheckBoxes{Text="UAV", Value="UAV" },
                    new CheckBoxes{Text="Other", Value="" }
                };
                return DAPSList;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSectorServed: " + ex.Message, this);
            }
            return null;
        }

        public List<CheckBoxes> GetSupplierToList()
        {
            try
            {
                List<CheckBoxes> GSList = new List<CheckBoxes>()
                {
                    new CheckBoxes{Text="Ministry of Defence", Value="Ministry of Defence" },
                    new CheckBoxes{Text="DRDO", Value="DRDO" },
                    new CheckBoxes{Text="DPSU", Value="DPSU"},
                    new CheckBoxes{Text="Airbus", Value="Airbus"},
                    new CheckBoxes{Text="Boeing", Value="Boeing"},
                    new CheckBoxes{Text="Other", Value=""}
                };
                return GSList;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSegmentServedTypeList: " + ex.Message, this);
            }
            return null;
        }

        public IEnumerable<SelectListItem> GetallManufacturTypeList()
        {
            try
            {
                Item listItem = db.GetItem("{05D728D6-6293-4F6B-88AC-F9079DBAEB08}");
                if (listItem != null)
                {
                    if (listItem.HasChildren)
                    {
                        List<SelectListItem> MfgList = new List<SelectListItem>();
                        {
                            foreach (Item ml in listItem.GetChildren())
                            {
                                SelectListItem itm = new SelectListItem { Value = ml.Fields["Value"].Value, Text = ml.Fields["Name"].Value };
                                MfgList.Add(itm);
                            }
                        };
                        return MfgList;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSegmentServedTypeList: " + ex.Message, this);
            }
            return null;
        }

        public IEnumerable<SelectListItem> GetallTrader_D_DTypeList()
        {
            try
            {
                Item listItem = db.GetItem("{D80DA46D-249D-4551-B168-D701486DC818}");
                if (listItem != null)
                {
                    if (listItem.HasChildren)
                    {
                        List<SelectListItem> TDDList = new List<SelectListItem>();
                        {
                            foreach (Item ml in listItem.GetChildren())
                            {
                                SelectListItem itm = new SelectListItem { Value = ml.Fields["Value"].Value, Text = ml.Fields["Name"].Value };
                                TDDList.Add(itm);
                            }
                        };
                        return TDDList;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSegmentServedTypeList: " + ex.Message, this);
            }
            return null;
        }

        public IEnumerable<SelectListItem> GetallSpclPro_TLTypeList()
        {
            try
            {
                Item listItem = db.GetItem("{7B598EA9-AB65-41F4-9920-B6664F5F1E83}");
                if (listItem != null)
                {
                    if (listItem.HasChildren)
                    {
                        List<SelectListItem> SPTLList = new List<SelectListItem>();
                        {
                            foreach (Item ml in listItem.GetChildren())
                            {
                                SelectListItem itm = new SelectListItem { Value = ml.Fields["Value"].Value, Text = ml.Fields["Name"].Value };
                                SPTLList.Add(itm);
                            }
                        };
                        return SPTLList;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSegmentServedTypeList: " + ex.Message, this);
            }
            return null;
        }

        public IEnumerable<SelectListItem> GetallEnggServicesTypeList()
        {
            try
            {
                Item listItem = db.GetItem("{55AA10AC-3F50-4F77-9910-5856A2499023}");
                if (listItem != null)
                {
                    if (listItem.HasChildren)
                    {
                        List<SelectListItem> EnggSList = new List<SelectListItem>();
                        {
                            foreach (Item ml in listItem.GetChildren())
                            {
                                SelectListItem itm = new SelectListItem { Value = ml.Fields["Value"].Value, Text = ml.Fields["Name"].Value };
                                EnggSList.Add(itm);
                            }
                        };
                        return EnggSList;
                    }
                }                
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSegmentServedTypeList: " + ex.Message, this);
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
            int maxSize = 10;
            int minSize = 5;
            char[] chars = new char[62];
            string a;
            a = "1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }
    }
}