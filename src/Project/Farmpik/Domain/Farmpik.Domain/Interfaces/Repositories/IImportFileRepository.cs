/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Dto;
using System.Threading.Tasks;
using System.Web;

namespace Farmpik.Domain.Interfaces.Repositories
{
    public interface IImportFileRepository
    {
        Task<TemplateDto<T>> GetDataFromTemplate<T>(HttpPostedFileBase formFile, string[] columnNames, int startRow,
            int startCol);
    }
}