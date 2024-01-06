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
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Services
{
    public interface IGoodsReceiptBusinessService
    {
        Task<ApiResponse<GoodsReceiptDto>> GetGoodsReceipts(QueryReceiptsCommand command);
    }
}
