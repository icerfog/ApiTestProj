using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using TestApiClient;
using NLog;

namespace ConsoleClient
{
	class Program
	{
		private static TestApiController client;
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static INIClientCfg cfgFile = new INIClientCfg("config.ini");

		static void Main(string[] args)
		{
			client = new TestApiController(cfgFile);
			TestScript();
			Console.ReadKey();
		}

		private static void TestScript()
		{
			try
			{
				Console.WriteLine(GetRecords());
				//Console.WriteLine(NewRecord());
				Console.WriteLine(GetRecord(5));
				Console.Write("id? ");
				var id = long.Parse(Console.ReadLine());
				Delete(id);
				Console.WriteLine(GetRecords());
				Console.WriteLine(GetRecord(115));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.InnerException?.Message);
				logger.Info("Unexpected Action: " + e.InnerException?.Message ?? e.Message);
			}
		}

		private static string GetRecords()
		{
			var str = "";
			var s = client.GetAll().Result;
			foreach (var item in s)
			{
				str += item.FirstName + ' ' + item.LastName + "\r\n";
			}

			logger.Info("Getting list success");
			return str;
		}

		private static string GetRecord(long id)
		{
			var item = client.GetById(id).Result;
			logger.Info("Record id=" + id + " received");
			return item.FirstName + ' ' + item.LastName;
		}

		private static string NewRecord()
		{
			var item = new Employee();
			Console.Write("Input FirstName: ");
			item.FirstName = Console.ReadLine();
			Console.Write("Input LastName: ");
			item.LastName = Console.ReadLine();
			Console.Write("Input DateOfBirth: ");
			DateTime date;
			while (!DateTime.TryParse(Console.ReadLine(), out date))
			{
				Console.Write("Incorrect format. Try again: ");
			}
			item.DateOfBirth = date;
			Console.Write("Input PhoneNumber: ");
			item.PhoneNumber = Console.ReadLine();
			Console.Write("Input Email: ");
			item.Email = Console.ReadLine();
			var newId = client.Create(item).Result.EmployeeId;

			logger.Info("Record created. It's id=" + newId + " received");
			return "Employee record added. It's id: " + newId;
		}

		private static async void Update(long id)
		{
			var item = client.GetById(id).Result;
			var nitem = new Employee();
			Console.Write($"Input FirstName ({item.FirstName}): ");
			nitem.FirstName = Console.ReadLine();
			Console.Write($"Input LastName ({item.LastName}): ");
			nitem.LastName = Console.ReadLine();
			Console.Write($"Input DateOfBirth ({item.DateOfBirth.ToShortDateString()}): ");
			nitem.DateOfBirth = DateTime.Parse(Console.ReadLine());
			Console.Write($"Input PhoneNumber ({item.PhoneNumber}): ");
			nitem.PhoneNumber = Console.ReadLine();
			Console.Write($"Input Email ({item.Email}): ");
			nitem.Email = Console.ReadLine();

			//не знаю, как сделать по-другому, пока так.
			logger.Info("Record update. It's id=" + id + "\r\n Updates:\r\n" +
				(item.FirstName != nitem.FirstName ? "FirstName:" + item.FirstName + ">" + nitem.FirstName + "\r\n" : "") +
				(item.LastName != nitem.LastName ? "LastName:" + item.LastName + ">" + nitem.LastName + "\r\n" : "") +
				(item.DateOfBirth != nitem.DateOfBirth ? "DateOfBirth:" + item.DateOfBirth + ">" + nitem.DateOfBirth + "\r\n" : "") +
				(item.PhoneNumber != nitem.PhoneNumber ? "PhoneNumber:" + item.PhoneNumber + ">" + nitem.PhoneNumber + "\r\n" : "") +
				(item.Email != nitem.Email ? "Email:" + item.Email + ">" + nitem.Email + "\r\n" : "") 
				);
			await client.Update(id, nitem);
		}
		

		private static async void Delete(long id)
		{
			var item = await client.GetById(id);
			Console.Write($" R u realy want to remove record({item.FirstName} {item.LastName})? (y-yes): ");
			if (Console.ReadLine().ToLower() != "y")
			{
				logger.Info("Record " + id + " deleting abort");
				return;
			}
			await client.Delete(id);
			logger.Info("Record id=" + id + " EXTERMINATED");
		}
	}
}