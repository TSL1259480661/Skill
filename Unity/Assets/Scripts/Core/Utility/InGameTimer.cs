using UnityEngine;

namespace App
{
	public class InGameTimer : Timer
	{
		private static InGameTimer _instance;
		private static readonly object _lock = new object();
		new public static InGameTimer Instance
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

		new public static InGameTimer GetInstance()
		{
			if (_instance == null)
			{
				_instance = new InGameTimer();
			}
			return _instance;
		}

		protected float nowTime = 0f;

		public override float GetNowTime()
		{
			return nowTime;
		}

		private float timeScale = 1;
		public void SetTimeScale(float timeScale)
		{
			this.timeScale = timeScale;
		}

		public override void FixedUpdate()
		{
			if (!pause)
			{
				nowTime = nowTime + Time.fixedDeltaTime * this.timeScale;
				base.FixedUpdate();
			}
		}
	}
}
