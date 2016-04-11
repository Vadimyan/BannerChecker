using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using BannerChecker.Lib.FileInfo;
using BannerChecker.Lib.FileInfo.Getter;
using BannerChecker.Wpf.Common;

namespace BannerChecker.Wpf.ViewModels
{
	public class DirectoryInfoViewModel : ViewModelBase
	{
		private readonly IImageInfoGetter _imageInfoGetter;
		private string _directoryPath;
		private ObservableCollection<ImageInfo> directoryFilesInfo;

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

		public ObservableCollection<ImageInfo> DirectoryFilesInfo
		{
			get { return directoryFilesInfo; }
			set
			{
				directoryFilesInfo = value;
				OnPropertyChanged();
			}
		}

		public DirectoryInfoViewModel(IImageInfoGetter imageInfoGetter)
		{
			_imageInfoGetter = imageInfoGetter;
		}

		private void UpdateDirectoryFilesInfo(string directoryPath)
		{
			if (Directory.Exists(directoryPath))
				UpdateFilesList(directoryPath);
		}

		private void UpdateFilesList(string directoryPath)
		{
			var imageInfos = GetAllNestedFiles(directoryPath)
				.Select(df => _imageInfoGetter.GetInfo(df))
				.Where(info => info != null);
			DirectoryFilesInfo = new ObservableCollection<ImageInfo>(imageInfos);
		}

		private static IEnumerable<string> GetAllNestedFiles(string directoryPath)
		{
			return Directory
				.GetDirectories(directoryPath)
				.SelectMany(GetAllNestedFiles)
				.Union(Directory.GetFiles(directoryPath));
		}
	}
}
