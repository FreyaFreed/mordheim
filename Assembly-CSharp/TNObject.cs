using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TNet;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("TNet/Network Object")]
[global::UnityEngine.ExecuteInEditMode]
public sealed class TNObject : global::UnityEngine.MonoBehaviour
{
	public uint uid
	{
		get
		{
			return (uint)((!(this.mParent != null)) ? this.id : ((int)this.mParent.uid));
		}
		set
		{
			if (this.mParent != null)
			{
				this.mParent.uid = value;
			}
			else
			{
				this.id = (int)(value & 16777215U);
			}
		}
	}

	public bool hasParent
	{
		get
		{
			return this.mParent != null;
		}
	}

	public bool isMine
	{
		get
		{
			return (this.mOwner != -1) ? (this.mOwner == global::TNManager.playerID) : global::TNManager.isThisMyObject;
		}
	}

	public int ownerID
	{
		get
		{
			return (!(this.mParent != null)) ? this.mOwner : this.mParent.ownerID;
		}
		set
		{
			if (this.mParent != null)
			{
				this.mParent.ownerID = value;
			}
			else if (this.mOwner != value)
			{
				this.Send("SetOwner", global::TNet.Target.All, new object[]
				{
					value
				});
			}
		}
	}

	[global::TNet.RFC]
	private void SetOwner(int val)
	{
		this.mOwner = val;
	}

	private void Awake()
	{
		if (this.id == 0 && !global::TNManager.isConnected)
		{
			this.id = ++global::TNObject.mDummyID;
		}
		this.mOwner = global::TNManager.objectOwnerID;
		if (global::TNManager.GetPlayer(this.mOwner) == null)
		{
			this.mOwner = global::TNManager.hostID;
		}
	}

	private void OnNetworkPlayerLeave(global::TNet.Player p)
	{
		if (p != null && this.mOwner == p.id)
		{
			this.mOwner = global::TNManager.hostID;
		}
	}

	public static global::TNObject Find(uint tnID)
	{
		if (global::TNObject.mDictionary == null)
		{
			return null;
		}
		global::TNObject result = null;
		global::TNObject.mDictionary.TryGetValue(tnID, out result);
		return result;
	}

	private static global::TNObject FindParent(global::UnityEngine.Transform t)
	{
		while (t != null)
		{
			global::TNObject component = t.gameObject.GetComponent<global::TNObject>();
			if (component != null)
			{
				return component;
			}
			t = t.parent;
		}
		return null;
	}

