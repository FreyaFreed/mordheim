using System;
using System.Collections.Generic;
using UnityEngine;

public class KGFLogicAnalyzer
{
	public static bool? Analyze(string theLogicString)
	{
		string empty = string.Empty;
		if (!global::KGFLogicAnalyzer.CheckSyntax(theLogicString, out empty))
		{
			global::UnityEngine.Debug.LogError("KGFLogicAnalyzer: syntax error: " + empty);
			return null;
		}
		if (!global::KGFLogicAnalyzer.CheckOperands(theLogicString, out empty))
		{
			global::UnityEngine.Debug.LogError("KGFLogicAnalyzer: syntax error: " + empty);
			return null;
		}
		int num = 0;
		if (!theLogicString.Contains(")"))
		{
			theLogicString = "(" + theLogicString + ")";
		}
		while (theLogicString.Contains(")"))
		{
			global::KGFLogicAnalyzer.EvaluateBraces(ref theLogicString);
			num++;
			if (num == 30)
			{
				break;
			}
		}
		if (theLogicString.ToLower() == "true")
		{
			return new bool?(true);
		}
		if (theLogicString.ToLower() == "false")
		{
			return new bool?(false);
		}
		global::UnityEngine.Debug.LogError("KGFLogicAnalyzer: unexpected result: " + theLogicString);
		return null;
	}

	private static void EvaluateBraces(ref string theLogicString)
	{
		string text = theLogicString.Replace(" ", string.Empty);
		int num = text.IndexOf(')');
		string text2 = text.Substring(0, num + 1);
		int num2 = text2.LastIndexOf('(');
		int length = num - num2 - 1;
		string theLogicString2 = text.Substring(num2 + 1, length);
		bool? flag = global::KGFLogicAnalyzer.AnalyseLogicBlock(theLogicString2);
		if (flag == null)
		{
			global::UnityEngine.Debug.LogError("Logic block result is null. Something went wrong!");
			return;
		}
		string str = theLogicString.Substring(0, num2);
		string str2 = theLogicString.Substring(num + 1);
		theLogicString = str + flag.Value.ToString() + str2;
	}

	public static void ClearOperandValues()
	{
		global::KGFLogicAnalyzer.itsOperandValues.Clear();
	}

	public static void SetOperandValue(string theOperandName, bool theValue)
	{
		if (global::KGFLogicAnalyzer.itsOperandValues.ContainsKey(theOperandName))
		{
			global::KGFLogicAnalyzer.itsOperandValues[theOperandName] = theValue;
		}
		else
		{
			global::KGFLogicAnalyzer.itsOperandValues.Add(theOperandName, theValue);
		}
	}

	public static bool? GetOperandValue(string theOperandName)
	{
		if (global::KGFLogicAnalyzer.itsOperandValues.ContainsKey(theOperandName))
		{
			return new bool?(global::KGFLogicAnalyzer.itsOperandValues[theOperandName]);
		}
		global::UnityEngine.Debug.LogError("KGFLogicAnalyzer: no operand value for operand: " + theOperandName);
		return null;
	}

	private static bool? AnalyseLogicBlock(string theLogicString)
	{
		global::KGFLogicAnalyzer.KGFLogicOperand kgflogicOperand = new global::KGFLogicAnalyzer.KGFLogicOperand();
		string text = theLogicString.Replace(" ", string.Empty);
		string[] separator = new string[]
		{
			global::KGFLogicAnalyzer.itsStringAnd,
			global::KGFLogicAnalyzer.itsStringOr
		};
		string[] array = text.Split(separator, global::System.StringSplitOptions.None);
		foreach (string name in array)
		{
			global::KGFLogicAnalyzer.KGFLogicOperand kgflogicOperand2 = new global::KGFLogicAnalyzer.KGFLogicOperand();
			kgflogicOperand2.SetName(name);
			kgflogicOperand.AddOperand(kgflogicOperand2);
		}
		for (int j = 0; j < array.Length - 1; j++)
		{
			text = text.Remove(0, array[j].Length);
			string theOperator = text.Substring(0, 2);
			kgflogicOperand.AddOperator(theOperator);
			text = text.Remove(0, 2);
		}
		return kgflogicOperand.Evaluate();
	}

