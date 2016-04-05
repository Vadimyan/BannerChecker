using System.Drawing;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	[ImageInfoGetter(".jpg")]
	[ImageInfoGetter(".jpeg")]
	[ImageInfoGetter(".png")]
	[ImageInfoGetter(".gif")]
	class BitmapImageInfoGetter : ImageInfoGetterBase
	{
		protected override Size GetImageSize(string filePath)
		{
			var image = Image.FromFile(filePath);
			return new Size(image.Width, image.Height);
		}
	}
}
