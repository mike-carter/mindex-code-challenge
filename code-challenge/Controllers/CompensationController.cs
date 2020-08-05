using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/employee")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        private readonly IEmployeeService _employeeService;

        public CompensationController(ILogger<EmployeeController> logger, ICompensationService compensationService, IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationService = compensationService;
            _employeeService = employeeService;
        }

        [HttpPost("{id}/compensation", Name = "createCompensation")]
        public IActionResult CreateCompensation(string id, [FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for employee '{compensation}'");

            compensation.EmployeeId = id;

            if (_compensationService.Create(compensation) == null)
            {
                // Most likely error is that the client tried to create an already existing compensation
                return BadRequest();
            }

            return CreatedAtRoute("getCompensationByEmployeeId", new { id = compensation.EmployeeId }, compensation);
        }

        [HttpGet("{id}/compensation", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(string id)
        {
            _logger.LogDebug($"Received compensation get request for employee '{id}'");

            var compensation = _compensationService.GetByEmployeeId(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
