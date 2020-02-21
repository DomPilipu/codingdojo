using System;
using System.Collections.Generic;
using System.IO;

namespace round_one
{
    class Program
    {
		private static string[] lines { get; set; }
		private static Dictionary<int, string[]> pages { get; set; }

		private static int currentPage { get; set; }

		private static int currentSearchResult = 0;

		private static List<Tuple<int, int>> searchResults { get; set; }

		private static bool continueFlag = true;

		private const int pageSize = 20;

		private static string searchInput { get; set; }

		private static int hitCount { get; set; }

        static void Main(string[] args)
		{
			string filePathFromArgs = checkArgs(args);

			if (!checkIfFileExists(filePathFromArgs))
			{
				errorClose("Die angegebene Textdatei existiert nicht!");
			}
			buildUpData(filePathFromArgs);
			printCurrentPage();

			while(continueFlag) {
				ConsoleKeyInfo info = Console.ReadKey();

				switch(info.Key) {
					case ConsoleKey.D7:
						if(info.KeyChar == '/') {
							searchInput = Console.ReadLine();
							searchBook(searchInput);

							if(hitCount > 0) {
								printWithSearchHighLight();
							} else {
								printCurrentPageWithError("Keine Suchtreffer!");
							}
							
						} else {
							printCurrentPageWithError("Unbekannter Befehl!");
						}
						break;
					case ConsoleKey.N:
						if(String.IsNullOrEmpty(searchInput)) {
							printCurrentPageWithError("Keine Suche ausgeführt!");
						} else {
							currentSearchResult++;
							printWithSearchHighLight();
						}
						
						break;
					case ConsoleKey.G:
						string jumpInput = Console.ReadLine();
						jumpToPage(jumpInput);
						break;
					case ConsoleKey.B:
						LastPage();
						break;
					case ConsoleKey.Spacebar:
						NextPage();
						break;
					case ConsoleKey.Q:
						Close();
						break;
					default:
						printCurrentPageWithError("Unbekannter Befehl!");
						break;
				}
			}			
		}

		private static void printWithSearchHighLight()
		{
			Console.Clear();
			Tuple<int, int> currentSearchResultTuple = searchResults[currentSearchResult];
			string[] pageToPrintWithHighLight = pages[currentSearchResultTuple.Item1];

			for(int i = 0; i < pageToPrintWithHighLight.Length; i++) {
				if(i != currentSearchResultTuple.Item2) {
					Console.WriteLine(pageToPrintWithHighLight[i]);
				} else {
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(pageToPrintWithHighLight[i]);
					Console.ResetColor();
				}
			}

			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Suchergebnis {0} von {1}", currentSearchResult + 1, hitCount);
			Console.ResetColor();
		}

		private static void searchBook(string searchInput)
		{
			hitCount = 0;
			searchResults = new List<Tuple<int, int>>();

			foreach(int key in pages.Keys) {
				string[] page = pages[key];
				for(int i = 0; i < page.Length; i++) {
					string line = page[i];
					if(line != null) {
						if(line.Contains(searchInput)) {
							hitCount++;
							searchResults.Add(Tuple.Create(key, i));
						}
					}					
				}
			}
		}

		private static void jumpToPage(string input)
		{
			try{
				int page = Convert.ToInt32(input);

				if(pages.ContainsKey(page)) {
					currentPage = page;
					printCurrentPage();
				} else {
					printCurrentPageWithError("Angegebene Seitenzahl liegt nicht vor!");
				}
			}
			catch(FormatException) {
				printCurrentPageWithError("Angegebene Seitenzahl liegt nicht vor!");
			}			
		}

		private static void printCurrentPageWithError(string message) {
			printCurrentPage();
			printErrorMessage(message);
		}

		private static void NextPage()
		{
			if(currentPage != pages.Count) {
				currentPage++;
			}			
			printCurrentPage();
		}

		private static void LastPage()
		{
			if(currentPage != 1) {
				currentPage--;
			}			
			printCurrentPage();
		}

		private static void printCurrentPage()
		{
			printPage(currentPage);
		}

		private static void buildUpData(string filePathFromArgs)
		{
			lines = File.ReadAllLines(filePathFromArgs);
			pages = new Dictionary<int, string[]>();
			
			int pageNumber = 1;
			
			for(int i = 0; i < lines.Length; i = i + pageSize) {

				string[] page = new string[pageSize];
				if(i + pageSize > lines.Length) {
					int k = lines.Length - i;
					Array.Copy(lines, i, page, 0, k);
					for(int j = 0; i < page.Length; j++) {
						if(String.IsNullOrEmpty(page[j])) {
							page[j] = "";
						}
					}
				} else {
					Array.Copy(lines, i, page, 0, pageSize);
				}				
				pages.Add(pageNumber, page);

				pageNumber++;
			}

			currentPage = 1;
		}

		private static void printPage(int pageNumber) {
			Console.Clear();
			foreach(string line in pages[pageNumber]) {
				Console.WriteLine(line);
			}
		}

		private static string checkArgs(string[] args)
		{
			try {
				string pathFromArgs = args[0];
				return pathFromArgs;
			} catch (IndexOutOfRangeException) {
				errorClose("Keine Textdatei angegeben!");
				return "";
			}
		}

		private static void errorClose(string message) {
			printErrorMessage(message);
			Close();
		}

		private static void Close() {
			Environment.Exit(0);
		}

		private static void printErrorMessage(string message) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
		}

		private static bool checkIfFileExists(string path) {
			return File.Exists(path);
		}
    }
}
