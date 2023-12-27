using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class Warning
    {
        public string code { get; set; }
        public string description { get; set; }
        public string source { get; set; }
    }

    public class Error
    {
        public int statusCode { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string source { get; set; }
    }

    public class TotalFare
    {
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
        public decimal perPaxAmount { get; set; }
    }

    public class TotalBaseFare
    {
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
    }

    public class Sgst
    {
        public decimal amount { get; set; }
        public int percentage { get; set; }
    }

    public class Cgst
    {
        public decimal amount { get; set; }
        public int percentage { get; set; }
    }

    public class TotalTax
    {
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
        public Sgst sgst { get; set; }
        public Cgst cgst { get; set; }
    }

    public class TotalExpressFare
    {
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
    }

    public class DiscountCouponDetail
    {
        public int couponId { get; set; }
        public string couponCode { get; set; }
        public decimal couponDiscountAmount { get; set; }
        public string currencyCode { get; set; }
    }

    public class BaseFare
    {
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
        public decimal perPaxAmount { get; set; }
    }

    public class Tax
    {
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
        public string taxCode { get; set; }
        public string taxDesc { get; set; }
        public decimal perPaxAmount { get; set; }
        public int taxPercent { get; set; }
    }

    public class Taxes
    {
        public List<Tax> tax { get; set; }
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
        public decimal perPaxAmount { get; set; }
    }

    public class ExpressFare
    {
        public decimal amount { get; set; }
        public string currencyCode { get; set; }
        public decimal perPaxAmount { get; set; }
    }

    public class PassengerTypeQuantity
    {
        public BaseFare baseFare { get; set; }
        public Taxes taxes { get; set; }
        public TotalFare totalFare { get; set; }
        public ExpressFare expressFare { get; set; }
        public string code { get; set; }
        public int quantity { get; set; }
    }

    public class PricingInfo
    {
        public TotalFare totalFare { get; set; }
        public TotalBaseFare totalBaseFare { get; set; }
        public TotalTax totalTax { get; set; }
        public TotalExpressFare totalExpressFare { get; set; }
        public DiscountCouponDetail discountCouponDetail { get; set; }
        public List<PassengerTypeQuantity> passengerTypeQuantity { get; set; }
    }

    public class PackageAddOn
    {
        public int id { get; set; }
        public int packageId { get; set; }
        public int addOnServiceId { get; set; }
        public string addOnServiceName { get; set; }
        public string addOnServiceDescription { get; set; }
        public int price { get; set; }
        public int qty { get; set; }
        public string addOnImage { get; set; }
        public int totalPrice { get; set; }
    }

    public class CancellationPolicy
    {
        public int id { get; set; }
        public int packageId { get; set; }
        public int hoursBefore { get; set; }
        public decimal refundableAmountPercentage { get; set; }
    }

    public class SecurityKey
    {
        public string securityGUID { get; set; }
        public string key { get; set; }
    }

    public class PackageService
    {
        public int addOnServiceId { get; set; }
        public string addOnServiceName { get; set; }
        public string addOnServiceDescription { get; set; }
    }

    public class PackageDetail
    {
        public PricingInfo pricingInfo { get; set; }
        public List<PackageAddOn> packageAddOn { get; set; }
        public CancellationPolicy cancellationPolicy { get; set; }
        public SecurityKey securityKey { get; set; }
        public List<PackageService> packageService { get; set; }
        public string sequenceNumber { get; set; }
        public string name { get; set; }
        public string shortDesc { get; set; }
        public string packageNumber { get; set; }
        public int packageId { get; set; }
        public int businessUnitId { get; set; }
        public int travelSectorId { get; set; }
        public int airportMasterId { get; set; }
        public int serviceTypeId { get; set; }
        public decimal priceToPay { get; set; }
    }

    public class Data
    {
        public List<PackageDetail> packageDetails { get; set; }
    }

    public class SchedulerPackages
    {
        public Warning warning { get; set; }
        public Error error { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
}