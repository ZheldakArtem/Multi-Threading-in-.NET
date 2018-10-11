using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Complex_Task_Client
{
	class Program
	{
		// адрес и порт сервера, к которому будем подключаться
		static int port = 8005; // порт сервера
		static string address = "127.0.0.1"; // адрес сервера

		static List<string> messages = new List<string>()
		{
			"Message - 1;", "Message - 2;",
			"Message - 3;", "Message - 4;",
			"Message - 5;", "Message - 6;",
			"Message - 7;", "Message - 8;",
			"Message - 9;", "Message - 10;"
		};

		static void Main(string[] args)
		{
			IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
			string command = string.Empty;
			Random random = new Random();
			CancellationTokenSource cts;
			Socket socket;

			do
			{
				cts = new CancellationTokenSource();
				socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				socket.Connect(ipPoint);

				try
				{
					// send message to the server
					SendMessageToServer(socket, random, cts);

					// retrieve data from server and print to the console
					RetrieveDataFromServer(socket, random, cts);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}

				Console.WriteLine("Close client? Y/N");

				command = Console.ReadLine();

				if (command == "N")
				{
					cts.Cancel();
					cts.Dispose();
				}

			} while (string.Compare(command, "Y", true) != 0);

			socket.Send(Encoding.Unicode.GetBytes("Disconnect"));
			socket.Shutdown(SocketShutdown.Both);
			socket.Close();

			Console.Read();
		}

		private static void RetrieveDataFromServer(Socket socket, Random random, CancellationTokenSource cts)
		{
			ThreadPool.QueueUserWorkItem((obj) =>
			{
				byte[] data = new byte[256];
				StringBuilder builder = new StringBuilder();
				int bytes = 0;
				CancellationToken token = (CancellationToken)obj;

				Console.WriteLine("Waiting messages from other clients");
				while (true)
				{
					try
					{
						if (token.IsCancellationRequested)
						{
							socket.Send(Encoding.Unicode.GetBytes("Disconnect"));
							socket.Shutdown(SocketShutdown.Both);
							socket.Close();
							break;
						}
						if (socket.Available > 0)
						{
							do
							{
								bytes = socket.Receive(data, data.Length, 0);
								builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
							}
							while (socket.Available > 0);
							if (builder.Length > 0)
							{
								Console.WriteLine("Response from the server:\n" + builder.ToString());
							}
							builder.Clear();
						}
					}
					catch (Exception)
					{
						break;
					}
				}
			}, cts.Token);
		}

		private static void SendMessageToServer(Socket socket, Random random, CancellationTokenSource cts)
		{
			ThreadPool.QueueUserWorkItem((obj) =>
			{
				string message = string.Empty;
				byte[] dataSend;
				int messageNumber;
				CancellationToken token = (CancellationToken)obj;

				for (int i = 0; i < random.Next(1, 10); i++)
				{
					Thread.Sleep(random.Next(1000, 3000));
					if (token.IsCancellationRequested)
					{
						break;
					}

					messageNumber = random.Next(0, messages.Count);
					message = messages[messageNumber];
					dataSend = Encoding.Unicode.GetBytes(message);
					socket.Send(dataSend);
				}
			}, cts.Token);
		}
	}
}
