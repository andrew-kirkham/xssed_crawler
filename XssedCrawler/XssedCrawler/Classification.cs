using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XssedCrawler {
	public class Classification {
		public int Characters; //number of special characters
		public int ScriptCount; //number of "script" references
		public int EncodedCharacters; //number of decimal/hex characters
		public int EventHandlers; //number of javascript event handlers

		public Classification classify(string url) {
			
			return new Classification();
		}
	}
}
