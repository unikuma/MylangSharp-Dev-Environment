using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.Messaging.Windows;
using MylangSharp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MylangSharp.ViewModels
{
	public class MainWindowViewModel : ViewModel
	{
		// バインディングプロパティ
		private MylangExecuter mylangExe = new MylangExecuter();

		private string _SourceCodeString = "1 2 +";
		public string SourceCodeString
		{
			get => _SourceCodeString;
			set => RaisePropertyChangedIfSet(ref _SourceCodeString, value);
		}

		public string StackString
		{
			get => mylangExe.StackString;
		}

		public string Output
		{
			get => mylangExe.Output;
		}

		public ObservableCollection<Variable> Variables
		{
			get => mylangExe.Variables;
		}

		// バインディングメソッド
		public void Initialize()
		{
			var listener = new PropertyChangedEventListener(mylangExe)
			{
				{ nameof(mylangExe.StackString), (_, __) => RaisePropertyChanged(nameof(StackString)) },
				{ nameof(mylangExe.Output), (_, __) => RaisePropertyChanged(nameof(Output)) },
				{ nameof(mylangExe.Variables), (_, __) => RaisePropertyChanged(nameof(Variables)) },
			};
		}
	}
}
