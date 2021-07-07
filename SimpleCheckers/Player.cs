using System;

namespace SimpleCheckers
{
	public class Player
	{
		public enum Color
        {
            None = 0,
			Light = 1,
			Dark = 2
		}

		public Color Player_Color { get; private set; }
		public int Pieces_Count { get; set; }

		public Player(Color pc)
		{
			Player_Color = pc;
		}

		public static Color Current_Player;

		public static Color Other_Player
		{
			get => (Current_Player == Color.Light) ? Color.Dark : Color.Light;
		}
	}
}