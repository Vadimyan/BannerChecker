using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace BannerChecker.Wpf.Common
{
	public abstract class CommandBase : INotifyPropertyChanged, ICommand
	{
		private class Listener<TEventArgs> : IWeakEventListener where TEventArgs : EventArgs
		{
			private readonly EventHandler<TEventArgs> realHandler;

			public Listener(EventHandler<TEventArgs> handler)
			{
				realHandler = handler;
			}

			bool IWeakEventListener.ReceiveWeakEvent(Type managerType, Object sender, EventArgs e)
			{
				ExecuteRealHandler(sender, e);
				return true;
			}

			private void ExecuteRealHandler(object sender, EventArgs e)
			{
				var realArgs = (TEventArgs) e;
				realHandler(sender, realArgs);
			}
		}

		private readonly Listener<PropertyChangedEventArgs> weakEventListener;

		public event EventHandler CanExecuteChanged;

		protected CommandBase()
		{
			weakEventListener = new Listener<PropertyChangedEventArgs>(RequeryCanExecute);
		}

		public abstract bool CanExecute(object parameter);
		protected abstract void PerformExecute(object parameter);

		public void Execute(object parameter)
		{
			BeforeExecure();
			PerformExecute(parameter);
			AfrerExecute();
		}

		public virtual void InvalidateCommand()
		{
			RaiseCanExecuteChanged();
		}

		protected void AddListenerInternal<TEntity>(TEntity source, string propertyName)
			where TEntity : INotifyPropertyChanged
		{
			PropertyChangedEventManager.AddListener(source, weakEventListener, propertyName);
		}

		protected virtual void BeforeExecure()
		{
		}

		protected virtual void AfrerExecute()
		{
		}

		private void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;
			handler?.Invoke(this, EventArgs.Empty);
		}

		private void RequeryCanExecute(object sender, PropertyChangedEventArgs args)
		{
			InvalidateCommand();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
