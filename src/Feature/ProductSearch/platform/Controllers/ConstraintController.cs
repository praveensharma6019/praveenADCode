using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Controllers
{
    public class ConstraintController : ApiController
    {
        private readonly ILogRepository logRepository;
        public ConstraintController(ILogRepository _logRepository)
        {
            this.logRepository = _logRepository;
        }
        [HttpPost]
        [Route("api/GetConstraint")]
        public IHttpActionResult GetConstraint([FromBody] ConstraintFilter ConstraintFilter)
        {
            Sitecore.Data.Items.Item constraintItem = GetProductContextDatabase().GetItem(ID.Parse(Constant.ConstraintFolderId));
            ResponseData responseData = new ResponseData();
            ResultData resultData = new ResultData();
            resultData.result = ParseConstraint(constraintItem);
            
            if (resultData.result != null)
            {
                resultData.count = resultData.result.Count;
                responseData.status = true;
                responseData.data = resultData;
            }               
            return Json(responseData);
        }
        /// <summary>
        /// Code to get the list of constraint
        /// </summary>
        /// <param name="constraintItem"></param>
        /// <returns></returns>
        private List<Object> ParseConstraint(Sitecore.Data.Items.Item constraintItem)
        {
            List<Object> constraintMappingsList = new List<Object>();
            Constraints constraints = null;
            try
            {
               foreach(Sitecore.Data.Items.Item cData in constraintItem.Children)
                {
                    constraints = new Constraints();
                    constraints.moduleType = cData.Fields[Constant.moduleType].Value;
                    constraints.stroreType =!string.IsNullOrEmpty(cData.Fields[Constant.storeType].Value)? getstoreTypeDisplayText(cData.Fields[Constant.storeType].Value):string.Empty;
                    constraints.materialGroup =!string.IsNullOrEmpty(cData.Fields[Constant.matrialGroup].Value) ? getstoreTypeDisplayText(cData.Fields[Constant.matrialGroup].Value):string.Empty;
                    constraints.type= !string.IsNullOrEmpty(cData.Fields[Constant.type].Value)? cData.Fields[Constant.type].Value:string.Empty;
                    constraints.value= !string.IsNullOrEmpty(cData.Fields[Constant.value].Value) ? cData.Fields[Constant.value].Value : string.Empty;
                    constraints.unit= !string.IsNullOrEmpty(cData.Fields[Constant.unit].Value) ? cData.Fields[Constant.unit].Value : string.Empty;
                    constraints.errorMessage = cData.Fields[Constant.errorMessage].Value.Replace("[VALUE]", constraints.value + constraints.unit);
                    Sitecore.Data.Fields.CheckboxField chkActicve = cData.Fields[Constant.active];
                    constraints.active = chkActicve.Checked;
                    constraintMappingsList.Add(constraints);

                }
            }
            catch (Exception ex)
            {
                logRepository.Error("ParseConstraint Method in Constraint Controller Class gives error -> " + ex.Message);
            }

            return constraintMappingsList;
        }
        /// <summary>
        /// Cod to get the context database
        /// </summary>
        /// <returns></returns>
        internal Sitecore.Data.Database GetProductContextDatabase()
        {
            return Sitecore.Context.Database;
        }
        /// <summary>
        /// Code to get the display text for items
        /// </summary>
        /// <param name="sitecoreId"></param>
        /// <returns></returns>
        public string getstoreTypeDisplayText(string sitecoreId)
        {
            string data = string.Empty;
            foreach(var id in sitecoreId.Split('|'))
            {
                if (string.IsNullOrEmpty(data))
                {
                    data = GetProductContextDatabase().GetItem(ID.Parse(id)).Fields["Title"].Value ;
                }
                else
                {
                    data = data+ "," + GetProductContextDatabase().GetItem(ID.Parse(id)).Fields["Title"].Value;
                }
            }
            return data;
        }

    }
}