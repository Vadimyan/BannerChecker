using System;
using System.IO;
using System.Windows.Navigation;

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
		public ShockwaveFlashObjects.ShockwaveFlash AxShockwaveFlash;
		public SwfPreviewWindow(string filePath)
		{
			InitializeComponent();
			var uri = CreateTempHtmlPage(filePath);
			SwfHostFrame.Source = new Uri(new Uri(uri).AbsoluteUri);
			
		}

		private string CreateTempHtmlPage(string filePath)
		{
			var htmlText = string.Format(swfHtmlTemplate, filePath);
			string fileName = Path.GetTempPath() + Guid.NewGuid() + ".html";
			using (var file = File.CreateText(fileName))
			{
				file.Write(htmlText);
				file.Close();
			}
			return fileName;
		}

		private void SwfHostFrame_OnNavigated(object sender, NavigationEventArgs e)
		{
			//SetSilent(sender as WebBrowser, true); // make it silent
		}
	}
}
