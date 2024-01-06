/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System;
using System.Web;

namespace Farmpik.Domain.Commands.ImportFileCommands
{
    public class ImportFileCommand 
    {
        public HttpPostedFileBase VendorMaster { get; set; }

        public HttpPostedFileBase PRNMaster { get; set; }

        public HttpPostedFileBase PriceMaster { get; set; }

        public HttpPostedFileBase ProductDetails { get; set; }

        public HttpPostedFileBase PaymentStatus { get; set; }

        public Guid CreatedBy { get; set; }
    }

    public class ImportDetails
    {
        public string TemplateName { get; set; }
        public DateTime? LastImportedDate { get; set; }
        public string DisplayLastImportedDate { get { return LastImportedDate.HasValue ? LastImportedDate.Value.ToString("d MMM yyyy") : "-"; } }
        public long NoOfUploads { get; set; }
        public string LastImportedUser { get; set; }
        public int TotalRecords { get; set; }
        public int ErrorRecords { get; set; }
        public bool? IsValidTemplate { get; set; }
    }
}