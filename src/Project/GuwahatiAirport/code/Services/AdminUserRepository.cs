using Sitecore.GuwahatiAirport.Website;
using Sitecore.GuwahatiAirport.Website.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Sitecore.GuwahatiAirport.Website.Services
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
                GAU_Registration user = (
                    from x in dbcontext.GAU_Registrations
                    where x.UserId == id
                    select x).Single<GAU_Registration>();
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
                Table<GAU_TenderList> GAUTenderLists = dbcontext.GAU_TenderLists;
                ParameterExpression parameterExpression = Expression.Parameter(typeof(GAU_TenderList), "r");
                IOrderedQueryable<GAU_TenderList> GAUTenderLists1 = GAUTenderLists.OrderBy<GAU_TenderList, DateTime?>(Expression.Lambda<Func<GAU_TenderList, DateTime?>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(GAU_TenderList).GetMethod("get_Adv_Date").MethodHandle)), new ParameterExpression[] { parameterExpression }));
                parameterExpression = Expression.Parameter(typeof(GAU_TenderList), "r");
                IQueryable<SelectListItem> list = GAUTenderLists1.Select<GAU_TenderList, SelectListItem>(Expression.Lambda<Func<GAU_TenderList, SelectListItem>>(Expression.MemberInit(Expression.New(typeof(SelectListItem)), new MemberBinding[] { Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(typeof(SelectListItem).GetMethod("set_Value", new Type[] { typeof(string) }).MethodHandle), Expression.Call(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(GAU_TenderList).GetMethod("get_Id").MethodHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(object).GetMethod("ToString").MethodHandle), Array.Empty<Expression>())), Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(typeof(SelectListItem).GetMethod("set_Text", new Type[] { typeof(string) }).MethodHandle), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(GAU_TenderList).GetMethod("get_NITNo").MethodHandle))) }), new ParameterExpression[] { parameterExpression }));
                selectListItems = list.ToList<SelectListItem>();
            }
            return selectListItems;
        }

        public GAU_Registration InsertRegistrationEnveleope(AdminUserDetails obj)
        {
            GAU_Registration GAURegistration;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                string RandomUserId = this.GenerateRandomUserId();
                string RandomPassword = this.GenerateRandomPassword();
                GAU_Registration list = new GAU_Registration()
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
                dbcontext.GAU_Registrations.InsertOnSubmit(list);
                dbcontext.SubmitChanges();
                string result = "0";
                GAU_UserTenderMapping GAUUserTenderMapping = new GAU_UserTenderMapping()
                {
                    Id = Guid.NewGuid(),
                    UserId = list.UserId,
                    TenderId = new Guid(obj.SelectTenderId),
                    Envelope = result
                };
                dbcontext.GAU_UserTenderMappings.InsertOnSubmit(GAUUserTenderMapping);
                dbcontext.SubmitChanges();
                GAURegistration = list;
            }
            return GAURegistration;
        }

        public List<AdminUserDetails> ListRegisterdEnvelope()
        {
            List<AdminUserDetails> adminUserDetails;
            using (TenderDBDataContext tenderDBDataContext = new TenderDBDataContext())
            {
                IQueryable<AdminUserDetails> list =
                    from r in tenderDBDataContext.GAU_Registrations
                    where r.UserType == "Admin" && r.status == (bool?)true
                    select new AdminUserDetails()
                    {
                        Name = r.Name,
                        Email = r.Email,
                        MobileNo = r.Mobile,
                        UserType = r.UserType,
                        UserId = r.UserId,
                        TenderNumber = tenderDBDataContext.GAU_TenderLists.Where<GAU_TenderList>((GAU_TenderList x) => (Guid?)x.Id == r.TenderId).FirstOrDefault<GAU_TenderList>().NITNo,
                        EnvelopRight = tenderDBDataContext.GAU_UserTenderMappings.Where<GAU_UserTenderMapping>((GAU_UserTenderMapping x) => x.UserId == r.UserId && ((Guid?)x.TenderId == r.TenderId)).FirstOrDefault<GAU_UserTenderMapping>().Envelope
                    };
                adminUserDetails = list.ToList<AdminUserDetails>();
            }
            return adminUserDetails;
        }
    }
}