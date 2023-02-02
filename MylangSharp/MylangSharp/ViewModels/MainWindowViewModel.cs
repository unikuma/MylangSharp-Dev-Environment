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
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.IO;

namespace MylangSharp.ViewModels
{
	public class MainWindowViewModel : ViewModel, IDisposable
	{
		// バインディングプロパティ
		private string _SourceCode = "1 2 +";
		public string SourceCode
		{
			get => _SourceCode;
			set => RaisePropertyChangedIfSet(ref _SourceCode, value);
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

		private string _WindowTitle = "MylangSharp Dev Environment";
		public string WindowTitle
		{
			get => _WindowTitle;
			private set => RaisePropertyChangedIfSet(ref _WindowTitle, value);
		}

		private MylangExecuter mylangExe = new MylangExecuter();

		private string oldSourceCode, currentFile;
		private bool isChanged, isConsoleAlloc;

		// バインディングメソッド
		public void Initialize()
		{
			var listener = new PropertyChangedEventListener(mylangExe)
			{
				{ nameof(mylangExe.StackString), (_, __) => RaisePropertyChanged(nameof(StackString)) },
				{ nameof(mylangExe.Output), (_, __) => RaisePropertyChanged(nameof(Output)) },
				{ nameof(mylangExe.Variables), (_, __) => RaisePropertyChanged(nameof(Variables)) },
			};
			oldSourceCode = SourceCode;
		}

		// ソースコードを実行
		public void RunSourceCode()
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(SourceCode))
					mylangExe.Run(SourceCode);
				else
					MessageBox.Show("ソースコードがnullか空白、空文字です", "MLGDE", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "MylangSharp Runtime Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// ファイルを開く
		public void OpenFileDialogFunc(OpeningFileSelectionMessage m)
		{
			if (m.Response != null && File.Exists(m.Response[0]))
			{
				currentFile = m.Response[0];
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
				SourceCode = oldSourceCode = File.ReadAllText(currentFile, Encoding.GetEncoding("shift_jis"));
			}
		}

		// ファイルを保存
		public void SaveFileDialogFunc(SavingFileSelectionMessage m)
		{
			if (m.Response != null)
			{
				currentFile = m.Response[0];
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
				File.WriteAllText(currentFile, SourceCode, Encoding.GetEncoding("shift_jis"));
				WindowTitle = "MylangSharp Dev Environment";
			}
		}

		// ファイルを上書き
		public void OverrideSaveFunc()
		{
			if (File.Exists(currentFile))
			{
				File.WriteAllText(currentFile, SourceCode, Encoding.GetEncoding("shift_jis"));
				WindowTitle = "MylangSharp Dev Environment";
			}
			else
			{
				var message = new SavingFileSelectionMessage("SaveDialog")
				{
					Title = "ファイルを保存",
					Filter = "mylangソースファイル(*.mylang)|*.mylang",
					OverwritePrompt = true,
				};

				Messenger.Raise(message);

				SaveFileDialogFunc(message);
			}
		}

		// ソースコードが変更されたかどうか
		public void SourceCodeTextChanged()
		{
			isChanged = (SourceCode != oldSourceCode);
			WindowTitle = isChanged ? "MylangSharp Dev Environment *" : "MylangSharp Dev Environment";
		}

		public void ShowConsole()
		{
			if (!isConsoleAlloc)
			{
				isConsoleAlloc = AllocConsole();
				Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
			}
		}

		public void HideConsole()
		{
			if (isConsoleAlloc)
			{
				FreeConsole();
				isConsoleAlloc = false;
			}
		}

		public new void Dispose()
		{
			if (isConsoleAlloc)
				FreeConsole();
		}

		[DllImport("kernel32.dll")]
		private static extern bool AllocConsole();

		[DllImport("kernel32.dll")]
		private static extern bool FreeConsole();
	}
}
