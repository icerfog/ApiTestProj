using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using TestApiWebApp.Contracts;
using TestApiWebApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestApiWebApp.Controllers
{
	[Route("api/employee")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IDataRepository<Employee> _dataRepository;
		private Logger logger = LogManager.GetCurrentClassLogger();

		public EmployeeController(IDataRepository<Employee> dataRepository)
		{
			_dataRepository = dataRepository;
		}

		// GET: api/Employee
		[HttpGet]
		public IActionResult Get()
		{
			logger.Info("Getting request for employees");
			IEnumerable<Employee> employees = _dataRepository.GetAll();
			return Ok(employees);
		}

		// GET: api/Employee/5
		[HttpGet("{id}", Name = "Get")]
		public IActionResult Get(long id)
		{
			logger.Info("Getting request for employee " + id);
			Employee employee = _dataRepository.Get(id);

			if (employee == null)
			{
				logger.Info("Record not found.");
				return NotFound("The Employee record couldn't be found.");
			}

			return Ok(employee);
		}

		// POST: api/Employee
		[HttpPost]
		public IActionResult Post([FromBody] Employee employee)
		{
			logger.Info("Getting post request for new employee: "+ employee?.FirstName[0]+". "+ employee?.LastName);
			if (employee == null)
			{
				logger.Info("Parsing employee returns null.");
				return BadRequest("Employee is null.");
			}

			_dataRepository.Add(employee);
			var res = CreatedAtRoute(
				  "Get",
				  new { Id = employee.EmployeeId },
				  employee);

			logger.Info("Post request success. New record id: "+ employee.EmployeeId);
			return res;
		}

		// PUT: api/Employee/5
		[HttpPut("{id}")]
		public IActionResult Put(long id, [FromBody] Employee employee)
		{
			logger.Info("Getting put request for employee "+id);
			if (employee == null)
			{
				logger.Info("Parsing new employee data returns null.");
				return BadRequest("Employee is null.");
			}

			Employee employeeToUpdate = _dataRepository.Get(id);
			if (employeeToUpdate == null)
			{
				logger.Info("Record for updating not found.");
				return NotFound("The Employee record couldn't be found.");
			}

			_dataRepository.Update(employeeToUpdate, employee);
			logger.Info("Update request complete.");
			return NoContent();
		}

		// DELETE: api/Employee/5
		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			logger.Info("Getting delete request for employee "+ id);
			Employee employee = _dataRepository.Get(id);
			if (employee == null)
			{
				logger.Info("Record for deleting not found.");
				return NotFound("The Employee record couldn't be found.");
			}

			_dataRepository.Delete(employee);
			logger.Info("Delete request complete (id-"+id+").");
			return NoContent();
		}
	}
}
