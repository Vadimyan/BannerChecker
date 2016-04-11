using System;
using System.IO;

namespace BannerChecker.Wpf.Views
{
	/// <summary>
	/// Interaction logic for SwfPreviewWindow.xaml
	/// </summary>
	public partial class SwfPreviewWindow
	{
		private static string swfHtmlTemplate = @"<!DOCTYPE html>
<!-- saved from url=(0014)about:internet -->
<html>
	<hed>
		<meta http-equiv=""X-UA-Compatible"" content=""IE=9"" >
	</head>
	<body>
		<embed src=""{0}"" width=""100%"" height=""100%"" SCALE=""exactfit"">
	</body>
</html>
";

		public SwfPreviewWindow(string filePath)
		{
			InitializeComponent();
			LoadSwf(filePath);
		}

		private void LoadSwf(string filePath)
		{
			var uri = CreateTempHtmlPage(filePath);
			SwfHostFrame.Source = new Uri(new Uri(uri).AbsoluteUri);
		}

		private string CreateTempHtmlPage(string swfFilePath)
		{
			string fileName = GetTempFileName();
			CreateTempHtmlPage(swfFilePath, fileName);
			return fileName;
		}

		private static void CreateTempHtmlPage(string swfFilePath, string fileName)
		{
			using (var file = File.CreateText(fileName))
				file.Write(GetHtmlContentFromTemplate(swfFilePath));
		}

		private static string GetHtmlContentFromTemplate(string filePath)
		{
			return string.Format(swfHtmlTemplate, filePath);
		}

		private static string GetTempFileName()
		{
			return Path.GetTempPath() + Guid.NewGuid() + ".html";
		}
	}
}
