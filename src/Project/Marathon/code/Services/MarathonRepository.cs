using Sitecore.Diagnostics;
using Sitecore.Marathon.Website.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Marathon.Website.Services
{
    public class MarathonRepository
    {
        EnquiryDataContext dc = new EnquiryDataContext();
        AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
        public void DeleteOldOtp(string ContactNumber = null)
        {
            if (!string.IsNullOrEmpty(ContactNumber))
            {
                var removeexistotp = dc.user_otps.Where(x => x.MobileNumber == ContactNumber);
                dc.user_otps.DeleteAllOnSubmit(removeexistotp);
                dc.SubmitChanges();
            }
        }
        public string StoreGeneratedOtp(RegistrationModel model)
        {
            string sRandomOTP = GenerateRandomOTP(5);
            //user_otp entity = new user_otp()
            //{
            //    ContactNumber = model.ContactNumber,
            //    otp = sRandomOTP,
            //    status = false
            //};
            //dc.user_otps.InsertOnSubmit(entity);
            //dc.SubmitChanges();
            return sRandomOTP;

        }
        public string GetOTP(string ContactNumber = null)
        {
            if (!string.IsNullOrEmpty(ContactNumber))
            {
                var data = dc.user_otps.Where(x => x.MobileNumber == ContactNumber).FirstOrDefault();
                return data.otp;
            }
            return string.Empty;
        }

        //public string GenerateRandomBIBNumber(string RaceDistance)
        //{
        //    string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        //    string distance = string.Empty;
        //    int counter = 0;
        //    string BIB = string.Empty;
        //    string sTempChars = string.Empty;
        //    Random rand = new Random();
        //    if (RaceDistance=="21.097KM")
        //    {
        //        distance = "21";
        //        for (int i = 0; i < 1000; i++)
        //        {

        //            int p = rand.Next(0, saAllowedCharacters.Length);

        //            do { sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)]; }
        //            while (BIB.Contains(sTempChars));

        //            //BIB += distance + i.ToString("0") + sTempChars;
        //            BIB += distance + sTempChars.PadLeft(3, '0');

        //        }
        //    }
        //    if(RaceDistance=="42.195KM")
        //    {
        //        distance = "42";
        //        for (int i = 0; i < 6; i++)
        //        {

        //            int p = rand.Next(0, saAllowedCharacters.Length);

        //            do { sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)]; }
        //            while (BIB.Contains(sTempChars));

        //            //BIB += distance + i.ToString("0") + sTempChars;
        //            BIB += distance + sTempChars.PadLeft(3, '0');

        //        }
        //    }
        //    if (RaceDistance == "10KM")
        //    {
        //        distance = "10";
        //        for (int i = 0; i < 6; i++)
        //        {

        //            int p = rand.Next(0, saAllowedCharacters.Length);

        //            do { sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)]; }
        //            while (BIB.Contains(sTempChars));

        //            //BIB += distance + i.ToString("0") + sTempChars;
        //            BIB += distance + sTempChars.PadLeft(3, '0');

        //        }
        //    }

        //    //if (counter < 1000)
        //    //    BIB = distance + counter++.ToString().PadLeft(3, '0');
        //    //else
        //    //    BIB = distance + counter++.ToString();


        //    return BIB;

        //}

        public string GenerateRandomBIBNumber(string RaceDistance)
        {
            AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
            string distance = string.Empty;
            string input = string.Empty;
            string BIB = string.Empty;
            input = rdb.AhmedabadMarathonRegistrations.Where(x => x.RaceDistance == RaceDistance).Select(x => x.BIBNumber).FirstOrDefault();
            if (RaceDistance == "21.097KM")
            {
                if (input != null)
                {
                    distance = input;
                }
                else
                {
                    distance = "21000";
                }
                int Num1 = int.Parse(distance.Substring(4, 1));
                int Num2 = int.Parse(distance.Substring(3, 2));
                int Num3 = int.Parse(distance.Substring(2, 3));

                if (Num1 < 9)
                {
                    int Num = Num1 + 1;
                    BIB = distance.Substring(0, 4) + Num;

                }
                else if (Num2 < 99)
                {
                    int Num = Num2 + 1;
                    BIB = distance.Substring(0, 3) + Num;

                }
                else if (Num3 < 999)
                {
                    int Num = Num3 + 1;
                    BIB = distance.Substring(0, 2) + Num;

                }
            }
            if (RaceDistance == "42.195KM")
            {
                if (input != null)
                {
                    distance = input;
                }
                else
                {
                    distance = "42000";
                }
                int Num1 = int.Parse(distance.Substring(4, 1));
                int Num2 = int.Parse(distance.Substring(3, 2));
                int Num3 = int.Parse(distance.Substring(2, 3));

                if (Num1 < 9)
                {
                    int Num = Num1 + 1;
                    BIB = distance.Substring(0, 4) + Num;

                }
                else if (Num2 < 99)
                {
                    int Num = Num2 + 1;
                    BIB = distance.Substring(0, 3) + Num;

                }
                else if (Num3 < 999)
                {
                    int Num = Num3 + 1;
                    BIB = distance.Substring(0, 2) + Num;

                }
            }
            if (RaceDistance == "10KM")
            {
                if (input != null)
                {
                    distance = input;
                }
                else
                {
                    distance = "10000";
                }
                int Num1 = int.Parse(distance.Substring(4, 1));
                int Num2 = int.Parse(distance.Substring(3, 2));
                int Num3 = int.Parse(distance.Substring(2, 3));

                if (Num1 < 9)
                {
                    int Num = Num1 + 1;
                    BIB = distance.Substring(0, 4) + Num;

                }
                else if (Num2 < 99)
                {
                    int Num = Num2 + 1;
                    BIB = distance.Substring(0, 3) + Num;

                }
                else if (Num3 < 999)
                {
                    int Num = Num3 + 1;
                    BIB = distance.Substring(0, 2) + Num;

                }
            }
            
            return BIB;

        }

        private string GenerateRandomOTP(int iOTPLength)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            string sOTP = string.Empty;

            string sTempChars = string.Empty;
            List<int> possible = Enumerable.Range(1, 48).ToList();
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }
            return sOTP;

        }
        public string StoreEmailOTP(string email, string SessionID,string count)
        {
            try
            {
                count = count ?? "1";
                string sRandomOTP = GenerateRandomOTP(6);
                var existingEmail = rdb.AhmedabadMarathonOTPs.Where(a => a.EmailId.ToLower() == email.ToLower()).FirstOrDefault();
                if (existingEmail != null)
                {
                    existingEmail.OTP = sRandomOTP;
                    existingEmail.Modified = DateTime.Now;
                    existingEmail.CurrentSession = SessionID;
                    int num = int.Parse(count) + 1;
                    existingEmail.Count = num.ToString();
                    existingEmail.ValidateOTPCount = "0";
                    //rdb.AhmedabadMarathonOTPs.InsertOnSubmit(existingEmail);
                    rdb.SubmitChanges();
                }
                else
                {
                    AhmedabadMarathonOTP otp = new AhmedabadMarathonOTP()
                    {
                        EmailId = email,
                        OTP = sRandomOTP,
                        CurrentSession = SessionID,
                        Modified = DateTime.Now,
                        Count = count
                    };
                    rdb.AhmedabadMarathonOTPs.InsertOnSubmit(otp);
                    rdb.SubmitChanges();
                }
                return sRandomOTP;
            }
            catch (Exception ex)
            {
                Log.Error("StoreEmailOTP ", ex.Message);
                return "";
            }            
        }
        public string GetEmailOTP(string email = null, string SessionID = null)
        {
            if (!string.IsNullOrEmpty(email))
            {                
                AhmedabadMarathonOTP getOTP = (
                        from x in rdb.AhmedabadMarathonOTPs
                        where x.EmailId == email && x.CurrentSession == SessionID
                        select x).FirstOrDefault();
                if (getOTP != null)
                {
                    if (string.IsNullOrEmpty(getOTP.ValidateOTPCount))
                    {
                        getOTP.ValidateOTPCount = "1";
                        rdb.SubmitChanges();
                    }
                    else
                    {
                        if (Int64.Parse(getOTP.ValidateOTPCount) <= 3)
                        {
                            int num = int.Parse(getOTP.ValidateOTPCount) + 1;
                            getOTP.ValidateOTPCount = num.ToString();
                            rdb.SubmitChanges();
                        }
                        else
                        {                            
                            return "InvalidAttempt";
                        }
                    }
                    return getOTP.OTP;
                }
            }
            return string.Empty;
        }

        public string ParticipantStatus(string emailorPhone)
        {
            try
            {
                var OldexistingEmail = rdb.AhmedabadMarathonRegistrations.Where(a => (a.Email.ToLower() == emailorPhone.ToLower() || a.ContactNumber== emailorPhone) && a.RegistrationStatus.ToLower() == "successful" && a.Updated == null).FirstOrDefault();
                var NewexistingEmail = rdb.AhmedabadMarathonRegistrations.Where(a => (a.Email.ToLower() == emailorPhone.ToLower() || a.ContactNumber == emailorPhone) && a.Updated != null).FirstOrDefault();

                string status = string.Empty;
                if (OldexistingEmail != null)
                {
                    status = "old";
                }
                if (NewexistingEmail != null)
                {
                    if (NewexistingEmail.PaymentStatus.ToLower().Trim() == "pending")
                        status = "newPending";
                    else
                        status = "newRegistered";
                }
                return status;
            }
            catch (Exception ex)
            {
                Log.Error("ParticipantStatus ", ex.Message);
                return string.Empty;
            }
        }
    }
}

