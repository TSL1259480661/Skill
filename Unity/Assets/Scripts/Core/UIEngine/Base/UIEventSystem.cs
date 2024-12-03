using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIEngine
{
	public class UIEventSystem
	{
		private EventSystem eventSystem;
		public void Init(GameObject rootGo, UnityEngine.EventSystems.EventSystem eventSystem)
		{
			if (eventSystem == null)
			{
				GameObject go = new GameObject("EventSystem");
				go.transform.SetParent(rootGo.transform);
				go.transform.SetAsFirstSibling();

				this.eventSystem = go.AddComponent<UnityEngine.EventSystems.EventSystem>();
				go.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
			}
			else
			{
				this.eventSystem = eventSystem;
			}
		}
	}
}
