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

		private static bool continueFlag = true;

		private const int pageSize = 15;

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
						printCurrentPage();
						Console.WriteLine("Unbekannter Befehl");
						break;
				}
			}			
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
