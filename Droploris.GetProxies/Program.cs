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
			Console.WriteLine("Proxy Collector made by Droploris");
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

			Type[] serviceRotation = new Type[] //Service rotation, switch to next service whenever end of proxy list is reached.
			{
				typeof(HideMyAss),
				typeof(Incloak),
				typeof(FreeProxyList)

			};

			int serviceIndex = 0;


			string file = $"{new Random().Next(0, 30000)}_proxies.txt";
			int totalProxies = 0;

			using (StreamWriter e = new StreamWriter(new FileStream(file + ".tmp", FileMode.Append)))
			{
				int page = 1;
				for (int i = 1; i <= no; i++)
				{
					Service service = null;

					if (serviceRotation.Length > serviceIndex && serviceRotation[serviceIndex] != null)
					{
						service = (Service)Activator.CreateInstance(serviceRotation[serviceIndex]);


						string[] proxies = service.GetProxies(page++);
						totalProxies += proxies.Length;

						if (proxies.Length == 0)
						{
							Console.WriteLine($"[{service.GetServiceName()}][{i}/{no}] End of Proxy list");
							serviceIndex++;
							Console.WriteLine($"Switching to next Provider");
						}
						else
						{
							foreach (var p in proxies)
							{
								e.WriteLine(p);
							}

							Console.WriteLine($"[{service.GetServiceName()}][{i}/{no}] Caught {proxies.Length} Proxies");
						}

					}
					else
					{
						break;
					}
				}
			}

			File.WriteAllLines(file, File.ReadAllLines(file + ".tmp").Distinct().ToArray());
			File.Delete(file + ".tmp");
			int actualCount = File.ReadLines(file).Count();
			Console.WriteLine($"Proxies written to {file}. Total proxy count is {actualCount}\n(From {totalProxies}, {totalProxies - actualCount} ({Math.Round(100 - ((double)actualCount / totalProxies) * 100)}%) duplicates were removed)");
			Console.ReadLine();


		}
	}
}
