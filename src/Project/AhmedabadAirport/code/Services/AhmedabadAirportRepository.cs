using Sitecore.AhmedabadAirport.Website;
using Sitecore.AhmedabadAirport.Website.Model;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.AhmedabadAirport.Website.Services
{
    public class AhmedabadAirportRepository
    {
        public AhmedabadAirportRepository()
        {
        }

        public void ActivateTender(Guid id)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                AMD_TenderList tenderlist = (
                    from x in dbcontext.AMD_TenderLists
                    where x.Id == id
                    select x).Single<AMD_TenderList>();
                tenderlist.Staus = "Open";
                tenderlist.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                tenderlist.Modified_Date = new DateTime?(DateTime.Now);
                List<TenderDetails> list = new List<TenderDetails>();
                List<Guid?> CorrigendumIdList = (
                    from x in dbcontext.AMD_CorrigendumTenderMappings
                    where x.TenderId == (Guid?)id
                    select x.CorrigendumId).ToList<Guid?>();
                foreach (Guid? nullable in CorrigendumIdList)
                {
                    AMD_Corrigendum corrigendum = (
                        from x in dbcontext.AMD_Corrigendums
                        where (Guid?)x.Id == nullable
                        select x).Single<AMD_Corrigendum>();
                    corrigendum.Status = new bool?(true);
                }
                dbcontext.SubmitChanges();
            }
        }

        private bool CheckExtension(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            if (!(fileExtension == ".jpg") && !(fileExtension == ".jpeg") && !(fileExtension == ".pdf") && !(fileExtension == ".doc") && !(fileExtension == ".docx") && !(fileExtension == ".xls") && !(fileExtension == ".xlsx"))
            {
                return false;
            }
            return true;
        }

        public string datefortender(DateTime? dt)
        {
            return dt.Value.ToString("dd-MM-yyyy HH:mm:ss");
        }

        public void DeleteTenderDocument(Guid id, string DocumentPath)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                AMD_TenderDocument docFile = (
                    from x in dbcontext.AMD_TenderDocuments
                    where x.Id == id
                    select x).Single<AMD_TenderDocument>();
                dbcontext.AMD_TenderDocuments.DeleteOnSubmit(docFile);
                try
                {
                    dbcontext.SubmitChanges();
                }
                catch (Exception exception)
                {
                    Exception ex = exception;
                    Log.Error(string.Concat("Error at TenderDetails Get:", ex.Message), this);
                }
            }
        }

        public int DeleteUserTenderDocument(Guid id, string userId, string tenderId)
        {
            int num;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                AMD_UserTenderDocument docFile = (
                    from x in dbcontext.AMD_UserTenderDocuments
                    where x.Id == id
                    select x).Single<AMD_UserTenderDocument>();
                if (docFile.UserId != userId)
                {
                    num = 1;
                }
                else
                {
                    dbcontext.AMD_UserTenderDocuments.DeleteOnSubmit(docFile);
                    try
                    {
                        dbcontext.SubmitChanges();
                        num = 0;
                    }
                    catch (Exception exception)
                    {
                        Exception ex = exception;
                        Log.Error(string.Concat("Error at TenderDetails Get:", ex.Message), this);
                        num = 2;
                    }
                }
            }
            return num;
        }

        public List<TenderEditModel> GetEditTenderList()
        {
            List<TenderEditModel> tenderEditModels;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                IQueryable<TenderEditModel> list =
                    from r in dbcontext.AMD_TenderLists
                    select new TenderEditModel()
                    {
                        Id = r.Id,
                        NITNo = r.NITNo,
                        Business = r.Business,
                        Description = r.Description,
                        Adv_Date = this.datefortender(r.Adv_Date),
                        Closing_Date = this.datefortender(r.Closing_Date),
                        Estimated_Cost = r.Estimated_Cost,
                        Cost_of_EMD = r.Cost_of_EMD,
                        Location = r.Location,
                        Status = r.Staus,
                        TenderDocuments = r.AMD_TenderDocuments.Where<AMD_TenderDocument>((AMD_TenderDocument x) => (x.TenderId == (Guid?)r.Id) && x.IsPQDoc == (bool?)false).ToList<AMD_TenderDocument>(),
                        PQDocuments = r.AMD_TenderDocuments.Where<AMD_TenderDocument>((AMD_TenderDocument x) => (x.TenderId == (Guid?)r.Id) && x.IsPQDoc == (bool?)true).ToList<AMD_TenderDocument>()
                    };
                tenderEditModels = list.ToList<TenderEditModel>();
            }
            return tenderEditModels;
        }

        public List<TenderModel> GetTenderList()
        {
            List<TenderModel> tenderModels;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                IQueryable<TenderModel> list =
                    from r in dbcontext.AMD_TenderLists
                    select new TenderModel()
                    {
                        Id = r.Id,
                        NITNo = r.NITNo,
                        Business = r.Business,
                        Description = r.Description,
                        Adv_Date = (DateTime)r.Adv_Date,
                        Closing_Date = (DateTime)r.Closing_Date,
                        Location = r.Location,
                        Status = r.Staus
                    };
                tenderModels = list.ToList<TenderModel>();
            }
            return tenderModels;
        }

        public void InactivateTender(Guid id)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                AMD_TenderList tenderlist = (
                    from x in dbcontext.AMD_TenderLists
                    where x.Id == id
                    select x).Single<AMD_TenderList>();
                tenderlist.Staus = "Inactive";
                tenderlist.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                tenderlist.Modified_Date = new DateTime?(DateTime.Now);
                List<TenderDetails> list = new List<TenderDetails>();
                List<Guid?> CorrigendumIdList = (
                    from x in dbcontext.AMD_CorrigendumTenderMappings
                    where x.TenderId == (Guid?)id
                    select x.CorrigendumId).ToList<Guid?>();
                foreach (Guid? nullable in CorrigendumIdList)
                {
                    AMD_Corrigendum corrigendum = (
                        from x in dbcontext.AMD_Corrigendums
                        where (Guid?)x.Id == nullable
                        select x).Single<AMD_Corrigendum>();
                    corrigendum.Status = new bool?(false);
                }
                dbcontext.SubmitChanges();
            }
        }

        public void InsertDocumentList(TenderCreateModel obj)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                if (obj.NITNo == "-")
                {
                    AMD_TenderList tender = (
                        from x in dbcontext.AMD_TenderLists
                        where x.Id == obj.Id
                        select x).Single<AMD_TenderList>();
                    tender.Location = obj.DocumentPath;
                }
                else
                {
                    AMD_TenderDocument aMDTenderDocument = new AMD_TenderDocument()
                    {
                        Id = Guid.NewGuid(),
                        TenderId = new Guid?(obj.Id),
                        FileName = obj.FileName,
                        ContentType = obj.ContentType,
                        IsPQDoc = new bool?(obj.IsPQDoc),
                        DocData = obj.DocData,
                        Created_Date = new DateTime?(DateTime.Now),
                        CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                    };
                    dbcontext.AMD_TenderDocuments.InsertOnSubmit(aMDTenderDocument);
                }
                dbcontext.SubmitChanges();
            }
        }

        public Guid InsertTenderList(TenderCreateModel obj)
        {
            Guid id;
            DateTime AvgDate = DateTime.ParseExact(obj.Adv_Date, "dd-MM-yyyy HH:mm:ss", new CultureInfo("en-US"));
            DateTime CloseDate = DateTime.ParseExact(obj.Closing_Date, "dd-MM-yyyy HH:mm:ss", new CultureInfo("en-US"));
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                AMD_TenderList Tlist = new AMD_TenderList()
                {
                    Id = Guid.NewGuid(),
                    NITNo = obj.NITNo,
                    Business = obj.Business,
                    Description = obj.Description,
                    Adv_Date = new DateTime?(AvgDate),
                    Closing_Date = new DateTime?(CloseDate),
                    Estimated_Cost = obj.Estimated_Cost,
                    Cost_of_EMD = obj.Cost_of_EMD,
                    Location = obj.Location,
                    Staus = obj.Status,
                    Created_Date = new DateTime?(DateTime.Now),
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId,
                    IsCloseEmailSent = new bool?(false),
                    IsReminderMailSent = new bool?(false),
                    PQApprovalRequired = new bool?(obj.PQApprovalRequired),
                    TenderType = obj.TenderType,
                    SupportEmailAddress = obj.SupportEmailAddress
                };
                dbcontext.AMD_TenderLists.InsertOnSubmit(Tlist);
                dbcontext.SubmitChanges();
                id = Tlist.Id;
            }
            return id;
        }

        public void UpdateTenderList(TenderEditModel obj)
        {
            byte[] bytes;
            DateTime AvgDate = DateTime.ParseExact(obj.Adv_Date, "dd-MM-yyyy HH:mm:ss", new CultureInfo("en-US"));
            DateTime CloseDate = DateTime.ParseExact(obj.Closing_Date, "dd-MM-yyyy HH:mm:ss", new CultureInfo("en-US"));
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                AMD_TenderList tenderlist = (
                    from x in dbcontext.AMD_TenderLists
                    where x.Id == obj.Id
                    select x).Single<AMD_TenderList>();
                tenderlist.NITNo = obj.NITNo;
                tenderlist.Business = obj.Business;
                tenderlist.Description = obj.Description;
                tenderlist.Adv_Date = new DateTime?(AvgDate);
                tenderlist.Closing_Date = new DateTime?(CloseDate);
                tenderlist.Location = obj.Location;
                tenderlist.Estimated_Cost = obj.Estimated_Cost;
                tenderlist.Cost_of_EMD = obj.Cost_of_EMD;
                tenderlist.Staus = obj.Status;
                tenderlist.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                tenderlist.Modified_Date = new DateTime?(DateTime.Now);
                tenderlist.IsCloseEmailSent = new bool?(false);
                tenderlist.IsReminderMailSent = new bool?(false);
                dbcontext.SubmitChanges();
                HttpPostedFileBase[] files = obj.Files;
                for (int i = 0; i < (int)files.Length; i++)
                {
                    HttpPostedFileBase file = files[i];
                    if (file != null && this.CheckExtension(file.FileName))
                    {
                        using (BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }
                        Path.GetFileNameWithoutExtension(file.FileName);
                        Path.GetExtension(file.FileName);
                        AMD_TenderDocument aMDTenderDocument = new AMD_TenderDocument()
                        {
                            Id = Guid.NewGuid(),
                            TenderId = new Guid?(tenderlist.Id),
                            FileName = file.FileName,
                            Created_Date = new DateTime?(DateTime.Now),
                            ContentType = file.ContentType,
                            DocData = bytes,
                            IsPQDoc = new bool?(false),
                            CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                        };
                        dbcontext.AMD_TenderDocuments.InsertOnSubmit(aMDTenderDocument);
                        dbcontext.SubmitChanges();
                    }
                }
                HttpPostedFileBase[] pQFiles = obj.PQFiles;
                for (int j = 0; j < (int)pQFiles.Length; j++)
                {
                    HttpPostedFileBase file = pQFiles[j];
                    if (file != null && this.CheckExtension(file.FileName))
                    {
                        using (BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }
                        Path.GetFileNameWithoutExtension(file.FileName);
                        Path.GetExtension(file.FileName);
                        AMD_TenderDocument aMDTenderDocument1 = new AMD_TenderDocument()
                        {
                            Id = Guid.NewGuid(),
                            TenderId = new Guid?(tenderlist.Id),
                            FileName = file.FileName,
                            Created_Date = new DateTime?(DateTime.Now),
                            ContentType = file.ContentType,
                            DocData = bytes,
                            IsPQDoc = new bool?(true),
                            CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                        };
                        dbcontext.AMD_TenderDocuments.InsertOnSubmit(aMDTenderDocument1);
                        dbcontext.SubmitChanges();
                    }
                }
            }
        }
    }
}