using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMulth3
{
	class Program
	{
		static void Main(string[] args)
		{
			string command;
			do
			{
				const int colCount = 6;
				const int rowCount = 6;

				var r = new Random();
				double[,] m1 = InitializeMatrix(rowCount, colCount, r);
				double[,] m2 = InitializeMatrix(rowCount, colCount, r);
				var result = new double[rowCount, colCount];

				MultiplyMatricesParallel(m1, m2, result);

				ShowMatrix(m1);
				Console.WriteLine(); Console.WriteLine();
				ShowMatrix(m2);
				Console.WriteLine(); Console.WriteLine();
				ShowMatrix(result);
				Console.WriteLine(); Console.WriteLine();

				command = Console.ReadLine();

			} while (command != "exit");
		}

		private static void ShowMatrix(double[,] m1)
		{
			var row = m1.GetLength(0);
			var col = m1.GetLength(1);

			for (int i = 0; i < row; i++)
			{
				for (int j = 0; j < col; j++)
				{
					Console.Write($"{m1[i,j]} ");
				}
				Console.WriteLine();
			}
		}

		private static void MultiplyMatricesParallel(double[,] mA, double[,] mB, double[,] result)
		{
			var matACols = mA.GetLength(1);
			var matBCols = mB.GetLength(1);
			var matARows = mA.GetLength(0);

			Parallel.For(0, matARows, i =>
			{
				for (var j = 0; j < matBCols; j++)
				{
					double temp = 0;
					for (var k = 0; k < matACols; k++)
					{
						temp += mA[i, k] * mB[k, j];
					}
					result[i, j] = temp;
				}
			});
		}

		private static double[,] InitializeMatrix(int rowCount, int colCount, Random r)
		{
			var matrix = new double[rowCount, colCount];

			for (var i = 0; i < rowCount; i++)
			{
				for (var j = 0; j < colCount; j++)
				{
					matrix[i, j] = r.Next(100);
				}
			}
			return matrix;
		}
	}
}
