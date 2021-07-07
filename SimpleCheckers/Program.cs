using System;

namespace SimpleCheckers
{
	public static class Program
	{
		static void Main(string[] args)
		{
			ProgramArguments.Handle();

            if (Config.play_recorded_game == true)
			{
				string game_name = Config.recorded_game_name;

				RecordedGame rg = new RecordedGame(game_name);

				rg.SetupInfo();

				if (Config.clear_enabled)
				{
					Console.Clear();
				}

				rg.DisplayInfo();

				if (Config.clear_enabled == true)
				{
					Console.WriteLine("\nPress any key to continue...");
					Console.ReadKey();
				}

				rg.Play();
			}
			else
			{
				if (Config.instructions_enabled)
				{
					Display.Instructions();
				}

				Game game = new Game();

				game.Loop();
			}
		}
	}
}
