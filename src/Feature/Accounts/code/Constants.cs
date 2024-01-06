namespace Sitecore.Feature.Accounts
{
    using Sitecore.Data;

    public struct Constants
    {


        public struct UserProfile
        {
            public struct Fields
            {
                public static readonly string FirstName = "FirstName";
                public static readonly string MiddleName = "MiddleName";
                public static readonly string LastName = "LastName";
                public static readonly string PhoneNumber = "Phone";
                public static readonly string Interest = "Interest";
                public static readonly string Gender = "Gender";
                public static readonly string Link = "Link";
                public static readonly string Location = "Location";
                public static readonly string Language = "Language";
                public static readonly string Timezone = "Timezone";
                public static readonly string PictureUrl = "PictureUrl";
                public static readonly string PictureMimeType = "PictureMimeType";
                public static readonly string Birthday = "Date of birth";
                public static readonly string MobileNo = "Mobile Number";
                public static readonly string LandLineNo = "Landline Number";
                public static readonly string Ebill = "E Bill";
                public static readonly string SMSUpdate = "SMS update";
                public static readonly string PaperlessBilling = "PaperlessBilling";
                public static readonly string PrimaryAccountNo = "Primary Account No";
                public static readonly string MultipleAccount = "Multiple Account";

            }
        }
        public struct TemplateIds
        {
            public static readonly string ProfileAccountTemplateId = "{38FCD543-B37B-46EE-B867-40A000148DDB}";
        }

        public struct DomainDetail
        {
            public static readonly string CurrentDomainName = "electricity";
        }

        public struct PaytmResponseStatus
        {
            public static readonly string Success = "TXN_SUCCESS";
            public static readonly string Failure = "TXN_FAILURE";
            public static readonly string Pending = "PENDING";
        }

        public struct PayUResponseStatus
        {
            public static readonly string Success = "success";
            public static readonly string Failure = "failure";
            public static readonly string Pending = "pending";
        }

        public struct PaymentResponse
        {
            public static readonly string Success = "Success";
            public static readonly string Failure = "Failure";
            public static readonly string Pending = "Pending";
        }

        public struct BillDeskResponse
        {
            public static readonly string SuccessCode = "0300";
        }

        public struct SafeXPayResponse
        {
            public static readonly string SuccessCode = "0";
        }
    }
}