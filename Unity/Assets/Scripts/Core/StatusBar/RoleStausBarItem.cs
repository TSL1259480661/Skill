using UnityEngine;
/// <summary>
/// 状态栏单体，数据操控
/// </summary>
public class RoleStausBarItem : IObjectPoolItem
{
	private GameObject g;
	private Transform hp;
	private Transform mp;
	private Transform pp;
	private SpriteRenderer mpRender;

	private ResourceInstanceItem resourceInstance;
	private static UObjectPool<RoleStausBarItem> pool = new UObjectPool<RoleStausBarItem>();

	private float standSx;
	private float standSy;
	private float standSz;

	private Color nor = new Color(1, 0.5f, 0, 1);
	private Color stun = new Color(1, 1, 1, 1);
	public void Init(ResourceInstanceItem resourceInstance)
	{
		this.resourceInstance = resourceInstance;
		this.g = resourceInstance.gameObject;
		this.hp = g.transform.GetChild(0).GetChild(0);
		this.mp = g.transform.GetChild(1).GetChild(0);
		this.pp = g.transform.GetChild(2).GetChild(0);
		this.mpRender = mp.GetComponent<SpriteRenderer>();
		standSx = hp.localScale.x;
		standSy = hp.localScale.y;
		standSz = hp.localScale.z;
	}
	/// <summary>
	/// 初始化血条
	/// </summary>
	/// <param name="parent">父物体</param>
	/// <param name="scale">缩放值</param>
	/// <param name="offset">偏移量（似乎用不上，看情况）</param>
	/// <param name="type">类型</param>
	public void InitParam(Transform parent, int scale,Vector3 offset,MonsterType type)
	{
		g.transform.SetParent(parent);
		g.transform.localScale = Vector3.one * scale;
		g.transform.localPosition = offset;

		Revert();
		#region 现阶段为测试状态，后续根据SceneObjectd的枚举进行判定
		if (type == MonsterType.Normal)
		{
			if(hp!=null) hp.parent.localPosition = new Vector3(0,0.15f,0);
			if (mp != null) mp.parent.gameObject.SetActive(true);
		}
		if(type == MonsterType.Boss)
		{
			g.SetActive(false);
		}
		if(type == MonsterType.Player)
		{
			if (hp!=null) hp.parent.localPosition = Vector3.zero;
			if (mp != null) mp.parent.gameObject.SetActive(false);
		}
		#endregion
	}

	public static RoleStausBarItem Create()
	{
		return pool.Get();
	}

	public void StartStun()//开始眩晕
	{
		mpRender.color = stun;
		RefreshMp(1);
	}

	public void EndStun()//眩晕结束
	{
		mpRender.color = nor;
		RefreshMp(1);
	}
	
	public void StartBehavior()//开始格挡或者闪避
	{
		if (pp != null) pp.parent.gameObject.SetActive(true);
		if (mp != null) mp.parent.gameObject.SetActive(false);
	}
	public void EndBehavior()//结束格挡或者闪避
	{
		if (pp != null) pp.parent.gameObject.SetActive(false);
		if (mp != null) mp.parent.gameObject.SetActive(true);
		RefreshPp(1);
	}

	public void RefreshHp(float pct)//生命值
	{
		if (hp != null)
		{
			hp.localScale = new Vector3(standSx * pct, standSy, standSz);
		}	
	}
	public void RefreshMp(float pct)//架势值,减少
	{
		if (mp != null)
		{
			mp.localScale = new Vector3(standSx * pct, standSy, standSz);
		}
	}
	public void RefreshPp(float pct)//格挡闪避配合读条
	{
		if (pp != null)
		{
			pp.localScale = new Vector3(standSx * pct, standSy, standSz);
		}
	}

	public void Revert()
	{
		RefreshPp(1);
		RefreshMp(1);
		RefreshHp(1);
		EndBehavior();
		EndStun();
	}
	public void Recycle()
	{
		Revert();
		resourceInstance?.Recycle();
		pool.Recycle(this);
	}

	public void OnRecycle()
	{
		g = null;
		hp = null;
		mp = null;
		pp = null;
		mpRender = null;
	}

	public void OnReuse()
	{
		
	}
}
