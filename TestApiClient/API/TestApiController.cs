using System;
using System.Collections.Generic;
using NLog;
using System.Text;
using System.Threading.Tasks;
using NLog.Config;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TestApiClient
{
	public class TestApiController : TestApiBase
	{
		public TestApiController(ClientProps props)
		{
			Initialize(props.ServerUrl);
			if (props.Showlogs)
			{
				ConsoleLogsON();
			}
		}

		private static Logger logger = LogManager.GetCurrentClassLogger();

		private static void ConsoleLogsON()
		{
			var logTarget = new NLog.Targets.ConsoleTarget("c");
			logTarget.Layout = "${longdate} ${callsite} ${level} ${message}";
			var loggingRule = new LoggingRule("*", LogLevel.Info, logTarget);
			LogManager.Configuration.AddTarget("ConsoleLog", logTarget);
			LogManager.Configuration.LoggingRules.Add(loggingRule);
			LogManager.ReconfigExistingLoggers();
			logger = LogManager.GetCurrentClassLogger();
		}

		public async Task<Employee> GetById(long id)
		{
			logger.Info("Sending request for employee " + id);
			return await GetRequest<Employee>("api/employee/" + id);
		}

		public async Task<IEnumerable<Employee>> GetAll()
		{
			logger.Info("Sending request for employees");
			return await GetRequest<IEnumerable<Employee>>("api/employee");
		}

		public async Task<Employee> Create(Employee entity)
		{
			logger.Info("Sending request for creating employee:\r\n" + entity.FirstName + " " + entity.LastName + " " + entity.DateOfBirth + "\r\n" + entity.Email + " " + entity.PhoneNumber);
			return await PostRequest("api/employee", entity);
		}

		public async Task Update(long id, Employee entity)
		{
			logger.Info("Sending request for updating employee " + id + ". New data:\r\n" + entity.FirstName + " " + entity.LastName + " " + entity.DateOfBirth + "\r\n" + entity.Email + " " + entity.PhoneNumber);
			await PutRequest("api/employee/" + id, entity);
		}

		public async Task Delete(long id)
		{
			logger.Info("Sending request for deleting employee " + id);
			await DeleteRequest("api/employee/" + id);
		}
	}
}
