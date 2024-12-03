using System;
using System.Collections.Generic;
using UnityEngine;

public class AppUtility
{
	public static int BitAnd(int value1, int value2)
	{
		return value1 & value2;
	}

	public static void SetActiveRecursively(Transform transform, bool active)
	{
		transform.gameObject.SetActive(active);
		if (active)
		{
			for (int i = 0, cnt = transform.childCount; i < cnt; ++i)
			{
				SetActiveRecursively(transform.GetChild(i), active);
			}
		}
	}

	public static T AddIfMissing<T>(GameObject go) where T : Component
	{
		T component = (T)go.GetComponent(typeof(T));
		if (component == null)
		{
			return (T)go.AddComponent(typeof(T));
		}
		return component;
	}

	public static MonoBehaviour AddIfMissing(GameObject go, Type type)
	{
		MonoBehaviour component = (MonoBehaviour)go.GetComponent(type);
		if (component == null)
		{
			return (MonoBehaviour)go.AddComponent(type);
		}
		return component;
	}

	public static MonoBehaviour AddIfMissing(GameObject go, string typeName)
	{
		MonoBehaviour component = (MonoBehaviour)go.GetComponent(typeName);
		if (component == null)
		{
			return (MonoBehaviour)go.AddComponent(Type.GetType(typeName));
		}
		return component;
	}

	public static T GetComponentInChildren<T>(GameObject go) where T : Component
	{
		T[] list = GetComponentsInChildren<T>(go);
		if (list.Length > 0)
		{
			return list[0];
		}
		return default(T);
	}

	public static T[] GetComponentsInChildren<T>(GameObject go) where T : Component
	{
		List<T> items = new List<T>();
		DoGetComponentsInChildren<T>(go.transform, items);
		return items.ToArray();
	}

	private static void DoGetComponentsInChildren<T>(Transform ts, List<T> items) where T : Component
	{
		if (ts != null)
		{
			T component = ts.GetComponent<T>();
			if (component != null)
			{
				items.Add(component);
			}

			for (int i = 0; i < ts.childCount; i++)
			{
				DoGetComponentsInChildren<T>(ts.GetChild(i), items);
			}
		}
	}

	public static Transform FindChild(Transform parent, string childName)
	{
		if (parent != null && parent.name == childName)
		{
			return parent;
		}
		else
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform target = FindChild(parent.GetChild(i), childName);
				if (target != null)
				{
					return target;
				}
			}
		}
		return null;
	}

	public static GameObject FindChild(GameObject parent, int index)
	{
		if (parent == null)
		{
			return null;
		}
		return parent.transform.GetChild(index).gameObject;
	}

	public static void AddChild(Component parent, GameObject child)
	{
		if (parent != null)
		{
			AddChild(parent.gameObject, child);
		}
	}

	public static void AddChild(GameObject parent, GameObject child)
	{
		AddChild(parent, child, true, true);
	}

	public static void AddChild(GameObject parent, GameObject child, bool resetPosition)
	{
		AddChild(parent, child, resetPosition, true);
	}

	public static void AddChild(GameObject parent, GameObject child, bool resetPosition, bool resetScale)
	{
		AddChild(parent, child, resetPosition, resetScale, true);
	}

	public static void AddChild(GameObject parent, GameObject child, bool resetPosition, bool resetScale, bool resetRotation)
	{
		Transform parentTs = parent == null ? null : parent.transform;
		Transform childTs = child == null ? null : child.transform;

		if (childTs != null)
		{
			RectTransform rect = child.gameObject.GetComponent<RectTransform>();
			if (rect != null)
			{
				rect.SetParent(null);
				rect.SetParent(parentTs);

				if (resetScale)
				{
					rect.localScale = Vector3.one;
				}

				if (resetPosition)
				{
					rect.localPosition = Vector3.zero;
				}

				if (resetRotation)
				{
					rect.localRotation = Quaternion.Euler(0f, 0f, 0f);
				}
			}
			else
			{
				childTs.SetParent(parentTs);

				if (resetScale)
				{
					childTs.localScale = Vector3.one;
				}

				if (resetPosition)
				{
					childTs.localPosition = Vector3.zero;
				}

				if (resetRotation)
				{
					childTs.localRotation = Quaternion.Euler(0f, 0f, 0f);
				}
			}

			childTs.SetAsLastSibling();
		}
	}

	public static void SetParent(GameObject child, GameObject parent)
	{
		if (child != null)
		{
			SetParent(child.transform, parent.transform);
		}
	}

	public static void SetParent(Transform child, Transform parent)
	{
		if (child != null)
		{
			RectTransform rect = AddIfMissing<RectTransform>(child.gameObject);
			if (rect != null)
			{
				rect.SetParent(null);
				rect.SetParent(parent);

				rect.pivot = new Vector2(.5f, .5f);
				rect.offsetMin = Vector2.zero;
				rect.offsetMax = Vector2.zero;
				rect.anchorMin = Vector2.zero;
				rect.anchorMax = Vector2.one;
				rect.anchoredPosition = Vector2.zero;
				rect.localScale = Vector3.one;
				rect.localPosition = Vector3.zero;
			}
		}
	}

	public static void InitBones(Transform child, List<string> boneList, Dictionary<string, Transform> boneDic)
	{
		if (child != null)
		{
			string childName = child.name;

			if (boneDic != null)
			{
				boneDic[childName] = child;
			}

			if (boneList != null)
			{
				boneList.Add(childName);
			}

			for (int i = 0; i < child.childCount; i++)
			{
				InitBones(child.GetChild(i), boneList, boneDic);
			}
		}
	}

	public static void SetScale(GameObject obj, float x, float y, float z)
	{
		obj.transform.localScale = new Vector3(x, y, z);
	}

	public static void SetScale(Component obj, float x, float y, float z)
	{
		obj.transform.localScale = new Vector3(x, y, z);
	}

	public static void GetAllChildren<T>(List<T> rendererList, Transform ts) where T : Component
	{
		if (ts != null)
		{
			T renderer = ts.GetComponent<T>();
			if (renderer != null)
			{
				rendererList.Add(renderer);
			}
		}

		for (int i = 0; i < ts.childCount; i++)
		{
			Transform child = ts.GetChild(i);
			GetAllChildren(rendererList, child);
		}
	}

	public static void SetLayer(GameObject go, string layer)
	{
		SetLayer(go, LayerMask.NameToLayer(layer));
	}

	public static void SetLayer(GameObject go, int layer)
	{
		if (go != null)
		{
			SetLayer(go.transform, layer);
		}
	}

	public static void SetLayer(Transform ts, int layer)
	{
		if (ts != null)
		{
			if (ts.gameObject.layer != layer)
			{
				ts.gameObject.layer = layer;
			}
		}

		for (int i = 0; i < ts.childCount; i++)
		{
			Transform child = ts.GetChild(i);
			SetLayer(child, layer);
		}
	}
}
