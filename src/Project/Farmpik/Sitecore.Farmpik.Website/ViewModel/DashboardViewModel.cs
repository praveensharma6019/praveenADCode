using Farmpik.Domain.Commands.ImportFileCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Farmpik.Website.ViewModel
{
    public class DashboardViewModel
    {
        public List<ImportDetails> Imports { get; set; }

        public string UserName { get; set; }
    }
}