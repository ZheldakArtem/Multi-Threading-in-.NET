using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskMulth
{
	class Program
	{
		private const int N = 100;
		private const int Iterations = 1000;
		static void Main(string[] args)
		{
			Task[] taskArr = CreateTasks(N);

			foreach (var task in taskArr)
			{
				task.Start();
			}

			Task.WaitAll(taskArr);
		}

		private static Task[] CreateTasks(int n)
		{
			Task[] tasks = new Task[n];
			return Enumerable.Range(0, n).Select(i => new Task(Work, i)).ToArray();
		}

		private static void Work(object numberOfTask)
		{
			for (int i = 0; i < Iterations; i++)
			{
				Console.WriteLine($"Task #{numberOfTask} - {i}");
			}
		}
	}
}
