using Sitecore.Diagnostics.PerformanceCounters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public static class PortAccountRegistrationDropdowns
    {

        public static List<TypeofCustomers> TypeofCustomers = new List<TypeofCustomers>
                {
                   new TypeofCustomers { CustomersValue=1, CustomersName="Shipping-Line"},
                   new TypeofCustomers { CustomersValue=2, CustomersName="NVOCC"},
                   new TypeofCustomers { CustomersValue=3, CustomersName="Importer"},
                   new TypeofCustomers { CustomersValue=4, CustomersName="Exporter"},
                   new TypeofCustomers { CustomersValue=5, CustomersName="CHA"},
                   new TypeofCustomers { CustomersValue=6, CustomersName="Empty-Depot-Operator"},
                   new TypeofCustomers { CustomersValue=7, CustomersName="CFS-Operator"},
                   new TypeofCustomers { CustomersValue=8, CustomersName="Container-Train-Operator"},
                   new TypeofCustomers { CustomersValue=9, CustomersName="Vessel-Agent"},
                };

        public static List<Port> Port = new List<Port>
                {
                   new Port { PortValue=1, PortName="Adani CMA Mundra Terminal Private Limited"},
                   new Port { PortValue=2, PortName="Adani Ennore Container Terminal Private Limited"},
                   new Port { PortValue=3, PortName="Adani International Container Terminal Private Limited"},
                   new Port { PortValue=4, PortName="Adani Ports and Special Economic Zone Limited"},
                   new Port { PortValue=5, PortName="Adani Kandla Bulk Terminal Private Limited"},
                   new Port { PortValue=6, PortName="Adani Murmugao Port Terminal Private Limited"},
                   new Port { PortValue=7, PortName="Adani Petronet (Dahej) Port Private Limited"},
                   new Port { PortValue=8, PortName="Adani Vizag Coal Terminal Private Limited"},
                   new Port { PortValue=9, PortName="Adani Vizhinjam Port Private Limited"},
                   new Port { PortValue=10, PortName="The Dhamra Port Company Limited"},
                   new Port { PortValue=11, PortName="Kattupalli – Marine Infrastructure Development Private Limited"},
                   new Port { PortValue=12, PortName="Adani Hazira Port Private Limited"},
                   new Port { PortValue=13, PortName="The Adani Harbour Services Private Limited"},
                   new Port { PortValue=14, PortName="South Port Rail Head"},
                };
        public static List<SCMTRCode> SCMTRCode = new List<SCMTRCode>
                  {
                       new SCMTRCode { SCMTRCodeValue=1, SCMTRCodeName="ASC"},
                       new SCMTRCode { SCMTRCodeValue=2, SCMTRCodeName="ASA"},
                       new SCMTRCode { SCMTRCodeValue=3, SCMTRCodeName="ANC"},
                       new SCMTRCode { SCMTRCodeValue=4, SCMTRCodeName="ATP"},
                       new SCMTRCode { SCMTRCodeValue=5, SCMTRCodeName="ACU"},
                       new SCMTRCode { SCMTRCodeValue=6, SCMTRCodeName="ATO"},
                       new SCMTRCode { SCMTRCodeValue=7, SCMTRCodeName="AES"},
                   };

    }
}