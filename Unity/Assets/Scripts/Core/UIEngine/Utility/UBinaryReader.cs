using System.IO;
using UnityEngine;

public class UBinaryReader : BinaryReader
{
	public UBinaryReader(Stream input) : base(input)
	{

	}

	public Vector3 ReadVector3()
	{
		return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
	}
}
