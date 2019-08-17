using ProjectProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApiWebApp.Models.Configs
{
	public class INIServerCfg : ServerProps
	{
		public INIServerCfg(string fname)
		{
			cfgfile = new IniFile(fname);
			EnsureProps();
		}

		private IniFile cfgfile;
		private bool onInit = true;

		protected override void cfgread()
		{
			if (cfgfile.KeyExists("ConnectionString", "ServerSettings"))
				ConnectionString = cfgfile.ReadINI("ServerSettings", "ConnectionString");
			else
				ConnectionString = "server=(localdb)\\mssqllocaldb;database=collegedb;Integrated Security=True;";
			onInit = false;
		}

		protected override void cfgwrite()
		{
			if (onInit)
				return;

			cfgfile.Write("ServerSettings", "ConnectionString", ConnectionString);
		}
	}
}
