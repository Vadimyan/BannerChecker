using System.IO;

namespace BannerChecker.Lib.FileInfo
{
    public class ImageInfo
    {
		public string DirectoryName { get; }
		public string FileName { get; }
		public string FileExtension { get; }
		public int? Width { get; set; }
		public int? Height { get; set; }
		public int Size { get; set; }

		public ImageInfo(string filePath, int size, int? width = null, int? height = null)
		{
			DirectoryName = Path.GetDirectoryName(filePath);
			FileName = Path.GetFileNameWithoutExtension(filePath);
			FileExtension = Path.GetExtension(filePath);
			Width = width;
			Height = height;
			Size = size;
		}

	    
    }
}
