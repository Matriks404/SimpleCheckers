using System;
using System.Collections.Generic;
using System.IO;

using IniParser;
using IniParser.Exceptions;
using IniParser.Model;

namespace SimpleCheckers
{
	public class Game
	{
		private static Player light = new Player(Player.Color.Light);
		private static Player dark = new Player(Player.Color.Dark);
		public static List<Player> players = new List<Player> { light, dark };

		public static bool do_display_update = true;

		private static Board board = new Board();

		public void Update(string move)
        {
            if (Config.clear_enabled && do_display_update == true)
			{
				Console.Clear();
			}

			var cycles = board.Cycles;
			var player_number = (int)Player.Current_Player;

			if (do_display_update == true)
			{
				Console.WriteLine($"=== cycle {cycles}, player {player_number} ===\n");

				board.DisplayResult();

				if (Config.printing_jumps_enabled)
				{
					board.PrintJumps();
				}
			}

			Console.Write("\nYour move: ");

			if (Config.play_recorded_game == true)
			{
				Console.WriteLine(move);
			}
			else
			{
				move = Console.ReadLine();
			}

			if (do_display_update == false)
			{
				do_display_update = true;
			}

			while (board.TryToHandleInput(move) == false)
			{
				if (Config.play_recorded_game == true)
				{
					Console.WriteLine("Error: Invalid move in recorded game!");

					return;
				}
				else
				{
					Console.Write("Move failed! Try again: ");
					move = Console.ReadLine();
				}
			};

			if (do_display_update == true)
			{
				Console.WriteLine();
			}
		}

		public void Loop()
		{
			string move = string.Empty;

			do
			{
				Update(move);
			} while (board.Still_Playing);
		}
	}
}