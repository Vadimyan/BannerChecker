namespace BannerChecker.Lib.FileInfo
{
    public class ImageInfo
    {
		public string FileName { get; }
	    public int? Width { get; }
		public int? Height { get; }
		public int Size { get; }

		public ImageInfo(string fileName, int size, int? width = null, int? height = null)
		{
			FileName = fileName;
			Width = width;
			Height = height;
			Size = size;
		}
	}
}
