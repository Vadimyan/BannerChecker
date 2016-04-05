using System.Windows;
using System.Windows.Controls;

namespace BannerChecker.Wpf.Common
{
	public abstract class ViewBase<TViewModel> : UserControl
		where TViewModel : ViewModelBase
	{
		protected ViewBase()
		{
			Loaded += ViewLoaded;
			Unloaded += ViewUnloaded;
		}

		private void ViewLoaded(object sender, RoutedEventArgs e)
		{
			DataContext = ViewModelFactory.Resolve<TViewModel>();
		}

		private void ViewUnloaded(object sender, RoutedEventArgs e)
		{
			var viewModel = DataContext as ViewModelBase;
			TryCleanUp(viewModel);
		}

		private void TryCleanUp(ViewModelBase viewModel)
		{
			if (viewModel != null)
				CleanUp(viewModel);
		}

		private void CleanUp(ViewModelBase viewModel)
		{
			viewModel.Uninitialize();
			DataContext = null;
		}
	}
}
