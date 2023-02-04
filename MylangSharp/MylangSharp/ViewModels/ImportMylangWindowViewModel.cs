using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using System.Collections.ObjectModel;
using MylangSharp.Models;

namespace MylangSharp.ViewModels
{
	public class ImportMylangWindowViewModel : ViewModel
	{
		private ObservableCollection<ImportFile> _ImportFiles;
		public ObservableCollection<ImportFile> ImportFiles
		{
			get => _ImportFiles;
			set => RaisePropertyChangedIfSet(ref _ImportFiles, value);
		}

		private int _SelectedIndex;
		public int SelectedIndex
		{
			get => _SelectedIndex;
			set => RaisePropertyChangedIfSet(ref _SelectedIndex, value);
		}

		public void Initialize()
		{
		}

		public void AddMylangFiles(OpeningFileSelectionMessage m)
		{
			if (m.Response == null)
				return;

			foreach (string filePath in m.Response)
			{
				ImportFiles.Add(new ImportFile(filePath));
			}
		}

		public void RemoveMylangFile()
		{
			if (SelectedIndex == -1)
				return;
			ImportFiles.RemoveAt(SelectedIndex);
		}

		public void IncreasePriority()
		{
			if (ImportFiles.Count > 1 && SelectedIndex > 0)
			{
				(ImportFiles[SelectedIndex], ImportFiles[SelectedIndex - 1]) = (ImportFiles[SelectedIndex - 1], ImportFiles[SelectedIndex]);
			}
		}

		public void DecreasePriority()
		{
			if (ImportFiles.Count > 1 && SelectedIndex < ImportFiles.Count - 1)
			{
				(ImportFiles[SelectedIndex], ImportFiles[SelectedIndex + 1]) = (ImportFiles[SelectedIndex + 1], ImportFiles[SelectedIndex]);
			}
		}

	}
}
