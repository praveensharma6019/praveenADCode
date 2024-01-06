

// Sitecore.TrivandrumAirport.Website.Services.EnvelopRepository
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Sitecore.TrivandrumAirport.Website;
using Sitecore.TrivandrumAirport.Website.Model;

public class EnvelopRepository
{
    public List<SelectListItem> GetOpenTenderListfordropdown(string UserId, string userType)
    {
        using (TenderDBDataContext dbcontext = new TenderDBDataContext())
        {
            if (userType == "Admin")
            {
                var list = (from r in dbcontext.TRV_TenderLists
                            where r.CreatedBy == UserId
                            orderby r.Adv_Date
                            select (new SelectListItem
                            {
                                Value = r.Id.ToString(),
                                Text = r.NITNo,
                            }));

                return list.ToList();
            }
            else
            {
                var list = (from r in dbcontext.TRV_TenderLists
                            orderby r.Adv_Date
                            select (new SelectListItem
                            {
                                Value = r.Id.ToString(),
                                Text = r.NITNo,
                            }));

                return list.ToList();
            }
        }
    }

    public string GenerateRandomUserId()
    {
        string returnString = string.Empty;
        Random random = new Random();
        return new string((from s in Enumerable.Repeat("0123456789", 10)
                           select s[random.Next(s.Length)]).ToArray());
    }

    public string GenerateRandomPassword()
    {
        string returnString = string.Empty;
        Random random = new Random();
        return new string((from s in Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                           select s[random.Next(s.Length)]).ToArray());
    }

    public TRV_Registration InsertRegistrationEnveleope(EnvelopUserDetails obj)
    {
        using (TenderDBDataContext dbcontext = new TenderDBDataContext())
        {
            string RandomUserId = GenerateRandomUserId();
            string RandomPassword = GenerateRandomPassword();
            List<EnvelopName> SelectedEnvelope = obj.EnvelopNameCheckboxs.Where((EnvelopName x) => x.IsChecked).ToList();
            TRV_Registration list = new TRV_Registration
            {
                Id = Guid.NewGuid(),
                Name = obj.Name,
                CompanyName = "Adani",
                Email = obj.Email,
                Mobile = obj.MobileNo,
                UserId = RandomUserId,
                Password = RandomPassword,
                UserType = "EnvelopeAdmin",
                TenderId = new Guid(obj.SelectTenderId),
                status = true,
                Created_Date = DateTime.Now,
                CreatedBy = TenderUserSession.TenderUserSessionContext.userId
            };
            dbcontext.TRV_Registrations.InsertOnSubmit(list);
            dbcontext.SubmitChanges();
            StringBuilder builder = new StringBuilder();
            foreach (EnvelopName item in SelectedEnvelope)
            {
                if (item.IsChecked)
                {
                    builder.Append(item.Value).Append(",");
                }
            }
            string result = builder.ToString().Substring(0, builder.Length - 1);
            TRV_UserTenderMapping UserEnvelopeTenderMappingList = new TRV_UserTenderMapping
            {
                Id = Guid.NewGuid(),
                UserId = list.UserId,
                TenderId = new Guid(obj.SelectTenderId),
                Envelope = result
            };
            dbcontext.TRV_UserTenderMappings.InsertOnSubmit(UserEnvelopeTenderMappingList);
            dbcontext.SubmitChanges();
            return list;
        }
    }

    public List<EnvelopUserDetails> ListRegisterdEnvelope(string userId, string userType)
    {
        TenderDBDataContext dbcontext = new TenderDBDataContext();
        try
        {
            if (userType == "Admin")
            {
                IQueryable<EnvelopUserDetails> list2 = from r in dbcontext.TRV_Registrations
                                                       where r.UserType == "EnvelopeAdmin" && r.status == (bool?)true && r.CreatedBy == userId
                                                       select new EnvelopUserDetails
                                                       {
                                                           Name = r.Name,
                                                           Email = r.Email,
                                                           MobileNo = r.Mobile,
                                                           UserType = r.UserType,
                                                           UserId = r.UserId,
                                                           TenderNumber = dbcontext.TRV_TenderLists.Where((TRV_TenderList x) => (Guid)(Guid?)x.Id == (Guid)r.TenderId).FirstOrDefault().NITNo,
                                                           EnvelopRight = dbcontext.TRV_UserTenderMappings.Where((TRV_UserTenderMapping x) => x.UserId == r.UserId && (Guid)(Guid?)x.TenderId == (Guid)r.TenderId).FirstOrDefault().Envelope
                                                       };
                return list2.ToList();
            }
            IQueryable<EnvelopUserDetails> list = from r in dbcontext.TRV_Registrations
                                                  where r.UserType == "EnvelopeAdmin" && r.status == (bool?)true
                                                  select new EnvelopUserDetails
                                                  {
                                                      Name = r.Name,
                                                      Email = r.Email,
                                                      MobileNo = r.Mobile,
                                                      UserType = r.UserType,
                                                      UserId = r.UserId,
                                                      TenderNumber = dbcontext.TRV_TenderLists.Where((TRV_TenderList x) => (Guid)(Guid?)x.Id == (Guid)r.TenderId).FirstOrDefault().NITNo,
                                                      EnvelopRight = dbcontext.TRV_UserTenderMappings.Where((TRV_UserTenderMapping x) => x.UserId == r.UserId && (Guid)(Guid?)x.TenderId == (Guid)r.TenderId).FirstOrDefault().Envelope
                                                  };
            return list.ToList();
        }
        finally
        {
            if (dbcontext != null)
            {
                ((IDisposable)dbcontext).Dispose();
            }
        }
    }

    public void DisableUser(string id)
    {
        using (TenderDBDataContext dbcontext = new TenderDBDataContext())
        {
            TRV_Registration user = dbcontext.TRV_Registrations.Where((TRV_Registration x) => x.UserId == id).Single();
            user.status = false;
            dbcontext.SubmitChanges();
        }
    }
}
