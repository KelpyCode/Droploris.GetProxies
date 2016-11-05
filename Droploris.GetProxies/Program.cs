/*
 ______   ______  _____   _____          _____   ______ _____ _______
 |     \ |_____/ |     | |_____] |      |     | |_____/   |   |______
 |_____/ |    \_ |_____| |       |_____ |_____| |    \_ __|__ ______|
					Making amazing tools since 2016

							 @Droploris
							 XaoticLabs
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Droploris.GetProxies.Services;
using System.IO;
using System.Threading;

namespace Droploris.GetProxies
{
	class Program
	{
		static void Main(string[] args)
		{

			Console.WriteLine("How many pages to scan proxies for?");

			var s = Console.ReadLine();
			int no = 0;
			try
			{
				no = Convert.ToInt32(s);
				if (no <= 0)
					throw new Exception("Number must be higher than 0");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error: " + e.Message);
				Console.ReadKey();
				Environment.Exit(0);
			}

			Console.WriteLine("Okay, just give me a second..");

			UsingService us = UsingService.HMA;

			string file = $"{new Random().Next(0, 30000)}_proxies.txt";
			int totalProxies = 0;
			using (StreamWriter e = new StreamWriter(new FileStream(file + ".tmp", FileMode.Append)))
			{
				int page = 1;
				for (int i = 1; i <= no; i++)
				{
					Service service;
					switch (us)
					{
						case (UsingService.HMA):
							{
								service = new HideMyAss();
								break;
							}

						case (UsingService.Incloak):
							{
								service = new Incloak();
								break;
							}
						case (UsingService.FreeProxyList):
							{
								service = new FreeProxyList();
								break;
							}

						default:
						case (UsingService.End):
							{
								service = null;
								break;
							}
					}

					if (us != UsingService.End)
					{
						List<string> proxies = service.GetProxies(page++).ToList();
						totalProxies += proxies.Count;
						if (proxies.Count == 0)
						{
							Console.WriteLine($"[{service.GetServiceName()}][{i}/{no}] End of Proxy list");
							switch (us)
							{
								case (UsingService.HMA):
									{
										us = UsingService.Incloak;
										page = 1;
										break;
									}
								case (UsingService.Incloak):
									{
										us = UsingService.FreeProxyList;
										page = 1;
										break;
									}

								default:
								case (UsingService.FreeProxyList):
									{
										us = UsingService.End;
										break;
									}
							}
							Console.WriteLine($"Switching to Mode {us.ToString()}");
						}
						else
						{

							foreach (var p in proxies)
							{
								e.WriteLine(p);
							}

							Console.WriteLine($"[{service.GetServiceName()}][{i}/{no}] Caught {proxies.Count} Proxies");
						}
					}
				}
			}

			File.WriteAllLines(file, File.ReadAllLines(file + ".tmp").Distinct().ToArray());
			File.Delete(file + ".tmp");
			int actualCount = File.ReadLines(file).Count();
			Console.WriteLine($"Proxies written to {file}. Total proxy count is {actualCount}\n(From {totalProxies}, {totalProxies - actualCount} ({Math.Round(((double)actualCount / totalProxies) * 100)}%) duplicates were removed)");
			Console.ReadLine();


		}
	}
}
