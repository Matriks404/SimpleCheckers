using System;
using System.IO;

using IniParser;
using IniParser.Exceptions;
using IniParser.Model;

namespace SimpleCheckers
{
	public class RecordedGame
	{
		string scgame_file_path;
		string config_file_path;

		private string title = "Untitled game";
		private string description = string.Empty;
		private string source = string.Empty;

		private string[] game_moves;

		FileIniDataParser parser = new FileIniDataParser();
		IniData data;

		public RecordedGame(string name)
		{
			scgame_file_path = $@"games/{ name }.scgame";
			config_file_path = $@"games/{ name }.config.ini";

			if (Check(name) == false)
			{
				Environment.Exit(0);
			}
		}

		public bool Check(string name)
		{
			Console.WriteLine($"Info: Checking game files...");

			if (File.Exists(scgame_file_path) == false)
			{
				Console.WriteLine($"Error: SCGame file called '{ name }.scgame' is not present!");

				return false;
			}

			Console.WriteLine($"Info: SCGame file is present.");

			if (File.Exists(config_file_path) == false)
			{
				Console.WriteLine($"Warning: SCGame configuration file called '{ name }.config.ini' file is not present!");
			}
			else
			{
				Console.WriteLine("Info: Checking SCGame configuration file integrity...");

				try
				{
					data = parser.ReadFile(config_file_path);

					if (data["info"]["title"] == string.Empty)
					{
						Console.WriteLine("Error: SCGame configuration file has invalid title defined.");

						return false;
					}
				}
				catch (ParsingException)
				{
					Console.WriteLine("Error: SCGame configuration file is not valid!");

					return false;
				}

				Console.WriteLine("Info: SCGame configuration file is OK!");
			}

			Console.WriteLine("Info: Checking SCGame file integrity...");

			game_moves = File.ReadAllLines(scgame_file_path);

			for (int i = 0; i < game_moves.Length; i++)
			{
				string move = game_moves[i];

				if (Input.IsValid(move) == false)
				{
					Console.WriteLine($"Error: Line { i } is invalid input: { move }");

					return false;
				}
			}

			Console.WriteLine("Info: SCGame file is OK!");

			return true;
		}

		public void SetupInfo()
		{
			try
			{
				KeyDataCollection info = data["info"];

				title = data["info"]["title"];
				description = info["description"];
				source = info["source"];
			}
			catch (NullReferenceException)
			{
				Console.WriteLine("Warning: Since no SCGame configuration file is present, we are assuming defaults for game info.");
			}
		}

		public void DisplayInfo()
        {
			Console.WriteLine($"\nGame title: { title }");
			if (description != string.Empty)
			{
				Console.WriteLine($"Game description: { description }");
			}
			if (source != string.Empty)
			{
				Console.WriteLine($"Game source: { source }");
			}

			Console.WriteLine();

		}

		public void Play()
		{
			Game game = new Game();

			foreach (string move in game_moves)
			{
				game.Update(move);
			}
		}
	}
}