using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace XssedCrawler {
	public class Classification {
		public int Characters; //number of special characters <>;()"'
		public int ScriptCount; //number of "script" references
		public int EncodedCharacters; //number of decimal/hex characters
		public int JsEventHandlers; //number of javascript event handlers
		public int DomEvents; //number of dom interactions
		public string Class; //positive (TRUE) or negative (FALSE) example

		public string url;

		public Classification(string url, string example) {
			this.url = url;
			classifyChars();
			classifyScript();
			classifyEncoded();
			classifyJavascriptEvents();
			classifyDomEvents();
			Class = example;
		}

		private void classifyChars() {
			Regex r = new Regex(@"[<>;()""']");
			Characters = r.Matches(url).Count;
		}

		private void classifyScript() {
			Regex r = new Regex("(script)");
			ScriptCount = r.Matches(url).Count;
		}

		private void classifyEncoded() {
			Regex r = new Regex(@"(&#)\d{0,}");
			int decimalCount = r.Matches(url).Count;
			r = new Regex(@"(&#x)\d{0,}\w{0,}");
			int hexCount = r.Matches(url).Count;
			r = new Regex(@"(%)\d{0,}");
			int encoded = r.Matches(url).Count;
			EncodedCharacters = decimalCount + hexCount + encoded;
		}

		private void classifyJavascriptEvents() {
			IEnumerable<string> events = FileManager.LoadJavascriptEvents();
			foreach (var ev in events.Where(ev => url.ToLower().Contains(ev.ToLower()))) {
				JsEventHandlers++;
			}
		}

		private void classifyDomEvents() {
			IEnumerable<string> events = FileManager.LoadDomEvents();
			foreach (var ev in events.Where(ev => url.ToLower().Contains(ev.ToLower()))) {
				DomEvents++;
			}
		}

		/// <summary>
		/// Compares two classifications and returns whether they are identical ignoring the source url
		/// </summary>
		/// <param name="c">The classification to compare with</param>
		/// <returns>True if the two are identical</returns>
		public bool AreIdentical(Classification c) {
			if (c == null) return false;
			if (c.Characters != this.Characters) return false;
			if (c.Class != this.Class) return false;
			if (c.DomEvents != this.DomEvents) return false;
			if (c.EncodedCharacters != this.EncodedCharacters) return false;
			if (c.JsEventHandlers != this.JsEventHandlers) return false;
			if (c.ScriptCount != this.ScriptCount) return false;
			return true;
		}
	}
}
