using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskMulth4
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start");

			CreateThread(10);

			Console.WriteLine("End");
			Console.ReadLine();
		}

		static void CreateThread(int n)
		{
			if (n > 0)
			{
				Thread thread = new Thread((state) => {

					Console.WriteLine($"Print state {state}");
					int newState = (int)state;

					CreateThread(--newState);
				});

				thread.Start(n);
				thread.Join();

			}
		}
	}
}
