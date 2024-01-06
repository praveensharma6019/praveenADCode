using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts
{
    public class EnumPayment
    {
        public enum GatewayType
        {
            PayUMoney = 1,
            BillDesk = 2,
            Paytm = 3,
            Ebixcash = 4,
            ICICIBank = 5,
            Benow = 6,
            BBPS = 7,
            HDFC=8,
            DBSUPI=9,
            CITYUPI=10,
            SafeXPay = 11,
            CashFree = 12
        }

        public enum PaymentType
        {
            BillingAmount = 1,
            SecurityDeposit = 2,
            VDS = 3
        }

        public enum UserType
        {
            Registered = 1,
            Guest = 2
        }
    }
}