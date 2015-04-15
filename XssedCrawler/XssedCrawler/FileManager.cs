using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XssedCrawler {
	/// <summary>
	/// Class to handle any saving to the file system
	/// </summary>
	public static class FileManager {

		public const string URL_LIST_FILE = @"Url List.txt";
		public const string VULN_URL_LIST_FILE = @"Vulnerable URLs.txt";
		public const string ARFF_FILE = @"classified_urls.arff";
		public const string EVENT_LIST_FILE = @"javascript_events.txt";

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
			File.AppendAllLines(URL_LIST_FILE, urls.ToArray());
		}

		private static void writeClassifiedToArff(List<Classification> classifiedUrls){
			File.Create(ARFF_FILE).Close();
			StreamWriter s = File.AppendText(ARFF_FILE);
			writeArffHeader(s);
			writeArffBody(classifiedUrls, s);
			s.Close();
		}

		public static IEnumerable<string> LoadEvents() {
			IEnumerable<string> events = File.ReadLines(EVENT_LIST_FILE);
			return events;
		}

		public static void ClassifyAll() {
			List<Classification> c = new List<Classification>();
			List<string> urls = File.ReadLines(FileManager.VULN_URL_LIST_FILE).ToList();
			urls = urls.Concat(File.ReadLines(FileManager.URL_LIST_FILE)).ToList();
			foreach (var url in urls) {
				c.Add(new Classification(url));
			}
			writeClassifiedToArff(c);
		}

		private static void writeArffBody(List<Classification> classifiedUrls, StreamWriter s) {
			foreach (var c in classifiedUrls) {
				s.WriteLine("{0}, {1}, {2}, {3}", c.Characters, c.ScriptCount, c.EncodedCharacters, c.EventHandlers);
			}
		}

		private static void writeArffHeader(StreamWriter s) {
			s.WriteLine("@RELATION xss\n");
			foreach (var property in typeof(Classification).GetFields()) {
				s.WriteLine("@ATTRIBUTE {0} NUMERIC", property.Name);
			}
			s.WriteLine("\n@DATA");
		}
	}
}
