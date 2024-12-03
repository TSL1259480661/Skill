using Spine;
using System;
using System.Collections.Generic;

namespace App
{
	public class Timer : Singleton<Timer>, IDisposable
	{
		class TimeVo : LinkedItemNode, IObjectPoolItem
		{
			public float duration = 0;
			public int executeTimes = 1;
			public float startTime = 0;
			public int doExecuteTimes = 0;
			public object param = null;
			public Action<int, object> cCallback;
			public int id = 0;

			public void OnRecycle()
			{
				duration = 0;
				executeTimes = 1;
				startTime = 0;
				doExecuteTimes = 0;
				param = null;
				cCallback = null;
				id = 0;
			}

			public void OnReuse()
			{
			}
		}

		public virtual float GetNowTime()
		{
			return UnityEngine.Time.realtimeSinceStartup;
		}

		//public Action<string> onError;
		private static UDebugger debugger = new UDebugger("Timer");

		private LinkedItemList timeList = new LinkedItemList();
		private List<Action<int, object>> callbackList = new List<Action<int, object>>(100);
		private List<int> callbackIdList = new List<int>(100);
		private List<object> paramList = new List<object>(100);
		private Dictionary<int, TimeVo> timerDic = new Dictionary<int, TimeVo>(100);
		private UObjectPool<TimeVo> pool = new UObjectPool<TimeVo>(100);
		private int _count = 0;
		private float delayCallCount = 0;
		private float nowTime = 0;

		public virtual void FixedUpdate()
		{
			if (!pause)
			{
				nowTime = GetNowTime();

				delayCallCount += 1;

				if (delayCallCount > 1)
				{
					delayCallCount = 0;

					if (timeList.Count > 0)
					{
						TimeVo current = (TimeVo)timeList.First;
						while (current != null)
						{
							if (nowTime - current.startTime >= current.duration)
							{
								current.startTime = nowTime;

								callbackList.Add(current.cCallback);
								paramList.Add(current.param);
								callbackIdList.Add(current.id);

								if (current.executeTimes > 0)
								{
									current.doExecuteTimes += 1;

									if (current.doExecuteTimes >= current.executeTimes)
									{
										TimeVo old = current;
										current = current.Next as TimeVo;
										Remove(old.id);
										continue;
									}
								}
							}

							current = current.Next as TimeVo;
						}

						if (callbackList.Count > 0)
						{
							for (int i = 0; i < callbackList.Count; i++)
							{
								try
								{
									if (callbackList[i] != null)
									{
										callbackList[i].Invoke(callbackIdList[i], paramList[i]);
									}
								}
								catch (Exception e)
								{
									debugger.LogError(e);
								}
							}

							callbackIdList.Clear();
							callbackList.Clear();
							paramList.Clear();
						}
					}
				}
			}
		}

		public int Add(float duration, int executeTimes, Action<int, object> cCallBack)
		{
			return Add(duration, executeTimes, cCallBack, null);
		}

		public int Add(float duration, int executeTimes, Action<int, object> cCallBack, object param)
		{
			TimeVo vo = GetVo(duration, executeTimes, cCallBack, param);
			timeList.AddLast(vo);
			return vo.id;
		}

		public bool GetIsRunning(int id)
		{
			return timerDic.ContainsKey(id);
		}

		public void Quick(int id)
		{
			TimeVo next = null;
			if (timerDic.TryGetValue(id, out next))
			{
				next.duration = 0;
			}
		}

		public void Remove(int id)
		{
			TimeVo next = null;
			if (timerDic.TryGetValue(id, out next))
			{
				pool.Recycle(next);
				timeList.Delink(next);
				timerDic.Remove(id);
			}
		}
		public void RecycleItem(LinkedItemNode node)
		{
			TimeVo vo = (TimeVo)node;
			pool.Recycle(vo);
		}

		public void Dispose()
		{
			if (timeList.Count > 0)
			{
				LinkedItemNode current = timeList.First;
				while (current != null && current != timeList.StaticLast)
				{
					if (current.traversalIndex != timeList.traversalIndex)
					{
						current.traversalIndex = timeList.traversalIndex;
						RecycleItem(current);
					}
					current = current.Next;
				}
			}
			timeList.Clear();

			timerDic.Clear();
		}

		private TimeVo GetVo(float duration, int executeTimes, Action<int, object> cCallBack, object param)
		{
			TimeVo vo = pool.Get();

			_count += 1;
			if (_count >= int.MaxValue)
			{
				_count = 0;
			}

			if (vo != null)
			{
				vo.startTime = GetNowTime();
				vo.duration = duration;
				vo.executeTimes = executeTimes;
				vo.cCallback = cCallBack;
				vo.param = param;
				vo.doExecuteTimes = 0;
				vo.id = _count;
				timerDic[vo.id] = vo;
			}
			return vo;
		}

		protected bool pause;
		public void Run()
		{
			pause = false;
		}

		public void Pause()
		{
			pause = true;
		}
	}
}
