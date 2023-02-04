using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Livet;

namespace MylangSharp.Models
{
    public class AppSettings
    {
        public ObservableCollection<ImportFile> ImportFiles { get; set; } = new ObservableCollection<ImportFile>();

        public double FontSize { get; set; } = 14;
    }
}
