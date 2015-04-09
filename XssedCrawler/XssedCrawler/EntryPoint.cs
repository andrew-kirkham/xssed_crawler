using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XssedCrawler {
	/// <summary>
	/// Crawl through xssed.com and save all of the archived vulnerable webpages to the file system
	/// </summary>
	/// <remarks>
	/// This takes a long time ~7hours. Not all archived pages have any useful html.
	/// </remarks>
	class EntryPoint {

		static void Main(string[] args) {
			Crawler c = new Crawler();
			c.Crawl();
		}
	}
}
