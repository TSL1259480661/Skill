using UnityEngine;

public interface IUILayer
{
	void Add(GameObject go, GameObject parent = null);
	void Add(Transform transform, Transform parent = null);
	void Add(ILayerItem item);
	void ResetOrderInLayers();
}
