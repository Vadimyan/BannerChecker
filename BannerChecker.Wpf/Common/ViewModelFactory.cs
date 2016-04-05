﻿using System;
using System.Collections.Generic;

namespace BannerChecker.Wpf.Common
{
	public static class ViewModelFactory
	{
		private static readonly Dictionary<Type, Func<ViewModelBase>> builders = new Dictionary<Type, Func<ViewModelBase>>
		{
			//{ typeof (CloudServicesViewModel), CreateCloudServicesViewModel },
		};

		public static TViewModel Resolve<TViewModel>()
			where TViewModel : ViewModelBase
		{
			var type = typeof(TViewModel);
			return TryResolve<TViewModel>(type);

			
		}

		private static TViewModel TryResolve<TViewModel>(Type type) where TViewModel : ViewModelBase
		{
			if (builders.ContainsKey(type))
			{
				return Resolve<TViewModel>(type);
			}
			throw new NotSupportedException();
		}

		private static TViewModel Resolve<TViewModel>(Type type) where TViewModel : ViewModelBase
		{
			var viewModel = builders[type]();
			viewModel.Initialize();
			return (TViewModel) viewModel;
		}

		//private static ViewModelBase CreateSettingsViewModel()
		//{
		//	var settingsProvider = new IsolatedStorageSettingsProvider();
		//	var viewModel = new SettingsViewModel(settingsProvider);
		//	return viewModel;
		//}
	}
}