namespace Project.MiningRenderingHost.Website.Configurations
{
    public class SitecoreConfigurations
    {
        public static readonly string Key = "Sitecore";
        public Uri? InstanceUri { get; set; } = new System.Uri("https://sitecorecm.uat.adanirealty.com/");   //Update URL accordingly
        public string LayoutServicePath { get; set; } = "/sitecore/api/layout/render/jss";
        public string? DefaultSiteName { get; set; } = "Mining";
        public string? ApiKey { get; set; } = "5B7A0B92-33C3-499E-8F23-A26F6951C628";   //Update API Key accordingly
        public Uri? RenderingHostUri { get; set; } = new System.Uri("https://localhost:7294");
        public bool EnableExperienceEditor { get; set; } = true;
        public Uri? LayoutServiceUri { get; set; } = new System.Uri("https://sitecorecm.uat.adanirealty.com/sitecore/api/layout/render/jss");   //Update URL accordingly

    }
}
