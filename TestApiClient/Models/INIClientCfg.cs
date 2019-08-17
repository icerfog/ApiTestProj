using ProjectProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApiClient
{
	public class INIClientCfg : ClientProps
	{
		public INIClientCfg(string fname)
		{
			cfgfile = new IniFile(fname);
			EnsureProps();
		}

		private IniFile cfgfile;
		private bool onInit = true;

		protected override void cfgread()
		{
			if (cfgfile.KeyExists("Url", "ServerSettings"))
				ServerUrl = new Uri(cfgfile.ReadINI("ServerSettings", "Url"));
			else
				ServerUrl = new Uri("http://localhost:54081/");

			if (cfgfile.KeyExists("ShowLogs", "ClientSettings"))
				Showlogs = bool.Parse(cfgfile.ReadINI("ClientSettings", "ShowLogs"));
			else
				Showlogs = true;
			onInit = false;
		}

		protected override void cfgwrite()
		{
			if (onInit)
				return;
			cfgfile.Write("ServerSettings", "Url", ServerUrl.AbsoluteUri);
			cfgfile.Write("ClientSettings", "ShowLogs", Showlogs.ToString());
		}
	}
}
