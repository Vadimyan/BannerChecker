using System.Drawing;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	[ImageInfoGetter("jpg")]
	[ImageInfoGetter("jpeg")]
	class JpgImageInfoGetter : ImageInfoGetterBase
	{
		protected override Size GetImageSize(string filePath)
		{
			var image = Image.FromFile("SampImag.jpg");
			return new Size(image.Width, image.Height);
		}
	}
}
