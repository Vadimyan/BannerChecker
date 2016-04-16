using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BannerChecker.Lib.FileInfo.Getter.SpecificGetter.Html;
using HtmlAgilityPack;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	[ImageInfoGetter(".html")]
	class HtmlImageInfoGetter : ImageInfoGetterBase
	{
		private readonly SwiffyContainerSizeRetiever _swiffyContainerSizeRetiever;
		private readonly ScriptStyleSizeRetriever _styleSizeRetriever;
		private readonly List<Func<HtmlNode, Size?>> _sizeRetrievers;

		public HtmlImageInfoGetter()
		{
			_swiffyContainerSizeRetiever = new SwiffyContainerSizeRetiever();
			_styleSizeRetriever = new ScriptStyleSizeRetriever();
			_sizeRetrievers = new List<Func<HtmlNode, Size?>>
			{
				TryGetObjectNodeSize, TryGetSwiffyContainerSize, TryGetScriptStyleSize
			};
		}

		protected override Size GetImageSize(string filePath)
		{
			var documentRootNode = LooadDocumentRootNode(filePath);
			return GetImageSize(documentRootNode);
		}

		private Size GetImageSize(HtmlNode documentRootNode)
		{
			return _sizeRetrievers
				.Select(x => x(documentRootNode))
				.FirstOrDefault(x => x.HasValue) 
				?? new Size();

		}

		private Size? TryGetScriptStyleSize(HtmlNode documentRootNode)
		{
			return documentRootNode
				.Descendants("script")
				.Select(script => _styleSizeRetriever.GetScriptStyleSize(script))
				.FirstOrDefault(imageSize => imageSize.HasValue);
		}

		private Size? TryGetSwiffyContainerSize(HtmlNode documentRootNode)
		{
			var swiffycontainerNode = documentRootNode.SelectSingleNode("//div[@id='swiffycontainer']");
			return swiffycontainerNode != null ? GetSwiffyContainerSize(swiffycontainerNode) : null;
		}

		private static Size? TryGetObjectNodeSize(HtmlNode documentRootNode)
		{
			var objectNode = documentRootNode.SelectSingleNode("//object");
			return objectNode != null ? GetEmbeddedObjectSize(objectNode) : null;
		}

		private Size? GetSwiffyContainerSize(HtmlNode swiffycontainerNode)
		{
			var style = swiffycontainerNode.Attributes["style"].Value;
			return _swiffyContainerSizeRetiever.GetSwiffyContainerSize(style);
		}
		
		private static Size? GetEmbeddedObjectSize(HtmlNode objectNode)
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
