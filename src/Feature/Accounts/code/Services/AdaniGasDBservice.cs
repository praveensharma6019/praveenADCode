using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Feature.Accounts.Models.AdaniGasCNGRegistration;

namespace Sitecore.Feature.Accounts.Services
{
    public class AdaniGasDBservice
    {
        public void StoreCNGcustomerRegistrationAdaniGas(AdaniGasCNG_CustomerRegistration model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - StoreCNGcustomerRegistrationAdaniGas Store request in Database EnquiryNo : - " + model.RegistrationNo + " , " + model.Mobile, this);
                        using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                        {
                            CNG_CustomerRegistration objdata = new CNG_CustomerRegistration();
                            Guid newid = Guid.NewGuid();
                            objdata.Id = newid;
                            objdata.RegistrationNo = model.RegistrationNo;
                            objdata.FirstName = model.FirstName ?? string.Empty;
                            objdata.LastName = model.LastName ?? string.Empty;
                            objdata.IsBS_VI = model.IsBSVI;
                            objdata.VehicleType = model.VehicleType ?? string.Empty;
                            objdata.YearOfPurchase = model.Year ?? string.Empty;
                            objdata.EmailId = model.Email ?? string.Empty;
                            objdata.MobileNo = model.Mobile ?? string.Empty;
                            objdata.CurrentAddressLine1 = !string.IsNullOrEmpty(model.CurrentAddressLine1) ? "\"" + model.CurrentAddressLine1 + "\"" : string.Empty;
                            objdata.CurrentAddressLine2 = !string.IsNullOrEmpty(model.CurrentAddressLine2) ? "\"" + model.CurrentAddressLine2 + "\"" : string.Empty;
                            objdata.CurrentState = model.CurrentState ?? string.Empty;
                            objdata.CurrentCity = model.CurrentCity ?? string.Empty;
                            objdata.CurrentArea = !string.IsNullOrEmpty(model.CurrentArea) ? "\"" + model.CurrentArea + "\"" : string.Empty;
                            objdata.CurrentPincode = model.CurrentPincode ?? string.Empty;
                            objdata.RegAddressLine1 = !string.IsNullOrEmpty(model.RegisteredAddressLine1) ? "\"" + model.RegisteredAddressLine1 + "\"" : string.Empty;
                            objdata.RegAddressLine2 = !string.IsNullOrEmpty(model.RegisteredAddressLine2) ? "\"" + model.RegisteredAddressLine2 + "\"" : string.Empty;
                            objdata.RegState = model.RegisteredState ?? string.Empty;
                            objdata.RegCity = model.RegisteredCity ?? string.Empty;
                            objdata.RegArea = !string.IsNullOrEmpty(model.RegisteredArea) ? "\"" + model.RegisteredArea + "\"" : string.Empty;
                            objdata.RegPincode = model.RegisteredPincode ?? string.Empty;
                            objdata.VehicleCompany = model.VehicleCompany ?? string.Empty;
                            objdata.VehicleModel = model.VehicleModel ?? string.Empty;
                            objdata.VehicleNumber = model.VehicleNo ?? string.Empty;
                            objdata.PageInfo = !string.IsNullOrEmpty(model.PageInfo) ? "\"" + model.PageInfo + "\"" : string.Empty;
                            objdata.FormName = !string.IsNullOrEmpty(model.FormName) ? "\"" + model.FormName + "\"" : string.Empty;
                            objdata.SubmittedOn = DateTime.Now;

                            dbcontext.CNG_CustomerRegistrations.InsertOnSubmit(objdata);
                            dbcontext.SubmitChanges();
                            model.IsSavedintoDB = true;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at submitting form in db for AdaniGas StoreCNGcustomerRegistrationAdaniGas " + model.Mobile + ": " + ex.Message, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at submitting form in db for AdaniGas StoreCNGcustomerRegistrationAdaniGas " + model.Mobile + ":" + ex.Message, this);
            }
        }
        public void StoreCNGDealerFormAdaniGas(AdaniGasCNG_DealerRegisterModel model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - AdaniGasCNG_DealerRegisterModel Store request in Database EnquiryNo : - " + model.CustomerRegistrationNo + " , " + model.DealerId, this);
                        using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                        {
                            var customerDetails = dbcontext.CNG_CustomerRegistrations.Where(x => x.RegistrationNo == model.CustomerRegistrationNo).FirstOrDefault();
                            if (dbcontext.CNG_DealerEnquiryRegistrationHistories.Any(a => a.DealerCode == model.DealerId && a.CustomerRegistrationId == customerDetails.Id))
                            {
                                var existingData = dbcontext.CNG_DealerEnquiryRegistrationHistories.Where(a => a.DealerCode == model.DealerId && a.CustomerRegistrationId == customerDetails.Id).FirstOrDefault();
                                existingData.CustomerRegistrationId = Guid.Parse(customerDetails.Id.ToString());
                                existingData.CNGKitNumber = model.CNG_KitNumber ?? string.Empty;
                                existingData.DocumentsVerified = model.IsVerifiedByDealer;
                                existingData.DocumentsVerifiedBy = model.DealerName ?? string.Empty;
                                existingData.CurrentStatus = model.CurrentStatus ?? "Enrolled";
                                existingData.AadharCardFile = string.IsNullOrEmpty(existingData.AadharCardFile) ?model.AadharCardFileLink??string.Empty:string.Empty;
                                existingData.PanCardFile = string.IsNullOrEmpty(existingData.AadharCardFile) ? model.PAN_CardFileLink ?? string.Empty : string.Empty;
                                existingData.VehicleInsuranceFile = string.IsNullOrEmpty(existingData.AadharCardFile) ? model.VehicleInsureanceFileLink ?? string.Empty : string.Empty;
                                existingData.RC_bookFile = string.IsNullOrEmpty(existingData.AadharCardFile) ? model.RC_BookFileLink ?? string.Empty : string.Empty;
                                existingData.RTO_ApplicationReceipt = string.IsNullOrEmpty(existingData.AadharCardFile) ? model.RTO_App_ReceiptFileLink ?? string.Empty : string.Empty;
                                existingData.RTO_CertificateCopy = string.IsNullOrEmpty(existingData.AadharCardFile) ? model.RTO_CertiFileLink ?? string.Empty : string.Empty;
                                existingData.InvoiceCopyFile = string.IsNullOrEmpty(existingData.AadharCardFile) ? model.InvoiceFileLink ?? string.Empty : string.Empty;
                                existingData.CNG_CylinderCertifFile = string.IsNullOrEmpty(existingData.AadharCardFile) ? model.CNG_CylinderCertiFileLink ?? string.Empty : string.Empty;
                                existingData.CustSignedSchemeDoc = string.IsNullOrEmpty(existingData.AadharCardFile) ? model.SignedSchemeDocFileLink ?? string.Empty : string.Empty;
                                existingData.UpdatedOn = DateTime.Now;
                                existingData.UpdatedBy = model.DealerName ?? string.Empty;
                                existingData.PageInfo = !string.IsNullOrEmpty(model.PageInfo) ? "\"" + model.PageInfo + "\"" : string.Empty;
                                existingData.DocumentsVerified = model.IsVerifiedByDealer;
                                existingData.DocumentsVerifiedBy = model.DealerId;
                                dbcontext.SubmitChanges();
                                model.IsSavedIntoDatabase = true;
                            }
                            else
                            {
                                CNG_DealerEnquiryRegistrationHistory objdata = new CNG_DealerEnquiryRegistrationHistory();
                                Guid newid = Guid.NewGuid();
                                objdata.Id = newid;
                                objdata.EnquiryNo = model.CustomerRegistrationNo ?? string.Empty;
                                objdata.DealerCode = model.DealerId ?? string.Empty;
                                objdata.CustomerRegistrationId = Guid.Parse(customerDetails.Id.ToString());
                                objdata.CNGKitNumber = model.CNG_KitNumber ?? string.Empty;
                                objdata.DocumentsVerified = model.IsVerifiedByDealer;
                                objdata.DocumentsVerifiedBy = model.DealerName ?? string.Empty;
                                objdata.CreatedOn = DateTime.Now;
                                objdata.CreatedBy = model.DealerName ?? string.Empty;
                                objdata.CurrentStatus = model.CurrentStatus ?? "Enrolled";
                                objdata.AadharCardFile = model.AadharCardFileLink ?? string.Empty;
                                objdata.PanCardFile = model.PAN_CardFileLink ?? string.Empty;
                                objdata.VehicleInsuranceFile = model.VehicleInsureanceFileLink ?? string.Empty;
                                objdata.RC_bookFile = model.RC_BookFileLink ?? string.Empty;
                                objdata.RTO_ApplicationReceipt = model.RTO_App_ReceiptFileLink ?? string.Empty;
                                objdata.RTO_CertificateCopy = model.RTO_CertiFileLink ?? string.Empty;
                                objdata.InvoiceCopyFile = model.InvoiceFileLink ?? string.Empty;
                                objdata.CNG_CylinderCertifFile = model.CNG_CylinderCertiFileLink ?? string.Empty;
                                objdata.CustSignedSchemeDoc = model.SignedSchemeDocFileLink ?? string.Empty;
                                objdata.UpdatedOn = DateTime.Now;
                                objdata.UpdatedBy = model.DealerName ?? string.Empty;
                                objdata.PageInfo = !string.IsNullOrEmpty(model.PageInfo) ? "\"" + model.PageInfo + "\"" : string.Empty;
                                objdata.DocumentsVerified = model.IsVerifiedByDealer;
                                objdata.DocumentsVerifiedBy = model.DealerId;
                                dbcontext.CNG_DealerEnquiryRegistrationHistories.InsertOnSubmit(objdata);
                                dbcontext.SubmitChanges();
                                model.IsSavedIntoDatabase = true;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at submitting form in db for AdaniGas StoreCNGcustomerRegistrationAdaniGas " + model.CustomerRegistrationNo + " , " + model.DealerId + ": " + ex.Message, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at submitting form in db for AdaniGas StoreCNGcustomerRegistrationAdaniGas " + model.CustomerRegistrationNo + " , " + model.DealerId + ":" + ex.Message, this);
            }
        }

