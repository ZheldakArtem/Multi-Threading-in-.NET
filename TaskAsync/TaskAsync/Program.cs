using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAsync
{
	class Program
	{
		static void Main(string[] args)
		{
			string reset = string.Empty;
			Task<long> summ = null;
			
			do
			{
				Console.WriteLine("Enter the number from 1 to 1000000000");
				long number = 0;

				if (long.TryParse(Console.ReadLine(), out number))
				{
					summ = CalculateSummAsync(number);
				}
				else
				{
					Console.WriteLine("Not a number");
				}

				Console.WriteLine("You can enter 'Y' if you whant to reset, otherwise press enter and see the result");
				reset = Console.ReadLine();

			} while (string.Compare(reset, "Y", true) == 0);

			summ.Wait(); //обязательно ли нужно вызывать wait()??? идентично работает и без этого вызова

			if (summ != null && summ.Result > 0)
			{
				Console.WriteLine($"Summ is {summ.Result}");
			}
			else
			{
				Console.WriteLine("Something was wrong");
			}

			Console.ReadLine(); 
		}

		private static async Task<long> CalculateSummAsync(long number)
		{
			long result = 0;

			try
			{
				result = await GetSummAsync(number);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return -1;
			}

			return result;
			
		}

		private static Task<long> GetSummAsync(long number)
		{
			var task = Task.Run(() =>
			{
				long result = 0;

				try
				{
					checked
					{
						for (int i = 0; i <= number; i++)
						{
							// Thread.Sleep(100);
							result += i;
						}
					}
				}
				catch (Exception e)
				{
					throw e;
				}
				return result;
			});

			return task;
		}
	}
}
