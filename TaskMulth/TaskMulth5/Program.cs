using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskMulth5
{
	class Program
	{
		private static Semaphore semaphore = new Semaphore(1, 1);
		private static int N = 1000;
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
				semaphore.WaitOne();

				ThreadPool.QueueUserWorkItem((state) => {
					Console.WriteLine($"Print state {state}");

					int newState = (int)state;

					CreateThread(--newState);
				}, n);

				semaphore.Release();
			}
		}
	}
}
