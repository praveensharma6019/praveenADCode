/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System;

namespace Farmpik.Domain.Entities
{
    public class PurchaseReport : AuditEntity
	{
		public string PlantName { get; set; }
		public string GateNumber { get; set; }
		public string VendorCode { get; set; }
		public string VendorName { get; set; }
		public string PurchaseOrderNumber { get; set; }
		public string MatOrderNumber { get; set; }
		public DateTime PostingDate { get; set; }
		public decimal TotalQuantity { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal A1PremiumELQuantity { get; set; }
		public decimal A1PremiumELAmount { get; set; }
		public decimal A2PremiumLMQuantity { get; set; }
		public decimal A2PremiumLMAmount { get; set; }
		public decimal A3PremiumP2Quantity { get; set; }
		public decimal A3PremiumP2Amount { get; set; }
		public decimal A4PremiumSQuantity { get; set; }
		public decimal A4PremiumSAmount { get; set; }
		public decimal A5PremiumESQuantity { get; set; }
		public decimal A5PremiumESAmount { get; set; }
		public decimal A6PremiumEESQuantity { get; set; }
		public decimal A6PremiumEESAmount { get; set; }
		public decimal B1SupremeELQuantity { get; set; }
		public decimal B1SupremeELAmount { get; set; }
		public decimal B2SupremeLMQuantity { get; set; }
		public decimal B2SupremeLMAmount { get; set; }
		public decimal B3SupremeP2Quantity { get; set; }
		public decimal B3SupremeP2Amount { get; set; }
		public decimal B4SupremeSQuantity { get; set; }
		public decimal B4SupremeSAmount { get; set; }
		public decimal B5SupremeESQuantity { get; set; }
		public decimal B5SupremeESAmount { get; set; }
		public decimal B6SupremeEESQuantity { get; set; }
		public decimal B6SupremeEESAmount { get; set; }
		public decimal C1UnderSizeQuantity { get; set; }
		public decimal C1UnderSizeAmount { get; set; }
		public decimal C2SuperEESP2Quantity { get; set; }
		public decimal C2SuperEESP2Amount { get; set; }
		public decimal D1PssEELQuantity { get; set; }
		public decimal D1PssEELAmount { get; set; }
		public decimal D2SuperEL2ESQuantity { get; set; }
		public decimal D2SuperEL2ESAmount { get; set; }
		public decimal W1PssROLQuantity { get; set; }
		public decimal W1PssROLAmount { get; set; }
		public decimal ROAQuantity { get; set; }
		public decimal ROAAmount { get; set; }

		public bool HasErrorField { get; set; }
		public string ErrorField { get; set; }
	}
}
