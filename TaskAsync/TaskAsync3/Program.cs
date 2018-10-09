using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAsync3
{
	class Program
	{
		static void Main(string[] args)
		{
			var backet = new List<Product>();

			var products = CreateListOfProducts();
			int productId;

			do
			{
				ShowProducts(products);

				Console.WriteLine(" Choose good and press 'Enter'.");

				if (int.TryParse(Console.ReadLine(), out productId))
				{
					backet.Add(products.FirstOrDefault(p => p.Id == productId));

					var task = RecountPriceAndShowAsync(backet);
				}
				else
				{
					Console.WriteLine("Bad product ID");
				}
				
				Console.WriteLine("Enter 'ex' to exit");
 
			} while (string.Compare(Console.ReadLine(), "ex", true) != 0);

			Console.ReadLine();

		}

		private static void ShowAllPrice(IList<Product> backet)
		{
			//Thread.Sleep(5000); // work
			decimal resultPrice = 0;
			foreach (var item in backet)
			{
				resultPrice += item.Price;
			}
			Console.WriteLine($"ALL PRICE: {resultPrice}");
		}

		private static void ShowProducts(IList<Product> products)
		{
			Console.WriteLine("ID \t| Product Name \t| Price ");
			Console.WriteLine("---------------------------------");
			foreach (var item in products)
			{
				Console.WriteLine($"{item.Id} \t| {item.Title} \t| {item.Price}\t");
			}
			Console.WriteLine("---------------------------------");
		}

		public static IList<Product> CreateListOfProducts()
		{
			return new List<Product>()
			{
				new Product() {Id = 1, Price = 100.9m, Title = "Bed\t"},
				new Product() {Id = 2, Price = 300m, Title = "TV\t"},
				new Product() {Id = 3, Price = 12500.4m, Title = "Car\t"},
				new Product() {Id = 4, Price = 379.9m, Title = "Bicycle"},
				new Product() {Id = 5, Price = 200m, Title = "Microwave"},
				new Product() {Id = 6, Price = 490.2m, Title = "Phone"},
				new Product() {Id = 7, Price = 700m, Title = "Fridge"},
			};
		}

		public static async Task RecountPriceAndShowAsync(IList<Product> backet)
		{
			await Task.Run(() => ShowAllPrice(backet));

		}

	}
}
