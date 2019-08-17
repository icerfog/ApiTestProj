using System;

namespace TestApiWebApp.Models.Configs
{
	public abstract class ServerProps
	{
		private static string connectionString;

		public string ConnectionString
		{
			get { return connectionString; }
			set { connectionString = value; cfgwrite(); }
		}

		public void EnsureProps()
		{
			cfgread();
			cfgwrite();
		}

		protected abstract void cfgread();
		protected abstract void cfgwrite();
	}
}
