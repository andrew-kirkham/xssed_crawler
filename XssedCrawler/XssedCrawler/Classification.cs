using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XssedCrawler {
	public class Classification {
		public int Characters = 0; //number of special characters <>;()"'
		public int ScriptCount = 0; //number of "script" references
		public int EncodedCharacters = 0; //number of decimal/hex characters
		public int EventHandlers = 0; //number of javascript event handlers

		public Classification Classify(string url) {
			classifyChars(url);
			classifyScript(url);
			classifyEncoded(url);
			classifyEvents(url);
			return new Classification();
		}

		public void ClassifyAll() {
			List<Classification> c = new List<Classification>();
			List<string> urls = File.ReadLines(FileManager.URL_LIST_FILE).ToList();
			foreach (var url in urls) {
				c.Add(Classify(url));
			}
			FileManager.WriteClassifiedToArff(c);
		}

		private void classifyChars(string url) {
			Regex r = new Regex(@"[<>;()""]''");
			Characters = r.Matches(url).Count;
		}

		private void classifyScript(string url) {
			Regex r = new Regex("(script)");
			ScriptCount = r.Matches(url).Count;
		}

		private void classifyEncoded(string url) {
			Regex r = new Regex(@"(&#)\d{0,}");
			int decimalCount = r.Matches(url).Count;
			r = new Regex(@"(&#x)\d{0,}\w{0,}");
			int hexCount = r.Matches(url).Count;
			EncodedCharacters = decimalCount + hexCount;
		}

		private void classifyEvents(string url) {
			IEnumerable<string> events = FileManager.LoadEvents();
			foreach (var ev in events) {
				if (url.Contains(ev)) EventHandlers++;
			}
		}
	}
}
