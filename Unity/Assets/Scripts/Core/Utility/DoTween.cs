using System.Collections.Generic;
using UnityEngine;
using System;
using Spine;

namespace App
{
	public class DoTween : Singleton<DoTween>, IDisposable
	{
		class DoTweenVo : LinkedItemNode, IObjectPoolItem
		{
			public int mode = 1;
			public float fromX = 0;
			public float fromY = 0;
			public float fromZ = 0;
			public float toX = 0;
			public float toY = 0;
			public float toZ = 0;
			public float runTime = 0;
			public float duration = 0;
			public object param = null;
			public Action<int, object> doneCallback;
			public Action<float, object> cCallback1;
			public Action<float, float, object> cCallback2;
			public Action<float, float, float, object> cCallback3;
			public int id = 0;

			public void OnRecycle()
			{
				fromX = 0;
				fromY = 0;
				fromZ = 0;
				toX = 0;
				toY = 0;
				toZ = 0;
				mode = 1;
				duration = 0;
				runTime = 0;
				param = null;
				doneCallback = null;
				cCallback1 = null;
				cCallback2 = null;
				cCallback3 = null;
				id = 0;
			}

			public void OnReuse()
			{
			}
		}

		protected virtual float GetDeltaTime()
		{
			return Time.fixedDeltaTime;
		}

		public Action<string> onError;

		private LinkedItemList timeList = new LinkedItemList();
		private Dictionary<int, DoTweenVo> timerDic = new Dictionary<int, DoTweenVo>(100);
		private UObjectPool<DoTweenVo> pool = new UObjectPool<DoTweenVo>(100);
		private int _count = 0;

		public virtual void FixedUpdate()
		{
			if (timeList.Count > 0)
			{
				float deltaTime = GetDeltaTime();
				DoTweenVo current = (DoTweenVo)timeList.First;
				while (current != null)
				{
					current.runTime += deltaTime;

					if (current.duration > 0)
					{
						if (current.runTime > current.duration)
						{
							current.runTime = current.duration;
						}

						float rate = current.runTime / current.duration;

						Execute(current, rate);
					}

					if (current.runTime >= current.duration)
					{
						int oldId = current.id;
						var next = current.Next as DoTweenVo;

						current.doneCallback?.Invoke(current.id, current.param);

						current = next;
						Remove(oldId);
					}
					else
					{
						current = current.Next as DoTweenVo;
					}
				}
			}
		}

		private void Execute(DoTweenVo current, float rate)
		{
			if (current != null)
			{
				float x = Mathf.Lerp(current.fromX, current.toX, rate);

				if (current.mode == 1)
				{
					current.cCallback1?.Invoke(x, current.param);
				}
				else if (current.mode == 2)
				{
					float y = Mathf.Lerp(current.fromY, current.toY, rate);
					current.cCallback2?.Invoke(x, y, current.param);
				}
				else if (current.mode == 3)
				{
					float y = Mathf.Lerp(current.fromY, current.toY, rate);
					float z = Mathf.Lerp(current.fromZ, current.toZ, rate);
					current.cCallback3?.Invoke(x, y, z, current.param);
				}
			}
		}

		public int Add(float fromX, float toX, float duration, Action<float, object> cCallBack)
		{
			return Add(fromX, toX, duration, cCallBack, null, null);
		}

		public int Add(float fromX, float toX, float duration, Action<float, object> cCallBack, Action<int, object> doneCallBack)
		{
			return Add(fromX, toX, duration, cCallBack, doneCallBack, null);
		}

		public int Add(float fromX, float toX, float duration, Action<float, object> cCallBack, Action<int, object> doneCallBack, object param)
		{
			DoTweenVo vo = GetVo(fromX, 0, 0, toX, 0
			, 0, duration, param, 1);
			vo.cCallback1 = cCallBack;
			vo.doneCallback = doneCallBack;
			timeList.AddLast(vo);
			Execute(vo, 0);
			return vo.id;
		}

