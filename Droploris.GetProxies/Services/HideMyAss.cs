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
using AngleSharp;
using AngleSharp.Parser.Html;
using AngleSharp.Dom;
using System.Text.RegularExpressions;

namespace Droploris.GetProxies.Services
{
	public class HideMyAss : Service
	{
		public override string GetServiceName()
		{
			return "HideMyAss";
		}
		public override string[] GetProxies(int page = 1) //HideMyAss it the most cancerous piece of shit to work with, sorry for sphagetti code, will fix
		{
			Uri url = new Uri($"http://proxylist.hidemyass.com/{page}");
			string data = Request.GetHTML(url);

			var parser = new HtmlParser();
			var doc = parser.Parse(data);

			var cells = doc.QuerySelectorAll("tr td:nth-child(2), tr td:nth-child(3)");
			string badClasses = GenerateClassFinder(GetBadClasses(cells));

			foreach (var l in cells)
			{
				foreach (var x in l.QuerySelectorAll(badClasses))
				{
					x.Remove();
				}
			}

			bool odd = false;
			var r = cells.Select(m => m.TextContent.EatWhitespaces());
			List<string> proxies = new List<string>();
			string t = string.Empty;
			foreach (var c in r) // because i have to fucking add the proxy port
			{
				if (odd)
				{
					t += ":"+c;
					proxies.Add(t);
				}
				else
				{
					t = c;
				}
				odd = !odd;
			}

			return proxies.ToArray();
		}

		private string GenerateClassFinder(string[] allowedClasses)
		{
			StringBuilder sb = new StringBuilder();
			foreach (var cl in allowedClasses)
			{
				sb.Append("span > .");
				sb.Append(cl);
				sb.Append(", ");
			}
			sb.Append("span > *[style=\"display:none\"], span > style");

			return sb.ToString();
		}

		public string[] GetBadClasses(IHtmlCollection<IElement> el)
		{
			List<string> badClasses = new List<string>();
			foreach (var e in el)
			{
				var selector = e.QuerySelector("style");
				if (selector != null)
				{
					var stylesheet = e.QuerySelector("style").TextContent;

					Regex regex = new Regex(@"(?:\.([A-Za-z0-9\-_]+){(display:none)})", RegexOptions.IgnoreCase | RegexOptions.Multiline);
					var matches = regex.Matches(stylesheet);

					foreach (Match m in matches)
					{
						//Console.WriteLine($"[CLASS {m.Groups[1]}] IS [{m.Groups[2]}]");
						badClasses.Add(m.Groups[1].ToString());
					}
				}
			}
			return badClasses.ToArray();
		}

	}
}
