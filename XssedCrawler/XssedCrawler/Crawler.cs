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
	class Crawler {

		const string BaseUrl = @"http://www.xssed.com";
		const string ArchiveUrl = BaseUrl + @"/archive/page=";
		static int xssPage = 1;
		static void Main(string[] args) {
			int maxPage = 1530;
			for (int page = 1; page <= maxPage; page++) {
				string data = tryGetUrlData(ArchiveUrl + page);
				Regex regexMirror = new Regex(@"/mirror/\w*"); //grab the url of /mirror/*
				var mirrors = regexMirror.Matches(data);
				getVulnerablePages(mirrors);
			}
		}

		private static void getVulnerablePages(MatchCollection mirrors) {
			foreach (var match in mirrors) {
				string data = tryGetUrlData(BaseUrl + match.ToString());
				Regex regexVuln = new Regex("http://vuln.xssed.net(.*?)(?=\\\">)"); //grab the entire url starting with vuln.xssed.net and stopping before the end of the <a> tag
				var vulnUrl = regexVuln.Matches(data);
				data = tryGetUrlData(vulnUrl[0].ToString());
				saveVulnPage(data);
			}
		}

		private static void saveVulnPage(string data) {
			string fileName = String.Format("Webpage_{0}", xssPage++);
			string filePath = String.Format(@"webpage\{0}.txt", fileName); //note this will save in Debug or Release folder if run in VS
			File.WriteAllText(filePath, data);
		}

		private static string tryGetUrlData(string url) {
			string data = "";
			try {
				data = getUrlData(url);
			}
			catch (Exception) { } //very bad but lots of possible exceptions that would get in the way of the crawl - some URL null, some invalid, some 404, etc.
			return data;
		}

		private static string getUrlData(string url) {
			WebRequest request = WebRequest.Create(url);
			WebResponse response = request.GetResponse();

			Stream responseStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(responseStream);
			string data = reader.ReadToEnd();
			return data;
		}
	}
}