//using Sitecore.Diagnostics;
//using Sitecore.Marathon.Website;
//using Sitecore.Marathon.Website.Models;
//using System;
//using System.Data.Linq;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Runtime.CompilerServices;

//namespace Sitecore.Marathon.Website.Services
//{
//    public class MarathonRepository
//    {
//        private EnquiryDataContext dc = new EnquiryDataContext();

//        private AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();

//        public MarathonRepository()
//        {
//        }

//        public void DeleteOldOtp(string ContactNumber = null)
//        {
//            if (!string.IsNullOrEmpty(ContactNumber))
//            {
//                IQueryable<user_otp> userOtps =
//                    from x in this.dc.user_otps
//                    where x.MobileNumber == ContactNumber
//                    select x;
//                this.dc.user_otps.DeleteAllOnSubmit<user_otp>(userOtps);
//                this.dc.SubmitChanges();
//            }
//        }

//        private string GenerateRandomOTP(int iOTPLength)
//        {
//            string[] strArrays = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
//            string empty = string.Empty;
//            string str = string.Empty;
//            Random random = new Random();
//            for (int i = 0; i < iOTPLength; i++)
//            {
//                random.Next(0, (int)strArrays.Length);
//                str = strArrays[random.Next(0, (int)strArrays.Length)];
//                empty = string.Concat(empty, str);
//            }
//            return empty;
//        }

