using System;

namespace BannerChecker.Lib.FileInfo.Getter
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	class ImageInfoGetterAttribute : Attribute
	{
		public string FileExtrnsion { get; }

		public ImageInfoGetterAttribute(string fileExtrnsion)
		{
			FileExtrnsion = fileExtrnsion;
		}
	}
}
