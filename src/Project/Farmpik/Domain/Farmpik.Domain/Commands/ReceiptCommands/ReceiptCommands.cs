/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Common;
using System;

namespace Farmpik.Domain.Commands.ReceiptCommands
{
    public class QueryReceiptsCommand: BasePagination
    {
        public string Search { get; set; }

        public string CurrentVendorCode { get; set; }
    }
}
