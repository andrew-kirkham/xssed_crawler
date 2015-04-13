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
			//Crawler c = new Crawler();
			AsyncCrawler c = new AsyncCrawler();
			c.Crawl(@"http://www.dmoz.org/");
		}
	}
}
