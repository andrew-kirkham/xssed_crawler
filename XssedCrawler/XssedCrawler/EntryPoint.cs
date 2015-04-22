using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace XssedCrawler {
	static class EntryPoint {

		static void Main() {
			char selection = promptUser();
			selectTask(selection);
		}

		private static void selectTask(char selection) {
			while (true) {
				switch (selection) {
					case 'c':
						var c = new AsyncCrawler();
						c.Crawl(@"http://www.reddit.com");
						break;
					case 'x':
						var x = new XssCrawler();
						x.Crawl();
						break;
					case 'm':
						FileManager.ClassifyAll();
						Console.WriteLine("Data classified");
						break;
					case 'd':
						cleanData();
						Console.WriteLine("Dataset Cleaned");
						break;
					default:
						Console.WriteLine("Command not recognized");
						selection = promptUser();
						continue;
				}
				break;
			}
		}

		/// <summary>
		/// Remove all vulnerable urls with all 0 for classification
		/// </summary>
		/// <remarks>
		/// This doesn't seem correct. However, looking through many of the returned results are not acutally XSS vulns
		/// Some were generic urls and some were SQL injections.
		/// </remarks>
		private static void cleanData() {
			var falsePositive = new Classification("", "TRUE");
			var data = File.ReadAllLines(FileManager.VULN_URL_LIST_FILE);
			List<Classification> cl = data.Select(url => new Classification(url, "TRUE")).Where(c => c.AreIdentical(falsePositive)).ToList();
			List<string> urls = cl.Select(c => c.url).ToList();
			File.WriteAllLines("test.txt", urls);
		}

		private static char promptUser() {
			Console.WriteLine("Input Command: (c) crawl all websites (x) save xssed data (m) classify data");
			return (char)Console.Read();
		}
	}
}
