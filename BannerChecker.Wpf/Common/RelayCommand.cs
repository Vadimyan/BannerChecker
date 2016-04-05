using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace BannerChecker.Wpf.Common
{
	public class RelayCommand : CommandBase
	{
		private readonly IEnumerable<Func<bool>> _canExecuteCollection;
		private readonly Action _execute;

		public RelayCommand(Action execute)
			: this(execute, () => true)
		{
		}

		public RelayCommand(Action execute, Func<bool> canExecutePredicate)
			: this(execute, new[] { canExecutePredicate }, new Tuple<INotifyPropertyChanged, string>[0])
		{
		}

		public RelayCommand(Action execute, IEnumerable<Func<bool>> canExecutePredicates, IEnumerable<Tuple<INotifyPropertyChanged, string>> invalidators)
		{
			_execute = execute;
			_canExecuteCollection = canExecutePredicates;
			AddInvalidators(invalidators);
		}

		private void AddInvalidators(IEnumerable<Tuple<INotifyPropertyChanged, string>> invalidators)
		{
			foreach (var invalidator in invalidators)
				AddListenerInternal(invalidator.Item1, invalidator.Item2);
		}

		public sealed override bool CanExecute(object parameter)
		{
			return _canExecuteCollection.All(ce => ce());
		}

		protected sealed override void PerformExecute(object parameter)
		{
			_execute();
		}
	}

	public class RelayCommand<T> : CommandBase
	{
		private readonly IEnumerable<Func<T, bool>> _canExecuteCollection;
		private readonly Action<T> _execute;

		public RelayCommand(Action<T> execute)
			: this(execute, par => true)
		{
		}

		public RelayCommand(Action<T> execute, Func<T, bool> canExecutePredicate)
			: this(execute, new[] { canExecutePredicate }, new Tuple<INotifyPropertyChanged, string>[0], false)
		{
		}

		public RelayCommand(Action<T> execute, IEnumerable<Func<T, bool>> canExecutePredicates, IEnumerable<Tuple<INotifyPropertyChanged, string>> invalidators, bool listenCommandManager)
		{
			_execute = execute;
			_canExecuteCollection = canExecutePredicates;
			AddInvalidators(invalidators);
			InitializeCommandManager(listenCommandManager);
		}
		
		public sealed override bool CanExecute(object parameter)
		{
			var par = (T)parameter;
			return _canExecuteCollection.All(ce => ce(par));
		}

		protected sealed override void PerformExecute(object parameter)
		{
			_execute((T)parameter);
		}

		private void InitializeCommandManager(bool listenCommandManager)
		{
			if (listenCommandManager)
				CommandManager.RequerySuggested += (sender, args) => InvalidateCommand();
		}

		private void AddInvalidators(IEnumerable<Tuple<INotifyPropertyChanged, string>> invalidators)
		{
			foreach (var invalidator in invalidators)
				AddListenerInternal(invalidator.Item1, invalidator.Item2);
		}
	}
}