	private void Start()
	{
		if (this.id == 0)
		{
			this.mParent = global::TNObject.FindParent(base.transform.parent);
			if (!global::TNManager.isConnected)
			{
				return;
			}
			if (this.mParent == null && global::UnityEngine.Application.isPlaying)
			{
				global::UnityEngine.Debug.LogError("Objects that are not instantiated via TNManager.Create must have a non-zero ID.", this);
				return;
			}
		}
		else
		{
			this.Register();
			int i = 0;
			while (i < global::TNObject.mDelayed.size)
			{
				global::TNObject.DelayedCall delayedCall = global::TNObject.mDelayed[i];
				if (delayedCall.objID == this.uid)
				{
					if (!string.IsNullOrEmpty(delayedCall.funcName))
					{
						this.Execute(delayedCall.funcName, delayedCall.parameters);
					}
					else
					{
						this.Execute(delayedCall.funcID, delayedCall.parameters);
					}
					global::TNObject.mDelayed.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
	}

	private void OnDestroy()
	{
		this.Unregister();
	}

	public void Register()
	{
		if (!this.mIsRegistered && this.uid != 0U && this.mParent == null)
		{
			global::TNObject.mDictionary[this.uid] = this;
			global::TNObject.mList.Add(this);
			this.mIsRegistered = true;
		}
	}

	private void Unregister()
	{
		if (this.mIsRegistered)
		{
			if (global::TNObject.mDictionary != null)
			{
				global::TNObject.mDictionary.Remove(this.uid);
			}
			if (global::TNObject.mList != null)
			{
				global::TNObject.mList.Remove(this);
			}
			this.mIsRegistered = false;
		}
	}

	public bool Execute(byte funcID, params object[] parameters)
	{
		if (this.mParent != null)
		{
			return this.mParent.Execute(funcID, parameters);
		}
		if (this.rebuildMethodList)
		{
			this.RebuildMethodList();
		}
		return global::TNet.UnityTools.ExecuteAll(this.mRFCs, funcID, parameters);
	}

	public bool Execute(string funcName, params object[] parameters)
	{
		if (this.mParent != null)
		{
			return this.mParent.Execute(funcName, parameters);
		}
		if (this.rebuildMethodList)
		{
			this.RebuildMethodList();
		}
		return global::TNet.UnityTools.ExecuteAll(this.mRFCs, funcName, parameters);
	}

	public static void FindAndExecute(uint objID, byte funcID, params object[] parameters)
	{
		global::TNObject tnobject = global::TNObject.Find(objID);
		if (tnobject != null)
		{
			if (!tnobject.Execute(funcID, parameters))
			{
			}
		}
		else if (global::TNManager.isInChannel)
		{
			global::TNObject.DelayedCall delayedCall = new global::TNObject.DelayedCall();
			delayedCall.objID = objID;
			delayedCall.funcID = funcID;
			delayedCall.parameters = parameters;
			global::TNObject.mDelayed.Add(delayedCall);
		}
	}

	public static void FindAndExecute(uint objID, string funcName, params object[] parameters)
	{
		global::TNObject tnobject = global::TNObject.Find(objID);
		if (tnobject != null)
		{
			if (!tnobject.Execute(funcName, parameters))
			{
			}
		}
		else if (global::TNManager.isInChannel)
		{
			global::TNObject.DelayedCall delayedCall = new global::TNObject.DelayedCall();
			delayedCall.objID = objID;
			delayedCall.funcName = funcName;
			delayedCall.parameters = parameters;
			global::TNObject.mDelayed.Add(delayedCall);
		}
	}

	private void RebuildMethodList()
	{
		this.rebuildMethodList = false;
		this.mRFCs.Clear();
		global::UnityEngine.MonoBehaviour[] components = base.GetComponents<global::UnityEngine.MonoBehaviour>();
		int i = 0;
		int num = components.Length;
		while (i < num)
		{
			global::UnityEngine.MonoBehaviour monoBehaviour = components[i];
			global::System.Type type = monoBehaviour.GetType();
			global::System.Reflection.MethodInfo[] methods = type.GetMethods(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic);
			for (int j = 0; j < methods.Length; j++)
			{
				if (methods[j].IsDefined(typeof(global::TNet.RFC), true))
				{
					global::TNet.CachedFunc item = default(global::TNet.CachedFunc);
					item.obj = monoBehaviour;
					item.func = methods[j];
					global::TNet.RFC rfc = (global::TNet.RFC)item.func.GetCustomAttributes(typeof(global::TNet.RFC), true)[0];
					item.id = rfc.id;
					this.mRFCs.Add(item);
				}
			}
			i++;
		}
	}

	public void Send(byte rfcID, global::TNet.Target target, params object[] objs)
	{
		this.SendRFC(rfcID, null, target, true, objs);
	}

	public void Send(string rfcName, global::TNet.Target target, params object[] objs)
	{
		this.SendRFC(0, rfcName, target, true, objs);
	}

	public void Send(byte rfcID, global::TNet.Player target, params object[] objs)
	{
		if (target != null)
		{
			this.SendRFC(rfcID, null, target, true, objs);
		}
		else
		{
			this.SendRFC(rfcID, null, global::TNet.Target.All, true, objs);
		}
	}

	public void Send(string rfcName, global::TNet.Player target, params object[] objs)
	{
		if (target != null)
		{
			this.SendRFC(0, rfcName, target, true, objs);
		}
		else
		{
			this.SendRFC(0, rfcName, global::TNet.Target.All, true, objs);
		}
	}

	public void SendQuickly(byte rfcID, global::TNet.Target target, params object[] objs)
	{
		this.SendRFC(rfcID, null, target, false, objs);
	}

	public void SendQuickly(string rfcName, global::TNet.Target target, params object[] objs)
	{
		this.SendRFC(0, rfcName, target, false, objs);
	}

	public void SendQuickly(byte rfcID, global::TNet.Player target, params object[] objs)
	{
		if (target != null)
		{
			this.SendRFC(rfcID, null, target, false, objs);
		}
		else
		{
			this.SendRFC(rfcID, null, global::TNet.Target.All, false, objs);
		}
	}

	public void SendQuickly(string rfcName, global::TNet.Player target, params object[] objs)
	{
		this.SendRFC(0, rfcName, target, false, objs);
	}

	public void BroadcastToLAN(int port, byte rfcID, params object[] objs)
	{
		this.BroadcastToLAN(port, rfcID, null, objs);
	}

	public void BroadcastToLAN(int port, string rfcName, params object[] objs)
	{
		this.BroadcastToLAN(port, 0, rfcName, objs);
	}

	public void Remove(string rfcName)
	{
		global::TNObject.RemoveSavedRFC(this.uid, 0, rfcName);
	}

	public void Remove(byte rfcID)
	{
		global::TNObject.RemoveSavedRFC(this.uid, rfcID, null);
	}

	private static uint GetUID(uint objID, byte rfcID)
	{
		return objID << 8 | (uint)rfcID;
	}

	public static void DecodeUID(uint uid, out uint objID, out byte rfcID)
	{
		rfcID = (byte)(uid & 255U);
		objID = uid >> 8;
	}

	private void SendRFC(byte rfcID, string rfcName, global::TNet.Target target, bool reliable, params object[] objs)
	{
		bool flag = false;
		if (target == global::TNet.Target.Broadcast)
		{
			if (global::TNManager.isConnected)
			{
				global::System.IO.BinaryWriter binaryWriter = global::TNManager.BeginSend(global::TNet.Packet.Broadcast);
				binaryWriter.Write(global::TNObject.GetUID(this.uid, rfcID));
				if (rfcID == 0)
				{
					binaryWriter.Write(rfcName);
				}
				binaryWriter.WriteArray(objs);
				global::TNManager.EndSend(reliable);
			}
			else
			{
				flag = true;
			}
		}
		else if (target == global::TNet.Target.Host && global::TNManager.isHosting)
		{
			flag = true;
		}
		else if (global::TNManager.isInChannel)
		{
			if (!reliable)
			{
				if (target == global::TNet.Target.All)
				{
					target = global::TNet.Target.Others;
					flag = true;
				}
				else if (target == global::TNet.Target.AllSaved)
				{
					target = global::TNet.Target.OthersSaved;
					flag = true;
				}
			}
			byte packetID = (byte)(38 + target);
			global::System.IO.BinaryWriter binaryWriter2 = global::TNManager.BeginSend(packetID);
			binaryWriter2.Write(global::TNObject.GetUID(this.uid, rfcID));
			if (rfcID == 0)
			{
				binaryWriter2.Write(rfcName);
			}
			binaryWriter2.WriteArray(objs);
			global::TNManager.EndSend(reliable);
		}
		else if (!global::TNManager.isConnected && (target == global::TNet.Target.All || target == global::TNet.Target.AllSaved))
		{
			flag = true;
		}
		if (flag)
		{
			if (rfcID != 0)
			{
				this.Execute(rfcID, objs);
			}
			else
			{
				this.Execute(rfcName, objs);
			}
		}
	}

	private void SendRFC(byte rfcID, string rfcName, global::TNet.Player target, bool reliable, params object[] objs)
	{
		if (global::TNManager.isConnected)
		{
			global::System.IO.BinaryWriter binaryWriter = global::TNManager.BeginSend(global::TNet.Packet.ForwardToPlayer);
			binaryWriter.Write(target.id);
			binaryWriter.Write(global::TNObject.GetUID(this.uid, rfcID));
			if (rfcID == 0)
			{
				binaryWriter.Write(rfcName);
			}
			binaryWriter.WriteArray(objs);
			global::TNManager.EndSend(reliable);
		}
		else if (target == global::TNManager.player)
		{
			if (rfcID != 0)
			{
				this.Execute(rfcID, objs);
			}
			else
			{
				this.Execute(rfcName, objs);
			}
		}
	}

	private void BroadcastToLAN(int port, byte rfcID, string rfcName, params object[] objs)
	{
		global::System.IO.BinaryWriter binaryWriter = global::TNManager.BeginSend(global::TNet.Packet.ForwardToAll);
		binaryWriter.Write(global::TNObject.GetUID(this.uid, rfcID));
		if (rfcID == 0)
		{
			binaryWriter.Write(rfcName);
		}
		binaryWriter.WriteArray(objs);
		global::TNManager.EndSend(port);
	}

	private static void RemoveSavedRFC(uint objID, byte rfcID, string funcName)
	{
		if (global::TNManager.isInChannel)
		{
			global::System.IO.BinaryWriter binaryWriter = global::TNManager.BeginSend(global::TNet.Packet.RequestRemoveRFC);
			binaryWriter.Write(global::TNObject.GetUID(objID, rfcID));
			if (rfcID == 0)
			{
				binaryWriter.Write(funcName);
			}
			global::TNManager.EndSend();
		}
	}

	private static int mDummyID = 0;

	private static global::TNet.List<global::TNObject> mList = new global::TNet.List<global::TNObject>();

	private static global::System.Collections.Generic.Dictionary<uint, global::TNObject> mDictionary = new global::System.Collections.Generic.Dictionary<uint, global::TNObject>();

	private static global::TNet.List<global::TNObject.DelayedCall> mDelayed = new global::TNet.List<global::TNObject.DelayedCall>();

	[global::UnityEngine.SerializeField]
	private int id;

	[global::UnityEngine.HideInInspector]
	[global::System.NonSerialized]
	public bool rebuildMethodList = true;

	[global::System.NonSerialized]
	private global::TNet.List<global::TNet.CachedFunc> mRFCs = new global::TNet.List<global::TNet.CachedFunc>();

	[global::System.NonSerialized]
	private bool mIsRegistered;

	[global::System.NonSerialized]
	private int mOwner = -1;

	[global::System.NonSerialized]
	private global::TNObject mParent;

	private class DelayedCall
	{
		public uint objID;

		public byte funcID;

		public string funcName;

		public object[] parameters;
	}
}
