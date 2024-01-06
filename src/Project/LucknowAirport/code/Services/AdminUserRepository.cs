using Sitecore.LucknowAirport.Website;
using Sitecore.LucknowAirport.Website.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Sitecore.LucknowAirport.Website.Services
{
    public class AdminUserRepository
    {
        public AdminUserRepository()
        {
        }

        public void DisableUser(string id)
        {
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                LKO_Registration user = (
                    from x in dbcontext.LKO_Registrations
                    where x.UserId == id
                    select x).Single<LKO_Registration>();
                user.status = new bool?(false);
                dbcontext.SubmitChanges();
            }
        }

        public string GenerateRandomPassword()
        {
            string returnString = string.Empty;
            Random random = new Random();
            returnString = new string((
                from s in Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                select s[random.Next(s.Length)]).ToArray<char>());
            return returnString;
        }

        public string GenerateRandomUserId()
        {
            string returnString = string.Empty;
            Random random = new Random();
            returnString = new string((
                from s in Enumerable.Repeat<string>("0123456789", 10)
                select s[random.Next(s.Length)]).ToArray<char>());
            return returnString;
        }

        public List<SelectListItem> GetOpenTenderListfordropdown()
        {
            List<SelectListItem> selectListItems;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                Table<LKO_TenderList> lKOTenderLists = dbcontext.LKO_TenderLists;
                ParameterExpression parameterExpression = Expression.Parameter(typeof(LKO_TenderList), "r");
                IOrderedQueryable<LKO_TenderList> lKOTenderLists1 = lKOTenderLists.OrderBy<LKO_TenderList, DateTime?>(Expression.Lambda<Func<LKO_TenderList, DateTime?>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(LKO_TenderList).GetMethod("get_Adv_Date").MethodHandle)), new ParameterExpression[] { parameterExpression }));
                parameterExpression = Expression.Parameter(typeof(LKO_TenderList), "r");
                IQueryable<SelectListItem> list = lKOTenderLists1.Select<LKO_TenderList, SelectListItem>(Expression.Lambda<Func<LKO_TenderList, SelectListItem>>(Expression.MemberInit(Expression.New(typeof(SelectListItem)), new MemberBinding[] { Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(typeof(SelectListItem).GetMethod("set_Value", new Type[] { typeof(string) }).MethodHandle), Expression.Call(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(LKO_TenderList).GetMethod("get_Id").MethodHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(object).GetMethod("ToString").MethodHandle), Array.Empty<Expression>())), Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(typeof(SelectListItem).GetMethod("set_Text", new Type[] { typeof(string) }).MethodHandle), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(LKO_TenderList).GetMethod("get_NITNo").MethodHandle))) }), new ParameterExpression[] { parameterExpression }));
                selectListItems = list.ToList<SelectListItem>();
            }
            return selectListItems;
        }

        public LKO_Registration InsertRegistrationEnveleope(AdminUserDetails obj)
        {
            LKO_Registration lKORegistration;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                string RandomUserId = this.GenerateRandomUserId();
                string RandomPassword = this.GenerateRandomPassword();
                LKO_Registration list = new LKO_Registration()
                {
                    Id = Guid.NewGuid(),
                    Name = obj.Name,
                    CompanyName = obj.Company,
                    Email = obj.Email,
                    Mobile = obj.MobileNo,
                    UserId = RandomUserId,
                    Password = RandomPassword,
                    UserType = "Admin",
                    TenderId = new Guid?(new Guid(obj.SelectTenderId)),
                    status = new bool?(true),
                    Created_Date = new DateTime?(DateTime.Now),
                    CreatedBy = TenderUserSession.TenderUserSessionContext.userId
                };
                dbcontext.LKO_Registrations.InsertOnSubmit(list);
                dbcontext.SubmitChanges();
                string result = "0";
                LKO_UserTenderMapping lKOUserTenderMapping = new LKO_UserTenderMapping()
                {
                    Id = Guid.NewGuid(),
                    UserId = list.UserId,
                    TenderId = new Guid(obj.SelectTenderId),
                    Envelope = result
                };
                dbcontext.LKO_UserTenderMappings.InsertOnSubmit(lKOUserTenderMapping);
                dbcontext.SubmitChanges();
                lKORegistration = list;
            }
            return lKORegistration;
        }

        public List<AdminUserDetails> ListRegisterdEnvelope()
        {
            List<AdminUserDetails> adminUserDetails;
            using (TenderDBDataContext tenderDBDataContext = new TenderDBDataContext())
            {
                IQueryable<AdminUserDetails> list =
                    from r in tenderDBDataContext.LKO_Registrations
                    where r.UserType == "Admin" && r.status == (bool?)true
                    select new AdminUserDetails()
                    {
                        Name = r.Name,
                        Email = r.Email,
                        MobileNo = r.Mobile,
                        UserType = r.UserType,
                        UserId = r.UserId,
                        TenderNumber = tenderDBDataContext.LKO_TenderLists.Where<LKO_TenderList>((LKO_TenderList x) => (Guid?)x.Id == r.TenderId).FirstOrDefault<LKO_TenderList>().NITNo,
                        EnvelopRight = tenderDBDataContext.LKO_UserTenderMappings.Where<LKO_UserTenderMapping>((LKO_UserTenderMapping x) => x.UserId == r.UserId && ((Guid?)x.TenderId == r.TenderId)).FirstOrDefault<LKO_UserTenderMapping>().Envelope
                    };
                adminUserDetails = list.ToList<AdminUserDetails>();
            }
            return adminUserDetails;
        }
    }
}