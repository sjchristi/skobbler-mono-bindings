using System;
using System.Text;
using System.Linq;
using System.Globalization;

namespace SKMapUtil
{
	public static class StringExtensions
	{
		// Function to down-case a string and remove all diacritics from characters in a string
		public static string ToLowerRemoveDiacritics(this string text) {
			string ntext = new string(text.ToLowerInvariant().Normalize(NormalizationForm.FormD).Where(c=>c<128).ToArray());
			return ntext;
		}

		// Returns true if the string matches
		public static bool MatchesIgnoreDiacritics(this string text, string compareWith) {
			string ntext = text.ToLowerRemoveDiacritics ();
			int i = ntext.IndexOf(compareWith, StringComparison.InvariantCultureIgnoreCase);
			return (i >= 0);
		}
	}
}

