using App;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneLayer : IUILayer
{
	private static UDebugger debugger = new UDebugger("GameSceneLayer");

	private Transform layerRoot;
	private string sortingLayerName;
	private int layerMask;

	public GameSceneLayer(string layerName, Transform parent, Vector3 position, int layerMask)
	{
		if (layerRoot == null)
		{
			layerRoot = new GameObject().transform;
#if UNITY_EDITOR
			layerRoot.name = layerName;
#endif
			layerRoot.gameObject.layer = layerMask;
		}
		layerRoot.SetParent(parent, true);
		layerRoot.position = position;

		sortingLayerName = layerName;

		this.layerMask = layerMask;

		maxOrder = 1;
	}

	private int maxOrder;
	List<Transform> decorateList = new List<Transform>();
	public void SortPositionY()
	{
		if (layerRoot != null)
		{
			decorateList.Clear();

			for (int i = 0; i < layerRoot.childCount; i++)
			{
				Transform child = layerRoot.GetChild(i);
				decorateList.Add(child);
			}

			//y大的在前面
			decorateList.Sort((a, b) => b.localPosition.y.CompareTo(a.localPosition.y));

			for (int i = 0; i < decorateList.Count; i++)
			{
				decorateList[i].SetAsLastSibling();
			}

			decorateList.Clear();
		}

		ResetOrderInLayers();
	}

	public void ResetOrderInLayers()
	{
		maxOrder = 1;

		ResetOrderInLayers(layerRoot, true);
	}

	public void ResetOrderInLayers(Transform transform, bool onlyChangeOrder = false)
	{
		if (transform != null)
		{
			transform.gameObject.layer = layerMask;

			Renderer renderer = transform.GetComponent<Renderer>();
			if (renderer != null)
			{
				if (!onlyChangeOrder)
				{
					renderer.sortingLayerName = sortingLayerName;
				}

				renderer.sortingOrder = maxOrder++;
			}

			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				ResetOrderInLayers(child);
			}
		}
	}

	public void Add(GameObject go, GameObject parent = null)
	{
		if (go != null)
		{
			if (parent != null)
			{
				Add(go.transform, parent.transform);
			}
			else
			{
				Add(go.transform);
			}
		}
	}

	public void Add(Transform transform, Transform parent = null)
	{
		if (transform != null)
		{
			if (parent == null)
			{
				parent = layerRoot;
			}

			transform.SetParent(parent);

			transform.localPosition = Vector3.zero;

			ResetOrderInLayers(transform);
		}
	}

	public void Add(ILayerItem item)
	{
	}
}
