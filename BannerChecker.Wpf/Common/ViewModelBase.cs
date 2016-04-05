using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BannerChecker.Wpf.Common
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		public void Initialize()
		{
			try
			{
				PerformInitialize();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Ошибка при инициализации {0}: {1}\n{2}", GetType(), ex.Message, ex.StackTrace);
				throw;
			}
		}

		public void Uninitialize()
		{
			try
			{
				PerformUninitialize();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Ошибка при деинициализации {0}: {1}\n{2}", GetType(), ex.Message, ex.StackTrace);
				throw;
			}
		}

		protected void PerformInitialize()
		{
		}

		protected virtual void PerformUninitialize()
		{
		}


		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
