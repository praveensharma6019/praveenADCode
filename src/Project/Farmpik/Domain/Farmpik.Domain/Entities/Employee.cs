/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using System;

namespace Farmpik.Domain.Entities
{
    public class Employee : AuditEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailId { get; set; }

        public string Password { get; set; }

        public int Attempted { get; set; }
        public DateTime? AttemptedOn { get; set; }
    }
}