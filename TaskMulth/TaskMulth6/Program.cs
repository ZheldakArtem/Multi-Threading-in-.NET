using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskMulth6
{

	class SynchronizedList<T>
	{
		private static ReaderWriterLockSlim listLock = new ReaderWriterLockSlim();
		private static object obj  = new object();
		private IList<T> innerList = new List<T>();
		private int prevCount;

		public int MaxSize { get;}
		

		public int Count
		{
			get {
				return innerList.Count;
			}
		}

		public SynchronizedList(int maxSize)
		{
			this.prevCount = this.innerList.Count;
			this.MaxSize = maxSize;
		}

		public void Add(T item)
		{
			//lock (obj)
			//{
			//	innerList.Add(item);
			//}

			listLock.EnterWriteLock();
			try
			{
				innerList.Add(item);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				listLock.ExitWriteLock();
			}
		}

		public void Show()
		{
			// The same behavior, but ReaderWriterLockSlim allows a range of threads read the collection in the same time
			//lock (obj)
			//{
			//	if (this.prevCount != this.Count)
			//	{
			//		foreach (var item in innerList)
			//		{
			//			//Thread.Sleep(10);
			//			Console.Write(item + " ");
			//		}
			//		Console.WriteLine();
			//	}
			//	this.prevCount = this.Count;
			//}

			listLock.EnterReadLock();
			try
			{
				if (this.prevCount != this.Count)
				{
					foreach (var item in innerList)
					{
						//Thread.Sleep(10);
						Console.Write(item + " ");
					}
					Console.WriteLine();
				}
				this.prevCount = this.Count;

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				listLock.ExitReadLock();
			}
		}

		~SynchronizedList()
		{
			listLock.Dispose();
		}

	}

	class Program
	{
		private const int N = 20;

		static void Main(string[] args)
		{
			var syncList = new SynchronizedList<int>(N);
			var random = new Random();
			
			var task1 = Task.Run(() =>
			{
				for (int i = 0; i < N; i++)
				{
					Thread.Sleep(1);//add element every 100mc
					syncList.Add(random.Next(100));
				}
			});

			var task2 = Task.Run(() =>
			{
				int currentCount = 0;

				while (syncList.Count <= syncList.MaxSize)
				{
					syncList.Show();
				}

			});

			Task.WaitAll(task1, task2);

			Console.ReadLine();
		}
	}
}
