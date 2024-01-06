using Sitecore.AdaniGas.Website.Models;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Sitecore.AdaniGas.Website.Controllers
{
    public class AdaniGasWebsiteController : Controller
    {
        private HttpClient client1;
        public string GetPriceByCityURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetPriceByCityURL", "PriceListSet?$filter=City%20eq%20'{0}'");
        public string UserName = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/UserName", "UMC_SRV_USR");
        public string Password = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/Password", "init@123");
        public string GetGasPriceURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/GetGasPriceURL", "PriceListSet");
        public string ServiceURL = DictionaryPhraseRepository.Current.Get("/AccountServices/Service URLs/ServiceURL", "https://www.adaniportal.com:8081/sap/opu/odata/sap/ZMCF001_SRV/");

        // GET: AdaniGasWebsite
        public ActionResult DomesticCostCalculator()
        {
            Log.Info("At DomesticCostCalculator", this);
            DomesticPNGCCModel model = new DomesticPNGCCModel();
            try
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");

                //Data.Items.Item locationWiseGasRate = db.GetItem(Templates.CostCalculatorstConfiguration.Datasource.DomesticPNGCC_LocationWiseCurrentPNGGasRate);
                //List<SelectListItem> locationWiseGasRateList = locationWiseGasRate.GetChildren().ToList().Select(x => new SelectListItem()
                //{
                //    Text = x.Fields["Text"].Value,
                //    Value = x.Fields["Value"].Value
                //}).ToList();

                var getAllCityWisePrice = GetGasPrice(null);
                var list = getAllCityWisePrice.Where(p => p.Product.Contains("Domestic")).ToList();
                model.LocationWiseGasRateList = list.Where(p => p.Product.Contains("Domestic") && !p.Product.Contains("Slab-2")).GroupBy(p => p.City).Select(x => new SelectListItem()
                {
                    Text = x.FirstOrDefault().City.Trim(),
                    Value = x.FirstOrDefault().MMBTU_Rate.Trim() + "|" + x.FirstOrDefault().Eff_date,
                }).OrderBy(o => o.Text).ToList();



                //model.LocationWiseGasRateList.Insert(0, new SelectListItem { Text = "- Select City -", Value = "" });

                //model.selectedLocationForGasRate = model.LocationWiseGasRateList.FirstOrDefault().Value;
                if (getAllCityWisePrice != null)
                {
                    foreach (var item in getAllCityWisePrice.Where(p => p.Product.Contains("Domestic") && !p.Product.Contains("Slab-2")).ToList())
                    {
                        if (item.City == model.selectedLocationForGasRate)
                        {
                            model.Date = item.Eff_date;
                        }
                        else
                        {
                            model.Date = "NA";
                        }
                    }
                }

                Data.Items.Item cylinderSizes = db.GetItem(Templates.CostCalculatorstConfiguration.Datasource.DomesticPNGCC_CylinderSizes);
                List<SelectListItem> cylinderSizesList = cylinderSizes.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();

                // model.LocationWiseGasRateList = locationWiseGasRateList;
                model.CylinderSizesList = cylinderSizesList;
                model.selectedCylinderSize = model.CylinderSizesList.FirstOrDefault().Value;
            }
            catch (Exception ex)
            {
                Log.Error("Error at Domestic cost calculator post: ", ex, this);
            }
            return View("DomesticCostCalculator", model);
        }

        [HttpPost]
        public ActionResult DomesticCostCalculator(DomesticPNGCCModel model)
        {
            Log.Info("At DomesticCostCalculator Post", this);
            try
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                Sitecore.Data.Items.Item itemInfo = db.GetItem(new Data.ID(Templates.CostCalculatorstConfiguration.ID.ToString()));

                var factor1 = System.Convert.ToDouble(itemInfo.Fields[Templates.CostCalculatorstConfiguration.DomesticPNGCostCalculator.EquivalentMMBTUFactor1].Value);
                var factor2 = System.Convert.ToDouble(itemInfo.Fields[Templates.CostCalculatorstConfiguration.DomesticPNGCostCalculator.EquivalentMMBTUFactor2].Value);
                var factor3 = System.Convert.ToDouble(itemInfo.Fields[Templates.CostCalculatorstConfiguration.DomesticPNGCostCalculator.EquivalentMMBTUFactor3].Value);

                //Data.Items.Item locationWiseGasRate = db.GetItem(Templates.CostCalculatorstConfiguration.Datasource.DomesticPNGCC_LocationWiseCurrentPNGGasRate);
                //List<SelectListItem> locationWiseGasRateList = locationWiseGasRate.GetChildren().ToList().Select(x => new SelectListItem()
                //{
                //    Text = x.Fields["Text"].Value,
                //    Value = x.Fields["Value"].Value
                //}).ToList();

                var getAllCityWisePrice = GetGasPrice(null);
                var list = getAllCityWisePrice.Where(p => p.Product.Contains("Domestic")).ToList();
                model.LocationWiseGasRateList = list.Where(p => p.Product.Contains("Domestic") && !p.Product.Contains("Slab-2")).GroupBy(p => p.City).Select(x => new SelectListItem()
                {
                    Text = x.FirstOrDefault().City.Trim(),
                    Value = x.FirstOrDefault().MMBTU_Rate.Trim() + "|" + x.FirstOrDefault().Eff_date,
                }).OrderBy(o => o.Text).ToList();
                //model.LocationWiseGasRateList.Insert(0, new SelectListItem { Text = "- Select City -", Value = "" });
                if (getAllCityWisePrice != null)
                {
                    foreach (var item in getAllCityWisePrice.Where(p => p.Product.Contains("Domestic") && !p.Product.Contains("Slab-2")).ToList())
                    {
                        if (item.City == model.selectedLocationForGasRate)
                        {
                            model.Date = item.Eff_date;
                            break;
                        }
                        else
                        {
                            model.Date = "NA";
                        }
                    }
                }

                Data.Items.Item cylinderSizes = db.GetItem(Templates.CostCalculatorstConfiguration.Datasource.DomesticPNGCC_CylinderSizes);
                List<SelectListItem> cylinderSizesList = cylinderSizes.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();

                //model.LocationWiseGasRateList = locationWiseGasRateList;
                model.CylinderSizesList = cylinderSizesList;

                if (!ModelState.IsValid)
                {
                    return View("DomesticCostCalculator", model);
                }
                else
                {
                    //Total kgs of LPG gas consumed = number of LPG cylinders * Size of the LPG cylinder(in kgs)
                    double numberOfLPGcylinders = model.NumberOfCylinders;
                    double sizeoftheLPGcylinder = System.Convert.ToDouble(model.selectedCylinderSize);
                    double totalkgsofLPGgasconsumed = numberOfLPGcylinders * sizeoftheLPGcylinder;
                    double averagecostperLPGcylinder = model.AverageCostPerCylinder;

                    //Cost of LPG gas per kg = Average cost per LPG cylinder / Size of the LPG cylinder(in kgs)
                    double costofLPGgasperkg = averagecostperLPGcylinder / sizeoftheLPGcylinder;

                    //Annual LPG Cost = Total kgs of LPG gas consumed * Cost of LPG gas per kg
                    double annualLPGCost = totalkgsofLPGgasconsumed * costofLPGgasperkg;

                    //Equivalent MMBTU = (Total kgs of LPG gas consumed * 11500 * 3.968321) / 1000000
                    double equivalentMMBTU = (totalkgsofLPGgasconsumed * System.Convert.ToDouble(factor1) * System.Convert.ToDouble(factor2)) / System.Convert.ToDouble(factor3);

                    var cngRatecities = getAllCityWisePrice.Where(p => p.Product.Contains("Domestic") && !p.Product.Contains("Slab-2")).Select(x => new SelectListItem()
                    {
                        Text = x.City,
                        Value = x.MMBTU_Rate
                    }).ToList();

                    //Estimated Cost for PNG gas = Equivalent MMBTU * Current PNG Gas Rate
                    string currentPNGGasRate = cngRatecities.Where(l => l.Text.ToLower() == model.selectedLocationForGasRate.ToLower()).Select(y => y.Value).FirstOrDefault() ?? "00.00";
                    // model.selectedLocationForGasRate;
                    double estimatedCostforPNGgas = System.Convert.ToDouble(equivalentMMBTU) * System.Convert.ToDouble(currentPNGGasRate);

                    //Savings = Annual LPG Cost - Estimated Cost for PNG gas
                    double savings = annualLPGCost - estimatedCostforPNGgas;

                    model.TotalCost = estimatedCostforPNGgas;
                    model.TotalSaving = savings;
                    model.EquivalentMMBTU = equivalentMMBTU;
                    if (!string.IsNullOrEmpty(currentPNGGasRate))
                    {
                        model.Averagecost = System.Convert.ToDouble(currentPNGGasRate);
                    }
                    else
                    {
                        model.Averagecost = 00.00;
                    }


                    return View("DomesticCostCalculator", model);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at Domestic cost calculator post: ", ex, this);
            }
            return View("DomesticCostCalculator", model);
        }

        public ActionResult CNGCostCalculator()
        {
            Log.Info("At CNGCostCalculator", this);
            CNGCCModel model = new CNGCCModel();
            try
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                //Data.Items.Item locationWiseGasRate = db.GetItem(Templates.CostCalculatorstConfiguration.Datasource.CNGCostCalculator_LocationWiseRateOfGasPerKG);
                //List<SelectListItem> locationWiseGasRateList = locationWiseGasRate.GetChildren().ToList().Select(x => new SelectListItem()
                //{
                //    Text = x.Fields["Text"].Value,
                //    Value = x.Fields["Value"].Value
                //}).ToList();
                //model.LocationWiseCNGRateList = locationWiseGasRateList;
                //model.selectedLocationForCNGRate = model.LocationWiseCNGRateList.FirstOrDefault().Value;

                var getAllCityWisePrice = GetGasPrice(null);
                //var cities = getAllCityWisePrice.Where(p => p.Product == "CNG In Rs./KG").Select(s => s.City).Distinct().ToList();
                model.LocationWiseCNGRateList = getAllCityWisePrice.Where(p => p.Product == "CNG In Rs./KG").Select(x => new SelectListItem()
                {
                    Text = x.City,
                    Value = x.MMBTU_Rate
                }).ToList();

                //model.selectedLocationForCNGRate = model.LocationWiseCNGRateList.FirstOrDefault().Value;

                Data.Items.Item FuelTypes = db.GetItem(Templates.CostCalculatorstConfiguration.Datasource.CNGCC_FuelTypes);
                List<SelectListItem> FuelTypeList = FuelTypes.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();
                model.FuelTypeList = FuelTypeList;
                model.selectedFuelType = model.FuelTypeList.FirstOrDefault().Value;
            }
            catch (Exception ex)
            {
                Log.Error("Error at Domestic cost calculator post: ", ex, this);
            }
            return View("CNGCostCalculator", model);
        }

        [HttpPost]
        public ActionResult CNGCostCalculator(CNGCCModel model)
        {
            Log.Info("At DomesticCostCalculator Post", this);
            try
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                Sitecore.Data.Items.Item itemInfo = db.GetItem(new Data.ID(Templates.CostCalculatorstConfiguration.ID.ToString()));
                var expectedCNGMileage = System.Convert.ToDouble(itemInfo.Fields[Templates.CostCalculatorstConfiguration.CNGCostCalculator.ExpectedCNGMileage].Value);

                //Data.Items.Item locationWiseGasRate = db.GetItem(Templates.CostCalculatorstConfiguration.Datasource.CNGCostCalculator_LocationWiseRateOfGasPerKG);
                //List<SelectListItem> locationWiseGasRateList = locationWiseGasRate.GetChildren().ToList().Select(x => new SelectListItem()
                //{
                //    Text = x.Fields["Text"].Value,
                //    Value = x.Fields["Value"].Value
                //}).ToList();
                //model.LocationWiseCNGRateList = locationWiseGasRateList;

                var getAllCityWisePrice = GetGasPrice(null);
                model.LocationWiseCNGRateList = getAllCityWisePrice.Where(p => p.Product == "CNG In Rs./KG").Select(x => new SelectListItem()
                {
                    Text = x.City,
                    Value = x.MMBTU_Rate
                }).ToList();

                Data.Items.Item FuelTypes = db.GetItem(Templates.CostCalculatorstConfiguration.Datasource.CNGCC_FuelTypes);
                List<SelectListItem> FuelTypeList = FuelTypes.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();
                model.FuelTypeList = FuelTypeList;

                Data.Items.Item LocationWisePetrolRate = db.GetItem(Templates.CostCalculatorstConfiguration.Datasource.CNGCostCalculator_CurrentFuelRate_Petrol);
                List<SelectListItem> LocationWisePetrolRateList = LocationWisePetrolRate.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();

                Data.Items.Item LocationWiseDieselRate = db.GetItem(Templates.CostCalculatorstConfiguration.Datasource.CNGCostCalculator_CurrentFuelRate_Diesel);
                List<SelectListItem> LocationWiseDieselRateList = LocationWiseDieselRate.GetChildren().ToList().Select(x => new SelectListItem()
                {
                    Text = x.Fields["Text"].Value,
                    Value = x.Fields["Value"].Value
                }).ToList();

                if (!ModelState.IsValid)
                {
                    return View("CNGCostCalculator", model);
                }
                else
                {
                    var avgRun = model.AverageRun;
                    var currentMileage = model.CurrentMileage;
                    var selectedLocation = string.Empty;
                    if (!string.IsNullOrEmpty(model.selectedLocationForCNGRate))
                    {
                        selectedLocation = model.selectedLocationForCNGRate;
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.selectedLocationForCNGRate), "Please select city.");
                        return this.View(model);
                    }


                    var cngRatecities = getAllCityWisePrice.Where(p => p.Product == "CNG In Rs./KG").Select(x => new SelectListItem()
                    {
                        Text = x.City,
                        Value = x.MMBTU_Rate
                    }).ToList();


                    var cngRate = System.Convert.ToDouble(cngRatecities.Where(m => m.Text.ToLower() == selectedLocation.ToLower()).Select(y => y.Value).FirstOrDefault());

                    var typeOfCNG = model.selectedFuelType;
                    double fuelRate = 0;

                    if (typeOfCNG == "petrol")
                    {
                        var petrolRateForCityObj = LocationWisePetrolRateList.Where(m => m.Text == selectedLocation).FirstOrDefault();
                        if (petrolRateForCityObj != null)
                        {
                            fuelRate = System.Convert.ToDouble(petrolRateForCityObj.Value);
                        }
                        else
                        {
                            fuelRate = 0;
                        }
                    }
                    else if (typeOfCNG == "diesel")
                    {
                        var deiselRateForCityObj = LocationWiseDieselRateList.Where(m => m.Text == selectedLocation).FirstOrDefault();
                        if (deiselRateForCityObj != null)
                        {
                            fuelRate = System.Convert.ToDouble(deiselRateForCityObj.Value);
                        }
                        else
                        {
                            fuelRate = 0;
                        }
                    }
                    else
                    {
                        fuelRate = 0;
                    }

                    //Cost calculator for CNG: -
                    //Savings = ((avgRun/currentMileage)*fuelRate)-((avgRun/expectCNGMileage)*cngRate)
                    double savings = 0;
                    if (fuelRate == 0)
                    {
                        ModelState.AddModelError(nameof(model.selectedLocationForCNGRate), typeOfCNG + " rates not available for " + selectedLocation);
                        return this.View(model);
                    }
                    else
                    {
                        savings = ((avgRun / currentMileage) * fuelRate) - ((avgRun / expectedCNGMileage) * cngRate);
                    }

                    var estimateCost = ((avgRun / expectedCNGMileage) * cngRate);
                    model.TotalCost = estimateCost;
                    model.TotalSaving = savings;
                    model.Averagecost = cngRate;
                    return View("CNGCostCalculator", model);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at Domestic cost calculator post: ", ex, this);
            }
            return View("CNGCostCalculator", model);
        }

        public List<CityWisePrice> GetGasPrice(string city)
        {
            var client = new HttpClient();
            List<CityWisePrice> cityWisePrice = new List<CityWisePrice>();
            CityWisePrice obj = new CityWisePrice();
            try
            {
                Diagnostics.Log.Info("WebAPIAdaniGas GetGasPrice API Call Start, API Name: " + GetPriceByCityURL, typeof(CityWisePrice));
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var authenticationBytes = Encoding.ASCII.GetBytes(UserName + ":" + Password);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                client.DefaultRequestHeaders.Add(
                                                    "User-Agent",
                                                    "Application/Website");
                string partialServiceUrl = string.Format(GetPriceByCityURL, city);
                if (string.IsNullOrEmpty(city))
                {
                    partialServiceUrl = GetGasPriceURL;
                }
                HttpResponseMessage response = client.GetAsync(ServiceURL + partialServiceUrl).Result;

                var resultContent = response.Content.ReadAsStreamAsync().Result;
                XDocument incomingXml = XDocument.Load(resultContent);
                StringReader stringReader = new StringReader(incomingXml.ToString());
                Diagnostics.Log.Info("WebAPIAdaniGas GetGasPrice API Response: " + incomingXml.ToString(), typeof(CityWisePrice));
                using (XmlReader reader = XmlReader.Create(stringReader))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "d:City":
                                    obj.City = reader.ReadString();
                                    break;
                                case "d:Product":
                                    obj.Product = reader.ReadString();
                                    break;
                                case "d:Eff_date":
                                    obj.Eff_date = reader.ReadString();
                                    break;
                                case "d:MMBTU_Rate":
                                    obj.MMBTU_Rate = reader.ReadString();
                                    cityWisePrice.Add(obj);
                                    obj = new CityWisePrice();
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Diagnostics.Log.Error("WebAPIAdaniGas GetGasPrice API Error, ErrorMessage: " + e.Message, typeof(Exception));
                Diagnostics.Log.Error("WebAPIAdaniGas GetGasPrice API Error: ", e, typeof(Exception));
            }
            return cityWisePrice;
        }

        //Sitecore Schedular For REmove Documents From Server -- Self Billing 
        public bool removeSelfBillingDocuments()
        {
            try
            {
                Log.Info("Start Self Billing Remove Documents :" + DateTime.Now, this);
                DateTime dtThree = DateTime.Now.AddDays(-3);

                string[] UrlList =
                {
                    Server.MapPath(DictionaryPhraseRepository.Current.Get("/Self Billing/RemoveMeterImage", "~/selfbilling/meterimage/")),
                    Server.MapPath(DictionaryPhraseRepository.Current.Get("/Self Billing/RemoveCapturedImage", "~/selfbilling/capturedimage/")),
                    Server.MapPath(DictionaryPhraseRepository.Current.Get("/Self Billing/RemoveUserFile", "~/selfbilling/userfile/"))
                };

                foreach (var item in UrlList)
                {
                    //var t = Task.Run(() =>
                    //{
                    Log.Info("Start Self Billing Remove Documents for this URL :" + item, this);
                    DeleteFileLogic(dtThree, item);
                    //});
                    //t.Wait();
                }

                Log.Info("Remove Self Billing Documents Schedular Process Completed." + DateTime.Now, this);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Self Billing Remove Documents using schedular error is : " + ex.Message, this);
                return false;
            }


        }
        private static void DeleteFileLogic(DateTime dtThree, string removeDocuments)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(removeDocuments);

            List<FileInfo> listOfFiles = directoryInfo.GetFiles().Where(x => x.CreationTime < dtThree).ToList();
            Log.Info("number of file will be delete : {0}", listOfFiles.Count());

            if (listOfFiles != null && listOfFiles.Count() > 0)
            {
                foreach (FileInfo file in listOfFiles)
                {
                    Log.Info(string.Format("Delete file from server delete file name is :{0} time is :{1} ", file.FullName, file.CreationTime), removeDocuments);
                    file.Delete();
                }
            }
            else
            {
                Log.Info("Self Billing Documents Not Avilable In This Folder : {0}", removeDocuments);
            }
        }
    }
}