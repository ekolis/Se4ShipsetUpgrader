using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using static System.ConsoleColor;
using static Se4ShipsetUpgrader.Result;

namespace Se4ShipsetUpgrader
{
	class Program
	{
		static void Main(string[] args)
		{
			string shipset;
			if (args.Length > 0)
				shipset = args[0];
			else
			{
				BackgroundColor = Black;
				ForegroundColor = White;
				WriteLine("Shipset path?");
				shipset = ReadLine();
				if (!Directory.Exists(shipset))
				{
					BackgroundColor = DarkRed;
					ForegroundColor = White;
					WriteLine($"Shipset {Path.Combine(Directory.GetCurrentDirectory(), shipset)} does not exist!");

					BackgroundColor = Black;
					ForegroundColor = White;
#if DEBUG
					WriteLine("We're in debug mode so press any key...");
					ReadKey();
#endif
					return;
				}
			}

			var rules = RuleList.LoadMany(File.ReadAllText("Config.txt"));

			foreach (var rl in rules)
			{
				BackgroundColor = DarkCyan;
				ForegroundColor = White;
				WriteLine($"Executing rule list {rl.Name}...");
				foreach (var r in rl)
				{
					if (r.BaseRuleGraphic == null)
					{
						BackgroundColor = Black;
						ForegroundColor = DarkGray;
						WriteLine($"\t{r.DesiredGraphic}: nothing to do, no base rule.");
						continue; // nothing to execute
					}

					var result = r.Execute(shipset);
					if (result == Present) // TODO - let user specify shipset name
					{
						// graphics were found, everything is OK
						BackgroundColor = Black;
						ForegroundColor = Gray;
						WriteLine($"\t{r.DesiredGraphic}: nothing to do, graphics already exist.");
					}
					else if (result == Copied)
					{
						// graphics were successfully copied
						BackgroundColor = DarkBlue;
						ForegroundColor = White;
						WriteLine($"\t{r.DesiredGraphic}: copied graphics from {r.BaseRuleGraphic}.");
					}
					else if (result == Missing)
					{
						if (r.IsRequired)
						{
							// graphics were required and could not be found or copied!
							BackgroundColor = DarkRed;
							ForegroundColor = White;
							WriteLine($"\t{r.DesiredGraphic}: required graphics were not found and could not be copied from {r.BaseRuleGraphic ?? "(no available source)"}!");
						}
						else
						{
							// graphics couldn't be found but it's OK, they're optional anyway
							BackgroundColor = Black;
							ForegroundColor = White;
							WriteLine($"\t{r.DesiredGraphic}: graphics were not found and could not be copied from {r.BaseRuleGraphic ?? "(no available source)"}.");
						}
					}
				}
			}

			BackgroundColor = Black;
			ForegroundColor = White;
#if DEBUG
			WriteLine("We're in debug mode so press any key...");
			ReadKey();
#endif
		}
	}
}
