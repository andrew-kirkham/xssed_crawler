using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XssedCrawler {
	public class Classification {
		public int Characters; //number of special characters <>;()"'
		public int ScriptCount; //number of "script" references
		public int EncodedCharacters; //number of decimal/hex characters
		public int EventHandlers; //number of javascript event handlers

		public Classification(string url) {
			classifyChars(url);
			classifyScript(url);
			classifyEncoded(url);
			classifyEvents(url);
		}

		private void classifyChars(string url) {
			Regex r = new Regex(@"[<>;()""']");
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
			r = new Regex(@"(%)\d{0,}");
			int encoded = r.Matches(url).Count;
			EncodedCharacters = decimalCount + hexCount + encoded;
		}

		private void classifyEvents(string url) {
			IEnumerable<string> events = FileManager.LoadEvents();
			foreach (var ev in events) {
				if (url.ToLower().Contains(ev.ToLower())) EventHandlers++;
			}
		}
	}
}
