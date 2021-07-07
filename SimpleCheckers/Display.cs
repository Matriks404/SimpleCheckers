using System;

namespace SimpleCheckers
{
	public static class Display
	{
		public static void PlayerStats()
		{
			Console.WriteLine("\nPlayer stats:");

			foreach (Player p in Game.players)
			{
				Console.WriteLine($"   Player { (int)p.Player_Color } pieces: { p.Pieces_Count }");
			}
		}

		public static void Instructions()
		{
			if (Config.clear_enabled)
			{
				Console.Clear();
			}

			Console.WriteLine("Welcome to the Checkers!\n");
			Console.WriteLine("For every cycle you need to enter your move, for example 'a6, b5' for light player.");
			Console.WriteLine("To see players stats enter one of these: 's', 'stat', stats'.");
			Console.WriteLine("To clear screen enter one of these: 'c', 'clear', 'cls'.");
			Console.WriteLine("To stop enter one of these: 'exit', 'stop', 'q!', 'quit'.\n");
			Console.WriteLine("Note: Successive jumps and kings are not implemented at yet!\n");

			if (Config.clear_enabled)
			{
				Console.WriteLine("Press any key if you have read instructions.");
				Console.ReadKey();
			}
		}

		public static void Help()
		{
			Console.WriteLine("SimpleCheckers command line list:\n");
			Console.WriteLine("  '-beeps' OR '-sound'       Enables beeps (sound) on Windows platform.");
			Console.WriteLine("  '-cmdlist' OR '-help'      Shows this command list.");
			Console.WriteLine("  '-dont-print-jumps'        Disables list of jumps printing.");
			Console.WriteLine("  '-no-auto-clear'           Disables automatic console clearing.");
			Console.WriteLine("  '-no-instructions'         Disables in-game instructions.");
			Console.WriteLine("  '-no-mandatory-jumps'      Disables mandatory jumps rule.");
			Console.WriteLine("  '-game-play=<NAME>         Plays recorded game.");
			Console.WriteLine("  '-quiet'                   Recorded game is quiet (Displays only last result).\n");
			Console.WriteLine("Example: 'simplecheckers -sound -no-instructions'  ->  Enables beeps (sound) and disables in-game instructions.");
			Console.WriteLine("Example: 'simplecheckers -game-play=example1'      ->  Plays game called 'example1'.\n\n");
			Console.WriteLine("SimpleCheckers in-game commands:\n");
			Console.WriteLine("  'xn,xn'                                 Move from position xn to position xn");
			Console.WriteLine("  'c' OR 'clear' OR 'cls'                 Clear screen.");
			Console.WriteLine("  'exit' OR 'stop' OR 'q!' OR 'quit'      Stop the game.");
			Console.WriteLine("  's' OR 'stat' OR 'stats'                See players stats.");
		}
	}
}