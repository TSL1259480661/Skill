using ClientData;
using App;
using System;

public class ConfigModule : ConfigModuleBase 
{
	private static UDebugger debugger = new UDebugger("ConfigModule");

	private static string Asset_effectCategoryPath = "Assets/Bundles/Config/Asset_effectCategory.bytes";
	private Asset_effectCategory _asset_effectCategory;
	public Asset_effectCategory asset_effectCategory
	{
		get
		{
			if (_asset_effectCategory == null)
			{
				_asset_effectCategory = configDic[Asset_effectCategoryPath] as Asset_effectCategory;
			}
			return _asset_effectCategory;
		}
	}
	private static string Asset_pictureCategoryPath = "Assets/Bundles/Config/Asset_pictureCategory.bytes";
	private Asset_pictureCategory _asset_pictureCategory;
	public Asset_pictureCategory asset_pictureCategory
	{
		get
		{
			if (_asset_pictureCategory == null)
			{
				_asset_pictureCategory = configDic[Asset_pictureCategoryPath] as Asset_pictureCategory;
			}
			return _asset_pictureCategory;
		}
	}
	private static string Asset_soundCategoryPath = "Assets/Bundles/Config/Asset_soundCategory.bytes";
	private Asset_soundCategory _asset_soundCategory;
	public Asset_soundCategory asset_soundCategory
	{
		get
		{
			if (_asset_soundCategory == null)
			{
				_asset_soundCategory = configDic[Asset_soundCategoryPath] as Asset_soundCategory;
			}
			return _asset_soundCategory;
		}
	}
	private static string Buff_Desc_buffCategoryPath = "Assets/Bundles/Config/Buff_Desc_buffCategory.bytes";
	private Buff_Desc_buffCategory _buff_Desc_buffCategory;
	public Buff_Desc_buffCategory buff_Desc_buffCategory
	{
		get
		{
			if (_buff_Desc_buffCategory == null)
			{
				_buff_Desc_buffCategory = configDic[Buff_Desc_buffCategoryPath] as Buff_Desc_buffCategory;
			}
			return _buff_Desc_buffCategory;
		}
	}
	private static string Buff_Desc_buff_displayCategoryPath = "Assets/Bundles/Config/Buff_Desc_buff_displayCategory.bytes";
	private Buff_Desc_buff_displayCategory _buff_Desc_buff_displayCategory;
	public Buff_Desc_buff_displayCategory buff_Desc_buff_displayCategory
	{
		get
		{
			if (_buff_Desc_buff_displayCategory == null)
			{
				_buff_Desc_buff_displayCategory = configDic[Buff_Desc_buff_displayCategoryPath] as Buff_Desc_buff_displayCategory;
			}
			return _buff_Desc_buff_displayCategory;
		}
	}
	private static string Buff_Desc_buff_display_positionCategoryPath = "Assets/Bundles/Config/Buff_Desc_buff_display_positionCategory.bytes";
	private Buff_Desc_buff_display_positionCategory _buff_Desc_buff_display_positionCategory;
	public Buff_Desc_buff_display_positionCategory buff_Desc_buff_display_positionCategory
	{
		get
		{
			if (_buff_Desc_buff_display_positionCategory == null)
			{
				_buff_Desc_buff_display_positionCategory = configDic[Buff_Desc_buff_display_positionCategoryPath] as Buff_Desc_buff_display_positionCategory;
			}
			return _buff_Desc_buff_display_positionCategory;
		}
	}
	private static string Buff_Desc_buff_effectCategoryPath = "Assets/Bundles/Config/Buff_Desc_buff_effectCategory.bytes";
	private Buff_Desc_buff_effectCategory _buff_Desc_buff_effectCategory;
	public Buff_Desc_buff_effectCategory buff_Desc_buff_effectCategory
	{
		get
		{
			if (_buff_Desc_buff_effectCategory == null)
			{
				_buff_Desc_buff_effectCategory = configDic[Buff_Desc_buff_effectCategoryPath] as Buff_Desc_buff_effectCategory;
			}
			return _buff_Desc_buff_effectCategory;
		}
	}
	private static string Buff_Desc_buff_typeCategoryPath = "Assets/Bundles/Config/Buff_Desc_buff_typeCategory.bytes";
	private Buff_Desc_buff_typeCategory _buff_Desc_buff_typeCategory;
	public Buff_Desc_buff_typeCategory buff_Desc_buff_typeCategory
	{
		get
		{
			if (_buff_Desc_buff_typeCategory == null)
			{
				_buff_Desc_buff_typeCategory = configDic[Buff_Desc_buff_typeCategoryPath] as Buff_Desc_buff_typeCategory;
			}
			return _buff_Desc_buff_typeCategory;
		}
	}
	private static string Global_Param_global_paramCategoryPath = "Assets/Bundles/Config/Global_Param_global_paramCategory.bytes";
	private Global_Param_global_paramCategory _global_Param_global_paramCategory;
	public Global_Param_global_paramCategory global_Param_global_paramCategory
	{
		get
		{
			if (_global_Param_global_paramCategory == null)
			{
				_global_Param_global_paramCategory = configDic[Global_Param_global_paramCategoryPath] as Global_Param_global_paramCategory;
			}
			return _global_Param_global_paramCategory;
		}
	}
	private static string Monster_Base_monsterG_LevelCategoryPath = "Assets/Bundles/Config/Monster_Base_monsterG_LevelCategory.bytes";
	private Monster_Base_monsterG_LevelCategory _monster_Base_monsterG_LevelCategory;
	public Monster_Base_monsterG_LevelCategory monster_Base_monsterG_LevelCategory
	{
		get
		{
			if (_monster_Base_monsterG_LevelCategory == null)
			{
				_monster_Base_monsterG_LevelCategory = configDic[Monster_Base_monsterG_LevelCategoryPath] as Monster_Base_monsterG_LevelCategory;
			}
			return _monster_Base_monsterG_LevelCategory;
		}
	}
	private static string Monster_Base_monster_baseCategoryPath = "Assets/Bundles/Config/Monster_Base_monster_baseCategory.bytes";
	private Monster_Base_monster_baseCategory _monster_Base_monster_baseCategory;
	public Monster_Base_monster_baseCategory monster_Base_monster_baseCategory
	{
		get
		{
			if (_monster_Base_monster_baseCategory == null)
			{
				_monster_Base_monster_baseCategory = configDic[Monster_Base_monster_baseCategoryPath] as Monster_Base_monster_baseCategory;
			}
			return _monster_Base_monster_baseCategory;
		}
	}
	private static string Monster_Base_monster_GroupCategoryPath = "Assets/Bundles/Config/Monster_Base_monster_GroupCategory.bytes";
	private Monster_Base_monster_GroupCategory _monster_Base_monster_GroupCategory;
	public Monster_Base_monster_GroupCategory monster_Base_monster_GroupCategory
	{
		get
		{
			if (_monster_Base_monster_GroupCategory == null)
			{
				_monster_Base_monster_GroupCategory = configDic[Monster_Base_monster_GroupCategoryPath] as Monster_Base_monster_GroupCategory;
			}
			return _monster_Base_monster_GroupCategory;
		}
	}
	private static string Monster_Base_monster_modelCategoryPath = "Assets/Bundles/Config/Monster_Base_monster_modelCategory.bytes";
	private Monster_Base_monster_modelCategory _monster_Base_monster_modelCategory;
	public Monster_Base_monster_modelCategory monster_Base_monster_modelCategory
	{
		get
		{
			if (_monster_Base_monster_modelCategory == null)
			{
				_monster_Base_monster_modelCategory = configDic[Monster_Base_monster_modelCategoryPath] as Monster_Base_monster_modelCategory;
			}
			return _monster_Base_monster_modelCategory;
		}
	}
	private static string Role_Appearance_role_appearanceCategoryPath = "Assets/Bundles/Config/Role_Appearance_role_appearanceCategory.bytes";
	private Role_Appearance_role_appearanceCategory _role_Appearance_role_appearanceCategory;
	public Role_Appearance_role_appearanceCategory role_Appearance_role_appearanceCategory
	{
		get
		{
			if (_role_Appearance_role_appearanceCategory == null)
			{
				_role_Appearance_role_appearanceCategory = configDic[Role_Appearance_role_appearanceCategoryPath] as Role_Appearance_role_appearanceCategory;
			}
			return _role_Appearance_role_appearanceCategory;
		}
	}
	private static string Role_Initial_role_initialCategoryPath = "Assets/Bundles/Config/Role_Initial_role_initialCategory.bytes";
	private Role_Initial_role_initialCategory _role_Initial_role_initialCategory;
	public Role_Initial_role_initialCategory role_Initial_role_initialCategory
	{
		get
		{
			if (_role_Initial_role_initialCategory == null)
			{
				_role_Initial_role_initialCategory = configDic[Role_Initial_role_initialCategoryPath] as Role_Initial_role_initialCategory;
			}
			return _role_Initial_role_initialCategory;
		}
	}
	private static string Skill_Desc_skill_activeCategoryPath = "Assets/Bundles/Config/Skill_Desc_skill_activeCategory.bytes";
	private Skill_Desc_skill_activeCategory _skill_Desc_skill_activeCategory;
	public Skill_Desc_skill_activeCategory skill_Desc_skill_activeCategory
	{
		get
		{
			if (_skill_Desc_skill_activeCategory == null)
			{
				_skill_Desc_skill_activeCategory = configDic[Skill_Desc_skill_activeCategoryPath] as Skill_Desc_skill_activeCategory;
			}
			return _skill_Desc_skill_activeCategory;
		}
	}
	private static string Skill_Desc_skill_baseCategoryPath = "Assets/Bundles/Config/Skill_Desc_skill_baseCategory.bytes";
	private Skill_Desc_skill_baseCategory _skill_Desc_skill_baseCategory;
	public Skill_Desc_skill_baseCategory skill_Desc_skill_baseCategory
	{
		get
		{
			if (_skill_Desc_skill_baseCategory == null)
			{
				_skill_Desc_skill_baseCategory = configDic[Skill_Desc_skill_baseCategoryPath] as Skill_Desc_skill_baseCategory;
			}
			return _skill_Desc_skill_baseCategory;
		}
	}
	private static string Skill_Upgrade_skill_upgradeCategoryPath = "Assets/Bundles/Config/Skill_Upgrade_skill_upgradeCategory.bytes";
	private Skill_Upgrade_skill_upgradeCategory _skill_Upgrade_skill_upgradeCategory;
	public Skill_Upgrade_skill_upgradeCategory skill_Upgrade_skill_upgradeCategory
	{
		get
		{
			if (_skill_Upgrade_skill_upgradeCategory == null)
			{
				_skill_Upgrade_skill_upgradeCategory = configDic[Skill_Upgrade_skill_upgradeCategoryPath] as Skill_Upgrade_skill_upgradeCategory;
			}
			return _skill_Upgrade_skill_upgradeCategory;
		}
	}

