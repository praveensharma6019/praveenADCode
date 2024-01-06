using Sitecore.Electricity.Website.Model;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Electricity.Website.Services
{
    //Repo: uses for Admin Tender CRUD
    //Electricity Tender Methods
    #region ElectricityRepository
    public class ElectricityRepository
    {
        public List<TenderModel> GetTenderList()
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var list = (from r in dbcontext.TenderLists
                            select (new TenderModel
                            {
                                Id = r.Id,
                                NITNo = r.NITNo,
                                Business = r.Business,
                                Description = r.Description,
                                Adv_Date = (DateTime)r.Adv_Date,
                                Closing_Date = (DateTime)r.Closing_Date,
                                Location = r.Location,
                                Status = r.Staus
                            }));

                return list.ToList();
            }
        }
        public List<TenderEditModel> GetEditTenderList()
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var list = (from r in dbcontext.TenderLists
                            select (new TenderEditModel
                            {
                                Id = r.Id,
                                NITNo = r.NITNo,
                                Business = r.Business,
                                Description = r.Description,
                                Adv_Date = datefortender(r.Adv_Date),
                                Closing_Date = datefortender(r.Closing_Date),
                                Estimated_Cost = r.Estimated_Cost,
                                Cost_of_EMD = r.Cost_of_EMD,
                                Location = r.Location,
                                Status = r.Staus,
                                OnHold = r.OnHold.GetValueOrDefault(),
                                TenderDocuments = r.TenderDocuments.Where(x => x.TenderId == r.Id).ToList(),
                            }));

                return list.ToList();
            }
        }

        public string datefortender(DateTime? dt)
        {
            return dt.Value.ToString("dd-MM-yyyy HH:mm:ss");
        }
        public Guid InsertTenderList(TenderCreateModel obj)
        {
            DateTime AvgDate = DateTime.ParseExact(obj.Adv_Date, "dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
            DateTime CloseDate = DateTime.ParseExact(obj.Closing_Date, "dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));

            //DateTime advDate = (DateTime.ParseExact(obj.Adv_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            //DateTime closeDate = (DateTime.ParseExact(obj.Closing_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));

            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                TenderList Tlist = new TenderList()
                {
                    Id = Guid.NewGuid(),
                    NITNo = obj.NITNo,
                    Business = obj.Business,
                    Description = obj.Description,
                    Adv_Date = AvgDate,
                    Closing_Date = CloseDate,
                    Estimated_Cost = obj.Estimated_Cost,
                    Cost_of_EMD = obj.Cost_of_EMD,
                    Location = obj.Location,
                    Staus = "Open", //obj.Status,
                    Created_Date = DateTime.Now,
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId,
                    IsCloseEmailSent = false,
                    IsReminderMailSent = false,
                    BuyerEmailId = obj.BuyerEmailId,
                    BuyerName = obj.BuyerName,
                    LeadBuyerName = obj.LeadBuyerName,
                    LeadBuyerEmailId = obj.LeadBuyerEmailId,
                    UserName = obj.UserName,
                    UserEmailId = obj.UserEmailId
                };
                dbcontext.TenderLists.InsertOnSubmit(Tlist);
                dbcontext.SubmitChanges();
                return Tlist.Id;
            }
        }
        public void InsertDocumentList(TenderCreateModel obj)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                if (obj.NITNo != "-")
                {
                    TenderDocument DList = new TenderDocument
                    {
                        Id = Guid.NewGuid(),
                        TenderId = obj.Id,
                        FileName = obj.FileName,
                        DocumentPath = obj.DocumentPath,
                        Created_Date = DateTime.Now,
                        CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                    };
                    dbcontext.TenderDocuments.InsertOnSubmit(DList);
                }
                else
                {
                    var tender = dbcontext.TenderLists.Where(x => x.Id == obj.Id).Single();
                    tender.Location = obj.DocumentPath;
                }
                dbcontext.SubmitChanges();
            }
        }
        public void UpdateTenderList(TenderEditModel obj)
        {
            DateTime AvgDate = DateTime.ParseExact(obj.Adv_Date, "dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
            DateTime CloseDate = DateTime.ParseExact(obj.Closing_Date, "dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var tenderlist = dbcontext.TenderLists.Where(x => x.Id == obj.Id).Single();
                tenderlist.NITNo = obj.NITNo;
                tenderlist.Business = obj.Business;
                tenderlist.Description = obj.Description;
                tenderlist.Adv_Date = AvgDate;
                tenderlist.Closing_Date = CloseDate;
                tenderlist.Location = obj.Location;
                tenderlist.Estimated_Cost = obj.Estimated_Cost;
                tenderlist.Cost_of_EMD = obj.Cost_of_EMD;
                tenderlist.Staus = obj.Status;
                tenderlist.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                tenderlist.Modified_Date = DateTime.Now;
                tenderlist.IsCloseEmailSent = false;
                tenderlist.IsReminderMailSent = false;
                dbcontext.SubmitChanges();

                foreach (HttpPostedFileBase file in obj.Files)
                {
                    if (file != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        var fileExt = Path.GetExtension(file.FileName);
                        string filenamewithtimestamp = fileName.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
                        string path = HttpContext.Current.Server.MapPath("/Tender/Uploadedfile/");
                        var filepath = "/Tender/Uploadedfile/" + filenamewithtimestamp;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        file.SaveAs(Path.Combine(path + filenamewithtimestamp));
                        TenderDocument DocumentList = new TenderDocument
                        {
                            Id = Guid.NewGuid(),
                            TenderId = tenderlist.Id,
                            FileName = fileName,
                            DocumentPath = filepath,
                            Created_Date = DateTime.Now,
                            CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                        };
                        dbcontext.TenderDocuments.InsertOnSubmit(DocumentList);
                        dbcontext.SubmitChanges();
                    }
                }

            }
        }

        public void InactivateTender(Guid id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var tenderlist = dbcontext.TenderLists.Where(x => x.Id == id).Single();
                tenderlist.Staus = "Inactive";
                tenderlist.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                tenderlist.Modified_Date = DateTime.Now;

                List<TenderDetails> list = new List<TenderDetails>();
                var CorrigendumIdList = dbcontext.CorrigendumTenderMappings.Where(x => x.TenderId == id).Select(x => x.CorrigendumId).ToList();
                foreach (var corrigendumItem in CorrigendumIdList)
                {
                    var corrigendum = dbcontext.Corrigendums.Where(x => x.Id == corrigendumItem).Single();
                    corrigendum.Status = false;
                }
                dbcontext.SubmitChanges();
            }

        }

        public void ActivateTender(Guid id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var tenderlist = dbcontext.TenderLists.Where(x => x.Id == id).Single();
                tenderlist.Staus = "Open";
                tenderlist.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                tenderlist.Modified_Date = DateTime.Now;

                List<TenderDetails> list = new List<TenderDetails>();
                var CorrigendumIdList = dbcontext.CorrigendumTenderMappings.Where(x => x.TenderId == id).Select(x => x.CorrigendumId).ToList();
                foreach (var corrigendumItem in CorrigendumIdList)
                {
                    var corrigendum = dbcontext.Corrigendums.Where(x => x.Id == corrigendumItem).Single();
                    corrigendum.Status = true;
                }
                dbcontext.SubmitChanges();
            }

        }

        public void UnHoldTender(Guid id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var tenderlist = dbcontext.TenderLists.Where(x => x.Id == id).Single();
                tenderlist.OnHold = false;
                tenderlist.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                tenderlist.Modified_Date = DateTime.Now;
                dbcontext.SubmitChanges();
            }

        }

        public void OnHoldTender(Guid id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var tenderlist = dbcontext.TenderLists.Where(x => x.Id == id).Single();
                tenderlist.OnHold = true;
                tenderlist.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                tenderlist.Modified_Date = DateTime.Now;
                dbcontext.SubmitChanges();
            }

        }

        public void DeleteTenderDocument(Guid id, string DocumentPath)
        {
            string path = HttpContext.Current.Server.MapPath(DocumentPath);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
            }
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var docFile = dbcontext.TenderDocuments.Where(x => x.Id == id).Single();
                dbcontext.TenderDocuments.DeleteOnSubmit(docFile);
                try
                {
                    dbcontext.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at TenderDetails Get:" + ex.Message, this);
                }
            }
        }
        public void DeleteUserTenderDocument(Guid id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var docFile = dbcontext.UserTenderDocuments.Where(x => x.Id == id).Single();

                string path = HttpContext.Current.Server.MapPath(docFile.DocumentPath);
                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();
                }
                //delete from db
                dbcontext.UserTenderDocuments.DeleteOnSubmit(docFile);

                try
                {
                    dbcontext.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at TenderDetails Get:" + ex.Message, this);
                }
            }
        }

        public TenderBidderCustomerCodeMaster GetCustomerVenderMapping(string PANNumber, string GSTNumber)
        {
            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    TenderBidderCustomerCodeMaster vendercustCode = dbcontext.TenderBidderCustomerCodeMasters.Where(c => c.PANNo == PANNumber && c.GSTNo == GSTNumber).FirstOrDefault();
                    if (vendercustCode != null)
                    {
                        return vendercustCode;
                    }
                    else
                    {
                        vendercustCode = dbcontext.TenderBidderCustomerCodeMasters.Where(c => c.GSTNo == GSTNumber).FirstOrDefault();
                        if (vendercustCode != null)
                        {
                            return vendercustCode;
                        }
                        else
                        {
                            vendercustCode = dbcontext.TenderBidderCustomerCodeMasters.Where(c => c.PANNo == PANNumber).FirstOrDefault();
                            if (vendercustCode != null)
                            {
                                return vendercustCode;
                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("GetCustomerVenderMapping error " + ex.Message, this);
                return null;
            }
        }
    }
    #endregion

    //Corrigendum Region Start
    //Corrigendum Related Methods Defined Here
    #region CorrigendumRepository
    public class CorrigendumRepository
    {
        public List<Check> GetTenderList()
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var list = (from r in dbcontext.TenderLists
                            select (new Check
                            {
                                Id = r.Id,
                                NITNo = r.NITNo,
                                Business = r.Business,
                                Description = r.Description,
                                Adv_Date = (DateTime)r.Adv_Date,
                                Closing_Date = (DateTime)r.Closing_Date,
                                Location = r.Location,
                                Status = r.Staus
                            }));

                return list.ToList();
            }
        }
        public List<CorrigendumModel> GetCorrigendumList()
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var list = (from r in dbcontext.Corrigendums
                            orderby r.Date descending
                            select (new CorrigendumModel
                            {
                                Id = r.Id,
                                Title = r.Title,
                                FormatedDate = (DateTime)r.Date,
                                Status = (bool)r.Status,
                                NITPRNo = NITRNCorrigendum(r.Id)
                            }));

                return list.ToList();
            }
        }

        public void InactivateCorrigendum(Guid id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var corrigendumItem = dbcontext.Corrigendums.Where(c => c.Id == id).Single();
                corrigendumItem.Status = false;
                dbcontext.SubmitChanges();
            }
        }

        public void ActivateCorrigendum(Guid id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var corrigendumItem = dbcontext.Corrigendums.Where(c => c.Id == id).Single();
                corrigendumItem.Status = true;
                dbcontext.SubmitChanges();
            }
        }

        public List<TenderDetails> NITRNCorrigendum(Guid corrigenudmID)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                List<TenderDetails> list = new List<TenderDetails>();
                var data = dbcontext.CorrigendumTenderMappings.Where(x => x.CorrigendumId == corrigenudmID).Select(x => x.TenderId).ToList();
                foreach (var item in data)
                {
                    var tendersNITList = dbcontext.TenderLists.Where(x => x.Id == item).Select(s => new TenderDetails() { NITPRNo = s.NITNo }).ToList();
                    list.AddRange(tendersNITList);
                }
                return list;
            }
        }
        public Guid InsertCorrigendum(CorrigendumModel obj)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                DateTime Date = DateTime.ParseExact(obj.Date, "dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
                Corrigendum list = new Corrigendum()
                {
                    Id = Guid.NewGuid(),
                    Title = obj.Title,
                    Date = Date,
                    Status = obj.Status,
                    Created_Date = DateTime.Now,
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                };
                dbcontext.Corrigendums.InsertOnSubmit(list);
                dbcontext.SubmitChanges();
                return list.Id;
            }
        }
        public void InsertCorrigendumDocument(CorrigendumModel obj)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                CorrigendumDocument list = new CorrigendumDocument
                {
                    Id = Guid.NewGuid(),
                    CorrigendumId = obj.Id,
                    FileName = obj.FileName,
                    DocumentPath = obj.DocumentPath,
                    Created_Date = DateTime.Now,
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                };
                dbcontext.CorrigendumDocuments.InsertOnSubmit(list);
                dbcontext.SubmitChanges();
            }
        }

        public void InsertCorrigendumTenderMapping(Guid tenderId, Guid corrigId)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                CorrigendumTenderMapping list = new CorrigendumTenderMapping
                {
                    Id = Guid.NewGuid(),
                    CorrigendumId = corrigId,
                    TenderId = tenderId,
                    Created_Date = DateTime.Now,
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                };

                dbcontext.CorrigendumTenderMappings.InsertOnSubmit(list);
                dbcontext.SubmitChanges();
            }
        }

        public CorrigedumEditModel GetEditCorrigendumList(Guid id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var list = (from r in dbcontext.Corrigendums
                            where r.Id == id
                            select (new CorrigedumEditModel
                            {
                                Id = r.Id,
                                Title = r.Title,
                                Date = dateforcorrigendum(r.Date),
                                Status = r.Status,
                                TenderList = GetTenderList().Where(x => x.Status == "Open").ToList(),
                                CorrigendumDocument = r.CorrigendumDocuments.Where(x => x.CorrigendumId == r.Id).ToList(),
                                CorrigendumTenderMapping = r.CorrigendumTenderMappings.Where(x => x.CorrigendumId == r.Id).ToList(),
                            }));
                return list.FirstOrDefault();
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
        public void UpdateCorrrigendum(CorrigedumEditModel obj)
        {
            DateTime Date = DateTime.ParseExact(obj.Date, "dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var CorrigenList = dbcontext.Corrigendums.Where(x => x.Id == obj.Id).Single();
                CorrigenList.Title = obj.Title;
                CorrigenList.Date = Date;
                CorrigenList.Status = obj.Status;
                CorrigenList.Modified_Date = DateTime.Now;
                CorrigenList.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                dbcontext.SubmitChanges();
            }
        }
        public void UpdateCorrigendumDocument(CorrigedumEditModel obj)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                CorrigendumDocument list = new CorrigendumDocument
                {
                    Id = Guid.NewGuid(),
                    CorrigendumId = obj.Id,
                    FileName = obj.FileName,
                    DocumentPath = obj.DocumentPath,
                    Created_Date = DateTime.Now,
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                };
                dbcontext.CorrigendumDocuments.InsertOnSubmit(list);
                dbcontext.SubmitChanges();
            }
        }
        public void UpdateCorrigendumMapping(CorrigedumEditModel obj, List<Check> selectedRecords)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var listdata = dbcontext.CorrigendumTenderMappings.Where(x => x.CorrigendumId == obj.Id).ToList();
                var del = dbcontext.CorrigendumTenderMappings.Where(x => x.CorrigendumId == obj.Id);
                dbcontext.CorrigendumTenderMappings.DeleteAllOnSubmit(del);
                dbcontext.SubmitChanges();

                foreach (var item in selectedRecords)
                {
                    CorrigendumTenderMapping Insertlist = new CorrigendumTenderMapping
                    {
                        Id = Guid.NewGuid(),
                        CorrigendumId = obj.Id,
                        TenderId = item.Id,
                        Created_Date = DateTime.Now,
                        CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                    };
                    dbcontext.CorrigendumTenderMappings.InsertOnSubmit(Insertlist);
                    dbcontext.SubmitChanges();
                }
            }
        }
        public void DeleteCorrigendumDocument(Guid id, string DocumentPath)
        {
            string path = HttpContext.Current.Server.MapPath(DocumentPath);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
            }
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var doc = dbcontext.CorrigendumDocuments.Where(x => x.Id == id).Single();
                dbcontext.CorrigendumDocuments.DeleteOnSubmit(doc);
                try
                {
                    dbcontext.SubmitChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

    }

    #endregion
    //Envelope Repository
    //Envelope Related Methods Define Here
    #region EnvelopeRepository
    public class EnvelopRepository
    {
        //listing tender for envelope dropdown
        public List<SelectListItem> GetOpenTenderListfordropdown()
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var list = (from r in dbcontext.TenderLists
                            orderby r.Created_Date descending
                            select (new SelectListItem
                            {
                                Value = r.Id.ToString(),
                                Text = r.NITNo,
                            }));

                return list.ToList();
            }
        }
        //Used for generating Random User Id
        public string GenerateRandomUserId()
        {
            string returnString = string.Empty;
            Random random = new Random();
            const string chars = "0123456789";
            returnString = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
            return returnString;
        }
        //Used for Generating Random User Password
        public string GenerateRandomPassword()
        {
            string returnString = string.Empty;
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            returnString = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
            return returnString;
        }
        //For Register Envelope User into Registation and UserTenderMapping Table.
        public Registration InsertRegistrationEnveleope(EnvelopUserDetails obj)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var RandomUserId = GenerateRandomUserId();
                var RandomPassword = GenerateRandomPassword();
                var SelectedEnvelope = obj.EnvelopNameCheckboxs.Where(x => x.IsChecked == true).ToList();

                Registration list = new Registration
                {
                    Id = Guid.NewGuid(),
                    Name = obj.Name,
                    CompanyName = obj.Company,
                    Email = obj.Email,
                    Mobile = obj.MobileNo,
                    UserId = RandomUserId,
                    Password = RandomPassword,
                    UserType = "EnvelopeAdmin",
                    TenderId = new Guid(obj.SelectTenderId),
                    status = true,
                    Created_Date = DateTime.Now,
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId,
                    IsBuyer = obj.IsBuyer,
                    BuyerType = obj.BuyerType
                };
                dbcontext.Registrations.InsertOnSubmit(list);
                dbcontext.SubmitChanges();

                StringBuilder builder = new StringBuilder();
                foreach (var item in SelectedEnvelope) // Loop through all strings
                {
                    if (item.IsChecked)
                    {
                        builder.Append(item.Value).Append(","); // Append string to StringBuilder
                    }
                }
                string result = builder.ToString().Substring(0, builder.Length - 1);

                //insert into UserTenderMapping
                UserTenderMapping UserEnvelopeTenderMappingList = new UserTenderMapping
                {
                    Id = Guid.NewGuid(),
                    UserId = list.UserId,
                    TenderId = new Guid(obj.SelectTenderId),
                    Envelope = result,
                };
                dbcontext.UserTenderMappings.InsertOnSubmit(UserEnvelopeTenderMappingList);
                dbcontext.SubmitChanges();

                return list;
            }
        }
        //List RegistedEnvelope
        public List<EnvelopUserDetails> ListRegisterdEnvelope()
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var list = (from r in dbcontext.Registrations
                            where r.UserType == "EnvelopeAdmin" && r.status == true
                            select (new EnvelopUserDetails
                            {
                                Name = r.Name,
                                Email = r.Email,
                                MobileNo = r.Mobile,
                                UserType = r.UserType,
                                UserId = r.UserId,
                                TenderNumber = dbcontext.TenderLists.Where(x => x.Id == r.TenderId).FirstOrDefault().NITNo,
                                EnvelopRight = dbcontext.UserTenderMappings.Where(x => x.UserId == r.UserId && x.TenderId == r.TenderId).FirstOrDefault().Envelope
                            }));
                return list.ToList();
            }
        }
        //Used for Desabling User from Registration Table
        public void DisableUser(string id)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var user = dbcontext.Registrations.Where(x => x.UserId == id).Single();
                user.status = false;
                dbcontext.SubmitChanges();
            }
        }
    }

    #endregion
}