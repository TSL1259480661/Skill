using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace UILoopScroll
{
	[Serializable]
	public class DRect
	{
		#region 简单对象池
		private static List<DRect> list = new List<DRect>(50);
		public static DRect Get(float x, float y, float width, float height, int index)
		{
			DRect rect;
			var count = list.Count;
			if (count > 0)
			{
				rect = list[count - 1];
				list.RemoveAt(count - 1);
				rect.SetSize(width, height);
				rect.SetPos(x, y);
			}
			else
			{
				rect = new DRect(0, 0, width, height, index);
			}
			rect.Index = index;
			rect.isBind = false;
			return rect;
		}
		public static void Store(DRect value)
		{
			list.Add(value);
		}
		#endregion


		public int Index;
		public Rect rect;
		[NonSerialized]
		public bool isBind;


		public DRect(float x, float y, float width, float height, int index)
		{
			this.Index = index;
			rect = new Rect(x, y, width, height);
			isBind = false;
		}

		public void Reset(float x, float y, float width, float height, int index)
		{
			this.Index = index;
			rect = new Rect(x, y, width, height);
		}

		public void SetPos(float x, float y)
		{
			rect.x = x;
			rect.y = y;
		}

		public void SetSize(float x, float y)
		{
			rect.width = x;
			rect.height = y;
		}

		public Vector2 position
		{
			get { return rect.position; }
			set { rect.position = value; }
		}

		public Vector2 size
		{
			get { return rect.size; }
			set { rect.size = value; }
		}

		public Vector2 min
		{
			get { return rect.min; }
			set { rect.min = value; }
		}

		public Vector2 max
		{
			get { return rect.max; }
			set { rect.max = value; }
		}

		public float xMin
		{
			get { return rect.xMin; }
			set { rect.xMin = value; }
		}
		public float yMin
		{
			get { return rect.yMin; }
			set { rect.yMin = value; }
		}
		public float yMax
		{
			get { return rect.yMax; }
			set { rect.yMax = value; }
		}

		public float xMax
		{
			get { return rect.xMax; }
			set { rect.xMax = value; }
		}

		/// <summary>
		/// 是否相交
		/// </summary>
		public bool Overlaps(Rect other)
		{
			return rect.Overlaps(other);
			//return (other.min > min && other.min < max) || (other.max > min && other.max < max);
		}

		public override string ToString()
		{
			return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
		}
		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (string.IsNullOrEmpty(format))
			{
				format = "F2";
			}
			return string.Format("(x:{0}, y:{1}, width:{2}, height:{3},index:{4})",
				rect.x.ToString(format, formatProvider), rect.y.ToString(format, formatProvider),
				 rect.width.ToString(format, formatProvider), rect.height.ToString(format, formatProvider),
				 Index.ToString());
		}
	}
}
