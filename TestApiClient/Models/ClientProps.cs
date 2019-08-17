using System;

namespace TestApiClient
{
	public abstract class ClientProps
	{
		private static Uri serverUrl;
		private static bool showlogs;

		public Uri ServerUrl
		{
			get { return serverUrl; }
			set { serverUrl = value; cfgwrite(); }
		}
		public bool Showlogs
		{
			get { return showlogs; }
			set { showlogs = value; cfgwrite(); }
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
