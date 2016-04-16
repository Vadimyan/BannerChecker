using System.Drawing;
using System.Text.RegularExpressions;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	class SwiffyContainerSizeRetiever
	{
		public Size GetSwiffyContainerSize(string style)
		{
			var width = GetSwiffyContainerAttributeValue(style, "width");
			var height = GetSwiffyContainerAttributeValue(style, "height");
			return new Size(width, height);
		}


		private static int GetSwiffyContainerAttributeValue(string style, string attributeName)
		{
			var match = MatchAttributeRegex(style, attributeName);
			return match.Success ? int.Parse(match.Groups[$"{attributeName}"].Value) : int.MinValue;
		}

		private static Match MatchAttributeRegex(string style, string attributeName)
		{
			var regex = new Regex($"{attributeName}: (?<{attributeName}>\\d+)px");
			return regex.Match(style);
		}
	}
}