using System;

public interface IMyrtilus
{
	uint uid { get; set; }

	uint owner { get; set; }

	void RegisterToHermes();

	void RemoveFromHermes();

	void Send(bool reliable, global::Hermes.SendTarget target, uint uid, uint command, params object[] parms);

	void Receive(ulong from, uint command, object[] parms);
}
