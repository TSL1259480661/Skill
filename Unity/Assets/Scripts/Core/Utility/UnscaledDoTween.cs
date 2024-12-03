using System.Collections.Generic;
using UnityEngine;
using System;

namespace App
{
	public class UnscaledDoTween : DoTween
	{
		private static UnscaledDoTween _instance;
		private static readonly object _lock = new object();
		new public static UnscaledDoTween Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_lock)
					{
						if (_instance == null)
							return GetInstance();
					}
				}
				return _instance;
			}
		}

		new public static UnscaledDoTween GetInstance()
		{
			if (_instance == null)
			{
				_instance = new UnscaledDoTween();
			}
			return _instance;
		}

		protected float nowTime = 0f;

		protected override float GetDeltaTime()
		{
			return Time.fixedUnscaledDeltaTime;
		}
	}
}
