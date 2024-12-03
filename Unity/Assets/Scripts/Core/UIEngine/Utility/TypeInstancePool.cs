using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIEngine
{
	public class TypeInstancePool
	{
		private Dictionary<Type, Queue<object>> obDic = new Dictionary<Type, Queue<object>>();

		public object Get(string typeName)
		{
			Queue<object> obQueue = null;

			Type type = Type.GetType(typeName);

			if (!obDic.TryGetValue(type, out obQueue))
			{
				obQueue = new Queue<object>();
				obDic[type] = obQueue;
			}

			object ob = null;
			if (obQueue.Count > 0)
			{
				ob = obQueue.Dequeue();
			}
			else
			{
				ob = Activator.CreateInstance(type);
			}

			return ob;
		}

		public void Recycle(object ob)
		{
			Queue<object> obQueue = null;
			if (!obDic.TryGetValue(ob.GetType(), out obQueue))
			{
				obQueue = new Queue<object>();
				obDic[ob.GetType()] = obQueue;
			}

			obQueue.Enqueue(ob);
		}
	}
}