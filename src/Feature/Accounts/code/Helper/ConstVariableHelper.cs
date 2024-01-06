using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Helper
{
    public static class ConstVariableHelper
    {
        public static string SocietyMethodName = "/ConnObjSet?$filter=RegioGroup eq '{0}' and Plant eq '{1}'";

        public static string CityRegionMethodName = "/RSA_MappingSet";

        public static string OTPMethodName="/SendOtpSet('{0}')";
    }
}