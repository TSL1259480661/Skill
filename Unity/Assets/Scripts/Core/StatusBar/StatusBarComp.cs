using System;
using UIEngine;
/// <summary>
/// 状态栏组件
/// </summary>
public class StatusBarComp 
{
	private static ResourceInstanceItemPool prefabItemPool;

	private UILayerRoot layerRoot;
	public void Init(IUILoadAsset loadAsset,UILayerRoot layerRoot)
	{
		prefabItemPool = new ResourceInstanceItemPool(loadAsset);
		this.layerRoot = layerRoot;
	}
	public void Creat(string assetPath, Action<RoleStausBarItem> creatDone)
	{
		prefabItemPool.Create(assetPath, (instanceItem) =>
		{
			OnLoadItemDone(instanceItem, creatDone);
		});
	}
	private void OnLoadItemDone(ResourceInstanceItem instanceItem, Action<RoleStausBarItem> creatDone)
	{
		RoleStausBarItem item = RoleStausBarItem.Create();
		item.Init(instanceItem);
		layerRoot.GetLayer(UILayerType.Character).Add(instanceItem.gameObject);
		creatDone.Invoke(item);
	}
}
