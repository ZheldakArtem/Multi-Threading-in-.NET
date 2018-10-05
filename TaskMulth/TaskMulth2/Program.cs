using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMulth2
{
	class Program
	{

		private const int N = 10;
		static void Main(string[] args)
		{
			var task = new Task<int[]>(CreateIntArray);

		task.ContinueWith(tArray => MultiplieArr(tArray.Result))
				.ContinueWith(tArray => SortByAscending(tArray.Result))
					.ContinueWith(tArray => ColculateAverage(tArray.Result));

			task.Start();

			task.Wait(); // Поток ждет выполнения только первой задачи(т.е. выводится результат первой задачи, затем вывод 'End' и потом вывод всех остальных

			Console.WriteLine("End");

			Console.ReadLine();
		}

		static int[] CreateIntArray()
		{
			Console.WriteLine("------------SUMM----------------");
			int[] arr = new int[N];
			var random = new Random();
			for (int i = 0; i < N; i++)
			{
				arr[i] = random.Next(100);
				Console.WriteLine(arr[i]);
			}

			return arr;
		}

		static int[] MultiplieArr(int[] array)
		{
			Console.WriteLine("------------Multiplie--------------");
			Random random = new Random();
			int randomNumber;
			int temp;

			for (int i = 0; i < array.Length; i++)
			{
				temp = array[i];
				randomNumber = random.Next(100);
				array[i] *= randomNumber;

				Console.WriteLine($"{temp} * {randomNumber} = {array[i]}");
			}

			return array;
		}

		static int[] SortByAscending(int[] array)
		{
			Console.WriteLine("-----------SortByAscending-------------");
			Array.Sort(array);
			foreach (var item in array)
			{
				Console.WriteLine(item);
			}

			return array;
		}

		private static void ColculateAverage(int[] array)
		{
			Console.WriteLine("------------Average-------------");
			double average = array.Average();
			Console.WriteLine(average);
		}
	}
}
