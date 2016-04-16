using System;

namespace BannerChecker.Wpf.Views
{
	public partial class HtmlPreviewWindow
	{
		public HtmlPreviewWindow(string filePath)
		{
			InitializeComponent();
			HtmlHostFrame.Source = new Uri(new Uri(filePath).AbsoluteUri);
		}
	}
}
