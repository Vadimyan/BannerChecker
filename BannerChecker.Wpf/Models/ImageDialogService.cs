using System;
using System.Collections.Generic;
using System.IO;
using BannerChecker.Wpf.Views;

namespace BannerChecker.Wpf.Models
{
	public class ImageDialogService
	{
		private readonly Dictionary<string, Action<string>> imageDialogOpeners = new Dictionary<string, Action<string>>()
		{
			{ ".jpg", ShowBitmapImage },
			{ ".jpeg", ShowBitmapImage },
			{ ".png", ShowBitmapImage },
			{ ".gif", ShowBitmapImage },
			{".swf", ShowSwfImage },
			{".html", ShowHtmlImage }
		};
		
		public void ShowImage(string filePath)
		{
			if (imageDialogOpeners.ContainsKey(Path.GetExtension(filePath)))
				imageDialogOpeners[Path.GetExtension(filePath)](filePath);
		}

		private static void ShowBitmapImage(string filePath)
		{
			var window = new ImagePreviewWindow(filePath);
			window.ShowDialog();
		}

		private static void ShowSwfImage(string filePath)
		{
			var window = new SwfPreviewWindow(filePath);
			window.ShowDialog();
		}

		private static void ShowHtmlImage(string filePath)
		{
			var window = new HtmlPreviewWindow(filePath);
			window.ShowDialog();
		}
	}
}
