using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XssedCrawler {
	class EntryPoint {

		static void Main(string[] args) {
			FileManager.WriteClassifiedToArff(null);
			char selection = promptUser();
			selectTask(selection);
		}

		private static void selectTask(char selection) {
			switch (selection) {
				case 'c':
					var c = new AsyncCrawler();
					c.Crawl(@"http://www.dmoz.org/");
					break;
				case 'x':
					var x = new XssCrawler();
					x.Crawl();
					break;
				case 'm':
					var m = new Classification();
					m.ClassifyAll();
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
