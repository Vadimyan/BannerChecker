using System.Drawing;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	class ScriptStyleSizeRetriever
	{
		public Size? GetScriptStyleSize(HtmlNode script)
		{
			var scriptText = script.InnerText.Replace(System.Environment.NewLine, " ");

			int width = GetScriptStyleAttributeValue(scriptText, "width");
			int height = GetScriptStyleAttributeValue(scriptText, "height");

			if (width != int.MinValue && height != int.MinValue)
			{
				return new Size(width, height);
			}
			return null;
		}

		private static int GetScriptStyleAttributeValue(string scriptText, string attributeName)
		{
			var widthMatch = MatchStyleAttributeRegex(scriptText, attributeName);
			return widthMatch.Success ? int.Parse(widthMatch.Groups[$"{attributeName}"].Value) : int.MinValue;
		}

		private static Match MatchStyleAttributeRegex(string scriptText, string attributeName)
		{
			var widthRegex = new Regex($"{attributeName}: \"(?<{attributeName}>\\d+)px\"");
			return widthRegex.Match(scriptText);
		}
	}
}