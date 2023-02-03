using Livet;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

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

		private string inbuf = string.Empty;

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
				Mes($"{progcnt}番目の要素は{elem[progcnt]}");

				if (elem[progcnt][0] >= 48 && elem[progcnt][0] <= 57)
				{
					Mes($"{progcnt}番目の要素は数、なのでスタックに入れる");
					stack[stackIndex++] = int.Parse(elem[progcnt]);
				}
				else
				{
					Mes($"{progcnt}番目の要素は数以外");

					if (elem[progcnt] == "+")
					{
						Mes("これは'+'");
						calcA = stack[stackIndex - 1];
						Mes($"取り出した数 1つ目は: {calcA}");
						calcB = stack[stackIndex - 2];
						Mes($"取り出した数 2つ目は: {calcB}");
						calcResult = calcB + calcA;
						Mes($"計算した結果は: {calcResult}");
						stack[stackIndex - 2] = calcResult;
						stackIndex--;
					}
					else if (elem[progcnt] == "-")
					{
						Mes("これは'-'");
						calcA = stack[stackIndex - 1];
						Mes($"取り出した数 1つ目は: {calcA}");
						calcB = stack[stackIndex - 2];
						Mes($"取り出した数 2つ目は: {calcB}");
						calcResult = calcB - calcA;
						Mes($"計算した結果は: {calcResult}");
						stack[stackIndex - 2] = calcResult;
						stackIndex--;
					}
					else if (elem[progcnt] == "*")
					{
						Mes("これは'*'");
						calcA = stack[stackIndex - 1];
						Mes($"取り出した数 1つ目は: {calcA}");
						calcB = stack[stackIndex - 2];
						Mes($"取り出した数 2つ目は: {calcB}");
						calcResult = calcB * calcA;
						Mes($"計算した結果は: {calcResult}");
						stack[stackIndex - 2] = calcResult;
						stackIndex--;
					}
					else if (elem[progcnt] == "/")
					{
						Mes("これは'/'");
						calcA = stack[stackIndex - 1];
						Mes($"取り出した数 1つ目は: {calcA}");
						calcB = stack[stackIndex - 2];
						Mes($"取り出した数 2つ目は: {calcB}");
						calcResult = calcB / calcA;
						Mes($"計算した結果は: {calcResult}");
						stack[stackIndex - 2] = calcResult;
						stackIndex--;
					}
					else if (elem[progcnt] == "=")
					{
						Mes("これは'='");
						calcA = stack[stackIndex - 1];
						Mes($"取り出した数 1つ目は: {calcA}");
						calcB = stack[stackIndex - 2];
						Mes($"取り出した数 2つ目は: {calcB}");
						calcResult = calcB == calcA ? 1 : 0;
						Mes($"計算した結果は: {calcResult}");
						stack[stackIndex - 2] = calcResult;
						stackIndex--;
					}
					else if (elem[progcnt] == "<")
					{
						Mes("これは'<'");
						calcA = stack[stackIndex - 1];
						Mes($"取り出した数 1つ目は: {calcA}");
						calcB = stack[stackIndex - 2];
						Mes($"取り出した数 2つ目は: {calcB}");
						calcResult = calcB < calcA ? 1 : 0;
						Mes($"計算した結果は: {calcResult}");
						stack[stackIndex - 2] = calcResult;
						stackIndex--;
					}
					else if (elem[progcnt] == ">")
					{
						Mes("これは'>'");
						calcA = stack[stackIndex - 1];
						Mes($"取り出した数 1つ目は: {calcA}");
						calcB = stack[stackIndex - 2];
						Mes($"取り出した数 2つ目は: {calcB}");
						calcResult = calcB > calcA ? 1 : 0;
						Mes($"計算した結果は: {calcResult}");
						stack[stackIndex - 2] = calcResult;
						stackIndex--;
					}
					else if (elem[progcnt] == "(" || elem[progcnt] == ")")
					{
						Mes($"これは{elem[progcnt]}、なので何もしない");
					}
					else if (elem[progcnt][0] == '@')
					{
						Mes("1文字目が@なので、変数に代入");
						VarStore();
					}
					else if (elem[progcnt][0] == '$')
					{
						Mes("1文字目が$なので、変数から値を取り出し");
						VarLoad();
					}
					else if (elem[progcnt] == "?{")
					{
						Mes("これは?{、なので条件分岐を行う");
						Branch();
					}
					else if (elem[progcnt] == "}")
					{
						Mes("これは}、なので何もしない");
					}
					else if (elem[progcnt] == "[")
					{
						Mes("これは[、なので何もしない");
					}
					else if (elem[progcnt] == "]")
					{
						Mes("これは]、なので対応する[に戻る");
						InfiniLoop();
					}
					else if (elem[progcnt] == "break")
					{
						Mes("これはbreak、なのでループから脱出する");
						EscapeLoop();
					}
					else if (elem[progcnt][0] == ':')
					{
						Mes("これは:(関数名){、なので飛ばす");
						MeetProcedure();
					}
					else if (elem[progcnt] == ".}")
					{
						Mes("これは.}、なのでreturnする");
						ReturnProcedure();
					}
					else if (elem[progcnt] == ".getchar")
					{
						Mes("これは.getchar、なので入力を受け取る");
						if (inbuf.Length == 0)
							inbuf = "test" + "\r\n";
						else
						{
							// 入力バッファに文字が残っている
						}
						stack[stackIndex++] = inbuf[0];
						inbuf = inbuf.Substring(1, inbuf.Length - 1);
					}
					else if (elem[progcnt] == ".putchar")
					{
						Mes("これは.putchar、なので出力する");
						Console.Write(Convert.ToChar(stack[--stackIndex]));
						
					}
					else if (elem[progcnt] == ".env-version")
					{
						stack[stackIndex++] = 0;
					}
					else if (elem[progcnt] == "breakpoint" || elem[progcnt] == "bp")
					{
						Mes("これはbreakpoint、なので実行を中断する");
						Mes("");
						running = true;
						++progcnt;
						return;
					}
					else
					{
						// 組み込み関数でなかった場合。ユーザー定義関数を検索する
						for (int i = 0; i < elemCount; i++)
						{
							var funcName = ":" + elem[progcnt].Substring(1, elem[progcnt].Length - 1) + "{";
							if (elem[i] == funcName)
							{
								callStack[callStackIndex++] = progcnt;
								progcnt = i;
								break;
							}
						}
					}
				}

				Mes("");

				StackString = "[ ";
				for (int i = 0; i < stackIndex; i++)
					StackString += $"{stack[i]} ";
				StackString += "]";

				// 要素を全て実行した
				if (++progcnt >= elemCount)
				{
					running = false;
					progcnt = 0;
					break;
				}
			}
			Mes("実行終了");
		}

		private void VarStore()
		{
			string varTarget = string.Empty;
			if (elem[progcnt][1] == '.')
				varTarget = stack[--stackIndex] + elem[progcnt].Substring(1);
			else
				varTarget = elem[progcnt].Substring(1);

			Mes($"変数名は{varTarget}");

			//var_nameの要素数ぶんループして、var_targetと同名のエントリを探す。
			for (int i = 0; i < Variables.Count; i++)
			{
				if (Variables[i].Name == varTarget)
				{
					Variables[i].Value = stack[--stackIndex].ToString();
					Mes($"変数エントリ{i}番目に代入した");
					break;
				}
				if (Variables[i].Name == string.Empty)
				{
					Variables[i].Name = varTarget;
					Variables[i].Value = stack[--stackIndex].ToString();
					Mes($"変数エントリ{i}番目に新しいエントリを作成し、代入した");
					break;
				}
			}
		}

		private void VarLoad()
		{
			string varTarget = string.Empty;
			if (elem[progcnt][1] == '.')
				varTarget = stack[--stackIndex] + elem[progcnt].Substring(1);
			else
				varTarget = elem[progcnt].Substring(1);

			Mes($"変数名は{varTarget}");

			int varFound = -1;

			for (int i = 0; i < Variables.Count; i++)
			{
				if (Variables[i].Name == varTarget)
				{
					varFound = i;
					Mes($"変数エントリ{i}番目に該当の変数があった");
					break;
				}
			}

			if (varFound == -1)
			{
				Mes("変数エントリに無かった");
				stack[stackIndex++] = 0;
			}
			else
			{
				stack[stackIndex++] = int.Parse(Variables[varFound].Value);
			}
		}

		private void Branch()
		{
			if (stack[--stackIndex] != 0)
			{
				Mes("条件は真");
			}
			else
			{
				Mes("条件は偽");
				int ndepth = 1;
				for (int i = 1; ; i++)
				{
					if (elem[progcnt + i].Substring(elem[progcnt + i].Length - "{".Length, "{".Length) == "{")
						ndepth++;
					if (elem[progcnt + i].Substring(elem[progcnt + i].Length - "}".Length, "}".Length) == "}")
						ndepth--;
					if (ndepth == 0)
					{
						Mes($"対応括弧は{i}個先");
						progcnt += i;
						break;
					}
				}
			}
		}

		private void InfiniLoop()
		{
			int ndepth = 1;
			for (int i = 1; ; i++)
			{
				if (elem[progcnt - i] == "]")
					ndepth++;
				if (elem[progcnt - i] == "[")
					ndepth--;
				if (ndepth == 0)
				{
					Mes($"対応括弧は{i}個前の要素");
					progcnt -= i;
					break;
				}
			}
		}

		private void EscapeLoop()
		{
			int ndepth = 1;
			for (int i = 1; ; i++)
			{
				if (elem[progcnt + i] == "[")
					ndepth++;
				if (elem[progcnt + i] == "]")
					ndepth--;
				if (ndepth == 0)
				{
					Mes($"対応括弧は{i}個先の要素");
					progcnt += i;
					break;
				}
			}
		}

		private void MeetProcedure()
		{
			int ndepth = 1;
			for (int i = 1; ; i++)
			{
				if (elem[progcnt + i].Substring(elem[progcnt + i].Length - "{".Length, "{".Length) == "{")
					ndepth++;
				if (elem[progcnt + i].Substring(elem[progcnt + i].Length - "}".Length, "}".Length) == "}")
					ndepth--;
				if (ndepth == 0)
				{
					Mes($"対応括弧は{i}個先の要素");
					progcnt += i;
					break;
				}
			}
		}

		private void ReturnProcedure()
		{
			progcnt = callStack[--callStackIndex];
		}

		private void Mes(string str) => Output += str + "\r\n";

		private void Initialize()
		{
			Array.Fill(stack, 0);
			Array.Fill(callStack, 0);

			for (int i = 0; i < 256; i++)
				Variables[i] = new Variable();

			stackIndex = callStackIndex = progcnt = 0;
			Output = inbuf = string.Empty;
		}
	}
}
