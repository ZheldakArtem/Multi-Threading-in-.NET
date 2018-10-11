using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Complex_Task
{
	class Program
	{
		private static int port = 8005;
		private static ConcurrentBag<string> historyOfMessages = new ConcurrentBag<string>();
		private static int N = 10;

		static void Main(string[] args)
		{

			IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

			Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			List<Socket> clienSocketList = new List<Socket>(); ;
			try
			{

				listenSocket.Bind(ipPoint);

				listenSocket.Listen(10);

				Console.WriteLine("Server waiting for connection...");
				while (true)
				{
					Socket handler = listenSocket.Accept();
					clienSocketList.Add(handler);

					if (historyOfMessages.Count > 0)
					{
						var lastTen = historyOfMessages.Count > N ? historyOfMessages.Skip(historyOfMessages.Count - N) : historyOfMessages;//historyOfMessages.Skip();

						handler.Send(Encoding.Unicode.GetBytes("History:\n"));
						foreach (var messageItem in lastTen)
						{
							handler.Send(Encoding.Unicode.GetBytes(messageItem + '\n'));
						}
					}

					ThreadPool.QueueUserWorkItem((state) =>
					{
						StringBuilder builder = new StringBuilder();
						int bytes = 0;
						byte[] data = new byte[256];
						Socket selfSocket = handler;

						while (true)
						{
							try
							{
								if (selfSocket.Available > 0)
								{
									do
									{
										bytes = selfSocket.Receive(data);
										builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
									} while (selfSocket.Available > 0);

									var temp = builder.ToString();
									if (builder.ToString() == "Disconnect")
									{
										clienSocketList.Remove(selfSocket);
										selfSocket.Shutdown(SocketShutdown.Both);
										selfSocket.Close();
										break;
									}
									historyOfMessages.Add(builder.ToString());

									Console.WriteLine("----------------------");
									Console.WriteLine(builder.ToString());
									Console.WriteLine("----------------------");

									foreach (var client in clienSocketList)
									{
										if (selfSocket != client)
										{
											client.Send(Encoding.Unicode.GetBytes(builder.ToString()));
										}
									}
									builder.Clear();
								}
							}
							catch (Exception)
							{
								break;
							}
						}
					});
				}
			}
			catch (Exception)
			{
				foreach (var socket in clienSocketList)
				{
					socket.Shutdown(SocketShutdown.Both);
					socket.Close();
				}
				listenSocket.Close();
			}
		}
	}
}
