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

		static void Main(string[] args)
		{
			int N = 15;
			NewMethod(N);

			Console.ReadLine();
		}

		public static void NewMethod(int n)
		{
			Console.WriteLine("Start");
			CreateThread(n);
		}

		public static void CreateThread(int n)
		{
			ThreadPool.QueueUserWorkItem((state) =>
			   {
				   Console.WriteLine($"Thread :  {Thread.CurrentThread.ManagedThreadId}");
				   if (n > 0)
				   {
						semaphore.WaitOne();

						Console.WriteLine($"Print state {state}");

						int newState = (int)state;

						CreateThread(--newState);
						semaphore.Release();
				   }
				   else
				   {
					   Console.WriteLine("End");
				   }
			   }, n);
		}
	}
}
