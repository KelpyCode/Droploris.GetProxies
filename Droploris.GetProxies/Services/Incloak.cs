/*
 ______   ______  _____   _____          _____   ______ _____ _______
 |     \ |_____/ |     | |_____] |      |     | |_____/   |   |______
 |_____/ |    \_ |_____| |       |_____ |_____| |    \_ __|__ ______|
					Making amazing tools since 2016

							 @Droploris
							 XaoticLabs
*/
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Droploris.GetProxies.Services
{
	public class Incloak : Service
	{
		public override string GetServiceName()
		{
			return "Incloak";
		}

		public override string[] GetProxies(int page = 1)
		{

			int offset = page - 1 * 64;

			if (page <= 20)
			{
				Uri url = new Uri($"https://incloak.com/proxy-list/?start={offset}");
				string data = Request.GetHTML(url);
				var parser = new HtmlParser();
				var doc = parser.Parse(data);

				var cells = doc.QuerySelectorAll("tbody tr");
				var proxies = new List<string>();

				foreach (IElement e in cells)
				{
					proxies.Add(e.QuerySelector("td:nth-child(1)").TextContent + ":" + e.QuerySelector("td:nth-child(2)").TextContent);
				}

				return proxies.ToArray();
			}
			else
			{
				return new string[] { };
			}
		}
	}
}
