/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Farmpik.Domain.Dto
{
    public class ProductDetailsDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Imagelink { get; set; }

        public List<string> AvailableAt
        {
            get
            {
                var list = new List<string>();
                if (IsAvailableInRampur) list.Add("Rampur");
                if (IsAvailableInRohru) list.Add("Rohru");
                if (IsAvailableInSainj) list.Add("Sainj");
                if (IsAvailableInOddi) list.Add("Oddi");
                return list;
            }
        }

        [JsonIgnore]
        public bool IsAvailableInRampur { get; set; }

        [JsonIgnore]
        public bool IsAvailableInRohru { get; set; }

        [JsonIgnore]
        public bool IsAvailableInSainj { get; set; }

        [JsonIgnore]
        public bool IsAvailableInOddi { get; set; }
    }
}