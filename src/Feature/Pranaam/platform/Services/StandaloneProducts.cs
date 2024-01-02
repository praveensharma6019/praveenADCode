using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class StandaloneProducts : IStandaloneProducts
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        public StandaloneProducts(ILogRepository logRepository, IHelper helper)
        {

            this._logRepository = logRepository;
            this._helper = helper;

        }
        public StandaloneProductTerminal GetPorterDetails(Item item, string airportcode)
        {
            StandaloneProductTerminal _termObj = new StandaloneProductTerminal();
            try
            {
                Item cityitem = item.Children.Where(x => x.TemplateID == Templates.AirportId.CityTemplateId && x.Fields["CityCode"].Value == airportcode).FirstOrDefault();
                var terminalList = cityitem.Children.Where(x => x.TemplateID == Templates.AirportId.AirportTemplateId);

                if (terminalList != null)
                {
                    //List<PorterDetail> _pckg = new List<PorterDetail>();
                    List<StandaloneProductTerminals> _terminalsObj = new List<StandaloneProductTerminals>();
                    foreach (Item porteritem in terminalList)
                    {
                        var terminalsList = porteritem.Children;
                        foreach (Item term in terminalsList)
                        {
                            StandaloneProductTerminals _terminalObj = new StandaloneProductTerminals();
                            _terminalObj.TerminalName = !string.IsNullOrEmpty(term.Fields[Templates.AirportTerminalDetails.Fields.TerminalName].Value) ? term.Fields[Templates.AirportTerminalDetails.Fields.TerminalName].Value : string.Empty;
                            MultilistField cityPorter = term?.Fields[Templates.AirportTerminalDetails.Fields.StandaloneProducts];

                            StandaloneProductDetailsList porterDetail = new StandaloneProductDetailsList();
                            porterDetail.StandaloneProductDetail = new List<StandaloneProductDetail>();
                            if (cityPorter != null && cityPorter.GetItems().Length > 0)
                            {
                                foreach (var _obj in cityPorter.GetItems())
                                {
                                    StandaloneProductDetail _porterDetails = new StandaloneProductDetail();
                                    _porterDetails.AirportMasterId = airportcode;
                                    _porterDetails.Name = _obj?.Fields[Templates.StandaloneProducts.Fields.Title]?.Value;
                                    _porterDetails.ShortDesc = _obj?.Fields[Templates.StandaloneProducts.Fields.Description]?.Value;
                                    _porterDetails.Id = _obj?.Fields[Templates.StandaloneProducts.Fields.Id]?.Value;
                                    _porterDetails.StandaloneProductImage = !string.IsNullOrEmpty(_helper.GetImageURL(_obj, Templates.PranaamPackages.Fields.StanderedImage.ToString())) ? _helper.GetImageURL(_obj, Templates.PranaamPackages.Fields.StanderedImage.ToString()) : Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/-/media/Project/Adani/Pranaam/Packages/SkycapPorter.png";
                                    porterDetail.StandaloneProductDetail.Add(_porterDetails);
                                }
                                _terminalObj.StandaloneProductDetails = porterDetail;
                            }
                            _terminalsObj.Add(_terminalObj);
                        }
                    }
                    _termObj.ListOfTerminals = _terminalsObj;
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return _termObj;
        }
    }
}