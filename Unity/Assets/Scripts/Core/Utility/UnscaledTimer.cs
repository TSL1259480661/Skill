using System.Collections.Generic;
using UnityEngine;
using System;

namespace App
{
	public class UnscaledTimer : Timer
	{
		private static UnscaledTimer _instance;
		private static readonly object _lock = new object();
		new public static UnscaledTimer Instance
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

		new public static UnscaledTimer GetInstance()
		{
			if (_instance == null)
			{
				_instance = new UnscaledTimer();
			}
			return _instance;
		}

		protected float nowTime = 0f;

		public override float GetNowTime()
		{
			return nowTime;
		}

		public override void FixedUpdate()
		{
			if (!pause)
			{
				nowTime = nowTime + Time.fixedDeltaTime;
				base.FixedUpdate();
			}
		}
	}
}
