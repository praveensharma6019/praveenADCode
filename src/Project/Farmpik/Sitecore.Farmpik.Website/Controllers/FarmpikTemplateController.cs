using Farmpik.Domain.Commands.ProductCommands;
using Farmpik.Domain.Interfaces.Services;
using log4net;
using Sitecore.Farmpik.Website.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Farmpik.Website.Controllers
{
    public class FarmpikTemplateController : Controller
    {
        private readonly ILog _logger;
        private readonly IProductBusinessService _productBusinessService;
        private readonly ITemplateBusinessService _templateBusinessService;

        public FarmpikTemplateController(IProductBusinessService productBusinessService,
            ITemplateBusinessService templateBusinessService)
        {
            _logger = LogManager.GetLogger(typeof(FarmpikTemplateController));
            _productBusinessService = productBusinessService;
            _templateBusinessService = templateBusinessService;
        }

        public async Task<ActionResult> Vendors(int currentPage = 1, int size = 10)
        {
            try
            {
                var vendors = await _templateBusinessService.GetVendors(currentPage, size);
                return View(new VendorViewModel
                {
                    Vendors = vendors.Payload,
                    TotalCount = vendors.Count ?? 0,
                    PageNumber = currentPage,
                    PageSize = size
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View("Error");
            }
        }

        public async Task<ActionResult> Purchases(int currentPage = 1, int size = 10)
        {
            try
            {
                var purchases = await _templateBusinessService.GetPurchases(currentPage, size);
                return View(new PurchaseViewModel
                {
                    Purchases = purchases.Payload,
                    TotalCount = purchases.Count ?? 0,
                    PageNumber = currentPage,
                    PageSize = size
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View("Error");
            }
        }

        public async Task<ActionResult> Prices()
        {
            try
            {
                var shimla = await _productBusinessService.GetProductPrices(new QueryProductPricesCommand
                {
                    Location = "Shimla",
                    PageNumber = 1,
                    PageSize = int.MaxValue
                });

                var kinnaur = await _productBusinessService.GetProductPrices(new QueryProductPricesCommand
                {
                    Location = "Kinnaur",
                    PageNumber = 1,
                    PageSize = int.MaxValue
                });

                return View(new PriceViewModel
                {
                    ShimlaEffectiveDate = shimla.Payload.FirstOrDefault()?.EffectiveDate,
                    KinnaurEffectiveDate = kinnaur.Payload.FirstOrDefault()?.EffectiveDate,
                    Shimla = shimla.Payload,
                    Kinnaur = kinnaur.Payload
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View("Error");
            }
        }

        public async Task<ActionResult> Products(int currentPage = 1, int size = 10)
        {
            try
            {
                var products = await _productBusinessService.GetProducts(new QueryProductsCommand
                {
                    PageNumber = currentPage,
                    PageSize = size
                });

                return View(new ProductViewModel
                {
                    Products = products.Payload,
                    TotalCount = products.Count ?? 0,
                    PageNumber = currentPage,
                    PageSize = size
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View("Error");
            }
        }

        public async Task<ActionResult> Payments(int currentPage = 1, int size = 10)
        {
            try
            {
                var payments = await _templateBusinessService.GetPayments(currentPage, size);
                return View(new PaymentViewModel
                {
                    Payments = payments.Payload,
                    TotalCount = payments.Count ?? 0,
                    PageNumber = currentPage,
                    PageSize = size
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return View("Error");
            }
        }

        //public async Task<FileResult> DownloadProducts(bool isErrorTemplate = false)
        //{
        //    byte[] fileBytes = await _templateBusinessService.ExportProductTemplate(Server.MapPath("~/Templates/Product Details.xlsx"), isErrorTemplate);
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Product Details.xlsx");
        //}


        //[Authorize]
        [AllowAnonymous]
        public async Task<FileResult> DownloadTemplate(string name, bool isErrorTemplate = false)
        {
            byte[] fileBytes = null;
            string filename = string.Empty;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    switch (name)
                    {
                        case "Vendor Master":
                            filename = "Vendor Master.xlsx";
                            fileBytes = await _templateBusinessService.ExportVendorTemplate(Server.MapPath("~/Templates/Vendor Master.xlsx"), isErrorTemplate);
                            break;
                        case "PRN Master":
                            filename = "Purchase Dump.xlsx";
                            fileBytes = await _templateBusinessService.ExportPurchaseTemplate(Server.MapPath("~/Templates/Purchase Dump.xlsx"), isErrorTemplate);
                            break;
                        case "Product details":
                            filename = "Product Details.xlsx";
                            fileBytes = await _templateBusinessService.ExportProductTemplate(Server.MapPath("~/Templates/Product Details.xlsx"), isErrorTemplate);
                            break;
                        case "Price Master":
                            filename = "Price Master.xlsx";
                            fileBytes = await _templateBusinessService.ExportPriceTemplate(Server.MapPath("~/Templates/Price Master.xlsx"), isErrorTemplate);
                            break;

                        case "Payment status":
                            filename = "Payment status.xlsx";
                            fileBytes = await _templateBusinessService.ExportPaymentTemplate(Server.MapPath("~/Templates/Payment Status.xlsx"), isErrorTemplate);
                            break;

                        case "GuestUser":
                            filename = "Guest Details.xlsx";
                            fileBytes = await _templateBusinessService.ExportGuestUser(null, null);
                            break;

                        default: break;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Farmpik DownloadTemplate Method:downloadTemplate", ex, this);
            }
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, (isErrorTemplate ? "Error " : "") + filename);

        }

        public async Task<FileResult> DownloadPrices(bool isErrorTemplate = false)
        {
            byte[] fileBytes = await _templateBusinessService.ExportPriceTemplate(Server.MapPath("~/Templates/Price Master.xlsx"), isErrorTemplate);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "Price Master.xlsx");
        }
    }
}