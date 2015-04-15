using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XssedCrawler {
#pragma warning disable 4014
	public class AsyncCrawler {

		static HashSet<string> history;
		static Stack<string> future;

		public AsyncCrawler() {
			history = new HashSet<string>();
			future = new Stack<string>(2500);
			Console.CancelKeyPress += new ConsoleCancelEventHandler(cancelEventHandler);
		}

		private void cancelEventHandler(object sender, ConsoleCancelEventArgs e) {
			FileManager.SaveUrlListToDisk(history);
		}

		/// <summary>
		/// Crawl through the web, starting at a given url. Will continue to crawl and output the console until the user kills it with Ctrl+C
		/// After the crawl is killed, all urls are written to a text file.
		/// </summary>
		/// <pparam name="start">The URL to start the crawl from</pparam>
		public void Crawl(string startUrl) {
			try {
				tryCrawl(startUrl);
			}
			catch (Exception) {
				FileManager.SaveUrlListToDisk(history);
			}
		}

		private void tryCrawl(string startUrl) {
			getUrlData(startUrl);
			while (future.Count < 1) { }
			List<string> data = new List<string>();

			while (future.Peek() != null) {
				getUrlData(future.Pop());
			}
		}

		async Task getUrlData(string url) {
			if (history.Contains(url)) return;

			history.Add(url);
			Console.WriteLine(url);

			string htmlPage = await asyncGetUrlData(url);
			var urls = WebPage.ParseWebPage(htmlPage, @"https?:\/\/(.*?)(?=[""\)'>< ])");
			
			foreach (var nextUrl in urls) {
				string newUrl = nextUrl.ToString();
				if (!newUrl.Contains('.')) continue; //removing local redirects
				if (newUrl.Contains("google") || newUrl.Contains("facebook")) continue; //prevent google/facebook honeypot
				//if (newUrl.Length > 100) continue; //lazy cut out of urls with tons of garbage
				if (newUrl.Contains("www.w3.org")) continue;
				if (future.Count < 2500) future.Push(newUrl.ToString());
			}
		}

		private Task<string> asyncGetUrlData(string url) {
			StreamReader reader = WebPage.GetData(url);
			return reader.ReadToEndAsync();
		}
	}
}
