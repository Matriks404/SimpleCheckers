using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SimpleCheckers
{
	public class Board
	{
		private enum FieldPiece
		{
			Nothing,
			Man,
            King
        }

		private struct FieldEntry
		{
			public Player.Color player_color;
			public FieldPiece piece;
		}

		private class Jump
		{
			public Player.Color player_color;

			public int piece_x;
			public int piece_y;

			public int target_x;
			public int target_y;

			public char[] piece_position;
			public char[] target_position;

			public Jump(Player.Color pc, int piece_x, int piece_y, int target_x, int target_y)
			{
				this.player_color = pc;

				this.piece_x = piece_x;
				this.piece_y = piece_y;

				this.target_x = target_x;
				this.target_y = target_y;

				piece_position = new char[2];
				target_position = new char[2];

				piece_position[0] = FieldXToLetter(piece_x);
				piece_position[1] = Convert.ToChar(piece_y + 1 + 48);

				target_position[0] = FieldXToLetter(target_x);
				target_position[1] = Convert.ToChar(target_y + 1 + 48);
			}

			public override bool Equals(object obj)
			{
				Jump jump = obj as Jump;

				return jump != null && jump.piece_x == this.piece_x && jump.piece_y == this.piece_y;
			}

			public override int GetHashCode()
			{
				return this.piece_x.GetHashCode() ^ this.piece_y.GetHashCode();
			}
		};

		private List<string> clear_strings = new List<string>() { "c", "clear", "cls" };
		private List<string> exit_strings = new List<string>() { "exit", "stop", "q!", "quit" };
		private List<string> stats_strings = new List<string>() { "s", "stat", "stats" };

		private FieldEntry[,] fields;
		private HashSet<Jump> jumps_list;

		private int height;
		private int width;

		public bool Still_Playing { get; private set; }
		public int Cycles { get; private set; }

		public Board()
		{
			Still_Playing = true;
			Cycles = 1;

			height = 8;
			width = 8;

			Player.Current_Player = Player.Color.Light;

			fields = new FieldEntry[height, width];
			jumps_list = new HashSet<Jump>();

			SetupInitialBoard();

			foreach (Player p in Game.players)
			{
				p.Pieces_Count = (width / 2) * 3;
			}
		}

		//TODO: Tide this up.
		private void SetupInitialBoard()
		{
			for (int i = 0; i < 3; i++)
				for (int j = 0; j < width; j++)
					if ((i + j) % 2 == 1)
						fields[i, j] = new FieldEntry { player_color = Player.Color.Dark, piece = FieldPiece.Man };

			for (int i = width - 3; i <= width - 1; i++)
				for (int j = 0; j < width; j++)
					if ((i + j) % 2 == 1)
						fields[i, j] = new FieldEntry { player_color = Player.Color.Light, piece = FieldPiece.Man };
		}

		private int GetYDir() => (Player.Current_Player == Player.Color.Dark) ? 1 : -1;

		private bool CheckMove(int x1, int y1, int x2, int y2)
		{
			FieldEntry entry = fields[y2, x2];

			return ((x2 == x1 - 1 || x2 == x1 + 1) &&
				  y2 == y1 + GetYDir() &&
				  entry.piece == FieldPiece.Nothing);
		}

		private bool CheckJump(int x1, int y1, int x2, int y2, out int target_piece_x, out int target_piece_y)
		{
			FieldEntry entry = fields[y2, x2];

			target_piece_x = 0;
			target_piece_y = 0;

			if ((x2 == x1 - 2 || x2 == x1 + 2) &&
				  y2 == y1 + 2 * GetYDir() &&
				  entry.piece == FieldPiece.Nothing)
			{

				int x_dir = (x2 - x1) / 2;

				target_piece_x = x1 + x_dir;
				target_piece_y = y1 + GetYDir();

				FieldEntry target_entry = fields[target_piece_y, target_piece_x];

				return (target_entry.player_color == Player.Other_Player);
			}

			return false;
		}

		private static char FieldXToLetter(int x) => (char)(97 + x);

		private void AddJump(int pos_x, int pos_y, int target_x, int target_y)
		{
			var jump = new Jump(Player.Current_Player, pos_x, pos_y, target_x, target_y);

			jumps_list.Add(jump);

			pos_y = pos_y + 1;
			target_y = target_y + 1;
		}

		private void SearchForJumps()
		{
			jumps_list.Clear();

			Player.Color temp_player = Player.Current_Player;

			for (int pos_y = 0; pos_y < height; pos_y++)
			{
				int x_offset = 1;

				if (pos_y % 2 == 1)
				{
					x_offset = 0;
				}

				for (int pos_x = x_offset; pos_x < width; pos_x += 2)
				{
					int dummy_x; // TODO: Remove this somehow.
					int dummy_y; // TODO: See above.

					if (fields[pos_y, pos_x].player_color == Player.Color.None)
					{
						continue;
					}

					Player.Current_Player = fields[pos_y, pos_x].player_color;


					int target_y = pos_y + GetYDir() * 2;

					if (pos_y >= 0 && pos_y < height && pos_x >= 2 && pos_x < width)
					{
						int target_x = pos_x - 2;

						if (CheckJump(pos_x, pos_y, target_x, target_y, out dummy_x, out dummy_y))
						{
							AddJump(pos_x, pos_y, target_x, target_y);
						}
					}

					if (pos_y >= 0 && pos_y < height && pos_x >= 0 && pos_x < width - 2)
					{
						int target_x = pos_x + 2;

						if (CheckJump(pos_x, pos_y, target_x, target_y, out dummy_x, out dummy_y))
						{
							AddJump(pos_x, pos_y, target_x, target_y);
						}
					}
				}
			}

			Player.Current_Player = temp_player;
		}

		public void PrintJumps()
		{
			int count = jumps_list.Count;

			if (count > 0)
			{
				Console.WriteLine("\n{0} jump(s) found:", count);

				foreach (Jump jump in jumps_list)
				{
					int player_number = (int)jump.player_color;

					string piece_position = new string(jump.piece_position);
					string target_position = new string(jump.target_position);

					Console.WriteLine($"   player: {player_number} - {piece_position} -> {target_position}");
				}
			}
		}

		private void Update()
		{
			Cycles++;

			SearchForJumps();

			Player.Current_Player = Player.Other_Player;
		}

		private static string GetPieceName(FieldPiece piece)
		{
			string str = Convert.ToString(piece);

			var r = new Regex(@"
			(?<=[A-Z])(?=[A-Z][a-z]) |
			(?<=[^A-Z])(?=[A-Z]) |
			(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

			return r.Replace(str, " ");
		}

		private void Place(int x, int y, FieldPiece piece)
		{
			fields[y, x].player_color = Player.Current_Player;
			fields[y, x].piece = piece;
		}

		private void Remove(int x, int y)
		{
			fields[y, x].player_color = Player.Color.None;
			fields[y, x].piece = FieldPiece.Nothing;
		}

		private void Move(int x1, int y1, int x2, int y2)
		{
			Remove(x1, y1);
			Place(x2, y2, FieldPiece.Man);

			if (Config.beeps_enabled)
			{
				Beeps.PlayMoveBeeps();
			}
		}

		private void Kill(int x, int y)
		{
			Player.Color killed_piece_color = fields[y, x].player_color;
			FieldPiece killed_piece = fields[y, x].piece;

			Remove(x, y);

			foreach (Player p in Game.players)
			{
				if (p.Player_Color == Player.Other_Player)
				{
					p.Pieces_Count -= 1;

					break;
				}
			}

			if (!Config.clear_enabled)
			{
				Console.WriteLine($"   Killed {Board.GetPieceName(killed_piece)}!");
			}

			if (Config.beeps_enabled)
			{
				Beeps.PlayKillBeeps();
			}

		}

		private bool IsAcceptableMove(int x1, int y1, int x2, int y2)
		{
			bool acceptable = true;

			foreach (Jump jump in jumps_list)
			{
				if (jump.player_color == Player.Current_Player)
				{
					if (jump.piece_x == x1 && jump.piece_y == y1 && jump.target_x == x2 && jump.target_y == y2)
					{
						acceptable = true;

						break;
					}
					else
					{
						acceptable = false;
					}
				}
			}

			return acceptable;
		}

		private bool TryToMove(int x1, int y1, int x2, int y2)
		{
			//TODO: This should be below I think.
			if (!IsAcceptableMove(x1, y1, x2, y2) && Config.jumps_mandatory)
			{
				Console.WriteLine("You have a mandatory jump to make!");

				if (Config.beeps_enabled)
				{
					Beeps.PlayMandatoryJumpError();
				}

				return false;
			}

			FieldEntry current_entry = fields[y1, x1];

			if ((Player.Current_Player == current_entry.player_color))
			{

				if (CheckMove(x1, y1, x2, y2))
				{
					Move(x1, y1, x2, y2);
					Update();

					return true;
				}

				int target_piece_x;
				int target_piece_y;

				if (CheckJump(x1, y1, x2, y2, out target_piece_x, out target_piece_y))
				{
					Move(x1, y1, x2, y2);
					Kill(target_piece_x, target_piece_y);

					Update();

					return true;
				}
			}

			return false;
		}

		public bool TryToHandleInput(string move)
		{
			if (clear_strings.Contains(move))
			{
				// We don't need to make another clear when automatic clearing is enabled. This should be refactored though.
				if (!Config.clear_enabled)
				{
					Console.Clear();
				}

				return true;
			}
			else if (exit_strings.Contains(move))
			{
				Still_Playing = false;

				return true;
			}
			else if (stats_strings.Contains(move))
			{
				Display.PlayerStats();

				Game.do_display_update = false;

				return true;
			}

			if (Input.IsValid(move, out int x1, out int y1, out int x2, out int y2))
			{
				return TryToMove(x1, y1, x2, y2);
			}

			return false;
		}

		public void DisplayResult()
		{
			int height = fields.GetLength(0);
			int width = fields.GetLength(1);

			char[] characters1 = new char[width];
			char[] characters2 = new char[width];

			for (int i = 0; i < width; i++)
			{
				characters1[i] = (char)(65 + i);
				characters2[i] += '-';
			}

			string letter_label = new string(characters1);
			string dashes = new string(characters2);

			Console.WriteLine($"  {letter_label}");
			Console.WriteLine($"  {dashes}");

			for (int i = 0; i < height; i++)
			{
				Console.Write($"{i + 1}|");

				for (int j = 0; j < height; j++)
				{
					FieldEntry field_entry = fields[i, j];

					switch (field_entry.piece)
					{
						case FieldPiece.Nothing:
							Console.Write(" ");
							break;
						case FieldPiece.Man:
							if (field_entry.player_color == Player.Color.Light)
							{
								Console.Write("1");
								break;
							}
							else
							{
								Console.Write("2");
								break;
							}
						case FieldPiece.King:
							if (field_entry.player_color == Player.Color.Light)
							{
								Console.Write("I");
								break;
							}
							else
							{
								Console.Write("J");
								break;
							}
					}
				}

				Console.Write($"|{i + 1}");

				Console.WriteLine();
			}

			Console.WriteLine($"  {dashes}");
			Console.WriteLine($"  {letter_label}");
		}
	}
}