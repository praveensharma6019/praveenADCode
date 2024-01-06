/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System.Collections.Generic;

namespace Farmpik.Domain.Dto
{
    public class TemplateDto<T>
    {
        public bool? IsValidTemplate { get; set; }

        public string Name { get; set; }

        public List<T> Records { get; set; } = new List<T>();

        public bool HasErrorFields { get; set; } = false;

        public int TotalRecords { get; set; }

        public int ErrorRecords { get; set; }
    }
}