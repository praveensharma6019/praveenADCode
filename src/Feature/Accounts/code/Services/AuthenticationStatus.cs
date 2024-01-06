namespace Sitecore.Feature.Accounts.Services
{
    public enum AuthenticationStatus
    {
        Unauthenticated,
        Authenticated,
        PVCAuthenticated,
        Domestic,
        Commercial,
        Industrial,
        DomesticUnauthenticated,
        CommercialUnauthenticated,
        IndustrialUnauthenticated,
        AuthenticatedForComplaint,
        UnAuthenticatedForComplaint,
        CNGDealerUnauthenticated,
        CNGDealerAuthenticated,
        CNGAdminUserUnauthenticated,
        CNGAdminUserAuthenticated
    }
}