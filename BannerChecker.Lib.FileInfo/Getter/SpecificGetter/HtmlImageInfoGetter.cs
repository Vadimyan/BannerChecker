using System.Drawing;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	[ImageInfoGetter(".html")]
	class HtmlImageInfoGetter : ImageInfoGetterBase
	{
		protected override Size GetImageSize(string filePath)
		{
			var doc = new HtmlDocument();
			doc.Load(filePath);

			var objectNode = doc.DocumentNode.SelectSingleNode("//object");
			if (objectNode != null)
			{
				var width = int.Parse(objectNode.Attributes["width"].Value);
				var height = int.Parse(objectNode.Attributes["height"].Value);
				return new Size(width, height);
			}

			var swiffycontainerNode = doc.DocumentNode.SelectSingleNode("//div[@id='swiffycontainer']");
			if (swiffycontainerNode != null)
			{
				var style = swiffycontainerNode.Attributes["style"].Value;

				int width = int.MinValue;
				var widthRegex = new Regex("width: (?<widthValue>\\d+)px");
				var widthMatch = widthRegex.Match(style);
				if (widthMatch.Success)
				{
					width = int.Parse(widthMatch.Groups["widthValue"].Value);
				}

				int height = int.MinValue;
				var heightRegex = new Regex("height: (?<heightValue>\\d+)px");
				var heightMatch = heightRegex.Match(style);
				if (heightMatch.Success)
				{
					height = int.Parse(heightMatch.Groups["heightValue"].Value);
				}

				return new Size(width, height);
			}

			var scripts = doc.DocumentNode.Descendants("script");
			foreach (var script in scripts)
			{
				var scriptText = script.InnerText.Replace(System.Environment.NewLine, " ");

				int width = int.MinValue;
				var widthRegex = new Regex("width: \"(?<widthValue>\\d+)px\"");
				var widthMatch = widthRegex.Match(scriptText);
				if (widthMatch.Success)
				{
					width = int.Parse(widthMatch.Groups["widthValue"].Value);
				}

				int height = int.MinValue;
				var heightRegex = new Regex("height: \"(?<heightValue>\\d+)px\"");
				var heightMatch = heightRegex.Match(scriptText);
				if (heightMatch.Success)
				{
					height = int.Parse(heightMatch.Groups["heightValue"].Value);
				}

				if(width != int.MinValue && height != int.MinValue)
					return new Size(width, height);
			}

			return new Size();
		}
	}
}
