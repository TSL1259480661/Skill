using System;
using System.Collections.Generic;
using System.Text;

public static class AssetPathHelper
{
	/// <summary>
	/// 预制体后缀
	/// </summary>
	public const string prefab_extension = "prefab";

	/// <summary>
	/// 常用资源组装路径管理器
	/// </summary>
	private static Dictionary<string, Dictionary<string, string>> pathArray = new Dictionary<string, Dictionary<string, string>>();

	/// <summary>
	/// UI特效路径
	/// </summary>
	public static string ParseUIEffectPath(string fileName)
	{
		return ParsePathAction(UIEffectPath, fileName, prefab_extension);
	}

	/// <summary>
	/// 获取精灵图路径
	/// </summary>
	public static string ParseSpritePath(string spritePath, string spriteName)
	{
		return ParsePathAction(spritePath, spriteName);
	}

	/// <summary>
	/// 通用路径组装格式
	/// </summary>
	/// <param name="spritePath">目录路径</param>
	/// <param name="spriteName">资源名称</param>
	/// <param name="extension">资源后缀</param>
	/// <returns></returns>
	private static string ParsePathAction(string spritePath, string spriteName, string extension = null)
	{
		if (pathArray.TryGetValue(spritePath, out Dictionary<string, string> array))
		{
			if (array.TryGetValue(spriteName, out string fullPath))
			{
				return fullPath;
			}
			else
			{
				if (string.IsNullOrEmpty(extension))
				{
					fullPath = string.Format("{0}/{1}", spritePath, spriteName);
				}
				else
				{
					fullPath = string.Format("{0}/{1}.{2}", spritePath, spriteName, extension);
				}
				array.Add(spriteName, fullPath);
				return fullPath;
			}
		}
		else
		{
			string fullPath;
			array = new Dictionary<string, string>();
			if (string.IsNullOrEmpty(extension))
			{
				fullPath = string.Format("{0}/{1}", spritePath, spriteName);
			}
			else
			{
				fullPath = string.Format("{0}/{1}.{2}", spritePath, spriteName, extension);
			}
			array.Add(spriteName, fullPath);
			pathArray.Add(spritePath, array);
			return fullPath;
		}
	}

	/// <summary>
	/// UI特效根路径
	/// </summary>
	public const string UIEffectPath = "Assets/Art/Units/UIEffect";

	/// <summary>
	/// 通用精灵根路径
	/// </summary>
	public const string CommonSpritePath = "Assets/Bundles/Sprites/Dynamics/Common";

	/// <summary>
	/// 英雄半身精灵根路径
	/// </summary>
	public const string HeroBodySpritePath = "Assets/Bundles/Sprites/Dynamics/HeroBody";

	/// <summary>
	/// 英雄头像精灵根路径
	/// </summary>
	public const string HeroHeadSpritePath = "Assets/Bundles/Sprites/Dynamics/HeroHead";

	/// <summary>
	/// 英雄横向立绘精灵根路径
	/// </summary>
	public const string HeroLandscapeBodySpritePath = "Assets/Bundles/Sprites/Dynamics/HeroLandscapeBody";
	//英雄卡牌品质背景(英雄商店卡牌)
	public const string HeroShopCardQualitySpritePath = "Assets/Bundles/Sprites/Dynamics/HeroShopCard";
	//英雄卡牌品质背景(备战区手牌)
	public const string HeroHandCardQualitySpritePath = "Assets/Bundles/Sprites/Dynamics/HandCard";

	/// <summary>
	/// 装备精灵根路径
	/// </summary>
	public const string EquipSpritePath = "Assets/Bundles/Sprites/Dynamics/EquipIcon";

	/// <summary>
	/// 技能精灵根路径
	/// </summary>
	public const string SkilSpritePath = "Assets/Bundles/Sprites/Dynamics/SkillIcon";

	/// <summary>
	/// 物品精灵根路径
	/// </summary>
	public const string ItemSpritePath = "Assets/Bundles/Sprites/Dynamics/Item";

	/// <summary>
	/// 连携精灵根路径
	/// </summary>
	public const string LinkSpritePath = "Assets/Bundles/Sprites/Dynamics/LinkIcon";

	/// <summary>
	/// 肉鸽精灵根路径
	/// </summary>
	public const string IntensifySpritePath = "Assets/Bundles/Sprites/Dynamics/Intensify";

	/// <summary>
	/// 商品精灵根路径
	/// </summary>
	public const string GoodsSpritePath = "Assets/Bundles/Sprites/Dynamics/Item";

	/// <summary>
	/// 新手引导精灵根路径
	/// </summary>
	public const string GuideSpritePath = "Assets/Bundles/Sprites/Dynamics/Guide";

	/// <summary>
	/// 地图路径
	/// </summary>
	public const string GroundSpritePath = "Assets/Bundles/Sprites/Dynamics/GroundImage";
}
