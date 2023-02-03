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

namespace MylangSharp.ViewModels
{
    public class InputWindowViewModel : ViewModel
    {
        private string _Input;
        public string Input
        {
            get => _Input;
            set => RaisePropertyChangedIfSet(ref _Input, value);
        }

        public void Initialize()
        {
        }
    }
}
