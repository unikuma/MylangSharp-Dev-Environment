using Livet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MylangSharp.Models
{
	public class MylangExecuter : NotificationObject
	{
		private int[] stack = new int[32];
		private int stackIndex = 0;

		private string[] elem = new string[65536];
		private int elemCount = 0;

		private int[] callStack = new int[2];
		private int callStackIndex = 0;

		private bool running = false;

		private int progcnt = 0;

		private string _Output;
		public string Output
		{
			get => _Output;
			set => RaisePropertyChangedIfSet(ref _Output, value);
		}

		private string _StackString;
		public string StackString
		{
			get => _StackString;
			set => RaisePropertyChangedIfSet(ref _StackString, value);
		}

		private ObservableCollection<Variable> _Variables = new ObservableCollection<Variable>();
		public ObservableCollection<Variable> Variables
		{
			get => _Variables;
			set => RaisePropertyChangedIfSet(ref _Variables, value);
		}

		public MylangExecuter()
		{
			for (int i = 0; i < 256; i++)
				Variables.Add(new Variable());
		}

		public void Run(string program)
		{
			if (!running)
			{
				Initialize();

				program = program.Replace("\r\n", " ");
				program = program.Replace("\t", " ");
				program = program.Replace("  ", " ");

				elem = program.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
				elemCount = elem.Length;
			}

			int calcA, calcB, calcResult;

			while (true)
			{
				if (elem[progcnt][0] >= 48 && elem[progcnt][0] <= 57)
				{

				}
			}
		}

		public void Initialize()
		{
			Array.Fill(stack, 0);
			Array.Fill(callStack, 0);

			for (int i = 0; i < 256; i++)
				Variables[i] = new Variable();

			stackIndex = callStackIndex = progcnt = 0;
			Output = string.Empty;
		}
	}
}
