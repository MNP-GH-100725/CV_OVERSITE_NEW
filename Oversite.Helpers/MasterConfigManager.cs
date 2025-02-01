using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Oversite.Helpers
{
	public class MasterConfigManager
	{
		
		public static string  leadCommonApiPath;
		public MasterConfigManager()
		{
			var configurationBuilder = new ConfigurationBuilder();
			var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
			leadCommonApiPath= Path.Combine(Directory.GetCurrentDirectory(), @"Master\leadCommonApi.json");
			var leadCommonApiconfigurationBuilder = new ConfigurationBuilder();
			configurationBuilder.AddJsonFile(leadCommonApiPath, false);
			leadCommonApiconfigurationBuilder.AddJsonFile(path, false);
			var root = configurationBuilder.Build();			
			var leadCommonApiroot = leadCommonApiconfigurationBuilder.Build();			
			
			

		}
		public string GetleadCommonApiPath
		{
			get => leadCommonApiPath;
		}


	}
}
