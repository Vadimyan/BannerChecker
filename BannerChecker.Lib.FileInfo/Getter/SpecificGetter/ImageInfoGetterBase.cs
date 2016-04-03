using System.Drawing;
using System.IO;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	internal abstract class ImageInfoGetterBase : IImageInfoGetter
	{
		public ImageInfo GetInfo(string filePath)
		{
			var imageSize = GetImageSize(filePath);
			return new ImageInfo(Path.GetFileName(filePath), (int)new System.IO.FileInfo(filePath).Length, imageSize.Width, imageSize.Height);
		}

		protected abstract Size GetImageSize(string filePath);
	}
}
