using System;
using System.Web.Services.Protocols;

namespace SapPiService
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SoapLoggerExtensionAttribute : SoapExtensionAttribute
    {
        public override int Priority { get; set; } = 1;

        public override Type ExtensionType => typeof (SoapLoggerExtension);
    }
    
   
}