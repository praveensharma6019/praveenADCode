using SapPiService.Domain;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Feature.Accounts;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.SessionHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Sitecore.Feature.Accounts.Services
{
    public class NameTransferService
    {
        public NameTransferRequestDetail GetRegisterNameTransferList(string id)
        {
            NameTransferRequestDetail result = new NameTransferRequestDetail();
            using (NameTransferRequestDataContext dbcontext = new NameTransferRequestDataContext())
            {
                result = dbcontext.NameTransferRequestDetails.Where(x => x.Id.ToString() == id).FirstOrDefault();
            }
            return result;
        }

        public List<NameTransferAdminRegistration> GetCreateAdminNameTransferList()
        {
            List<NameTransferAdminRegistration> result = new List<NameTransferAdminRegistration>();
            using (NameTransferRequestDataContext dbcontext = new NameTransferRequestDataContext())
            {
                result = dbcontext.NameTransferAdminRegistrations.Where(x => x.Role == "0").OrderByDescending(x => x.CreatedDate).ToList();
            }
            return result;
        }

        public NameTransferDocument GetDocumentNameTransferList(string id)
        {
            NameTransferDocument result = new NameTransferDocument();
            using (NameTransferRequestDataContext dbcontext = new NameTransferRequestDataContext())
            {
                result = dbcontext.NameTransferDocuments.Where(x => x.Id.ToString() == id).FirstOrDefault();
            }
            return result;
        }

        public NameTransferChangeNameByAdmin GetAdminActionUpdateList(string id)
        {
            NameTransferChangeNameByAdmin result = new NameTransferChangeNameByAdmin();
            using (NameTransferRequestDataContext dbcontext = new NameTransferRequestDataContext())
            {
                result = dbcontext.NameTransferChangeNameByAdmins.Where(x => x.UserId.ToString() == id).FirstOrDefault();
            }
            return result;
        }

        public NameTransferAdminRegistration GetAdminRegisterNameTransferList(string id)
        {
            NameTransferAdminRegistration result = new NameTransferAdminRegistration();
            using (NameTransferRequestDataContext dbcontext = new NameTransferRequestDataContext())
            {
                result = dbcontext.NameTransferAdminRegistrations.Where(x => x.Id.ToString() == id).FirstOrDefault();
            }
            return result;
        }
        public class CheckBoxes
        {
            public string Text { get; set; }
            public string Value { get; set; }
            public bool Checked { get; set; }
        }

    }
}