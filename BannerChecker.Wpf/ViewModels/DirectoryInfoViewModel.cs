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
				UpdateFilesInfo(GetAllNestedFiles(directoryPath));
		}

		private static IEnumerable<string> GetAllNestedFiles(string directoryPath)
		{
			return Directory
				.GetDirectories(directoryPath)
				.SelectMany(GetAllNestedFiles)
				.Union(Directory.GetFiles(directoryPath));
		}

		private void UpdateFilesInfo(IEnumerable<string> directoryFiles)
		{
			DirectoryFilesInfo.Clear();
			FillFilesInfo(directoryFiles);
		}

		private void FillFilesInfo(IEnumerable<string> directoryFiles)
		{
			foreach (var directoryFile in directoryFiles)
				TryAddImageFileInfo(directoryFile);
		}

		private void TryAddImageFileInfo(string directoryFile)
		{
			var imageInfo = _imageInfoGetter.GetInfo(directoryFile);
			TryAddImageFileInfo(imageInfo);
		}

		private void TryAddImageFileInfo(ImageInfo imageInfo)
		{
			if (imageInfo != null)
				DirectoryFilesInfo.Add(imageInfo);
		}
	}
}
