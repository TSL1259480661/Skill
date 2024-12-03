public enum ActivityActionType
{
	None = 0,
	Common_Query = 1, // 通用查询
	Common_ProgressUpdate = 2, // 通用进度更新
	Common_Reward = 3, // 通用领取奖励
	Flip_AddUseCount = 101, // 幸运秘宝活动--翻牌增加可用次数
	Flip_Rest = 102, // 幸运秘宝活动--重置层数
}

public enum BiReportType
{
	BiReportNone = 0, // 默认值
	BiReportAd = 1, // 广告相关
	BiReportPay = 2, // 支付相关
	BiReportSkillChoice = 3, // 技能选择相关
	BiReportBeforeLogin = 4, // 登录前相关
}

public enum EquipForgeAttrType
{
	EATTR_NONE = 0,
	EATTR_DEPUTY = 1, // 副词条
	EATTR_RARE = 2, // 稀有词条
	EATTR_MAIN = 3, // 主词条
}

public enum RechargeScene
{
	/// <summary>
	/// default none
	/// </summary>
	NoneScene = 0,
	/// <summary>
	/// 礼包充值
	/// </summary>
	Gift = 1,
	/// <summary>
	/// 商城充值
	/// </summary>
	Mall = 2,
	/// <summary>
	/// 月卡充值
	/// </summary>
	Periodic = 3,
	/// <summary>
	/// 小猪银行（存钱罐）
	/// </summary>
	PiggyBank = 4,
	/// <summary>
	/// 战令
	/// </summary>
	Token = 5,
	/// <summary>
	/// 限时礼包（客户端触发的）
	/// </summary>
	RSLimitedGift = 6,
	/// <summary>
	/// 限时礼包（充值购买后触发的）
	/// </summary>
	RSLimitedGiftInstantly = 7,
}

public enum RewardType
{
	/// <summary>
	/// 任务奖励
	/// </summary>
	Task = 0,
	/// <summary>
	/// 活跃度进度奖励
	/// </summary>
	Active = 1,
	/// <summary>
	/// 累充任务奖励
	/// </summary>
	Recharge = 2,
}

public enum TaskType
{
	/// <summary>
	/// 
	/// </summary>
	Stub = 0,
	/// <summary>
	/// 日常任务
	/// </summary>
	Day = 1,
	/// <summary>
	/// 周常任务
	/// </summary>
	Week = 2,
	/// <summary>
	/// 主线任务
	/// </summary>
	Main = 3,
	/// <summary>
	/// 新人任务
	/// </summary>
	New = 4,
	/// <summary>
	/// 累充任务
	/// </summary>
	RechargeReward = 5,
}

