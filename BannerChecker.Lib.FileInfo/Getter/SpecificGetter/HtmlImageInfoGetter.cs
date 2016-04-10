using System.Drawing;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	[ImageInfoGetter(".html")]
	class HtmlImageInfoGetter : ImageInfoGetterBase
	{
		protected override Size GetImageSize(string filePath)
		{
			return new Size();
		}
	}
}
