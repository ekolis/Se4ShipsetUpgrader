using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Se4ShipsetUpgrader
{
	/// <summary>
	/// Result of executing a rule.
	/// </summary>
	public enum Result
	{
		/// <summary>
		/// Graphics were already present.
		/// </summary>
		Present = 2,

		/// <summary>
		/// Graphics were successfully copied.
		/// </summary>
		Copied = 1,

		/// <summary>
		/// Graphics could not be successfully copied.
		/// </summary>
		Missing = 0,
	}
}
