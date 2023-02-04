using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Livet;

namespace MylangSharp.Models
{
    public class ImportFile : NotificationObject
    {
        private string _FileName;
        public string FileName
        {
            get => _FileName;
            set => RaisePropertyChangedIfSet(ref _FileName, value);
        }

        private string _FileDir;
        public string FileDir
        {
            get => _FileDir;
            set => RaisePropertyChangedIfSet(ref _FileDir, value);
        }

        public ImportFile()
        {
        }

        public ImportFile(string filePath)
        {
            FileName = Path.GetFileName(filePath);
            FileDir = Path.GetDirectoryName(filePath);
        }

		public override string ToString()
		{
			return Path.Combine(FileDir, FileName);
		}
	}
}
