using Sitecore.AdaniGreenTalks.Website.Models;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website.Provider
{

    public class AdaniGreenTalksJoinUsFormProvider
    {

        public  bool JoinUsForm(AdaniGreenTalks_JoinUs_Model m)
        {
            try
            {
                AdaniGreenTalksDataContext talksDataContext = new AdaniGreenTalksDataContext();
                AdaniGreenTalks_JoinUsForm entity = new AdaniGreenTalks_JoinUsForm();
                entity.ParticipantName = m.ParticipantName;
                entity.BusinessName = m.BusinessName;
                entity.BusinessWebsite = m.BusinessWebsite;
                entity.Email_Id = m.Email_Id;
                entity.BusinessType = m.BusinessType;
                entity.Address = m.Address;
                entity.ContactNumber = m.ContactNumber;
                entity.Category = m.Category;

                Log.Info("Blob storage start here for Doc_ProjectName", this);


                AdaniGreenTalksBlobProvider blobProvider = new AdaniGreenTalksBlobProvider();
                entity.Doc_ProjectName = blobProvider.UploadFileToBlob(m.Doc_UploadProject, m.Doc_UploadProject.FileName).ToString();

                Log.Info("Blob storage finish for Doc_ProjectName"+entity.Doc_ProjectName,this);

                Log.Info("Blob storage start here for rest files", this);
                if (!string.IsNullOrEmpty(m.Participate_AdaniPrizes))
                {
                    
                    if (m.Participate_AdaniPrizes.ToLower() == "check")
                    {
                        entity.Participate_AdaniPrizes = "true";
                        entity.CauseReason = m.reasonCause;



                        entity.Doc_OwnershipName = blobProvider.UploadFileToBlob(m.Doc_Ownership, m.Doc_Ownership.FileName).ToString();
                        entity.Doc_ProjectionSaleName = blobProvider.UploadFileToBlob(m.Doc_ProjectionSales, m.Doc_ProjectionSales.FileName).ToString();
                        entity.Doc_ValuationReportName = blobProvider.UploadFileToBlob(m.Doc_ValuationReport, m.Doc_ValuationReport.FileName).ToString();
                        entity.Doc_ProfitLossName = blobProvider.UploadFileToBlob(m.Doc_ProfitLoss, m.Doc_ProfitLoss.FileName).ToString();


                        if (!string.IsNullOrEmpty(m.chkDocuments_approved))
                        {
                            if (m.chkDocuments_approved.ToLower() == "checkdoc")
                            {
                                entity.chk_Doc_approved = "true";
                            }
                        }
                        else
                        {
                            entity.chk_Doc_approved = "false";
                        }
                    }
                }
                else
                {
                    entity.Participate_AdaniPrizes = "false";
                }
                entity.Id = Guid.NewGuid();
                entity.SubmittedDate = DateTime.Now;
                entity.FormUrl = m.FormUrl;
                entity.FormType = m.FormType;
                talksDataContext.AdaniGreenTalks_JoinUsForms.InsertOnSubmit(entity);
                talksDataContext.SubmitChanges();
                Log.Info("JoinUsForm submitted successfully", this);
                return true;
            }
            catch (Exception ex)
            {
                Log.Info("JoinUsForm Failed. Exception occured"+ ex,this);
                return false;
            }
        }
    }
}