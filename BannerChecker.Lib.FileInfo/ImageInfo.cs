using System.IO;

namespace BannerChecker.Lib.FileInfo
{
    public class ImageInfo
    {
		public string FilePath { get; }
		public string FileName { get; }
	    public int? Width { get; }
		public int? Height { get; }
		public int Size { get; }

		public ImageInfo(string filePath, int size, int? width = null, int? height = null)
		{
			FilePath = filePath;
			FileName = Path.GetFileName(filePath);
			Width = width;
			Height = height;
			Size = size;
		}
	}
}