		public int Add(float fromX, float fromY, float toX, float toY, float duration, Action<float, float, object> cCallBack)
		{
			return Add(fromX, fromY, toX, toY, duration, cCallBack, null, null);
		}

		public int Add(float fromX, float fromY, float toX, float toY, float duration, Action<float, float, object> cCallBack, Action<int, object> doneCallBack)
		{
			return Add(fromX, fromY, toX, toY, duration, cCallBack, doneCallBack, null);
		}

		public int Add(float fromX, float fromY, float toX, float toY, float duration, Action<float, float, object> cCallBack, Action<int, object> doneCallBack, object param)
		{
			DoTweenVo vo = GetVo(fromX, fromY, 0, toX, toY
			, 0, duration, param, 2);
			vo.cCallback2 = cCallBack;
			vo.doneCallback = doneCallBack;
			timeList.AddLast(vo);
			Execute(vo, 0);
			return vo.id;
		}

		public int Add(float fromX, float fromY, float fromZ, float toX, float toY, float toZ, float duration, Action<float, float, float, object> cCallBack)
		{
			return Add(fromX, fromY, fromZ, toX, toY, toZ, duration, cCallBack, null, null);
		}

		public int Add(float fromX, float fromY, float fromZ, float toX, float toY, float toZ, float duration, Action<float, float, float, object> cCallBack, Action<int, object> doneCallBack)
		{
			return Add(fromX, fromY, fromZ, toX, toY, toZ, duration, cCallBack, doneCallBack, null);
		}

		public int Add(float fromX, float fromY, float fromZ, float toX, float toY, float toZ, float duration, Action<float, float, float, object> cCallBack, Action<int, object> doneCallBack, object param)
		{
			DoTweenVo vo = GetVo(fromX, fromY, fromZ, toX, toY
			, toZ, duration, param, 3);
			vo.cCallback3 = cCallBack;
			vo.doneCallback = doneCallBack;
			timeList.AddLast(vo);
			Execute(vo, 0);
			return vo.id;
		}


		public int Add(Vector2 from, Vector2 to, float duration, Action<float, float, object> cCallBack, Action<int, object> doneCallBack)
		{
			return Add(from.x, from.y, to.x, to.y, duration, cCallBack, doneCallBack);
		}

		public int Add(Vector3 from, Vector3 to, float duration, Action<float, float, float, object> cCallBack, Action<int, object> doneCallBack)
		{
			return Add(from.x, from.y, from.z, to.x, to.y, to.z, duration, cCallBack, doneCallBack);
		}


		public bool GetIsRunning(int id)
		{
			return timerDic.ContainsKey(id);
		}

		public void Quick(int id)
		{
			DoTweenVo next = null;
			if (timerDic.TryGetValue(id, out next))
			{
				next.duration = 0;
			}
		}

		public void Remove(int id)
		{
			Remove(id, false);
		}

		public void Remove(int id, bool complete)
		{
			DoTweenVo next = null;
			if (timerDic.TryGetValue(id, out next))
			{
				if (complete)
				{
					Execute(next, 1f);
				}

				timeList.Delink(next);
				pool.Recycle(next);
				timerDic.Remove(id);
			}
		}

		public void RecycleItem(LinkedItemNode node)
		{
			DoTweenVo vo = (DoTweenVo)node;
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

		private DoTweenVo GetVo(float fromX, float fromY, float fromZ, float toX, float toY, float toZ, float duration, object param, int mode = 1)
		{
			DoTweenVo vo = pool.Get();

			_count += 1;
			if (_count >= int.MaxValue)
			{
				_count = 0;
			}

			if (vo != null)
			{
				vo.runTime = 0f;
				vo.mode = mode;
				vo.fromX = fromX;
				vo.fromY = fromY;
				vo.fromZ = fromZ;
				vo.toX = toX;
				vo.toY = toY;
				vo.toZ = toZ;
				vo.duration = duration;
				vo.param = param;
				vo.id = _count;
				timerDic[vo.id] = vo;
			}
			return vo;
		}
	}
}
