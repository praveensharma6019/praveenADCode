/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using AutoMapper;
using Farmpik.Domain.Commands.EmployeeCommands;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Domain.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Farmpik.Services.BackendServices
{
    public class EmployeeLoginBusinessService : IEmployeeLoginBusinessService
    {
        private readonly IMapper _mapper;
        private readonly IHelperMethod _helper;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeLoginBusinessService(IMapper mapper,IHelperMethod helper, IEmployeeRepository employeeRepository)
        {
            _mapper = mapper;
            _helper = helper;
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto> GetEmployee(EmployeeLoginCommand command)
        {
            command.Password = _helper.Encrypt(command.Password);
            var employee = await _employeeRepository.GetAsync(x => x.EmailId == command.EmailId && x.IsActive);

            if (employee != null)
            {
                bool isValid = employee.Password == command.Password
               && (employee.Attempted < 10 || !employee.AttemptedOn.HasValue || employee.AttemptedOn.Value.AddHours(6) < DateTime.Now);
                employee.Attempted = isValid ? 0 : employee.Attempted + 1;
                employee.AttemptedOn = DateTime.Now;
                await _employeeRepository.UpdateAsync(employee);
            }
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> GetEmployee(Guid id)
        {
            return _mapper.Map<EmployeeDto>(await _employeeRepository.GetAsync(id));
        }
    }
}