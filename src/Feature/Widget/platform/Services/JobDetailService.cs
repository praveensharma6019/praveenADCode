using Adani.SuperApp.Realty.Feature.Widget.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Reflection;
using static Adani.SuperApp.Realty.Feature.Carousel.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Services
{
    public class JobDetailService : IJobDetailService
    {
        private readonly ILogRepository _logRepository;

        public JobDetailService(ILogRepository logRepository)
        {

            this._logRepository = logRepository;
        }

        public JobDetails GetJobDetails(Item jobdetail)
        {

            JobDetails jobdata = new JobDetails();
            try
            {
                ObjectItems jobDetails = new ObjectItems();
                jobDetails.JobDetails = new List<object>();
                foreach (Item data in jobdetail.Children)
                {
                    var dataobject = new JobData()
                    {
                        Role = !string.IsNullOrEmpty(data.Fields[JobContentDetail.Fields.Role].Value.ToString()) ? data.Fields[JobContentDetail.Fields.Role].Value : "",
                        Department = !string.IsNullOrEmpty(data.Fields[JobContentDetail.Fields.Department].Value.ToString()) ? data.Fields[JobContentDetail.Fields.Department].Value : "",
                        Location = !string.IsNullOrEmpty(data.Fields[JobContentDetail.Fields.Location].Value.ToString()) ? data.Fields[JobContentDetail.Fields.Location].Value : "",
                        Description = !string.IsNullOrEmpty(data.Fields[JobContentDetail.Fields.Description].Value.ToString()) ? data.Fields[JobContentDetail.Fields.Description].Value : "",
                        DownloadText = !string.IsNullOrEmpty(data.Fields[JobContentDetail.Fields.DownloadText].Value.ToString()) ? data.Fields[JobContentDetail.Fields.DownloadText].Value : "",
                        DownloadUrl = Helper.GetLinkURLbyField(data, data.Fields[JobContentDetail.Fields.DownloadUrl]),
                        ShareText = !string.IsNullOrEmpty(data.Fields[JobContentDetail.Fields.ShareText].Value.ToString()) ? data.Fields[JobContentDetail.Fields.ShareText].Value : "",
                        ShareUrl = Helper.GetLinkURLbyField(data, data.Fields[JobContentDetail.Fields.ShareUrl]),
                        ButtonText = !string.IsNullOrEmpty(data.Fields[JobContentDetail.Fields.ButtonText].Value.ToString()) ? data.Fields[JobContentDetail.Fields.ButtonText].Value : "",
                        ButtonUrl = Helper.GetLinkURLbyField(data, data.Fields[JobContentDetail.Fields.ButtonUrl]),
                        RealityLogo = Helper.GetLinkURLbyField(data, data.Fields[JobContentDetail.Fields.RealityLogo]),
                        RealityAlt = !string.IsNullOrEmpty(data.Fields[JobContentDetail.Fields.RealityAlt].Value.ToString()) ? data.Fields[JobContentDetail.Fields.RealityAlt].Value : "",
                    };
                    jobDetails.JobDetails.Add(dataobject);
                }
                jobdata.JobList = jobDetails;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return jobdata;
        }

        public ProjectAction GetProjectAction(Item projectAction)
        {
            var prjAction = new ProjectAction();
            try
            {
                var action = new PAction()
                {
                    ButtonText = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.ButtonText].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.ButtonText].Value : "",
                    imgTitle = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.imgTitle].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.imgTitle].Value : "",
                    ModalTitle = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.ModalTitle].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.ModalTitle].Value : "",
                    DownloadText = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.DownloadText].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.DownloadText].Value : "",
                    downloadModalTitle = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.downloadModalTitle].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.downloadModalTitle].Value : "",
                    ShareText = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.ShareText].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.ShareText].Value : "",
                    Backlink = Helper.GetLinkURLbyField(projectAction, projectAction.Fields[ProjectActionTemplate.Fields.Backlink]),
                    Src = Helper.GetPropLinkURLbyField(projectAction, projectAction.Fields[ProjectActionTemplate.Fields.Src]),
                    ImgAlt = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.ImgAlt].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.ImgAlt].Value : "",
                    Label = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.Label].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.Label].Value : "",
                    Location = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.Location].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.Location].Value : "",
                    CopyLink = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.CopyLink].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.CopyLink].Value : "",
                    Email = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.Email].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.Email].Value : "",
                    Twitter = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.Twitter].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.Twitter].Value : "",
                    Facebook = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.Facebook].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.Facebook].Value : "",
                    Whatsapp = !string.IsNullOrEmpty(projectAction.Fields[ProjectActionTemplate.Fields.Whatsapp].Value.ToString()) ? projectAction.Fields[ProjectActionTemplate.Fields.Whatsapp].Value : "",
                    Downloadurl = Helper.GetPropLinkURLbyField(projectAction, projectAction.Fields[ProjectActionTemplate.Fields.Downloadurl]),
                };

                var lifeAtAdani = new LifeatAdani()
                {
                    Content = !string.IsNullOrEmpty(projectAction.Fields[LifeAtAdaniTemplate.Fields.Content].Value.ToString()) ? projectAction.Fields[LifeAtAdaniTemplate.Fields.Content].Value : "",
                    Imgalt = !string.IsNullOrEmpty(projectAction.Fields[LifeAtAdaniTemplate.Fields.Imgalt].Value.ToString()) ? projectAction.Fields[LifeAtAdaniTemplate.Fields.Imgalt].Value : "",
                    Imgsrc = Helper.GetPropLinkURLbyField(projectAction, projectAction.Fields[LifeAtAdaniTemplate.Fields.Imgsrc]),
                    Label = !string.IsNullOrEmpty(projectAction.Fields[LifeAtAdaniTemplate.Fields.Label].Value.ToString()) ? projectAction.Fields[LifeAtAdaniTemplate.Fields.Label].Value : "",
                    ViewAllJobs = !string.IsNullOrEmpty(projectAction.Fields[LifeAtAdaniTemplate.Fields.ViewAllJobs].Value.ToString()) ? projectAction.Fields[LifeAtAdaniTemplate.Fields.ViewAllJobs].Value : "",
                    ViewAllJobsLink = Helper.GetLinkURLbyField(projectAction, projectAction.Fields[LifeAtAdaniTemplate.Fields.ViewAllJobsLink]),
                };

                action.lifeatAdani = lifeAtAdani;
                prjAction.ProjectActions = action;

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }


            return prjAction;
        }
    }
}