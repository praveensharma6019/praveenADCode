using Project.AAHL.Website.Models.About_Us;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Services.Certifications
{
    public interface ICertificationsService
    {
        CertificationsModel GetCertificate(Rendering rendering);
    }
}