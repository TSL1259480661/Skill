using System.Collections.Generic;
using System;

namespace App
{
	public class EventSystem
	{
		private static int _id = 0;

		private interface IEvent
		{
			Type GetDataType();
			int GetId();
			void SetId(int id);
			void Recycle();
			IEvent Clone();
			bool IsRecycled();
		}

		private class EventVo<T> : IEvent where T : struct
		{
			private int id;
			public Action<T> callback;
			public Type dataType;

			public bool recycled;

			public bool IsRecycled()
			{
				return recycled;
			}

			public Type GetDataType()
			{
				return dataType;
			}

			public int GetId()
			{
				return id;
			}

			public void SetId(int id)
			{
				this.id = id;
			}

			public void Recycle()
			{
				recycled = true;
				callback = null;
				eventList.Add(this);
			}

			public IEvent Clone()
			{
				EventVo<T> eventVo = Get();
				eventVo.id = id;
				eventVo.dataType = dataType;
				eventVo.callback = callback;
				return eventVo;
			}

			private static List<EventVo<T>> eventList = new List<EventVo<T>>(22);
			public static EventVo<T> Get()
			{
				EventVo<T> eventItem;
				if (eventList.Count <= 0)
				{
					eventItem = new EventVo<T>();
				}
				else
				{
					eventItem = eventList[eventList.Count - 1];
					eventList.RemoveAt(eventList.Count - 1);
				}

				if (_id >= int.MaxValue)
				{
					_id = 0;
				}

				eventItem.recycled = false;
				eventItem.id = ++_id;
				return eventItem;
			}
		}

		public EventSystem(int capacity)
		{
			listeners = new Dictionary<Type, List<IEvent>>(capacity);
			listenersDic = new Dictionary<int, IEvent>(capacity);
		}

		private Dictionary<Type, List<IEvent>> listeners;
		private Dictionary<int, IEvent> listenersDic;

		public int AddListener<T>(Action<T> callback) where T : struct
		{
			return AddListener<T>(callback, 0);
		}

		public int AddListener<T>(Action<T> callback, int id) where T : struct
		{
			List<IEvent> list = null;
			if (!listeners.TryGetValue(typeof(T), out list))
			{
				list = new List<IEvent>(5);
				listeners[typeof(T)] = list;
			}
			EventVo<T> eventVo = EventVo<T>.Get();
			eventVo.callback = callback;
			eventVo.dataType = typeof(T);
			if (id > 0)
			{
				eventVo.SetId(id);
			}
			listenersDic[eventVo.GetId()] = eventVo;
			list.Add(eventVo);
			return eventVo.GetId();
		}

		private void AddListener(IEvent eventVo)
		{
			Type dataType = eventVo.GetDataType();
			List<IEvent> list = null;
			if (!listeners.TryGetValue(dataType, out list))
			{
				list = new List<IEvent>(5);
				listeners[dataType] = list;
			}
			listenersDic[eventVo.GetId()] = eventVo;
			list.Add(eventVo);
		}

		public bool AddListener(EventSystem listenerSystem)
		{
			if (listenerSystem != null)
			{
				foreach (var item in listenerSystem.listeners)
				{
					for (int i = 0; i < item.Value.Count; i++)
					{
						AddListener(item.Value[i].Clone());
					}
				}
				return true;
			}
			return false;
		}

		public bool RemoveListener(int id)
		{
			List<IEvent> list = null;
			if (listenersDic.TryGetValue(id, out IEvent eventVo))
			{
				if (listeners.TryGetValue(eventVo.GetDataType(), out list))
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].GetId() == id)
						{
							list[i].Recycle();
							list.RemoveAt(i);
							listenersDic.Remove(id);
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool RemoveListener(EventSystem listenerSystem)
		{
			if (listenerSystem != null)
			{
				foreach (var item in listenerSystem.listeners)
				{
					for (int i = 0; i < item.Value.Count; i++)
					{
						RemoveListener(item.Value[i].GetId());
					}
				}
			}
			return false;
		}

		private List<IEvent> tempList = new List<IEvent>(500);
		public void Dispatch<T>(T data) where T : struct
		{
			bool needClear = false;
			if (tempList.Count <= 0)
			{
				needClear = true;
			}

			int startIndex = 0;
			if (tempList.Count > 0)
			{
				startIndex = tempList.Count;
			}

			List<IEvent> list = null;
			if (listeners.TryGetValue(data.GetType(), out list))
			{
				tempList.AddRange(list);
			}

			int count = tempList.Count;
			if (count > startIndex)
			{
				for (int i = startIndex; i < count; i++)
				{
					if (!tempList[i].IsRecycled())
					{
						var vo = tempList[i] as EventVo<T>;
						if (vo != null)
						{
							vo.callback?.Invoke(data);
						}
					}
				}
			}

			if (needClear)
			{
				tempList.Clear();
			}
		}

		public void Clear()
		{
			foreach (var item in listeners)
			{
				item.Value.Clear();
			}
			listeners.Clear();

			foreach (var item in listenersDic)
			{
				item.Value.Recycle();
			}
			listenersDic.Clear();

			tempList.Clear();
		}
	}
}
