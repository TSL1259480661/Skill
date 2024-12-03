using System;

namespace App
{
	public class EventSystemContainer
	{
		private EventSystem selfEventSystem = new EventSystem(10);

		private EventSystem eventSystem;

		private Action onLockAdd;
		public void Init(EventSystemContainer eventSystemContainer, Action onLockAdd = null)
		{
			this.eventSystem = eventSystemContainer.eventSystem;
			this.removed = false;
			this.onLockAdd = onLockAdd;
			selfEventSystem.Clear();
		}

		public void Init(EventSystem eventSystem, Action onLockAdd = null)
		{
			this.eventSystem = eventSystem;
			this.removed = false;
			this.onLockAdd = onLockAdd;
			selfEventSystem.Clear();
		}

		private bool lockAdd;
		public void SetLock(bool lockAdd)
		{
			this.lockAdd = lockAdd;
		}

		private bool removed = false;
		public void RemoveAllListeners()
		{
			if (!removed)
			{
				removed = true;
				eventSystem.RemoveListener(selfEventSystem);
			}
		}

		public void AddAllListeners()
		{
			if (removed)
			{
				removed = false;
				eventSystem.AddListener(selfEventSystem);
			}
		}

		public int AddListener<T>(Action<T> callback) where T : struct
		{
			if (lockAdd)
			{
				onLockAdd?.Invoke();
			}
			else
			{
				int id = eventSystem.AddListener<T>(callback);
				selfEventSystem.AddListener<T>(callback, id);
				return id;
			}
			return -1;
		}

		public bool RemoveListener(int listenerId)
		{
			selfEventSystem.RemoveListener(listenerId);
			return eventSystem.RemoveListener(listenerId);
		}

		public void Dispatch<T>(T data) where T : struct
		{
			eventSystem.Dispatch(data);
		}

		public void Clear()
		{
			removed = false;

			RemoveAllListeners();

			selfEventSystem.Clear();

			removed = false;

			eventSystem = null;
		}
	}
}
