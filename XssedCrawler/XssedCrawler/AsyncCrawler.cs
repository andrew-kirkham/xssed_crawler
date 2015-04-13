using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XssedCrawler {
	public class AsyncCrawler {

		HashSet<string> history;

		public AsyncCrawler() {
			history = new HashSet<string>();
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
			getUrlData(startUrl);
			while (true) { 
			} //run indefinitely otherwise the async tasks end
		}

		async Task getUrlData(string url) {
			if (history.Contains(url)) return;

			history.Add(url);
			Console.WriteLine(url);

			string htmlPage = await asyncGetUrlData(url);
			var urls = WebPage.ParseWebPage(htmlPage, @"https?:\/\/(.*?)(?=[""\)'])");

			foreach (var newUrl in urls) {
				getUrlData(newUrl.ToString());
			}
		}

		private Task<string> asyncGetUrlData(string url) {
			StreamReader reader = WebPage.GetData(url);
			return reader.ReadToEndAsync();
		}
	}
}
