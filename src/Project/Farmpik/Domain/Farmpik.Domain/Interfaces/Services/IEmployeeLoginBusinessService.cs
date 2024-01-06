/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Commands.EmployeeCommands;
using Farmpik.Domain.Dto;
using System;
using System.Threading.Tasks;

namespace Farmpik.Domain.Interfaces.Services
{
    public interface IEmployeeLoginBusinessService
    {
        Task<EmployeeDto> GetEmployee(EmployeeLoginCommand command);
        Task<EmployeeDto> GetEmployee(Guid id);
    }
}