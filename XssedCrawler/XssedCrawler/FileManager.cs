using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XssedCrawler {
	/// <summary>
	/// Class to handle any saving to the file system
	/// </summary>
	public static class FileManager {

		/// <summary>
		/// Saves an html page as Webpage_"pageNumber"
		/// </summary>
		/// <param name="htmlData">A string containing the entire html page</param>
		/// <param name="pageNumber">The number of the page in the crawl</param>
		public static void SaveHtmlToDisk(string htmlData, int pageNumber) {
			string fileName = String.Format("Webpage_{0}", pageNumber);
			string filePath = String.Format(@"webpage\{0}.txt", fileName); //note this will save in Debug or Release folder if run in VS
			File.WriteAllText(filePath, htmlData);
		}

		/// <summary>
		/// Saves a list of urls to a single txt document
		/// </summary>
		public static void SaveUrlListToDisk(IEnumerable<String> urls) {
			string fileName = "Url List.txt";
			File.WriteAllLines(fileName, urls.ToArray());
		}
	}
}
