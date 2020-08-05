using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class ReportingStructure
    {
        public ReportingStructure() { }

        public ReportingStructure(Employee employee)
        {
            Employee = employee;
            NumberOfReports = CalculateNumberOfReports(employee);
        }

        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }

        private int CalculateNumberOfReports(Employee employee)
        {
            int numReports = 0;

            if (employee?.DirectReports != null) // Base Case
            {
                foreach (Employee e in employee.DirectReports)
                {
                    numReports += 1 + CalculateNumberOfReports(e);
                }
            }

            return numReports;
        }
    }
}
