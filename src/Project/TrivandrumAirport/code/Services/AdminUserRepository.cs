using Sitecore.TrivandrumAirport.Website;
using Sitecore.TrivandrumAirport.Website.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace Sitecore.TrivandrumAirport.Website.Services
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
                TRV_Registration user = (
                    from x in dbcontext.TRV_Registrations
                    where x.UserId == id
                    select x).Single<TRV_Registration>();
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
                Table<TRV_TenderList> TRVTenderLists = dbcontext.TRV_TenderLists;
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TRV_TenderList), "r");
                IOrderedQueryable<TRV_TenderList> TRVTenderLists1 = TRVTenderLists.OrderBy<TRV_TenderList, DateTime?>(Expression.Lambda<Func<TRV_TenderList, DateTime?>>(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(TRV_TenderList).GetMethod("get_Adv_Date").MethodHandle)), new ParameterExpression[] { parameterExpression }));
                parameterExpression = Expression.Parameter(typeof(TRV_TenderList), "r");
                IQueryable<SelectListItem> list = TRVTenderLists1.Select<TRV_TenderList, SelectListItem>(Expression.Lambda<Func<TRV_TenderList, SelectListItem>>(Expression.MemberInit(Expression.New(typeof(SelectListItem)), new MemberBinding[] { Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(typeof(SelectListItem).GetMethod("set_Value", new Type[] { typeof(string) }).MethodHandle), Expression.Call(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(TRV_TenderList).GetMethod("get_Id").MethodHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(object).GetMethod("ToString").MethodHandle), Array.Empty<Expression>())), Expression.Bind((MethodInfo)MethodBase.GetMethodFromHandle(typeof(SelectListItem).GetMethod("set_Text", new Type[] { typeof(string) }).MethodHandle), Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(TRV_TenderList).GetMethod("get_NITNo").MethodHandle))) }), new ParameterExpression[] { parameterExpression }));
                selectListItems = list.ToList<SelectListItem>();
            }
            return selectListItems;
        }

        public TRV_Registration InsertRegistrationEnveleope(AdminUserDetails obj)
        {
            TRV_Registration TRVRegistration;
            using (TenderDBDataContext dbcontext = new TenderDBDataContext())
            {
                string RandomUserId = this.GenerateRandomUserId();
                string RandomPassword = this.GenerateRandomPassword();
                TRV_Registration list = new TRV_Registration()
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
                dbcontext.TRV_Registrations.InsertOnSubmit(list);
                dbcontext.SubmitChanges();
                string result = "0";
                TRV_UserTenderMapping TRVUserTenderMapping = new TRV_UserTenderMapping()
                {
                    Id = Guid.NewGuid(),
                    UserId = list.UserId,
                    TenderId = new Guid(obj.SelectTenderId),
                    Envelope = result
                };
                dbcontext.TRV_UserTenderMappings.InsertOnSubmit(TRVUserTenderMapping);
                dbcontext.SubmitChanges();
                TRVRegistration = list;
            }
            return TRVRegistration;
        }

        public List<AdminUserDetails> ListRegisterdEnvelope()
        {
            List<AdminUserDetails> adminUserDetails;
            using (TenderDBDataContext tenderDBDataContext = new TenderDBDataContext())
            {
                IQueryable<AdminUserDetails> list =
                    from r in tenderDBDataContext.TRV_Registrations
                    where r.UserType == "Admin" && r.status == (bool?)true
                    select new AdminUserDetails()
                    {
                        Name = r.Name,
                        Email = r.Email,
                        MobileNo = r.Mobile,
                        UserType = r.UserType,
                        UserId = r.UserId,
                        TenderNumber = tenderDBDataContext.TRV_TenderLists.Where<TRV_TenderList>((TRV_TenderList x) => (Guid?)x.Id == r.TenderId).FirstOrDefault<TRV_TenderList>().NITNo,
                        EnvelopRight = tenderDBDataContext.TRV_UserTenderMappings.Where<TRV_UserTenderMapping>((TRV_UserTenderMapping x) => x.UserId == r.UserId && ((Guid?)x.TenderId == r.TenderId)).FirstOrDefault<TRV_UserTenderMapping>().Envelope
                    };
                adminUserDetails = list.ToList<AdminUserDetails>();
            }
            return adminUserDetails;
        }
    }
}