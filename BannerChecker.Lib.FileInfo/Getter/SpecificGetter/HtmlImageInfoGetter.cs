using System.Drawing;
using HtmlAgilityPack;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	[ImageInfoGetter(".html")]
	class HtmlImageInfoGetter : ImageInfoGetterBase
	{
		private readonly SwiffyContainerSizeRetiever _swiffyContainerSizeRetiever;
		private readonly ScriptStyleSizeRetriever _styleSizeRetriever;

		public HtmlImageInfoGetter()
		{
			_swiffyContainerSizeRetiever = new SwiffyContainerSizeRetiever();
			_styleSizeRetriever = new ScriptStyleSizeRetriever();
		}

		protected override Size GetImageSize(string filePath)
		{
			var documentRootNode = LooadDocumentRootNode(filePath);
			return GetImageSize(documentRootNode);
		}

		private Size GetImageSize(HtmlNode documentRootNode)
		{
			var objectNode = documentRootNode.SelectSingleNode("//object");
			if (objectNode != null)
			{
				return GetEmbeddedObjectSize(objectNode);
			}

			var swiffycontainerNode = documentRootNode.SelectSingleNode("//div[@id='swiffycontainer']");
			if (swiffycontainerNode != null)
			{
				return GetSwiffyContainerSize(swiffycontainerNode);
			}

			var scripts = documentRootNode.Descendants("script");
			foreach (var script in scripts)
			{
				Size? imageSize = _styleSizeRetriever.GetScriptStyleSize(script);
				if (imageSize.HasValue)
					return imageSize.Value;
			}

			return new Size();
		}

		private Size GetSwiffyContainerSize(HtmlNode swiffycontainerNode)
		{
			var style = swiffycontainerNode.Attributes["style"].Value;
			return _swiffyContainerSizeRetiever.GetSwiffyContainerSize(style);
		}
		
		private static Size GetEmbeddedObjectSize(HtmlNode objectNode)
		{
			return new Size(int.Parse(objectNode.Attributes["width"].Value), int.Parse(objectNode.Attributes["height"].Value));
		}

		private static HtmlNode LooadDocumentRootNode(string filePath)
		{
			var doc = LoadHtmlDocument(filePath);
			return doc.DocumentNode;
		}

		private static HtmlDocument LoadHtmlDocument(string filePath)
		{
			var doc = new HtmlDocument();
			doc.Load(filePath);
			return doc;
		}
	}
}
