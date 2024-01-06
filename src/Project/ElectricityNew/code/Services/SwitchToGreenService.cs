using Sitecore.ElectricityNew.Website.Model;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.ElectricityNew.Website.Services
{
    public class SwitchToGreenService
    {
        public List<SwitchToGreenPledge_VehicleMaster> GetVihecleList()
        {
            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    return dbcontext.SwitchToGreenPledge_VehicleMasters.Where(s => s.IsActive == true).OrderBy(s => s.VehicleTypeName).ToList();
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("GetVihecleList error " + ex.Message, this);
                return null;
            }
        }

        public bool SwitchToGreen_Pledge_CheckLimit(string email, string formType)
        {
            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    if (dbcontext.SwitchToGreen_PledgeRequests.Any(s => s.EmailId == email && s.FormType==formType))
                        return false;
                    else
                        return true;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("GetVihecleList error " + ex.Message, this);
                return false;
            }
        }

        public bool SwitchToGreen_EVCharging_CheckLimit(string email, string formType)
        {
            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    if (dbcontext.SwitchToGreen_ChargingStationRequests.Any(s => s.EmailId == email && s.RequestType == formType))
                        return false;
                    else
                        return true;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("GetVihecleList error " + ex.Message, this);
                return false;
            }
        }

        public bool CreateSwitchToGreen_Pledge(ApplyFormModel model)
        {
            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    SwitchToGreen_PledgeRequest obj = new SwitchToGreen_PledgeRequest();
                    switch (model.FormType)
                    {
                        case "I":
                            obj = new SwitchToGreen_PledgeRequest
                            {
                                CreatedDateTime = DateTime.Now,
                                EmailId = model.EmailId_GreenIn,
                                FormType = model.FormType,
                                FullName = model.FullName,
                                IsSignUp = model.IsSignUp_In,
                                MobileNumber = model.MobileNumber,
                                PledgeType = model.FormType,
                                VehicleType = model.VehicleType_In,
                                ZipCode = model.ZipCode
                            };
                            break;
                        case "O":
                            obj = new SwitchToGreen_PledgeRequest
                            {
                                CreatedDateTime = DateTime.Now,
                                EmailId = model.EmailId_GreenOrg,
                                FormType = model.FormType,
                                FullName = model.OrganizationName,
                                IsSignUp = model.IsSignUp_Org,
                                PledgeType = model.FormType,
                                VehicleType = model.VehicleType_Org
                            };
                            break;

                        default:
                            obj = new SwitchToGreen_PledgeRequest
                            {
                                CreatedDateTime = DateTime.Now,
                                EmailId = model.EmailId_GreenIn,
                                FormType = model.FormType,
                                FullName = model.FullName,
                                IsSignUp = model.IsSignUp_In,
                                MobileNumber = model.MobileNumber,
                                PledgeType = model.FormType,
                                VehicleType = model.VehicleType_In,
                                ZipCode = model.ZipCode
                            };
                            break;
                    }

                    dbcontext.SwitchToGreen_PledgeRequests.InsertOnSubmit(obj);
                    dbcontext.SubmitChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("CreateSwitchToGreen_Pledge error " + ex.Message, this);
                return false;
            }
        }

        public bool CreateSwitchToGreen_ChargingRequest(ApplyFormModel model)
        {
            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    SwitchToGreen_ChargingStationRequest obj = new SwitchToGreen_ChargingStationRequest();
                    switch (model.FormType)
                    {
                        case "E":
                            obj = new SwitchToGreen_ChargingStationRequest
                            {
                                CreatedDate = DateTime.Now,
                                EmailId = model.EmailId_EVIn,
                                MobileNumber = model.MobileNumber_EVIn,
                                RequestType = model.FormType,
                                FullName = model.FullName_EV
                            };
                            break;
                        case "S":
                            obj = new SwitchToGreen_ChargingStationRequest
                            {
                                CreatedDate = DateTime.Now,
                                EmailId = model.EmailId_EVS,
                                MobileNumber = model.MobileNumber_EVS,
                                RequestType = model.FormType,
                                FullName = model.FullName_EVS
                            };
                            break;
                        case "P":
                            obj = new SwitchToGreen_ChargingStationRequest
                            {
                                CreatedDate = DateTime.Now,
                                EmailId = model.EmailId_EVP,
                                MobileNumber = model.MobileNumberEVP,
                                RequestType = model.FormType,
                                FullName = model.FullName_EVP
                            };
                            break;
                        default:
                            obj = new SwitchToGreen_ChargingStationRequest
                            {
                                CreatedDate = DateTime.Now,
                                EmailId = model.EmailId_EVIn,
                                MobileNumber = model.MobileNumber_EVIn,
                                RequestType = model.FormType,
                                FullName = model.FullName_EV
                            };
                            break;
                    }
                    dbcontext.SwitchToGreen_ChargingStationRequests.InsertOnSubmit(obj);
                    dbcontext.SubmitChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("CreateSwitchToGreen_ChargingRequest error " + ex.Message, this);
                return false;
            }
        }
    }

}