using System.IO;
using UnityEngine;

public class UBinaryWriter : BinaryWriter
{
	public override void Write(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			base.Write(string.Empty);
		}
	}

	public void Write(Vector3 vec)
	{
		Write(vec.x);
		Write(vec.y);
		Write(vec.z);
	}
}
