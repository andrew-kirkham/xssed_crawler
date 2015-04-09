using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XssedCrawler {
	public class FileManager {

		public static void SaveHtmlToDisk(string htmlData, int pageNumber) {
			string fileName = String.Format("Webpage_{0}", pageNumber);
			string filePath = String.Format(@"webpage\{0}.txt", fileName); //note this will save in Debug or Release folder if run in VS
			File.WriteAllText(filePath, htmlData);
		}

		public static void SaveUrlListToDisk(List<String> urls) {
			string fileName = "Vulnerable URLs.txt";
			File.WriteAllLines(fileName, urls.ToArray());
		}
	}
}
