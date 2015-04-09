using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XssedCrawler {
	public class Crawler {
		const string BaseUrl = @"http://www.xssed.com";
		const string ArchiveUrl = BaseUrl + @"/archive/page=";
		static int xssPage = 1;
		private List<String> urls = new List<String>(46000);

		public void Crawl() {
			int maxPage = 1530; //the website hasn't been updated in a while, so this is valid now. might need to dynamically set this if the website becomes active
			for (int page = 1; page <= maxPage; page++) {
				string data = tryGetUrlData(ArchiveUrl + page);
				Regex regexMirror = new Regex(@"/mirror/\w*"); //grab the url of /mirror/*
				var mirrors = regexMirror.Matches(data);
				getVulnerablePages(mirrors);
			}
			FileManager.SaveUrlListToDisk(urls);
		}

		private void getVulnerablePages(MatchCollection mirrors) {
			foreach (var match in mirrors) {
				string data = tryGetUrlData(BaseUrl + match.ToString());
				extractUrl(data);
				//extractAndSaveHtmlPage(data);
			}
		}

		private void extractAndSaveHtmlPage(string data) {
			Regex regexUrl = new Regex("http://vuln.xssed.net(.*?)(?=\\\">)"); //grab the entire url starting with vuln.xssed.net and stopping before the end of the <a> tag
			string url = regexUrl.Matches(data)[0].ToString();
			data = tryGetUrlData(url);
			FileManager.SaveHtmlToDisk(data, xssPage++);
		}

		private void extractUrl(string data) {
			Regex regexUrl = new Regex(@"(?<=URL: )https?:\/\/(.*?)(?=<\/t)"); //match a URL that starts with URL: and ends with an html tag, but dont include either
			string url = regexUrl.Matches(data)[0].ToString();
			url = String.Join("", (url.Split(new[] { "<br>" }, StringSplitOptions.None))); //strip out <br> tags from the URL
			urls.Add(url);
		}

		private string tryGetUrlData(string url) {
			string data = "";
			try {
				data = getUrlData(url);
			}
			catch (Exception) { } //very bad but lots of possible exceptions that would get in the way of the crawl - some URL null, some invalid, some 404, etc.
			return data;
		}

		private string getUrlData(string url) {
			WebRequest request = WebRequest.Create(url);
			WebResponse response = request.GetResponse();

			Stream responseStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(responseStream);
			return reader.ReadToEnd();
		}

	}
}