//        public string GetEmailOTP(string email = null)
//        {
//            string empty;
//            if (string.IsNullOrEmpty(email))
//            {
//                empty = string.Empty;
//            }
//            else
//            {
//                AhmedabadMarathonOTP ahmedabadMarathonOTP = (
//                    from x in this.rdb.AhmedabadMarathonOTPs
//                    where x.EmailId == email
//                    select x).FirstOrDefault<AhmedabadMarathonOTP>();
//                empty = ahmedabadMarathonOTP.OTP;
//            }
//            return empty;
//        }

//        public string GetOTP(string ContactNumber = null)
//        {
//            string empty;
//            if (string.IsNullOrEmpty(ContactNumber))
//            {
//                empty = string.Empty;
//            }
//            else
//            {
//                user_otp userOtp = (
//                    from x in this.dc.user_otps
//                    where x.MobileNumber == ContactNumber
//                    select x).FirstOrDefault<user_otp>();
//                empty = userOtp.otp;
//            }
//            return empty;
//        }

//        public string ParticipantStatus(string email)
//        {
//            MarathonRepository variable = null;
//            string empty;
//            try
//            {
//                Table<AhmedabadMarathonRegistration> ahmedabadMarathonRegistrations = this.rdb.AhmedabadMarathonRegistrations;
//                ParameterExpression parameterExpression = Expression.Parameter(typeof(AhmedabadMarathonRegistration), "a");
//                bool? nullable = null;
//                AhmedabadMarathonRegistration ahmedabadMarathonRegistration = ahmedabadMarathonRegistrations.Where<AhmedabadMarathonRegistration>(Expression.Lambda<Func<AhmedabadMarathonRegistration, bool>>(Expression.AndAlso(Expression.AndAlso(Expression.Equal(Expression.Call(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AhmedabadMarathonRegistration).GetMethod("get_Email").MethodHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(string).GetMethod("ToLower").MethodHandle), Array.Empty<Expression>()), Expression.Call(Expression.Field(Expression.Constant(variable, typeof(MarathonRepository)), FieldInfo.GetFieldFromHandle(typeof(MarathonRepository).GetField("email").FieldHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(string).GetMethod("ToLower").MethodHandle), Array.Empty<Expression>())), Expression.Equal(Expression.Call(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AhmedabadMarathonRegistration).GetMethod("get_RegistrationStatus").MethodHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(string).GetMethod("ToLower").MethodHandle), Array.Empty<Expression>()), Expression.Constant("successful", typeof(string)))), Expression.Equal(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AhmedabadMarathonRegistration).GetMethod("get_Updated").MethodHandle)), Expression.Constant(nullable, typeof(bool?)))), new ParameterExpression[] { parameterExpression })).FirstOrDefault<AhmedabadMarathonRegistration>();
//                Table<AhmedabadMarathonRegistration> ahmedabadMarathonRegistrations1 = this.rdb.AhmedabadMarathonRegistrations;
//                parameterExpression = Expression.Parameter(typeof(AhmedabadMarathonRegistration), "a");
//                nullable = null;
//                AhmedabadMarathonRegistration ahmedabadMarathonRegistration1 = ahmedabadMarathonRegistrations1.Where<AhmedabadMarathonRegistration>(Expression.Lambda<Func<AhmedabadMarathonRegistration, bool>>(Expression.AndAlso(Expression.Equal(Expression.Call(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AhmedabadMarathonRegistration).GetMethod("get_Email").MethodHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(string).GetMethod("ToLower").MethodHandle), Array.Empty<Expression>()), Expression.Call(Expression.Field(Expression.Constant(variable, typeof(MarathonRepository)), FieldInfo.GetFieldFromHandle(typeof(MarathonRepository).GetField("email").FieldHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(string).GetMethod("ToLower").MethodHandle), Array.Empty<Expression>())), Expression.NotEqual(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AhmedabadMarathonRegistration).GetMethod("get_Updated").MethodHandle)), Expression.Constant(nullable, typeof(bool?)))), new ParameterExpression[] { parameterExpression })).FirstOrDefault<AhmedabadMarathonRegistration>();
//                string str = string.Empty;
//                if (ahmedabadMarathonRegistration != null)
//                {
//                    str = "old";
//                }
//                if (ahmedabadMarathonRegistration1 != null)
//                {
//                    str = (ahmedabadMarathonRegistration1.PaymentStatus.ToLower().Trim() != "pending" ? "newRegistered" : "newPending");
//                }
//                empty = str;
//            }
//            catch (Exception exception)
//            {
//                Log.Error("ParticipantStatus ", exception.Message);
//                empty = string.Empty;
//            }
//            return empty;
//        }

