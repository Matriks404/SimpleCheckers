using System;

namespace SimpleCheckers
{
	public static class Config
	{
		public static bool beeps_enabled = false;
		public static bool clear_enabled = true;
		public static bool instructions_enabled = true;
		public static bool play_recorded_game = false;
		public static bool printing_jumps_enabled = true;

        public static string recorded_game_name = String.Empty;

		// Game rules
		public static bool jumps_mandatory = true;
	}
}