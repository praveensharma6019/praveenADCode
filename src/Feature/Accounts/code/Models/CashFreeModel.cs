using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{

    public class CustomerDetails
    {
        public string customer_id { get; set; }
        public string customer_email { get; set; }
        public string customer_phone { get; set; }
        public string customer_name { get; set; }
    }

    public class OrderTags
    {
        public string payment_types { get; set; }
    }
    public class OrderMeta
    {
        public string return_url { get; set; }
        public object notify_url { get; set; }
        public object payment_methods { get; set; }
    }

    public class Payments
    {
        public string url { get; set; }
    }

    public class Refunds
    {
        public string url { get; set; }
    }

    public class RootJsonRequest
    {

        public string order_id { get; set; }

        public string order_currency { get; set; }
        public string order_amount { get; set; }

        public CustomerDetails customer_details { get; set; } = new CustomerDetails();
        public OrderMeta order_meta { get; set; } = new OrderMeta();
        public OrderTags order_tags { get; set; } = new OrderTags();
    }

    public class RootJson
    {
        public int cf_order_id { get; set; }
        public string order_id { get; set; }
        public string entity { get; set; }
        public string order_currency { get; set; }
        public string order_amount { get; set; }
        public DateTime order_expiry_time { get; set; }
        public DateTime Transaction_date { get; set; }
        public CustomerDetails customer_details { get; set; } = new CustomerDetails();
        public OrderMeta order_meta { get; set; } = new OrderMeta();
        public OrderTags order_tags { get; set; } = new OrderTags();
        public Settlements settlements { get; set; } = new Settlements();
        public Payments payments { get; set; } = new Payments();
        public Refunds refunds { get; set; } = new Refunds();
        public string order_status { get; set; }
        public string order_token { get; set; }
        public object order_note { get; set; }
        public string payment_link { get; set; }        
        public string paymentType { get; set; }
        public List<object> order_splits { get; set; }
    }

    public class Settlements
    {
        public string url { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Card
    {
        public object channel { get; set; }
        public string card_number { get; set; }
        public string card_network { get; set; }
        public string card_type { get; set; }
        public string card_country { get; set; }
        public string card_bank_name { get; set; }
    }

    public class CashFreeData
    {
        public Order order { get; set; }
        public Payment payment { get; set; }
        public CustomerDetails customer_details { get; set; }
        public OrderTags order_Tags { get; set; }
    }

    public class Order
    {
        public string order_id { get; set; }
        public double order_amount { get; set; }
        public string order_currency { get; set; }
        public OrderTags order_tags { get; set; }
    }

    public class Payment
    {
        public int cf_payment_id { get; set; }
        public string payment_status { get; set; }
        public double payment_amount { get; set; }
        public string payment_currency { get; set; }
        public string payment_message { get; set; }
        public DateTime payment_time { get; set; }
        public string bank_reference { get; set; }
        public object auth_id { get; set; }
        public PaymentMethod payment_method { get; set; }
        public string payment_group { get; set; }
    }

    public class PaymentMethod
    {
        public Card card { get; set; }
    }

    public class Root
    {
        public CashFreeData data { get; set; }
        public DateTime event_time { get; set; }
        public string type { get; set; }
    }



}