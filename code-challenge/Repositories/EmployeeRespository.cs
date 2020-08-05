using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            // So, this fixes the null DirectReports bug. I don't like it and it feels wrong.
            var employees = _employeeContext.Employees.ToArray();
            return employees.SingleOrDefault(e => e.EmployeeId == id);

            //return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
            // Ideally ^this^ should work, but the reason why it doesn't is beyond my understanding of 
            // how the in-memory database works.
            // If this was an actual database, it would probably be fixed via migrations.
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
