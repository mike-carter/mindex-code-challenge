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
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _context;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext compensationContext)
        {
            _context = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            // Compensation is 1-1 with employee
            if (GetById(compensation.EmployeeId) == null)
            {
                _context.Compensations.Add(compensation);
                return compensation;
            }
            return null;
        }

        public Compensation GetById(string id)
        {
            return _context.Compensations.SingleOrDefault(c => c.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
