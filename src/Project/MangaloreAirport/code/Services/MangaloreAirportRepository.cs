using Sitecore.Diagnostics;
using Sitecore.MangaloreAirport.Website;
using Sitecore.MangaloreAirport.Website.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.MangaloreAirport.Website.Services
{
    public class MangaloreAirportRepository
    {
        public MangaloreAirportRepository()
        {
        }

        public void ActivateTender(Guid id)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                TenderList tenderlist = (
                    from x in dbcontext.TenderLists
                    where x.Id == id
                    select x).Single<TenderList>();
                tenderlist.Staus = "Open";
                tenderlist.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                tenderlist.Modified_Date = new DateTime?(DateTime.Now);
                List<TenderDetails> list = new List<TenderDetails>();
                List<Guid?> CorrigendumIdList = (
                    from x in dbcontext.CorrigendumTenderMappings
                    where x.TenderId == (Guid?)id
                    select x.CorrigendumId).ToList<Guid?>();
                foreach (Guid? nullable in CorrigendumIdList)
                {
                    Corrigendum corrigendum = (
                        from x in dbcontext.Corrigendums
                        where (Guid?)x.Id == nullable
                        select x).Single<Corrigendum>();
                    corrigendum.Status = new bool?(true);
                }
                dbcontext.SubmitChanges();
            }
        }

        private bool CheckExtension(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            return ((fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".pdf" || fileExtension == ".doc" || fileExtension == ".docx" || fileExtension == ".xls" ? false : fileExtension != ".xlsx") ? false : true);
        }

        public string datefortender(DateTime? dt)
        {
            return dt.Value.ToString("dd-MM-yyyy HH:mm:ss");
        }

        public void DeleteTenderDocument(Guid id, string DocumentPath)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                TenderDocument docFile = (
                    from x in dbcontext.TenderDocuments
                    where x.Id == id
                    select x).Single<TenderDocument>();
                dbcontext.TenderDocuments.DeleteOnSubmit(docFile);
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
                UserTenderDocument docFile = (
                    from x in dbcontext.UserTenderDocuments
                    where x.Id == id
                    select x).Single<UserTenderDocument>();
                if (docFile.UserId != userId)
                {
                    num = 1;
                }
                else
                {
                    dbcontext.UserTenderDocuments.DeleteOnSubmit(docFile);
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
                    from r in dbcontext.TenderLists
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
                        TenderDocuments = r.TenderDocuments.Where<TenderDocument>((TenderDocument x) => (x.TenderId == (Guid?)r.Id) && x.IsPQDoc == (bool?)false).ToList<TenderDocument>(),
                        PQDocuments = r.TenderDocuments.Where<TenderDocument>((TenderDocument x) => (x.TenderId == (Guid?)r.Id) && x.IsPQDoc == (bool?)true).ToList<TenderDocument>()
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
                    from r in dbcontext.TenderLists
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
                TenderList tenderlist = (
                    from x in dbcontext.TenderLists
                    where x.Id == id
                    select x).Single<TenderList>();
                tenderlist.Staus = "Inactive";
                tenderlist.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                tenderlist.Modified_Date = new DateTime?(DateTime.Now);
                List<TenderDetails> list = new List<TenderDetails>();
                List<Guid?> CorrigendumIdList = (
                    from x in dbcontext.CorrigendumTenderMappings
                    where x.TenderId == (Guid?)id
                    select x.CorrigendumId).ToList<Guid?>();
                foreach (Guid? nullable in CorrigendumIdList)
                {
                    Corrigendum corrigendum = (
                        from x in dbcontext.Corrigendums
                        where (Guid?)x.Id == nullable
                        select x).Single<Corrigendum>();
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
                    TenderList tender = (
                        from x in dbcontext.TenderLists
                        where x.Id == obj.Id
                        select x).Single<TenderList>();
                    tender.Location = obj.DocumentPath;
                }
                else
                {
                    TenderDocument tenderDocument = new TenderDocument()
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
                    dbcontext.TenderDocuments.InsertOnSubmit(tenderDocument);
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
                TenderList Tlist = new TenderList()
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
                dbcontext.TenderLists.InsertOnSubmit(Tlist);
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
                TenderList tenderlist = (
                    from x in dbcontext.TenderLists
                    where x.Id == obj.Id
                    select x).Single<TenderList>();
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
                    if (file != null)
                    {
                        if (this.CheckExtension(file.FileName))
                        {
                            using (BinaryReader br = new BinaryReader(file.InputStream))
                            {
                                bytes = br.ReadBytes(file.ContentLength);
                            }
                            Path.GetFileNameWithoutExtension(file.FileName);
                            Path.GetExtension(file.FileName);
                            TenderDocument tenderDocument = new TenderDocument()
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
                            dbcontext.TenderDocuments.InsertOnSubmit(tenderDocument);
                            dbcontext.SubmitChanges();
                        }
                    }
                }
                HttpPostedFileBase[] pQFiles = obj.PQFiles;
                for (int j = 0; j < (int)pQFiles.Length; j++)
                {
                    HttpPostedFileBase file = pQFiles[j];
                    if (file != null)
                    {
                        if (this.CheckExtension(file.FileName))
                        {
                            using (BinaryReader br = new BinaryReader(file.InputStream))
                            {
                                bytes = br.ReadBytes(file.ContentLength);
                            }
                            Path.GetFileNameWithoutExtension(file.FileName);
                            Path.GetExtension(file.FileName);
                            TenderDocument tenderDocument1 = new TenderDocument()
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
                            dbcontext.TenderDocuments.InsertOnSubmit(tenderDocument1);
                            dbcontext.SubmitChanges();
                        }
                    }
                }
            }
        }
    }
}