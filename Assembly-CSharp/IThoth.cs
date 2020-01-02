using System;
using System.IO;

public interface IThoth
{
	int GetVersion();

	int GetCRC(bool read);

	void Write(global::System.IO.BinaryWriter writer);

	void Read(global::System.IO.BinaryReader reader);
}
