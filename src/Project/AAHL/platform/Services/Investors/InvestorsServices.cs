using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.InvestorDownloads;
using Project.AAHL.Website.Models.Investors;
using Project.AAHL.Website.Models.OurAirports;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.Investors
{
    public class InvestorsServices : IInvestorsServices
    {

        private readonly IMvcContext _mvcContext;

        public InvestorsServices(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }


        public InvestorModel GetInvestors(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var InvestorsData = _mvcContext.GetDataSourceItem<InvestorModel>();
                return InvestorsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public InvestorDownloadsModel GetInvestorDownload(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var InvestorsDownloadData = _mvcContext.GetDataSourceItem<InvestorDownloadsModel>();
                return InvestorsDownloadData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public PerformanceModel GetPerformance(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var PerformanceModelData = _mvcContext.GetDataSourceItem<PerformanceModel>();
                return PerformanceModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}