//        public void StoreEmailOTP(string email)
//        {
//            MarathonRepository variable = null;
//            try
//            {
//                string str = this.GenerateRandomOTP(6);
//                Table<AhmedabadMarathonOTP> ahmedabadMarathonOTPs = this.rdb.AhmedabadMarathonOTPs;
//                ParameterExpression parameterExpression = Expression.Parameter(typeof(AhmedabadMarathonOTP), "a");
//                AhmedabadMarathonOTP nullable = ahmedabadMarathonOTPs.Where<AhmedabadMarathonOTP>(Expression.Lambda<Func<AhmedabadMarathonOTP, bool>>(Expression.Equal(Expression.Call(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(AhmedabadMarathonOTP).GetMethod("get_EmailId").MethodHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(string).GetMethod("ToLower").MethodHandle), Array.Empty<Expression>()), Expression.Call(Expression.Field(Expression.Constant(variable, typeof(MarathonRepository)), FieldInfo.GetFieldFromHandle(typeof(MarathonRepository).GetField("email").FieldHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(string).GetMethod("ToLower").MethodHandle), Array.Empty<Expression>())), new ParameterExpression[] { parameterExpression })).FirstOrDefault<AhmedabadMarathonOTP>();
//                if (nullable == null)
//                {
//                    AhmedabadMarathonOTP ahmedabadMarathonOTP = new AhmedabadMarathonOTP()
//                    {
//                        EmailId = email,
//                        OTP = str,
//                        Modified = new DateTime?(DateTime.Now)
//                    };
//                    this.rdb.AhmedabadMarathonOTPs.InsertOnSubmit(ahmedabadMarathonOTP);
//                    this.rdb.SubmitChanges();
//                }
//                else
//                {
//                    nullable.OTP = str;
//                    nullable.Modified = new DateTime?(DateTime.Now);
//                    this.rdb.SubmitChanges();
//                }
//            }
//            catch (Exception exception)
//            {
//                Log.Error("StoreEmailOTP ", exception.Message);
//            }
//        }

//        public string StoreGeneratedOtp(RegistrationModel model)
//        {
//            return this.GenerateRandomOTP(5);
//        }
//    }
//}