	public static bool CheckSyntax(string theLogicString, out string theErrorString)
	{
		theErrorString = string.Empty;
		if (theLogicString.IndexOf(global::KGFLogicAnalyzer.itsStringAnd) == 0)
		{
			theErrorString = "condition cannot start with &&";
			return false;
		}
		if (theLogicString.IndexOf(global::KGFLogicAnalyzer.itsStringOr) == 0)
		{
			theErrorString = "condition cannot start with ||";
			return false;
		}
		if (theLogicString.LastIndexOf(global::KGFLogicAnalyzer.itsStringAnd) == theLogicString.Length - 2 && theLogicString.Length != 1)
		{
			theErrorString = "condition cannot end with &&";
			return false;
		}
		if (theLogicString.LastIndexOf(global::KGFLogicAnalyzer.itsStringOr) == theLogicString.Length - 2 && theLogicString.Length != 1)
		{
			theErrorString = "condition cannot end with ||";
			return false;
		}
		string text = theLogicString.Replace(" ", string.Empty);
		int num = text.Split(new char[]
		{
			'('
		}).Length - 1;
		int num2 = text.Split(new char[]
		{
			')'
		}).Length - 1;
		if (num > num2)
		{
			theErrorString = "missing closing brace";
			return false;
		}
		if (num2 > num)
		{
			theErrorString = "missing opening brace";
			return false;
		}
		string[] separator = new string[]
		{
			global::KGFLogicAnalyzer.itsStringAnd,
			global::KGFLogicAnalyzer.itsStringOr
		};
		string text2 = text.Replace("(", string.Empty);
		text2 = text2.Replace(")", string.Empty);
		string[] array = text2.Split(separator, global::System.StringSplitOptions.None);
		foreach (string text3 in array)
		{
			if (text3.Contains("&"))
			{
				theErrorString = "condition cannot contain the character &. Use && for logical and.";
				return false;
			}
			if (text3.Contains("|"))
			{
				theErrorString = "condition cannot contain the character |. Use || for logical or.";
				return false;
			}
		}
		return true;
	}

	public static bool CheckOperands(string theLogicString, out string theErrorString)
	{
		theErrorString = string.Empty;
		string[] separator = new string[]
		{
			global::KGFLogicAnalyzer.itsStringAnd,
			global::KGFLogicAnalyzer.itsStringOr
		};
		string text = theLogicString.Replace(" ", string.Empty);
		string text2 = text.Replace("(", string.Empty);
		text2 = text2.Replace(")", string.Empty);
		string[] array = text2.Split(separator, global::System.StringSplitOptions.None);
		foreach (string text3 in array)
		{
			if (global::KGFLogicAnalyzer.GetOperandValue(text3) == null)
			{
				theErrorString = "no operand value for operand: " + text3;
				return false;
			}
		}
		return true;
	}

	private static string itsStringAnd = "&&";

	private static string itsStringOr = "||";

	private static global::System.Collections.Generic.Dictionary<string, bool> itsOperandValues = new global::System.Collections.Generic.Dictionary<string, bool>();

	public class KGFLogicOperand
	{
		public void AddOperand(global::KGFLogicAnalyzer.KGFLogicOperand theOperand)
		{
			this.itsListOfOperands.Add(theOperand);
		}

		public void AddOperator(string theOperator)
		{
			this.itsListOfOperators.Add(theOperator);
		}

		public void SetName(string theName)
		{
			this.itsOperandName = theName;
			if (theName.ToLower() == "true")
			{
				this.itsValue = new bool?(true);
			}
			else if (theName.ToLower() == "false")
			{
				this.itsValue = new bool?(false);
			}
		}

		public string GetName()
		{
			return this.itsOperandName;
		}

		public void SetValue(bool theValue)
		{
			this.itsValue = new bool?(theValue);
		}

		public bool? GetValue()
		{
			bool? flag = this.itsValue;
			if (flag != null)
			{
				return new bool?(this.itsValue.Value);
			}
			if (!(this.itsOperandName != string.Empty))
			{
				return this.Evaluate();
			}
			this.itsValue = global::KGFLogicAnalyzer.GetOperandValue(this.itsOperandName);
			bool? flag2 = this.itsValue;
			if (flag2 == null)
			{
				return null;
			}
			return this.itsValue;
		}

		public bool? Evaluate()
		{
			if (this.itsListOfOperands.Count == 1)
			{
				return this.itsListOfOperands[0].GetValue();
			}
			bool? flag = new bool?(false);
			for (int i = 0; i < this.itsListOfOperands.Count - 1; i++)
			{
				if (i == 0)
				{
					flag = this.EveluateTwoOperands(this.itsListOfOperands[i].GetValue(), this.itsListOfOperands[i + 1].GetValue(), this.itsListOfOperators[i]);
				}
				else
				{
					flag = this.EveluateTwoOperands(flag, this.itsListOfOperands[i + 1].GetValue(), this.itsListOfOperators[i]);
				}
			}
			return flag;
		}

		private bool? EveluateTwoOperands(bool? theValue1, bool? theValue2, string theOperator)
		{
			if (theValue1 == null)
			{
				global::UnityEngine.Debug.LogError("KGFLogicAnalyzer: cannot evaluate because theValue1 is null");
				return null;
			}
			if (theValue2 == null)
			{
				global::UnityEngine.Debug.LogError("KGFLogicAnalyzer: cannot evaluate because theValue2 is null");
				return null;
			}
			if (theOperator == "&&")
			{
				return new bool?(theValue1.Value && theValue2.Value);
			}
			if (theOperator == "||")
			{
				return new bool?(theValue1.Value || theValue2.Value);
			}
			global::UnityEngine.Debug.LogError("KGFLogicAnalyzer: wrong operator: " + theOperator);
			return null;
		}

		public string itsOperandName = string.Empty;

		private bool? itsValue;

		public global::System.Collections.Generic.List<global::KGFLogicAnalyzer.KGFLogicOperand> itsListOfOperands = new global::System.Collections.Generic.List<global::KGFLogicAnalyzer.KGFLogicOperand>();

		public global::System.Collections.Generic.List<string> itsListOfOperators = new global::System.Collections.Generic.List<string>();
	}
}
