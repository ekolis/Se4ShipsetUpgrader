using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Se4ShipsetUpgrader
{
	/// <summary>
	/// A rule for copying stock or modded graphics to create missing graphics.
	/// </summary>
	public class Rule
	{
		public Rule(string desiredG, string baseG, bool isRequired)
		{
			if (desiredG == null)
				throw new ArgumentNullException(nameof(desiredG), "Rules require a desired graphic.");

			DesiredGraphic = desiredG;
			BaseRuleGraphic = baseG;
			IsRequired = isRequired;
		}

		/// <summary>
		/// The name of the desired graphic, e.g. "LightCruiser".
		/// </summary>
		/// <value>
		/// The desired graphic.
		/// </value>
		public string DesiredGraphic { get; set; }

		/// <summary>
		/// The rule specifying where the graphic comes from if it's not already present.
		/// </summary>
		/// <value>
		/// The base rule.
		/// </value>
		public string BaseRuleGraphic { get; set; }

		/// <summary>
		/// Is this rule's graphic required for normal game operation?
		/// Basically, is it in the list of graphics required by stock?
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is required; otherwise, <c>false</c>.
		/// </value>
		public bool IsRequired { get; set; }

		public Result Execute(string shipsetName)
		{
			var pr = Execute(shipsetName, "Portrait");
			var mr = Execute(shipsetName, "Mini");
			if (pr < mr)
				return pr;
			else
				return mr;
		}

		public Result Execute(string shipsetName, string portraitOrMini)
		{
			var file = Path.Combine(shipsetName, $"{shipsetName}_{portraitOrMini}_{DesiredGraphic}.bmp");
			if (File.Exists(file))
				return Result.Present;
			var bfile = Path.Combine(shipsetName, $"{shipsetName}_{portraitOrMini}_{BaseRuleGraphic}.bmp");
			if (File.Exists(bfile))
			{
				File.Copy(bfile, file);
				return Result.Copied;
			}
			return Result.Missing;
		}

		public static Rule Load(string s)
		{
			var spl = s.Split(':');
			var dg = spl.Length >= 1 ? spl[0].Trim() : null;
			var bg = spl.Length >= 2 ? spl[1].Trim() : null;
			if (dg.StartsWith("!")) // ! symbol indicates requiredness
				return new Rule(dg.Substring(1), bg, true);
			else
				return new Rule(dg, bg, false);
		}
	}
}
