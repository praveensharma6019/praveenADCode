using Sitecore.JaipurAirport.Website;
using Sitecore.JaipurAirport.Website.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.JaipurAirport.Website.Services
{
    public class CorrigendumRepository
    {
        public CorrigendumRepository()
        {
        }

        public void ActivateCorrigendum(Guid id)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                JAI_Corrigendum corrigendumItem = (
                    from c in dbcontext.JAI_Corrigendums
                    where c.Id == id
                    select c).Single<JAI_Corrigendum>();
                corrigendumItem.Status = new bool?(true);
                dbcontext.SubmitChanges();
            }
        }

        public string dateforcorrigendum(DateTime? dt)
        {
            return dt.Value.ToString("dd-MM-yyyy HH:mm:ss");
        }

        public string dateforcorrigendumList(DateTime? dt)
        {
            return dt.Value.ToString("dd/MM/yy");
        }

        public void DeleteCorrigendumDocument(Guid id, string DocumentPath)
        {
            string path = HttpContext.Current.Server.MapPath(DocumentPath);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                JAI_CorrigendumDocument doc = (
                    from x in dbcontext.JAI_CorrigendumDocuments
                    where x.Id == id
                    select x).Single<JAI_CorrigendumDocument>();
                dbcontext.JAI_CorrigendumDocuments.DeleteOnSubmit(doc);
                try
                {
                    dbcontext.SubmitChanges();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }

        public List<CorrigendumModel> GetCorrigendumList(string UserId, string UserType)
        {
            List<CorrigendumModel> corrigendumModels;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                if (UserType != "Admin")
                {
                    IQueryable<CorrigendumModel> list =
                        from r in dbcontext.JAI_Corrigendums
                        orderby r.Date descending
                        select new CorrigendumModel()
                        {
                            Id = r.Id,
                            Title = r.Title,
                            FormatedDate = (DateTime)r.Date,
                            Status = (bool?)((bool)r.Status),
                            NITPRNo = this.NITRNCorrigendum(r.Id)
                        };
                    corrigendumModels = list.ToList<CorrigendumModel>();
                }
                else
                {
                    IQueryable<CorrigendumModel> list =
                        from r in dbcontext.JAI_Corrigendums
                        orderby r.Date descending
                        where r.CreatedBy == UserId
                        select new CorrigendumModel()
                        {
                            Id = r.Id,
                            Title = r.Title,
                            FormatedDate = (DateTime)r.Date,
                            Status = (bool?)((bool)r.Status),
                            NITPRNo = this.NITRNCorrigendum(r.Id)
                        };
                    corrigendumModels = list.ToList<CorrigendumModel>();
                }
            }
            return corrigendumModels;
        }

        public CorrigedumEditModel GetEditCorrigendumList(Guid id)
        {
            CorrigedumEditModel corrigedumEditModel;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                IQueryable<CorrigedumEditModel> list =
                    from r in dbcontext.JAI_Corrigendums
                    where r.Id == id
                    select new CorrigedumEditModel()
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Date = this.dateforcorrigendum(r.Date),
                        Status = r.Status,
                        TenderList = this.GetTenderList(TenderUserSession.TenderUserSessionContext.userId, TenderUserSession.TenderUserSessionContext.UserType).Where<Check>((Check x) => x.Status == "Open").ToList<Check>(),
                        CorrigendumDocument = r.JAI_CorrigendumDocuments.Where<JAI_CorrigendumDocument>((JAI_CorrigendumDocument x) => x.CorrigendumId == (Guid?)r.Id).ToList<JAI_CorrigendumDocument>(),
                        CorrigendumTenderMapping = r.JAI_CorrigendumTenderMappings.Where<JAI_CorrigendumTenderMapping>((JAI_CorrigendumTenderMapping x) => x.CorrigendumId == (Guid?)r.Id).ToList<JAI_CorrigendumTenderMapping>()
                    };
                corrigedumEditModel = list.FirstOrDefault<CorrigedumEditModel>();
            }
            return corrigedumEditModel;
        }

        public List<Check> GetTenderList(string userId, string userType)
        {
            List<Check> checks;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                if (userType != "Admin")
                {
                    IQueryable<Check> list =
                        from r in dbcontext.JAI_TenderLists
                        select new Check()
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
                    checks = list.ToList<Check>();
                }
                else
                {
                    IQueryable<Check> list =
                        from r in dbcontext.JAI_TenderLists
                        where r.CreatedBy == userId
                        select new Check()
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
                    checks = list.ToList<Check>();
                }
            }
            return checks;
        }

        public void InactivateCorrigendum(Guid id)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                JAI_Corrigendum corrigendumItem = (
                    from c in dbcontext.JAI_Corrigendums
                    where c.Id == id
                    select c).Single<JAI_Corrigendum>();
                corrigendumItem.Status = new bool?(false);
                dbcontext.SubmitChanges();
            }
        }

        public Guid InsertCorrigendum(CorrigendumModel obj)
        {
            Guid id;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                DateTime Date = DateTime.ParseExact(obj.Date, "dd-MM-yyyy HH:mm:ss", new CultureInfo("en-US"));
                JAI_Corrigendum list = new JAI_Corrigendum()
                {
                    Id = Guid.NewGuid(),
                    Title = obj.Title,
                    Date = new DateTime?(Date),
                    Status = obj.Status,
                    Created_Date = new DateTime?(DateTime.Now),
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                };
                dbcontext.JAI_Corrigendums.InsertOnSubmit(list);
                dbcontext.SubmitChanges();
                id = list.Id;
            }
            return id;
        }

        public void InsertCorrigendumDocument(CorrigendumModel obj)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                JAI_CorrigendumDocument aMDCorrigendumDocument = new JAI_CorrigendumDocument()
                {
                    Id = Guid.NewGuid(),
                    CorrigendumId = new Guid?(obj.Id),
                    FileName = obj.FileName,
                    DocumentPath = obj.DocumentPath,
                    ContentType = obj.ContentType,
                    IsPQDoc = new bool?(obj.IsPQDoc),
                    DocData = obj.DocData,
                    Created_Date = new DateTime?(DateTime.Now),
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                };
                dbcontext.JAI_CorrigendumDocuments.InsertOnSubmit(aMDCorrigendumDocument);
                dbcontext.SubmitChanges();
            }
        }

        public void InsertCorrigendumTenderMapping(Guid tenderId, Guid corrigId)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                JAI_CorrigendumTenderMapping aMDCorrigendumTenderMapping = new JAI_CorrigendumTenderMapping()
                {
                    Id = Guid.NewGuid(),
                    CorrigendumId = new Guid?(corrigId),
                    TenderId = new Guid?(tenderId),
                    Created_Date = new DateTime?(DateTime.Now),
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                };
                dbcontext.JAI_CorrigendumTenderMappings.InsertOnSubmit(aMDCorrigendumTenderMapping);
                dbcontext.SubmitChanges();
            }
        }

        public List<TenderDetails> NITRNCorrigendum(Guid corrigenudmID)
        {
            List<TenderDetails> tenderDetails;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                List<TenderDetails> list = new List<TenderDetails>();
                List<Guid?> data = (
                    from x in dbcontext.JAI_CorrigendumTenderMappings
                    where x.CorrigendumId == (Guid?)corrigenudmID
                    select x.TenderId).ToList<Guid?>();
                foreach (Guid? nullable in data)
                {
                    List<TenderDetails> tendersNITList = (
                        from x in dbcontext.JAI_TenderLists
                        where (Guid?)x.Id == nullable
                        select x into s
                        select new TenderDetails()
                        {
                            NITPRNo = s.NITNo
                        }).ToList<TenderDetails>();
                    list.AddRange(tendersNITList);
                }
                tenderDetails = list;
            }
            return tenderDetails;
        }

        public void UpdateCorrigendumDocument(CorrigedumEditModel obj)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                JAI_CorrigendumDocument aMDCorrigendumDocument = new JAI_CorrigendumDocument()
                {
                    Id = Guid.NewGuid(),
                    CorrigendumId = new Guid?(obj.Id),
                    FileName = obj.FileName,
                    DocumentPath = obj.DocumentPath,
                    ContentType = obj.ContentType,
                    IsPQDoc = new bool?(obj.IsPQDoc),
                    DocData = obj.DocData,
                    Created_Date = new DateTime?(DateTime.Now),
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                };
                dbcontext.JAI_CorrigendumDocuments.InsertOnSubmit(aMDCorrigendumDocument);
                dbcontext.SubmitChanges();
            }
        }

        public void UpdateCorrigendumMapping(CorrigedumEditModel obj, List<Check> selectedRecords)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                (
                    from x in dbcontext.JAI_CorrigendumTenderMappings
                    where x.CorrigendumId == (Guid?)obj.Id
                    select x).ToList<JAI_CorrigendumTenderMapping>();
                IQueryable<JAI_CorrigendumTenderMapping> del =
                    from x in dbcontext.JAI_CorrigendumTenderMappings
                    where x.CorrigendumId == (Guid?)obj.Id
                    select x;
                dbcontext.JAI_CorrigendumTenderMappings.DeleteAllOnSubmit<JAI_CorrigendumTenderMapping>(del);
                dbcontext.SubmitChanges();
                foreach (Check item in selectedRecords)
                {
                    JAI_CorrigendumTenderMapping aMDCorrigendumTenderMapping = new JAI_CorrigendumTenderMapping()
                    {
                        Id = Guid.NewGuid(),
                        CorrigendumId = new Guid?(obj.Id),
                        TenderId = new Guid?(item.Id),
                        Created_Date = new DateTime?(DateTime.Now),
                        CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                    };
                    dbcontext.JAI_CorrigendumTenderMappings.InsertOnSubmit(aMDCorrigendumTenderMapping);
                    dbcontext.SubmitChanges();
                }
            }
        }

        public void UpdateCorrrigendum(CorrigedumEditModel obj)
        {
            DateTime Date = DateTime.ParseExact(obj.Date, "dd-MM-yyyy HH:mm:ss", new CultureInfo("en-US"));
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                JAI_Corrigendum CorrigenList = (
                    from x in dbcontext.JAI_Corrigendums
                    where x.Id == obj.Id
                    select x).Single<JAI_Corrigendum>();
                CorrigenList.Title = obj.Title;
                CorrigenList.Date = new DateTime?(Date);
                CorrigenList.Status = obj.Status;
                CorrigenList.Modified_Date = new DateTime?(DateTime.Now);
                CorrigenList.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                dbcontext.SubmitChanges();
            }
        }
    }
}