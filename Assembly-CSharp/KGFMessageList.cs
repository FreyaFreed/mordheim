using System;
using System.Collections.Generic;

public class KGFMessageList
{
	public void AddError(string theMessage)
	{
		this.itsHasErrors = true;
		this.itsMessageList.Add(new global::KGFMessageList.Error(theMessage));
	}

	public void AddInfo(string theMessage)
	{
		this.itsHasInfos = true;
		this.itsMessageList.Add(new global::KGFMessageList.Info(theMessage));
	}

	public void AddWarning(string theMessage)
	{
		this.itsHasWarnings = true;
		this.itsMessageList.Add(new global::KGFMessageList.Warning(theMessage));
	}

	public string[] GetErrorArray()
	{
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		foreach (global::KGFMessageList.Message message in this.itsMessageList)
		{
			if (message is global::KGFMessageList.Error)
			{
				list.Add(message.Description);
			}
		}
		return list.ToArray();
	}

	public string[] GetInfoArray()
	{
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		foreach (global::KGFMessageList.Message message in this.itsMessageList)
		{
			if (message is global::KGFMessageList.Info)
			{
				list.Add(message.Description);
			}
		}
		return list.ToArray();
	}

	public string[] GetWarningArray()
	{
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		foreach (global::KGFMessageList.Message message in this.itsMessageList)
		{
			if (message is global::KGFMessageList.Warning)
			{
				list.Add(message.Description);
			}
		}
		return list.ToArray();
	}

	public global::KGFMessageList.Message[] GetAllMessagesArray()
	{
		return this.itsMessageList.ToArray();
	}

	public void AddMessages(global::KGFMessageList.Message[] theMessages)
	{
		foreach (global::KGFMessageList.Message message in theMessages)
		{
			this.itsMessageList.Add(message);
			if (message is global::KGFMessageList.Error)
			{
				this.itsHasErrors = true;
			}
			if (message is global::KGFMessageList.Warning)
			{
				this.itsHasWarnings = true;
			}
			if (message is global::KGFMessageList.Info)
			{
				this.itsHasInfos = true;
			}
		}
	}

	public bool itsHasErrors;

	public bool itsHasWarnings;

	public bool itsHasInfos;

	private global::System.Collections.Generic.List<global::KGFMessageList.Message> itsMessageList = new global::System.Collections.Generic.List<global::KGFMessageList.Message>();

	public abstract class Message
	{
		public Message(string theMessage)
		{
			this.itsMessage = theMessage;
		}

		public string Description
		{
			get
			{
				return this.itsMessage;
			}
		}

		private string itsMessage;
	}

	public class Error : global::KGFMessageList.Message
	{
		public Error(string theMessage) : base(theMessage)
		{
		}
	}

	public class Info : global::KGFMessageList.Message
	{
		public Info(string theMessage) : base(theMessage)
		{
		}
	}

	public class Warning : global::KGFMessageList.Message
	{
		public Warning(string theMessage) : base(theMessage)
		{
		}
	}
}
