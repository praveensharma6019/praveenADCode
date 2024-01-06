using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Foundation.OTPService.Templates
{
    public class Template
    {       

        public class SitecoreOTPFields
        {           
            public readonly static ID OTPLength = new ID("{CDCDB9C2-4429-4510-80E5-0C440A34F8A9}");
            public readonly static ID OTPMessageBody = new ID("{6B2F73CB-0D46-4B1C-AE89-2123A9E48B67}");
        }

        public class SitecoreOTPMessages
        {
            public readonly static ID NameErrorMessage = new ID("{EA830927-19B2-4A46-9062-474DE57D886E}");
            public readonly static ID MobileErrorMessage = new ID("{095F3094-8D6A-4871-AEE0-3611CDAE80B5}");
            public readonly static ID EmailErrorMessage = new ID("{A3B0CF5D-C8F9-4C18-9412-DA0660B3D97A}");
            public readonly static ID OTPAttemptExceedErrorMessage = new ID("{C3FECC48-6C1C-43C8-BBCC-8DE5E36AA1BB}");
            public readonly static ID OTPCustomErrorMessage = new ID("{E205CEEF-48AC-4ACA-8097-A70F61A1057E}");
        }

    }
}