	public void InitAllConfig(Action onInitDone)
	{
		maxCount = 18;
		this.onInitDone = onInitDone;

		typeDic[Asset_effectCategoryPath] = typeof(Asset_effectCategory);
		typeDic[Asset_pictureCategoryPath] = typeof(Asset_pictureCategory);
		typeDic[Asset_soundCategoryPath] = typeof(Asset_soundCategory);
		typeDic[Buff_Desc_buffCategoryPath] = typeof(Buff_Desc_buffCategory);
		typeDic[Buff_Desc_buff_displayCategoryPath] = typeof(Buff_Desc_buff_displayCategory);
		typeDic[Buff_Desc_buff_display_positionCategoryPath] = typeof(Buff_Desc_buff_display_positionCategory);
		typeDic[Buff_Desc_buff_effectCategoryPath] = typeof(Buff_Desc_buff_effectCategory);
		typeDic[Buff_Desc_buff_typeCategoryPath] = typeof(Buff_Desc_buff_typeCategory);
		typeDic[Global_Param_global_paramCategoryPath] = typeof(Global_Param_global_paramCategory);
		typeDic[Monster_Base_monsterG_LevelCategoryPath] = typeof(Monster_Base_monsterG_LevelCategory);
		typeDic[Monster_Base_monster_baseCategoryPath] = typeof(Monster_Base_monster_baseCategory);
		typeDic[Monster_Base_monster_GroupCategoryPath] = typeof(Monster_Base_monster_GroupCategory);
		typeDic[Monster_Base_monster_modelCategoryPath] = typeof(Monster_Base_monster_modelCategory);
		typeDic[Role_Appearance_role_appearanceCategoryPath] = typeof(Role_Appearance_role_appearanceCategory);
		typeDic[Role_Initial_role_initialCategoryPath] = typeof(Role_Initial_role_initialCategory);
		typeDic[Skill_Desc_skill_activeCategoryPath] = typeof(Skill_Desc_skill_activeCategory);
		typeDic[Skill_Desc_skill_baseCategoryPath] = typeof(Skill_Desc_skill_baseCategory);
		typeDic[Skill_Upgrade_skill_upgradeCategoryPath] = typeof(Skill_Upgrade_skill_upgradeCategory);

		LoadConfig(Asset_effectCategoryPath);
		LoadConfig(Asset_pictureCategoryPath);
		LoadConfig(Asset_soundCategoryPath);
		LoadConfig(Buff_Desc_buffCategoryPath);
		LoadConfig(Buff_Desc_buff_displayCategoryPath);
		LoadConfig(Buff_Desc_buff_display_positionCategoryPath);
		LoadConfig(Buff_Desc_buff_effectCategoryPath);
		LoadConfig(Buff_Desc_buff_typeCategoryPath);
		LoadConfig(Global_Param_global_paramCategoryPath);
		LoadConfig(Monster_Base_monsterG_LevelCategoryPath);
		LoadConfig(Monster_Base_monster_baseCategoryPath);
		LoadConfig(Monster_Base_monster_GroupCategoryPath);
		LoadConfig(Monster_Base_monster_modelCategoryPath);
		LoadConfig(Role_Appearance_role_appearanceCategoryPath);
		LoadConfig(Role_Initial_role_initialCategoryPath);
		LoadConfig(Skill_Desc_skill_activeCategoryPath);
		LoadConfig(Skill_Desc_skill_baseCategoryPath);
		LoadConfig(Skill_Upgrade_skill_upgradeCategoryPath);
	}
}

