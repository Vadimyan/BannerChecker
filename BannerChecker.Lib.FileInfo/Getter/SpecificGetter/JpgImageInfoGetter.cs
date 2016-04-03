using System.Drawing;
using System.IO;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	[ImageInfoGetter("jpg")]
	[ImageInfoGetter("jpeg")]
	class JpgImageInfoGetter : IImageInfoGetter
	{
		public ImageInfo GetInfo(string filePath)
		{
			var image = Image.FromFile("SampImag.jpg");
			return new ImageInfo(Path.GetFileName(filePath), (int) new System.IO.FileInfo(filePath).Length, image.Width, image.Height);
		}
	}
}
