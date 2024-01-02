using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Realty.Foundation;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using static Adani.SuperApp.Realty.Feature.Career.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Career.Platform.Services
{
    public class CareerServices : ICareerServices
    {
        private readonly ILogRepository _logRepository;
        public CareerServices(ILogRepository logRepository)
        {

            this._logRepository = logRepository;
        }
        public JobOpeningsList GetJobOpeningsList(Rendering rendering)
        {
            JobOpeningsList jobList = new JobOpeningsList();
            try
            {

                jobList.data = jobListItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" CareerServices GetJobOpeningsList gives -> " + ex.Message);
            }


            return jobList;
        }
        public List<Object> jobListItem(Rendering rendering)
        {
            List<Object> JobListItem = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                JobOpeningItem jobOpeningItemobj;



                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    jobOpeningItemobj = new JobOpeningItem();
                    jobOpeningItemobj.role = !string.IsNullOrEmpty(item.Fields[Careers.Fields.RoleFieldName].Value.ToString()) ? item.Fields[Careers.Fields.RoleFieldName].Value.ToString() : "";
                    jobOpeningItemobj.department = !string.IsNullOrEmpty(item.Fields[Careers.Fields.DepartmentFieldName].Value.ToString()) ? item.Fields[Careers.Fields.DepartmentFieldName].Value.ToString() : "";
                    jobOpeningItemobj.location = !string.IsNullOrEmpty(item.Fields[Careers.Fields.LocationFieldName].Value.ToString()) ? item.Fields[Careers.Fields.DepartmentFieldName].Value.ToString() : "";
                          string date = item.Fields[Careers.Fields.PostingdateFieldName].ToString();
                    if (date != null && date != "")
                    {
                        string format = "yyyyMMdd'T'HHmmss'Z'";
                        var datedata = DateTime.ParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture);
                        jobOpeningItemobj.postingdate = datedata.ToString("MMM dd , yyyy");
                    }
                    jobOpeningItemobj.link = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, Careers.Fields.linkFieldName) != null ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, Careers.Fields.linkFieldName) : "";

                    JobListItem.Add(jobOpeningItemobj);

                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" CareerServices jobListItem gives -> " + ex.Message);
            }

            return JobListItem;
        }
        public JobsAnchorsList GetJobsAnchorsList(Rendering rendering)
        {
            JobsAnchorsList JobsAnchorsItemList = new JobsAnchorsList();
            try
            {

                JobsAnchorsItemList.data = JobsAnchorsItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" CareerServices GetJobsAnchorsList gives -> " + ex.Message);
            }


            return JobsAnchorsItemList;
        }
        public List<Object> JobsAnchorsItem(Rendering rendering)
        {
            List<Object> JobAnchorsListItem = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                JobsAnchorsItem jobsAnchors;



                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    jobsAnchors = new JobsAnchorsItem();
                    jobsAnchors.link = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, JobsAnchors.Fields.linkFieldName) != null ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, JobsAnchors.Fields.linkFieldName) : "";
                    jobsAnchors.title = !string.IsNullOrEmpty(item.Fields[JobsAnchors.Fields.TitleFieldName].Value.ToString()) ? item.Fields[JobsAnchors.Fields.TitleFieldName].Value.ToString() : "";

                    JobAnchorsListItem.Add(jobsAnchors);

                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" CareerServices JobsAnchorsItem gives -> " + ex.Message);
            }

            return JobAnchorsListItem;
        }
        public EmployeeCareList GetemployeeCareList(Rendering rendering)
        {
            EmployeeCareList employeeCareList = new EmployeeCareList();
            try
            {

                employeeCareList.data = EmployeeCareItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" CareerServices GetemployeeCareList gives -> " + ex.Message);
            }


            return employeeCareList;
        }
        public List<Object> EmployeeCareItem(Rendering rendering)
        {
            List<Object> employeeCareItem = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                EmployeeCareItem employeeCare;



                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    employeeCare = new EmployeeCareItem();
                    employeeCare.name = !string.IsNullOrEmpty(item.Fields[EmployeeCare.Fields.headingFieldName].Value.ToString()) ? item.Fields[EmployeeCare.Fields.headingFieldName].Value.ToString() : "";
                    employeeCare.description = !string.IsNullOrEmpty(item.Fields[EmployeeCare.Fields.subheadingFieldName].Value.ToString()) ? item.Fields[EmployeeCare.Fields.subheadingFieldName].Value.ToString() : "";
                    employeeCare.inclusion = !string.IsNullOrEmpty(item.Fields[EmployeeCare.Fields.TitleName].Value.ToString()) ? item.Fields[EmployeeCare.Fields.TitleName].Value.ToString() : "";

                    employeeCareItem.Add(employeeCare);

                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" CareerServices EmployeeCareItem gives -> " + ex.Message);
            }

            return employeeCareItem;
        }

    }
}