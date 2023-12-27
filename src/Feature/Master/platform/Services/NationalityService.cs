using Adani.SuperApp.Airport.Feature.Master.Platform.Constant;
using Adani.SuperApp.Airport.Feature.Master.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public class NationalityService : INationalityService
    {
        private readonly IHelper _helper;

      
        private readonly ILogRepository _logRepository;
        public NationalityService(ILogRepository logRepository, IHelper helper)
        {

            this._logRepository = logRepository;
            this._helper = helper;
        }
        public List<NationalityModel> GetNationalityData(Item datasource)
        {
            List<NationalityModel> nationalityResult = new List<NationalityModel>();
            try
            {
                List<Item> children = datasource.GetChildren().ToList();
                foreach (Item child in children)
                {
                    nationalityResult.Add(new NationalityModel
                    {
                        CountryName = child[Constants.name],
                        Nationality = child[Constants.value],
                        CountryFlagImage = child.Fields[Constants.Image] != null ? _helper.GetImageURL(child, Constants.Image) : String.Empty
                    });
                }
            }
            catch(Exception ex)
            {
                _logRepository.Error(" NationalityService GetNationalityData gives -> " + ex.Message);
            }
            return nationalityResult;
        }
    }
}