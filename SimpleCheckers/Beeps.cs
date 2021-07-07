using System;

namespace SimpleCheckers
{
	public static class Beeps
	{
		public enum Frequency
		{
			Low = 200,
			Medium = 500,
            High = 1000,
			Highest = 1500
		}

		public enum Duration
		{
			Shortest = 25,
			Shorter = 50,
			Short = 150,
			Medium = 300,
			Long = 450
		}

		private static void Play(Frequency f, Duration d)
		{
			Console.Beep((int)f, (int)d);
		}

		private static void Wait(Duration d)
		{
			System.Threading.Thread.Sleep((int)d);
		}

		public static void PlayMoveBeeps()
		{
			Play(Frequency.High, Duration.Shorter);
		}

		public static void PlayKillBeeps()
		{
			Play(Frequency.Low, Duration.Short);
		}

		public static void PlayMandatoryJumpError()
		{
			Play(Frequency.Low, Duration.Shorter);
			Wait(Duration.Shorter);
			Play(Frequency.Low, Duration.Shorter);
			Wait(Duration.Shorter);
			Play(Frequency.Low, Duration.Shorter);
		}
	}
}