        public void StoreCNGUploadedDocument(CNGDocuments model)
        {
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    try
                    {
                        Sitecore.Diagnostics.Log.Info("Method - AdaniGasCNG_StoreCNGUploadedDocument Store request in Database EnquiryNo : - " + model.CustomerRegistrationNo + " , " + model.CreatedBy, this);
                        using (AdadniGasDBModelDataContext dbcontext = new AdadniGasDBModelDataContext())
                        {
                            var customerDetails = dbcontext.CNG_CustomerRegistrations.Where(x => x.RegistrationNo == model.CustomerRegistrationNo).FirstOrDefault();
                            CNG_Document objdata = new CNG_Document();
                            if (dbcontext.CNG_Documents.Any(x => x.CreatedBy == model.CreatedBy && x.CustRegistrationNo == model.CustomerGUID && x.DocumentType == model.DocType))
                            {
                                var existDoc = dbcontext.CNG_Documents.Where(x => x.CreatedBy == model.CreatedBy && x.CustRegistrationNo == model.CustomerGUID && x.DocumentType == model.DocType).FirstOrDefault();
                                objdata.DocumentContentType = model.DocContentType ?? string.Empty;
                                objdata.DocumentType = model.DocType ?? string.Empty;
                                objdata.DocumentData = model.DocData;
                                objdata.DocumentName = model.DocName ?? string.Empty;
                                objdata.updatedBy = model.CreatedBy;
                                objdata.UpdatedOn = model.CreatedOn;
                                dbcontext.SubmitChanges();
                                model.SavedIntoDB = true;
                            }
                            else
                            {
                                objdata.Id = model.Id;
                                objdata.DocumentContentType = model.DocContentType ?? string.Empty;
                                objdata.DocumentType = model.DocType ?? string.Empty;
                                objdata.DocumentData = model.DocData;
                                objdata.DocumentName = model.DocName ?? string.Empty;
                                objdata.CustRegistrationNo = model.CustomerGUID;
                                objdata.CreatedBy = model.CreatedBy;
                                objdata.CreatedOn = model.CreatedOn;
                                objdata.CreatedDate = DateTime.Now;
                                dbcontext.CNG_Documents.InsertOnSubmit(objdata);
                                dbcontext.SubmitChanges();
                                model.SavedIntoDB = true;
                            }                            
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at submitting form in db for AdaniGas StoreCNGcustomerRegistrationAdaniGas " + model.CustomerRegistrationNo + " , " + model.CreatedBy + ": " + ex.Message, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at submitting form in db for AdaniGas StoreCNGcustomerRegistrationAdaniGas " + model.CustomerRegistrationNo + " , " + model.CreatedBy + ":" + ex.Message, this);
            }
        }
    }
}