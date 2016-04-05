using System.Collections.ObjectModel;
using BannerChecker.Lib.FileInfo;
using BannerChecker.Wpf.Common;

namespace BannerChecker.Wpf.ViewModels
{
	public class DirectoryInfoViewModel : ViewModelBase
	{
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

		public DirectoryInfoViewModel()
		{
			
		}

		private void UpdateDirectoryFilesInfo(string value)
		{
			throw new System.NotImplementedException();
		}
	}
}
