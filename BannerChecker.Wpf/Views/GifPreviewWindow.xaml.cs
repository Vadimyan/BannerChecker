using System;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace BannerChecker.Wpf.Views
{
	/// <summary>
	/// Interaction logic for GifPreviewWindow.xaml
	/// </summary>
	public partial class GifPreviewWindow
	{
		public GifPreviewWindow(string filePath)
		{
			InitializeComponent();
			LoadGif(filePath);
		}

		private void LoadGif(string filePath)
		{
			var image = GetImageSource(filePath);
			ImageBehavior.SetAnimatedSource(SourceImage, image);
		}

		private static BitmapImage GetImageSource(string filePath)
		{
			var image = new BitmapImage();
			image.BeginInit();
			image.UriSource = new Uri(filePath);
			image.EndInit();
			return image;
		}
	}
}
