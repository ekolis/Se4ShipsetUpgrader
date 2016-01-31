using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Se4ShipsetUpgrader
{
	public class RuleList : KeyedCollection<string, Rule>
	{
		public RuleList(string name)
			: base()
		{
			Name = name;
		}

		public string Name { get; set; }

		protected override string GetKeyForItem(Rule item)
		{
			return item.DesiredGraphic;
		}

		/// <summary>
		/// Loads any number of rule lists.
		/// </summary>
		/// <param name="s">The string to load from.</param>
		/// <returns>The rule lists.</returns>
		public static IEnumerable<RuleList> LoadMany(string s)
		{
			s = s.Replace("\r\n", "\n"); // normalize line endings
			var breaks = new string[] { "\n\n" };
			var spl = s.Split(breaks, StringSplitOptions.RemoveEmptyEntries);
			foreach (var x in spl)
				yield return Load(x);
		}

		/// <summary>
		/// Loads a single rule list.
		/// </summary>
		/// <param name="s">The string to load from.</param>
		/// <returns></returns>
		public static RuleList Load(string s)
		{
			var spl = s.Split('\n');
			var name = spl[0].Trim('[', ']');
			var list = new RuleList(name);
			foreach (var x in spl.Skip(1))
				list.Add(Rule.Load(x));
			return list;
		}
	}
}
