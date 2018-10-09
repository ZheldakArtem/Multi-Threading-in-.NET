using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskAsync2
{
	class Program
	{
		static void Main(string[] args)
		{
			string command = string.Empty;
			Task<byte[]>[] taskArr;
			WebClient[] wc;

			do
			{
				Console.WriteLine("Enter one or more address. Use ';' as a separator");
				string temp = "https://www.tut.by/;https://www.onliner.by/";
				string[] urls = temp.Split(';');//Console.ReadLine().Split(';')   uncomment to write url manually

				wc = new WebClient[urls.Length];
				taskArr = new Task<byte[]>[urls.Length];

				for (int i = 0; i < urls.Length; i++)
				{
					wc[i] = new WebClient();
					taskArr[i] = DownloadDataAsync(urls[i], wc[i]);
				}

				Console.WriteLine("1. Enter '1' to exit;");
				Console.WriteLine("2. Enter '2' to prevent loading and try again;");
				Console.WriteLine("3. Enter '3' to get result;");

				command = Console.ReadLine();

				switch (command)
				{
					case "1":
					case "3":
						break;
					case "2":
						foreach (var wClient in wc)
						{
							wClient.CancelAsync();
						}
						continue;
				}

			} while (command == "2");

			if (command == "3")
			{
				Task.WaitAll(taskArr);
				foreach (var task in taskArr)
				{
					var result = Encoding.UTF8.GetString(task.Result);
					Console.WriteLine(result);
				}
			}

			Console.ReadLine();
		}

		public static async Task<byte[]> DownloadDataAsync(string url, WebClient wClient)
		{
			byte[] result = await wClient.DownloadDataTaskAsync(url);

			return result;

		}
	}

}
