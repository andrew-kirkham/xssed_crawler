using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace XssedCrawler {
	static class WebPage {

		/// <summary>
		/// Create a request and return the html contents of the given url
		/// </summary>
		public static StreamReader GetData(string url) {
			WebRequest request = WebRequest.Create(url);
			WebResponse response = request.GetResponse();

			Stream responseStream = response.GetResponseStream();
			return new StreamReader(responseStream);
		}

		/// <summary>
		/// Searches through the html of a webpage and returns all results matching a regex
		/// </summary>
		public static MatchCollection ParseWebPage(string webPage, string regex) {
			Regex r = new Regex(regex);
			return r.Matches(webPage);
		}
	}
}
