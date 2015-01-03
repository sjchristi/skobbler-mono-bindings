using System;
using System.Text;
using System.Linq;
using System.Globalization;

namespace SKMapUtil
{
	public static class NumberExtensions
	{
		// Function to down-case a string and remove all diacritics from characters in a string
		public static double ToRadians(this double number) {
			return number * (Math.PI / 180.0);
		}
	}
}

