using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XssedCrawler {
	public class Dmoz {

		HashSet<string> history;
		public Dmoz() {
			history = new HashSet<string>();
			Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);
		}

		private void myHandler(object sender, ConsoleCancelEventArgs e) {
			File.AppendAllLines("output.txt", history);
		}

		/// <summary>
		/// Crawl through the web, starting at a given url. Will continue to crawl and output the console until the user kills it with Ctrl+C
		/// After the crawl is killed, all urls are written to a text file.
		/// </summary>
		public void Crawl(string start) {
			getUrlData(start);
			while (true) { }
		}

		async Task getUrlData(string url) {
			if (history.Contains(url)) return;
			history.Add(url);
			Console.WriteLine(url);
			string htmlPage = await getData(url);
			var urls = parseData(htmlPage);
			foreach (var newUrl in urls) {
				getUrlData(newUrl.ToString());
			}
		}

		private MatchCollection parseData(string data) {
			Regex regexUrl = new Regex(@"https?:\/\/(.*?)(?=[""\)'])"); //match a URL and ends with an html tag, but dont include either
			return regexUrl.Matches(data);
		}

		private Task<string> getData(string url) {
			WebRequest request = WebRequest.Create(url);
			WebResponse response = request.GetResponse();

			Stream responseStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(responseStream);
			return reader.ReadToEndAsync();
		}
	}
}
