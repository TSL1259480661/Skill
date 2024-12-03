public interface ILayerItem
{
	public int layerIndex { get; }
	public bool GetSortEnable();
	public void SetSortingOrder(int sortingOrder);
	public void SetSortingLayer(int layer);
}
