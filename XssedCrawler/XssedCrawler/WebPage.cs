using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XssedCrawler {
	class WebPage {

		public static StreamReader GetData(string url) {
			WebRequest request = WebRequest.Create(url);
			WebResponse response = request.GetResponse();

			Stream responseStream = response.GetResponseStream();
			return new StreamReader(responseStream);
		}

		public static MatchCollection ParseWebPage(string webPage, string regex) {
			Regex r = new Regex(regex);
			return r.Matches(webPage);
		}
	}
}
