/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.ReceiptCommands;
using Farmpik.Domain.Common;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Interfaces.Services;
using log4net;
using Sitecore.Farmpik.Api.Website.Filters;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sitecore.Farmpik.Api.Website.Controllers
{
    [ExcludeFromCodeCoverage]
    [RoutePrefix("v1/Receipt")]
    [CustomAuthorized]
    public class ReceiptController : BaseApiController
    {
        private readonly ILog _logger;
        private readonly IGoodsReceiptBusinessService _goodsReceiptBusinessService;

        public ReceiptController(IGoodsReceiptBusinessService goodsReceiptBusinessService)
        {
            _logger = LogManager.GetLogger(typeof(ReceiptController));
            _goodsReceiptBusinessService = goodsReceiptBusinessService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ApiResponse<GoodsReceiptDto>> Receipts([FromUri] string search ="", [FromUri] int pageNumber = 1, [FromUri] int pageSize = 4)
        {
            try
            {
                return await _goodsReceiptBusinessService.GetGoodsReceipts(new QueryReceiptsCommand
                {
                    Search = search,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    CurrentVendorCode = CurrentVendorCode
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return GetServerError<GoodsReceiptDto>();
            }
        }
    }
}