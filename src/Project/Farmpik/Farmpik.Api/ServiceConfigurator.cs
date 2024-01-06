using Sitecore.DependencyInjection;
using Farmpik.Domain.Interfaces.Services;
using Farmpik.Services.MobileServices;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Infrastructure.Repository;
using Farmpik.Infrastructure;
using Farmpik.Services.BackendServices;
using Farmpik.Services;
using System.ComponentModel;
using Farmpik.Domain.Common.ConfigurationModel;

namespace Sitecore.Farmpik.Api.Website
{
    public class ServiceConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            //serviceCollection.AddTransient<IMapper, Mapper>();
            //serviceCollection.AddTransient<IProductStockKeepingUnitRepository, ProductStockKeepingUnitRepository>();
            //serviceCollection.AddTransient<IProductRepository, ProductRepository>();
            //serviceCollection.AddTransient<IProductBusinessService, ProductBusinessService>();

                var mapperConfiguration = new MapperConfiguration(config =>
                {
                    config.AddProfile(new ProfileMapper());
                });
                serviceCollection.AddSingleton(mapperConfiguration.CreateMapper());
                 //   serviceCollection.AddSingleton(ConfigurationOptions);
            
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
        }

    }
}