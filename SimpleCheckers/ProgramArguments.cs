using System;
using System.Collections.Generic;

namespace SimpleCheckers
{
	public static class ProgramArguments
	{
		public enum ProgramArgument
		{
			Beeps,
			DontPrintJumps,
			Help,
			NoClear,
			NoInstructions,
			NoMandatoryJumps,
			RecordedGamePlay,
		}

		private static HashSet<ProgramArgument> program_arguments = new HashSet<ProgramArgument>();

		private static void Add()
		{
			string[] args = Environment.GetCommandLineArgs();

			for (int i = 1; i < args.Length; i++)
			{
				string argument = args[i];

				if (argument.StartsWith("-game-play="))
				{
					string name = argument.Substring(argument.IndexOf('=') + 1);

					if (name.Length > 0)
					{
						program_arguments.Add(ProgramArgument.RecordedGamePlay);

						Config.recorded_game_name = name;
					}
					else
					{
						Console.WriteLine("Error: You must specify a name for -game-play= argument!");

						Environment.Exit(0);
					}

					continue;
				}

				switch (argument)
				{
					case "-beeps":
						program_arguments.Add(ProgramArgument.Beeps);

						break;
					case "-cmdlist":
						program_arguments.Add(ProgramArgument.Help);

						break;
					case "-dont-print-jumps":
						program_arguments.Add(ProgramArgument.DontPrintJumps);

						break;
					case "-help":
						program_arguments.Add(ProgramArgument.Help);

						break;
					case "-no-auto-clear":
						program_arguments.Add(ProgramArgument.NoClear);

						break;
					case "-no-instructions":
						program_arguments.Add(ProgramArgument.NoInstructions);

						break;
					case "-no-mandatory-jumps":
						program_arguments.Add(ProgramArgument.NoMandatoryJumps);

						break;
					case "-sound":
						program_arguments.Add(ProgramArgument.Beeps);

						break;
					default:
						Console.WriteLine($"Error: Invalid argument: { argument }, launch with '-cmdlist' or '-help' argument to get some help!");

						Environment.Exit(0);

						break;
				}
			}
		}

		public static void Handle()
		{
			Add();

			foreach (ProgramArgument argument in program_arguments)
			{
				switch (argument)
				{
					case ProgramArgument.Beeps:
						if (Environment.OSVersion.Platform == PlatformID.Win32NT)
						{
							Config.beeps_enabled = true;
						}
						else
						{
							Console.WriteLine("Error: Beeps (sound) are only available on Windows platform.");

							Environment.Exit(0);
						}

						break;
					case ProgramArgument.DontPrintJumps:
						Config.printing_jumps_enabled = false;

                        break;
					case ProgramArgument.Help:
						Display.Help();

						Environment.Exit(0);

						break;
					case ProgramArgument.NoClear:
						Config.clear_enabled = false;

						break;
					case ProgramArgument.NoInstructions:
						Config.instructions_enabled = false;

						break;
					case ProgramArgument.NoMandatoryJumps:
						Config.jumps_mandatory = false;

						break;
					case ProgramArgument.RecordedGamePlay:
						Config.play_recorded_game = true;

						break;
					default:
						throw new NotImplementedException();

				}
			}
		}
	}
}