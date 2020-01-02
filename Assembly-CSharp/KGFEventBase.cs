using System;

[global::System.Serializable]
public abstract class KGFEventBase : global::KGFObject, global::KGFIValidator
{
	public abstract void Trigger();

	public virtual global::KGFMessageList Validate()
	{
		return new global::KGFMessageList();
	}
}
