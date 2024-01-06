/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

namespace Farmpik.Domain.Commands.EmployeeCommands
{
    public class EmployeeLoginCommand
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
    }
}