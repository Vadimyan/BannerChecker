using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using BannerChecker.Lib.FileInfo;
using BannerChecker.Lib.FileInfo.Getter;
using BannerChecker.Wpf.Common;
using BannerChecker.Wpf.Models;

namespace BannerChecker.Wpf.ViewModels
{
	public class DirectoryInfoViewModel : ViewModelBase
	{
		private readonly IImageInfoGetter _imageInfoGetter;
		private readonly ImageDialogService _imageDialogService;
		private string _directoryPath;
		private ObservableCollection<ImageInfo> directoryFilesInfo;

		public ICommand ShowImageCommand { get; private set; }

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

		protected DirectoryInfoViewModel()
		{
			ShowImageCommand = new RelayCommand<ImageInfo>(ShowImage);
		}
		
		public DirectoryInfoViewModel(IImageInfoGetter imageInfoGetter, ImageDialogService imageDialogService)
			: this()
		{
			_imageInfoGetter = imageInfoGetter;
			_imageDialogService = imageDialogService;
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

		private void ShowImage(ImageInfo image)
		{
			_imageDialogService.ShowImage(Path.Combine(image.DirectoryName, Path.ChangeExtension(image.FileName, image.FileExtension)));
		}
	}
}
