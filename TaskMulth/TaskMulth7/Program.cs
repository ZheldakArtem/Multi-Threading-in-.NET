using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskMulth7
{
	class Program
	{
		static void Main(string[] args)
		{
			#region a. Continuation task should be executed regardless of the result of the parent task.
			//var taskA = Task.Factory.StartNew(() =>
			//{
			//	Console.WriteLine("Parent taskA executing.");
			//	throw new Exception("Parent throw the exception");
			//}).ContinueWith((task) =>
			//{
			//	Console.WriteLine("Continuation taskA");
			//});

			//taskA.Wait();
			#endregion


			#region b. Continuation task should be executed when the parent task finished without success.
			//var taskB = Task.Factory.StartNew(() =>
			//{
			//	Console.WriteLine("Parent taskB executing.");
			//	throw new Exception("Parent throw the exception");

			//}).ContinueWith((task) => {
			//	if (task.Status == TaskStatus.Faulted || task.Status == TaskStatus.Canceled)
			//	{
			//		Console.WriteLine("Continuation taskB");
			//	}
			//});

			//taskB.Wait();
			#endregion


			#region Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.  не совсем понял как именно реюзнуть родительский таск
			//var taskC = Task.Factory.StartNew(() =>
			//{
			//	Console.WriteLine("Parent taskC executing.");
			//	throw new Exception("Parent throw the exception");
			//}).ContinueWith((task) => {
			//	if (task.IsFaulted)
			//	{
			//		task.ContinueWith((t)=> {
			//			Console.WriteLine("Reuse parent task");
			//		});
			//	}
			//});

			//taskC.Wait();

			#endregion


			#region d. Continuation task should be executed outside of the thread pool when the parent task would be cancelled
			var tokenSource = new CancellationTokenSource();
			CancellationToken ct = tokenSource.Token;

			var taskD = Task.Factory.StartNew(() =>
			{
				Console.WriteLine("Parent taskD executing.");

				// if already requested
				ct.ThrowIfCancellationRequested();

				bool moreToDo = true;
				while (moreToDo)
				{
					Console.WriteLine("wefwf");
					if (ct.IsCancellationRequested)
					{
						ct.ThrowIfCancellationRequested();
					}
				}

			}, tokenSource.Token);

			tokenSource.Cancel();

			Thread thread = new Thread(() =>
			{
				taskD.ContinueWith((t) =>
				{
					if (t.IsCompleted)
					{
						Console.WriteLine("Continuation taskD outside of thread pool");
					}
				});
			});

			thread.Start();
			thread.Join();

			try
			{
				taskD.Wait();
			}
			catch (AggregateException e)
			{
				foreach (var v in e.InnerExceptions)
					Console.WriteLine(e.Message + " " + v.Message);
			}
			finally
			{
				tokenSource.Dispose();
			}
			#endregion

			Console.ReadLine();
		}

	}
}
