using System;

namespace XssedCrawler {
	static class EntryPoint {

		static void Main() {
			char selection = promptUser();
			selectTask(selection);
		}

		private static void selectTask(char selection) {
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
					break;
				default:
					Console.WriteLine("Command not recognized");
					selectTask(promptUser());
					break;
			}
		}

		private static char promptUser() {
			Console.WriteLine("Input Command: (c) crawl all websites (x) save xssed data (m) classify data");
			return (char)Console.Read();
		}
	}
}
