using Livet;

namespace MylangSharp.Models
{
	public class Variable : NotificationObject
	{
		private string _Name;
		public string Name
		{
			get => _Name;
			set => RaisePropertyChangedIfSet(ref _Name, value);
		}

		private string _Value;
		public string Value
		{
			get => _Value;
			set => RaisePropertyChangedIfSet(ref _Value, value);
		}

		public Variable() : this("", "") { }

		public Variable(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}
