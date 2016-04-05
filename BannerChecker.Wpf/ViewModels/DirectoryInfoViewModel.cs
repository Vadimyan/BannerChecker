using System.Collections.ObjectModel;
using System.IO;
using BannerChecker.Lib.FileInfo;
using BannerChecker.Lib.FileInfo.Getter;
using BannerChecker.Wpf.Common;

namespace BannerChecker.Wpf.ViewModels
{
	public class DirectoryInfoViewModel : ViewModelBase
	{
		private readonly IImageInfoGetter _imageInfoGetter;
		private string _directoryPath;
		public string DirectoryPath
		{
			get { return _directoryPath; }
			set
			{
				_directoryPath = value;
				OnPropertyChanged();
				UpdateDirectoryFilesInfo(value);
			}
		}
		
		public ObservableCollection<ImageInfo> DirectoryFilesInfo { get; set; } = new ObservableCollection<ImageInfo>();

		public DirectoryInfoViewModel(IImageInfoGetter imageInfoGetter)
		{
			_imageInfoGetter = imageInfoGetter;
		}

		private void UpdateDirectoryFilesInfo(string directoryPath)
		{
			if (Directory.Exists(directoryPath))
				UpdateFilesInfo(Directory.GetFiles(directoryPath));
		}

		private void UpdateFilesInfo(string[] directoryFiles)
		{
			DirectoryFilesInfo.Clear();
			FillFilesInfo(directoryFiles);
		}

		private void FillFilesInfo(string[] directoryFiles)
		{
			foreach (var directoryFile in directoryFiles)
				DirectoryFilesInfo.Add(_imageInfoGetter.GetInfo(directoryFile));
		}
	}
}
