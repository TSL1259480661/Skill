
// 全局枚举定义 
public enum Platform
{
	Android = 1,
	iOS,
	Web,
	Linux,
	windows,
}

public enum EnterMainMenuType
{
	Collection, // 藏品
	Shop,       // 商店
	Warehouse,  // 仓库
	Summon,     // 召唤
	Task, 任务
}

public enum WaveType
{
	Unknown = 0,
	Normal = 1, // 小怪
	Elit = 2, // 精英
	Boss = 3, // BOSS
	Baby = 4, //奖励怪
	GoldChanllenge = 5, // 金币挑战怪
	PropChanllenge = 6, // 装备挑战怪
	DiamondChanllenge = 7, // 钻石挑战怪
	Record = 8, // 记录怪
}

public enum MonsterType
{
	Unknown = 0,
	Normal = 1, // 小怪
	Boss = 2, // BOSS
	Player = 3,//玩家
	Baby = 4, //奖励怪
	GoldChanllenge = 5, // 金币挑战怪
	PropChanllenge = 6, // 装备挑战怪
	DiamondChanllenge = 7, // 钻石挑战怪
	Record = 8, // 记录怪
}

public enum DropStuffType
{
	Gold, // 金币
	Ordinary, // 一般道具
	Senior, // 高级
	Legend, //传说
	Exclusive, // 专属
}

public enum GameGoldConsumeType
{
	LevelUp, // 购买经验升级
	RefreshHero, // 刷新英雄
	BuyHero, // 购买英雄
	BuyProp, // 购买道具
	BuyTreasure, // 购买宝箱
	Challenge, // 挑战关卡资格
	DiamondLevelUp, // 钻石等级提升
}
public enum GameGoldGetType
{
	Drop = 0, // 掉落
	GoldChallenge, // 金币挑战
	AutoGet, // 自动增长
	GM, // GM工具
	SellHero, // 出售英雄
	SellEquip, // 出售装备
	Buff, //被动触发
	Guide, //新手引导
	CimeliaSkill, // 宝物技能
}

public enum ChallengeMonsterType
{
	Gold, //金币挑战
	Prop // 装备挑战
}


public enum DamageType
{
	None, //无伤害
	Physical, //物理伤害
	Magic, //魔法伤害
	Real //真实伤害
}

public enum UpgradeState 
{
	None =0,
	NoProp= 1,//道具不足
	NoCurrency=2,//货币不足
	CanUpgrade=3, //能升级 
	Max=4, //满级
}


//道具类型
public enum PropType 
{
	None=0,
	Prop=1,//道具
	Currency=2	//货币
}

//道具子类型
public enum PropChildType
{
	None = 0,
	Equip = 2,   //装备
	Prop = 3     //道具
}

public enum EquipType 
{
	None=0,
	Necklace =1,  //头盔
	Clothes=2,    //衣服
	Glove = 3,    //手套
	Belt =4,      //腰带
	Trousers = 5, //裤子
	Shoes=6,      //鞋
}

public enum EMailStatus
{
	Unread = 0, // 未读
	ReadedUnRecive = 1, // 已读未领取

	ReadedReviced = 2 // 已读已领取
}

public enum EMailOperateType
{
	Readed = 1, // 已读
	Recived = 2, // 领取

	ReadedReviced = 3 // 已读已领取
}

public enum BoxState 
{
	CanGet=1, //能够领取
	NoGet=2, //不能领取
	Got=3    //已领取
}

/// <summary>
/// 物品品质的颜色
/// </summary>
public enum ItemQualityColorType
{
	Grey = 0,
	Green,
	Blue,
	Purple,
	Orange,
	Red,
	Color
}
