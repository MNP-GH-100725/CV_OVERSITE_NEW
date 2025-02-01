
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.Helpers
{
	public class ErrorLog
	{
		string fileLoc = string.Empty;
		string directory = string.Empty;
		FileStream fs = null;
		public ErrorLog()
		{
			//directory = new AppConfigManager().getFolderPath; 
			bool exists = System.IO.Directory.Exists(directory);
			if (!exists)
			{
				System.IO.Directory.CreateDirectory(directory);
			}

			fileLoc = directory + "Log_" + DateTime.Now.ToString("yyyyMMddHH") + ".txt";

			if (!File.Exists(fileLoc))
			{
				fs = File.Create(fileLoc);
			}
			try
			{
				fs.Close();
			}
			catch (Exception ex)
			{
			}
		}

		public async Task writeLog(string msg)
		{
			try
			{
				using (fs = new FileStream(fileLoc, FileMode.Append, FileAccess.Write))
				{
					using (StreamWriter sw = new StreamWriter(fs))
					{
						await sw.WriteAsync(DateTime.Now + " [ " + msg + " ] \r\n");
					}
				}
			}
			catch (Exception ex)
			{

			}

		}
		public async Task writeLogNewLine()
		{

			try
			{
				using (fs = new FileStream(fileLoc, FileMode.Append, FileAccess.Write))
				{
					using (StreamWriter sw = new StreamWriter(fs))
					{
						await sw.WriteAsync("\r\n");
					}
				}
			}
			catch (Exception ex)
			{

			}
		}
	}


}
