using Farmpik.Domain.Common.ConfigurationModel;
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Domain.Interfaces.Services;
using Farmpik.Infrastructure;
using Farmpik.Infrastructure.Repository;
using Farmpik.Services.BackendServices;
using Farmpik.Services.MobileServices;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Farmpik.Website.Repositories;
using System.ComponentModel;

namespace Sitecore.Farmpik.Website
{
    public class ServiceConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ConfigurationOptions, ConfigurationOptions>();
            serviceCollection.AddTransient<AppDbContext, AppDbContext>();
            serviceCollection.AddTransient<IEmployeeRepository, EmployeeRepository>();
            serviceCollection.AddTransient<IHelperMethod, HelperMethod>();
            serviceCollection.AddTransient<IPaymentRepository, PaymentRepository>();
            serviceCollection.AddTransient<IProductRepository, ProductRepository>();
            serviceCollection.AddTransient<IEmployeeRepository, EmployeeRepository>();
            serviceCollection.AddTransient<IProductStockKeepingUnitRepository, ProductStockKeepingUnitRepository>();
            serviceCollection.AddTransient<IVendorBusinessService, VendorBusinessService>();
            serviceCollection.AddTransient<IRoleRepository, RoleRepository>();
            serviceCollection.AddTransient<IReportRepository, ReportRepository>();
            serviceCollection.AddTransient<IVendorRepository, VendorRepository>();
            serviceCollection.AddTransient<IPurchaseRepository, PurchaseRepository>();
            serviceCollection.AddTransient<IImportFileRepository, ImportFileRepository>();
            serviceCollection.AddTransient<IProductBusinessService, ProductBusinessService>();
            serviceCollection.AddTransient<IEmployeeLoginBusinessService, EmployeeLoginBusinessService>();
            serviceCollection.AddTransient<IImportFileBusinessService, ImportFileBusinessService>();
            serviceCollection.AddTransient<IGoodsReceiptBusinessService, GoodsReceiptBusinessService>();
            serviceCollection.AddTransient<ITemplateBusinessService, TemplateBusinessService>();
            serviceCollection.AddTransient<IFarmpikRepository, FarmpikRepository>();
            serviceCollection.AddTransient<IGuestUserRepository, GuestUserRepository>();

        }

    }
}