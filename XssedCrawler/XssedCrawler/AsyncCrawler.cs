﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XssedCrawler {
#pragma warning disable 4014
	public class AsyncCrawler {

		static HashSet<string> history;
		static Stack<string> future;

		public AsyncCrawler() {
			history = new HashSet<string>();
			future = new Stack<string>(10000);
			Console.CancelKeyPress += cancelEventHandler;
		}

		private void cancelEventHandler(object sender, ConsoleCancelEventArgs e) {
			FileManager.SaveUrlListToDisk(history);
			FileManager.SaveUrlListToDisk(future);
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
				FileManager.SaveUrlListToDisk(future);
			}
		}

		private void tryCrawl(string startUrl) {
			getUrlData(startUrl);
			while (future.Count < 1) { } //waiting for the crawler to return results as we have exhausted the list

			while (future.Peek() != null) {
				getUrlData(future.Pop());
			}
		}

		async void getUrlData(string url) {
			if (history.Contains(url)) return;

			history.Add(url);
			Console.WriteLine(url);
			var htmlPage = await asyncGetHtmlPage(url);

			var urls = WebPage.ParseWebPage(htmlPage, @"<a href=(.*?)(?=<)");
			var list = (from object u in urls let r = new Regex("(?<=>)http:(.*)") select r.Match(u.ToString()).ToString());
			addToFuture(list);
		}

		private static void addToFuture(IEnumerable urls) {
			foreach (var nextUrl in urls) {
				string newUrl = nextUrl.ToString();
				if (!newUrl.Contains('.')) continue; //removing local redirects
				if (newUrl.Contains("google") || newUrl.Contains("facebook")) continue; //prevent google/facebook honeypot
				if (newUrl.Length > 100) continue; //lazy cut out of urls with tons of garbage
				if (newUrl.Contains("www.w3.org")) continue; //lots of pages link to w3 standards
				if (future.Count < 10000) future.Push(newUrl);
			}
		}

		private async Task<string> asyncGetHtmlPage(string url) {
			string htmlPage;
			try {
				htmlPage = await asyncGetUrlData(url);
			}
			catch (Exception) {
				htmlPage = "";
			}
			return htmlPage;
		}

		private Task<string> asyncGetUrlData(string url) {
			StreamReader reader= WebPage.GetData(url);
			return reader.ReadToEndAsync();
		}
	}
}
