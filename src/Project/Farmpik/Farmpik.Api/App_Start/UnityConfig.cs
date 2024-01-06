/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using AutoMapper;
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Domain.Interfaces.Services;
using Farmpik.Infrastructure;
using Farmpik.Infrastructure.Repository;
using Farmpik.Services;
using Farmpik.Services.BackendServices;
using Farmpik.Services.MobileServices;
using System;

using Unity;

namespace Sitecore.Farmpik.Api.Website
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new ProfileMapper());
            });
            container.RegisterInstance(mapperConfiguration.CreateMapper());

            container.RegisterType<AppDbContext, AppDbContext>();
            container.RegisterType<IEmployeeRepository, EmployeeRepository>();
            container.RegisterType<IHelperMethod, HelperMethod>();
            container.RegisterType<IPaymentRepository, PaymentRepository>();
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IEmployeeRepository, EmployeeRepository>();
            container.RegisterType<IProductStockKeepingUnitRepository, ProductStockKeepingUnitRepository>();
            container.RegisterType<IVendorBusinessService, VendorBusinessService>();
            container.RegisterType<IRoleRepository, RoleRepository>();
            container.RegisterType<IReportRepository, ReportRepository>();
            container.RegisterType<IVendorRepository, VendorRepository>();
            container.RegisterType<IPurchaseRepository, PurchaseRepository>();

            container.RegisterType<IGuestUserRepository, GuestUserRepository>();
            container.RegisterType<IProductBusinessService, ProductBusinessService>();
            container.RegisterType<IEmployeeLoginBusinessService, EmployeeLoginBusinessService>();
            container.RegisterType<IImportFileBusinessService, ImportFileBusinessService>();
            container.RegisterType<IGoodsReceiptBusinessService, GoodsReceiptBusinessService>();
        }
    }
}