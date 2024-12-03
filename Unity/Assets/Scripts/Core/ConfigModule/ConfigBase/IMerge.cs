using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using ProtoBuf;
using System.IO;
using System.Collections;
using System;
using System.IO.Pipes;

namespace ClientData
{
	public interface IMerge
	{
		void Merge(object o);
	}

	[ProtoContract]
	public class ProtobufArray : ProtoObject
	{
		[ProtoMember(1)]
		public string ArrayString;

		[ProtoIgnore]
		[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		private int[][] intArrays;

		[ProtoIgnore]
		[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		public int[][] IntArrays
		{
			get
			{
				if (intArrays == null)
				{
					intArrays = Analyze(ArrayString);
				}

				return intArrays;
			}
		}

		[ProtoIgnore]
		[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		private string[][] stringArrays;

		[ProtoIgnore]
		[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		public string[][] StrintArrays
		{
			get
			{
				if (stringArrays == null)
				{
					stringArrays = AnalyzeStr(ArrayString);
				}

				return stringArrays;
			}
		}

		private int[][] Analyze(string arrayStr)
		{
			if (string.IsNullOrEmpty(arrayStr))
				return null;
			if (!arrayStr.Contains(","))
				return null;
			string[] ss = arrayStr.Split("],[");
			int[][] intArray = new int[ss.Length][];

			for (int i = 0; i < ss.Length; i++)
			{
				string s = ss[i].Replace("[[", "").Replace("]]", "");

				string[] ss2 = s.Split(',');
				intArray[i] = new int[ss2.Length];
				for (int j = 0; j < ss2.Length; j++)
				{
					if (string.IsNullOrEmpty(ss2[j]))
					{
						intArray[i][j] = 0;
					}
					else
					{
						intArray[i][j] = int.Parse(ss2[j]);
					}
				}
			}

			return intArray;
		}

		private string[][] AnalyzeStr(string arrayStr)
		{
			if (string.IsNullOrEmpty(arrayStr))
				return null;
			if (!arrayStr.Contains(","))
				return null;
			string[] ss = arrayStr.Split("],[");
			string[][] intArray = new string[ss.Length][];

			for (int i = 0; i < ss.Length; i++)
			{
				string s = ss[i].Replace("[[", "").Replace("]]", "");

				string[] ss2 = s.Split(',');
				intArray[i] = new string[ss2.Length];
				for (int j = 0; j < ss2.Length; j++)
				{
					intArray[i][j] = ss2[j];
				}
			}

			return intArray;
		}
	}

	[ProtoContract]
	public class ProtobufArrayInt : ProtoObject
	{
		[ProtoIgnore]
		public string ArrayString;

		[ProtoMember(1)]
		public byte[] ArrayData;

		[ProtoIgnore]
		[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		private int[][] intArrays;

		[ProtoIgnore]
		[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		public int[][] IntArrays
		{
			get
			{
				if (intArrays == null)
				{
					intArrays = AnalyzeInt(ArrayData);
				}

				return intArrays;
			}
		}

		private int[][] AnalyzeInt(byte[] bytes)
		{
			int[][] intArray = null;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				using (BinaryReader reader = new BinaryReader(memoryStream))
				{
					int length = reader.ReadInt32();
					intArray = new int[length][];
					for (int i = 0; i < length; i++)
					{
						int count = reader.ReadInt32();
						intArray[i] = new int[count];
						for (int j = 0; j < count; j++)
						{
							int value = reader.ReadInt32();
							intArray[i][j] = value;
						}
					}
				}
			}

			return intArray;
		}


	}

	[ProtoContract]
	public class ProtobufArrayString : ProtoObject
	{
		[ProtoIgnore]
		public string ArrayString;

		[ProtoMember(1)]
		public byte[] ArrayData;

		[ProtoIgnore]
		[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		private string[][] stringArrays;

		[ProtoIgnore]
		[MongoDB.Bson.Serialization.Attributes.BsonIgnore]
		public string[][] StrintArrays
		{
			get
			{
				if (stringArrays == null)
				{
					stringArrays = AnalyzeString(ArrayData);
				}

				return stringArrays;
			}
		}

		private string[][] AnalyzeString(byte[] bytes)
		{
			string[][] intArray = null;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				using (BinaryReader reader = new BinaryReader(memoryStream))
				{
					int length = reader.ReadInt32();
					intArray = new string[length][];
					for (int i = 0; i < length; i++)
					{
						int count = reader.ReadInt32();
						intArray[i] = new string[count];
						for (int j = 0; j < count; j++)
						{
							string value = reader.ReadString();
							intArray[i][j] = value;
						}
					}
				}
			}

			return intArray;
		}
	}
}
