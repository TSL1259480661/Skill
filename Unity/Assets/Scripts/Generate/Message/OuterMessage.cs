using ClientData;
using ProtoBuf;
using System.Collections.Generic;
namespace ClientData
{
	// 查询正在进行的活动列表
	//ResponseType = ActivityListResp
	//MessageVerb = POST
	//GameUrlOpcode = api/activity/list
	[ProtoContract]
	public partial class ActivityListReq: IRequest
	{
		public string url => "api/activity/list";

	}

	// 广告获奖响应
	[ProtoContract]
	public partial class ActivityListResp: IResponse
	{
		// 奖励
		[ProtoMember(1)]
		public List<ActivityDto> activities = new List<ActivityDto>();

	}

	// 活动行为请求
	//ResponseType = ActivityActionResp
	//MessageVerb = POST
	//GameUrlOpcode = api/activity/action
	[ProtoContract]
	public partial class ActivityActionReq: IRequest
	{
		public string url => "api/activity/action";

		[ProtoMember(1)]
		public long activityId { get; set; }

		[ProtoMember(2)]
		public ActivityActionType type { get; set; }

		//  string body = 3;// 具体message序列化后的字符串
		[ProtoMember(3)]
		public byte[] body { get; set; }

	}

	// 获取玩家相关的广告信息
	[ProtoContract]
	public partial class ActivityActionResp: IResponse
	{
		//  string body = 1;// 具体message序列化后的字符串
		[ProtoMember(1)]
		public byte[] body { get; set; }

		//  repeated RoleItemChangeDto rewards = 2; // 奖励(如果有）
	}

	[ProtoContract]
	public partial class ActivityDto
	{
		[ProtoMember(1)]
		public long id { get; set; }

		[ProtoMember(2)]
		public string name { get; set; }

		[ProtoMember(3)]
		public string desc { get; set; }

		[ProtoMember(4)]
		public long startTime { get; set; }

		[ProtoMember(5)]
		public long endTime { get; set; }

		[ProtoMember(6)]
		public int configId { get; set; }

	}

	// 活动行为枚举，对应各类型活动的行为方式。这里列举了常用行为，自定义枚举按此格式：{activityType}_{messageName}
	// 定义具体的活动请求和响应↓↓↓
	// 翻牌活动请求 ActivityActionType = Common_Query
	[ProtoContract]
	public partial class ActivityFlipQueryReq
	{
	}

	// 翻牌活动响应
	[ProtoContract]
	public partial class ActivityFlipQueryResp
	{
		[ProtoMember(1)]
		public ActivityFlipDto result { get; set; }

	}

	// 翻牌活动信息
	[ProtoContract]
	public partial class ActivityFlipDto
	{
		[ProtoMember(1)]
		public int MaxDepth { get; set; }

		[ProtoMember(2)]
		public int leftCount { get; set; }

		[ProtoMember(3)]
		public int depth { get; set; }

		[ProtoMember(4)]
		public List<ActivityFlipSlotDto> slots = new List<ActivityFlipSlotDto>();

		[ProtoMember(5)]
		public int leftAdCount { get; set; }

		[ProtoMember(6)]
		public int resetCount { get; set; }

	}

	// 翻牌活动槽位
	[ProtoContract]
	public partial class ActivityFlipSlotDto
	{
		[ProtoMember(1)]
		public int index { get; set; }

		[ProtoMember(2)]
		public ChangeItemDto rewards { get; set; }

	}

	// 翻牌活动增加可用次数请求 ActivityActionType = Flip_AddUseCount
	[ProtoContract]
	public partial class ActivityFlipAddUseCountReq
	{
		[ProtoMember(1)]
		public int costType { get; set; }

	}

	// 翻牌活动增加可用次数响应
	[ProtoContract]
	public partial class ActivityFlipAddUseCountResp
	{
		[ProtoMember(1)]
		public int leftCount { get; set; }

		[ProtoMember(2)]
		public RoleItemChangeDto leftItem { get; set; }

	}

	// 翻牌活动选牌请求 ActivityActionType = Common_Reward
	[ProtoContract]
	public partial class ActivityFlipSelectReq
	{
		[ProtoMember(1)]
		public int index { get; set; }

	}

	// 翻牌活动选牌响应
	[ProtoContract]
	public partial class ActivityFlipSelectResp
	{
		[ProtoMember(1)]
		public ActivityFlipDto nowStatus { get; set; }

		[ProtoMember(2)]
		public bool hasHitTarget { get; set; }

		[ProtoMember(3)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	// 翻牌活动重置请求 ActivityActionType = Flip_Reset
	[ProtoContract]
	public partial class ActivityFlipResetReq
	{
	}

	[ProtoContract]
	public partial class ActivityFlipResetResp
	{
		[ProtoMember(1)]
		public ActivityFlipDto nowStatus { get; set; }

	}

	// 获取打工收益页面内容
	//ResponseType = EarningsInfoResp
	//MessageVerb = POST
	//GameUrlOpcode = api/afk_earnings/info
	[ProtoContract]
	public partial class EarningsInfoReq: IRequest
	{
		public string url => "api/afk_earnings/info";

	}

	// 打工收益页面内容响应
	[ProtoContract]
	public partial class EarningsInfoResp: IResponse
	{
		// 可领取的奖励内容
		[ProtoMember(1)]
		public List<ChangeItemDto> earnings = new List<ChangeItemDto>();

		// 玩家上一次领取的时间戳，单位：毫秒
		[ProtoMember(2)]
		public long lastReceivedTime { get; set; }

		// 玩家上一次领取到现在的时间段，单位：毫秒
		[ProtoMember(3)]
		public long lastReceivedToNowDuration { get; set; }

	}

	// 获取打工收益奖励请求
	//ResponseType = EarningsReceiveResp
	//MessageVerb = POST
	//GameUrlOpcode = api/afk_earnings/receive
	[ProtoContract]
	public partial class EarningsReceiveReq: IRequest
	{
		public string url => "api/afk_earnings/receive";

	}

	// 获取打工收益奖励响应
	[ProtoContract]
	public partial class EarningsReceiveResp: IResponse
	{
		// 玩家道具的变动
		[ProtoMember(1)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	// Deprecated 获取快速收取奖励道具信息请求
	//ResponseType = QuickEarningsItemResp
	//MessageVerb = POST
	//GameUrlOpcode = api/afk_earnings/quick/item
	[ProtoContract]
	public partial class QuickEarningsItemReq: IRequest
	{
		public string url => "api/afk_earnings/quick/item";

	}

	// Deprecated 获取快速收取奖励道具信息响应
	[ProtoContract]
	public partial class QuickEarningsItemResp: IResponse
	{
		// 玩家快速收取奖励道具信息
		[ProtoMember(1)]
		public List<RoleItemDto> quickEarningsItems = new List<RoleItemDto>();

	}

	// Deprecated 使用快速收取奖励道具请求
	//ResponseType = QuickEarningsItemUseResp
	//MessageVerb = POST
	//GameUrlOpcode = api/afk_earnings/quick/use
	[ProtoContract]
	public partial class QuickEarningsItemUseReq: IRequest
	{
		public string url => "api/afk_earnings/quick/use";

		// 需要使用的道具id
		[ProtoMember(1)]
		public int cid { get; set; }

	}

	// Deprecated 使用快速收取奖励道具响应
	[ProtoContract]
	public partial class QuickEarningsItemUseResp: IResponse
	{
		// 玩家道具的变动
		[ProtoMember(1)]
		public List<RoleItemChangeDto> changes = new List<RoleItemChangeDto>();

	}

	// 通用服务端响应
	[ProtoContract]
	public partial class GenericResponse
	{
		// 错误信息。成功时为ok
		[ProtoMember(1)]
		public string msg { get; set; }

		// 错误码。成功时为0
		[ProtoMember(2)]
		public int code { get; set; }

		// 数据。http code为200时才有数据
		[ProtoMember(3)]
		public byte[] data { get; set; }

	}

	// 通用服务端响应
	[ProtoContract]
	public partial class RawResponse
	{
		// 错误信息。成功时为ok
		[ProtoMember(1)]
		public string msg { get; set; }

		// 错误码。成功时为0
		[ProtoMember(2)]
		public int code { get; set; }

		// 数据。http code为200时才有数据
		[ProtoMember(3)]
		public byte[] data { get; set; }

	}

	[ProtoContract]
	public partial class EmptyResp
	{
	}

	[ProtoContract]
	public partial class reportDto
	{
		[ProtoMember(1)]
		public string reportData { get; set; }

		[ProtoMember(2)]
		public BiReportType biType { get; set; }

	}

	// bi埋点上报请求
	//ResponseType = BiReportsResponse
	//MessageVerb = POST
	//GameUrlOpcode = api/bi/reports
	[ProtoContract]
	public partial class BiReportsReq: IRequest
	{
		public string url => "api/bi/reports";

		[ProtoMember(1)]
		public List<reportDto> actions = new List<reportDto>();

	}

	// bi埋点多个上报响应
	[ProtoContract]
	public partial class BiReportsResponse: IResponse
	{
	}

	//请求关卡列表信息
	//ResponseType = GetLevelInfoResp
	//MessageVerb = POST
	//GameUrlOpcode = api/combat/info
	[ProtoContract]
	public partial class GetLevelInfoReq: IRequest
	{
		public string url => "api/combat/info";

	}

	//响应关卡列表信息
	[ProtoContract]
	public partial class GetLevelInfoResp: IResponse
	{
		// 关卡信息
		[ProtoMember(1)]
		public List<LevelInfoDto> levels = new List<LevelInfoDto>();

	}

	//请求关卡列表信息
	//ResponseType = GetLevelInfoPageResp
	//MessageVerb = POST
	//GameUrlOpcode = api/combat/info/page
	[ProtoContract]
	public partial class GetLevelInfoPageReq: IRequest
	{
		public string url => "api/combat/info/page";

		[ProtoMember(1)]
		public List<int> pageNums = new List<int>();

	}

	//响应关卡列表信息
	[ProtoContract]
	public partial class GetLevelInfoPageResp: IResponse
	{
		// 关卡分页信息
		[ProtoMember(1)]
		public List<PageLevelInfoDto> pages = new List<PageLevelInfoDto>();

	}

	[ProtoContract]
	public partial class PageLevelInfoDto
	{
		// 约定每页20条信息
		[ProtoMember(1)]
		public List<LevelInfoDto> infos = new List<LevelInfoDto>();

		[ProtoMember(2)]
		public int pageNum { get; set; }

	}

	// 关卡信息
	[ProtoContract]
	public partial class LevelInfoDto
	{
		// 关卡配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 关卡领奖状态
		[ProtoMember(2)]
		public List<bool> rewards = new List<bool>();

		// 关卡战斗结果 -1=投降 0=失败 1~3=星级
		[ProtoMember(3)]
		public int score { get; set; }

		// 关卡最佳时长（秒）
		[ProtoMember(4)]
		public int bestTime { get; set; }

		// 关卡最佳防守（百分比0~100)
		[ProtoMember(5)]
		public int bestDefence { get; set; }

	}

	//请求上传战斗数据
	//ResponseType = UploadCombatResp
	//MessageVerb = POST
	//GameUrlOpcode = api/combat/upload
	[ProtoContract]
	public partial class UploadCombatReq: IRequest
	{
		public string url => "api/combat/upload";

		// 关卡配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 战斗数据。目前由客户端自定义格式，以string类型传输
		[ProtoMember(2)]
		public string combatInfo { get; set; }

	}

	//响应上传战斗数据
	[ProtoContract]
	public partial class UploadCombatResp: IResponse
	{
	}

	//请求下载战斗数据
	//ResponseType = ResumeCombatResp
	//MessageVerb = POST
	//GameUrlOpcode = api/combat/resume
	[ProtoContract]
	public partial class ResumeCombatReq: IRequest
	{
		public string url => "api/combat/resume";

	}

	//响应下载战斗数据
	[ProtoContract]
	public partial class ResumeCombatResp: IResponse
	{
		// 是否存在未完成的战斗
		[ProtoMember(1)]
		public bool inCombat { get; set; }

		// 关卡配置id
		[ProtoMember(2)]
		public int cid { get; set; }

		// 战斗数据。目前由客户端自定义格式，以string类型传输
		[ProtoMember(3)]
		public string combatInfo { get; set; }

	}

	//请求开始战斗
	//ResponseType = StartCombatResp
	//MessageVerb = POST
	//GameUrlOpcode = api/combat/start
	[ProtoContract]
	public partial class StartCombatReq: IRequest
	{
		public string url => "api/combat/start";

		// 关卡配置id
		[ProtoMember(1)]
		public int cid { get; set; }

	}

	//响应开始战斗
	[ProtoContract]
	public partial class StartCombatResp: IResponse
	{
		// 进入战斗消耗
		[ProtoMember(1)]
		public List<RoleItemChangeDto> costs = new List<RoleItemChangeDto>();

		// 本场战斗的唯一标识符
		[ProtoMember(2)]
		public string combatUid { get; set; }

	}

	//请求结束战斗并结算
	//ResponseType = FinishCombatResp
	//MessageVerb = POST
	//GameUrlOpcode = api/combat/finish
	[ProtoContract]
	public partial class FinishCombatReq: IRequest
	{
		public string url => "api/combat/finish";

		// 关卡配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 战斗结果  -1=投降 0=失败 1~3=星级
		[ProtoMember(2)]
		public int score { get; set; }

		// 战斗时长（秒）
		[ProtoMember(3)]
		public int time { get; set; }

		// 防守百分比（0~100）
		[ProtoMember(4)]
		public int defense { get; set; }

		// 完成的波次
		[ProtoMember(5)]
		public int wave { get; set; }

		// 达到的等级
		[ProtoMember(6)]
		public int lv { get; set; }

		// 技能选择
		[ProtoMember(7)]
		public List<int> skillChoices = new List<int>();

		// 击杀敌人数量
		[ProtoMember(8)]
		public List<EnemyDto> killedEnemies = new List<EnemyDto>();

		// 元素精灵技能主动施放次数
		[ProtoMember(9)]
		public int spiritSkillCount { get; set; }

	}

	//响应结束战斗并结算
	[ProtoContract]
	public partial class FinishCombatResp: IResponse
	{
		// 结算奖励
		[ProtoMember(1)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

		// 战斗结果  -1=投降 0=失败 1~3=星级
		[ProtoMember(2)]
		public int score { get; set; }

		// 该关卡已失败通关次数
		[ProtoMember(3)]
		public int failedCount { get; set; }

	}

	//请求扫荡
	//ResponseType = SweepLevelResp
	//MessageVerb = POST
	//GameUrlOpcode = api/combat/sweep
	[ProtoContract]
	public partial class SweepLevelReq: IRequest
	{
		public string url => "api/combat/sweep";

		// 关卡配置id
		[ProtoMember(1)]
		public int cid { get; set; }

	}

	//响应扫荡
	[ProtoContract]
	public partial class SweepLevelResp: IResponse
	{
		// 扫荡奖励
		[ProtoMember(1)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	//请求领取关卡奖励
	//ResponseType = RetrieveRewardsResp
	//MessageVerb = POST
	//GameUrlOpcode = api/combat/retrieve
	[ProtoContract]
	public partial class RetrieveRewardsReq: IRequest
	{
		public string url => "api/combat/retrieve";

		// 关卡配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 星级[1-3]
		[ProtoMember(2)]
		public int score { get; set; }

	}

	//响应领取关卡奖励
	[ProtoContract]
	public partial class RetrieveRewardsResp: IResponse
	{
		// 奖励 返回至请求星级的所有奖励
		[ProtoMember(1)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	[ProtoContract]
	public partial class EnemyDto
	{
		// 怪物配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 击杀的敌人数量
		[ProtoMember(2)]
		public long num { get; set; }

	}

	// 装备信息
	[ProtoContract]
	public partial class EquipmentDto
	{
		// 角色UID
		[ProtoMember(1)]
		public long roleId { get; set; }

		// 装备配置id；携带装备品质、装备品阶、强化等级上限、装备部位
		[ProtoMember(2)]
		public int cid { get; set; }

		// 装备的唯一id
		[ProtoMember(4)]
		public long id { get; set; }

		//  // 装备的强化配置id；同时能携带强化功能提供的战斗力值、强化功能的等级、属性类型与值
		//  int32 enhanceCid = 5;
		// 精工的配置id；同时携带精工等级、主词条与值、战斗力值
		[ProtoMember(6)]
		public int forgeCid { get; set; }

		// 其它词条
		[ProtoMember(7)]
		public List<EquipForgeAttr> attrs = new List<EquipForgeAttr>();

		// 是否穿戴在身上
		[ProtoMember(8)]
		public bool hasWore { get; set; }

		// 是否已锁定
		[ProtoMember(9)]
		public bool hasLocked { get; set; }

	}

	// 装备精工词条结构
	[ProtoContract]
	public partial class EquipForgeAttr
	{
		// 词条配置id； 同时携带属性id、设置颜色
		[ProtoMember(1)]
		public int cId { get; set; }

		// 属性值
		[ProtoMember(2)]
		public float attrValue { get; set; }

		// 排序值 值越小排名越前
		[ProtoMember(3)]
		public int sort { get; set; }

		// 词条类别
		[ProtoMember(4)]
		public EquipForgeAttrType eType { get; set; }

	}

	// 装备升级时的请求结构体
	[ProtoContract]
	public partial class EquipUpToLevel
	{
		// 装备唯一id
		[ProtoMember(1)]
		public long id { get; set; }

		// 提升的目标等级
		[ProtoMember(2)]
		public int upToLevel { get; set; }

	}

	// 装备部位升级的请求结构体
	[ProtoContract]
	public partial class EquipPartUpToLevel
	{
		// 装备部位id
		[ProtoMember(1)]
		public int partId { get; set; }

		// 提升的目标等级
		[ProtoMember(2)]
		public int upToLevel { get; set; }

	}

	//装备强化升级请求
	//ResponseType = EnhanceUpgradeToLevelResp
	//MessageVerb = POST
	//GameUrlOpcode = api/equip/enhance/upgrade
	[ProtoContract]
	public partial class EnhanceUpgradeToLevelReq: IRequest
	{
		public string url => "api/equip/enhance/upgrade";

		// 需要强化升级的装备信息
		[ProtoMember(1)]
		public List<EquipPartUpToLevel> upToLevelInfo = new List<EquipPartUpToLevel>();

	}

	[ProtoContract]
	public partial class EnhanceUpgradeToLevelResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> usedItems = new List<RoleItemChangeDto>();

	}

	//装备精工升级请求
	//ResponseType = ForgeUpgradeToLevelResp
	//MessageVerb = POST
	//GameUrlOpcode = api/equip/forge/upgrade
	[ProtoContract]
	public partial class ForgeUpgradeToLevelReq: IRequest
	{
		public string url => "api/equip/forge/upgrade";

		// 需要精工升级的装备信息
		[ProtoMember(1)]
		public EquipUpToLevel upToLevelInfo { get; set; }

	}

	[ProtoContract]
	public partial class ForgeUpgradeToLevelResp: IResponse
	{
		[ProtoMember(1)]
		public EquipmentDto afterEquip { get; set; }

		[ProtoMember(2)]
		public List<RoleItemChangeDto> usedItems = new List<RoleItemChangeDto>();

	}

	//装备分解请求
	//ResponseType = DecomposeResp
	//MessageVerb = POST
	//GameUrlOpcode = api/equip/decompose
	[ProtoContract]
	public partial class DecomposeReq: IRequest
	{
		public string url => "api/equip/decompose";

		// 需要分解的装备唯一id列表
		[ProtoMember(1)]
		public List<long> ids = new List<long>();

	}

	[ProtoContract]
	public partial class DecomposeResp: IResponse
	{
		// 分解后物品信息
		[ProtoMember(1)]
		public List<RoleItemChangeDto> addItems = new List<RoleItemChangeDto>();

	}

	//装备穿戴请求
	//ResponseType = EquipWearResp
	//MessageVerb = POST
	//GameUrlOpcode = api/equip/wear
	[ProtoContract]
	public partial class EquipWearReq: IRequest
	{
		public string url => "api/equip/wear";

		// 装备唯一id
		[ProtoMember(1)]
		public long id { get; set; }

	}

	[ProtoContract]
	public partial class EquipWearResp: IResponse
	{
		[ProtoMember(1)]
		public bool success { get; set; }

	}

	//装备上锁操作请求
	//ResponseType = LockResp
	//MessageVerb = POST
	//GameUrlOpcode = api/equip/lock/operate
	[ProtoContract]
	public partial class EquipLockReq: IRequest
	{
		public string url => "api/equip/lock/operate";

		// 装备唯一id
		[ProtoMember(1)]
		public long id { get; set; }

		// 操作类型 1:上锁；2：解锁
		[ProtoMember(2)]
		public int operateType { get; set; }

	}

	[ProtoContract]
	public partial class EquipLockResp: IResponse
	{
		[ProtoMember(1)]
		public bool success { get; set; }

	}

	//获取所有装备请求
	//ResponseType = GetAllEquipResp
	//MessageVerb = POST
	//GameUrlOpcode = api/equip/list
	[ProtoContract]
	public partial class GetAllEquipReq: IRequest
	{
		public string url => "api/equip/list";

	}

	// 批量装备的返回
	[ProtoContract]
	public partial class GetAllEquipResp: IResponse
	{
		// 装备信息
		[ProtoMember(1)]
		public List<EquipmentDto> equips = new List<EquipmentDto>();

		// 位置强化信息 位置强化的配置id；同时能携带强化功能提供的战斗力值、强化功能的等级、属性类型与值
		[ProtoMember(2)]
		public List<int> enhanceCid = new List<int>();

	}

	//请求功能解锁列表
	//ResponseType = GetFuncOpenResp
	//MessageVerb = POST
	//GameUrlOpcode = api/func_open/list
	[ProtoContract]
	public partial class GetFuncOpenReq: IRequest
	{
		public string url => "api/func_open/list";

	}

	// 响应功能解锁列表
	[ProtoContract]
	public partial class GetFuncOpenResp: IResponse
	{
		// 技能列表
		[ProtoMember(1)]
		public List<FuncOpenDto> funcs = new List<FuncOpenDto>();

	}

	// 功能解锁数据
	[ProtoContract]
	public partial class FuncOpenDto
	{
		// 功能解锁配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 解锁时间戳(毫秒)
		[ProtoMember(2)]
		public long time { get; set; }

	}

	//debug功能解锁
	//ResponseType = OpenFuncResp
	//MessageVerb = POST
	//GameUrlOpcode = api/func_open/open
	[ProtoContract]
	public partial class OpenFuncReq: IRequest
	{
		public string url => "api/func_open/open";

		// 功能解锁配置id
		[ProtoMember(1)]
		public List<int> cids = new List<int>();

	}

	// 响应功能解锁列表
	[ProtoContract]
	public partial class OpenFuncResp: IResponse
	{
		// 解锁成功的配置id
		[ProtoMember(1)]
		public List<int> cids = new List<int>();

	}

	//debug功能关闭
	//ResponseType = CloseFuncResp
	//MessageVerb = POST
	//GameUrlOpcode = api/func_open/close
	[ProtoContract]
	public partial class CloseFuncReq: IRequest
	{
		public string url => "api/func_open/close";

		// 功能解锁配置id
		[ProtoMember(1)]
		public int cid { get; set; }

	}

	// 响应功能解锁列表
	[ProtoContract]
	public partial class CloseFuncResp: IResponse
	{
	}

	[ProtoContract]
	public partial class PresetGems
	{
		// 装备位置id
		[ProtoMember(1)]
		public int partId { get; set; }

		// 装备的预设页数 有效值[0,2]
		[ProtoMember(2)]
		public int presetNum { get; set; }

		// 装备的宝石  下标为宝石槽位索引：[0,4]；值为宝石配置id；若没有宝石则填充0
		[ProtoMember(3)]
		public List<int> gemCidList = new List<int>();

	}

	// 宝石合成的请求
	//ResponseType = FusionGemsResp
	//MessageVerb = POST
	//GameUrlOpcode = api/gem/fusion
	[ProtoContract]
	public partial class FusionGemsReq: IRequest
	{
		public string url => "api/gem/fusion";

		// 需要合成的宝石
		[ProtoMember(1)]
		public List<RoleItemDto> gemsToFusion = new List<RoleItemDto>();

		// 是否合成至可合成的最高等级
		[ProtoMember(2)]
		public bool rollToTop { get; set; }

	}

	[ProtoContract]
	public partial class FusionGemsResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> useItems = new List<RoleItemChangeDto>();

		[ProtoMember(2)]
		public List<RoleItemChangeDto> addGems = new List<RoleItemChangeDto>();

	}

	// 宝石洗炼的请求
	//ResponseType = RefineGemResp
	//MessageVerb = POST
	//GameUrlOpcode = api/gem/refine
	[ProtoContract]
	public partial class RefineGemReq: IRequest
	{
		public string url => "api/gem/refine";

		// 宝石配置id
		[ProtoMember(1)]
		public int cid { get; set; }

	}

	[ProtoContract]
	public partial class RefineGemResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> chgItems = new List<RoleItemChangeDto>();

		//  int32 resultGemCid = 2; // 洗炼结果宝石的cid
		[ProtoMember(2)]
		public RoleItemChangeDto newGem { get; set; }

	}

	//// 宝石洗炼保存的请求
	////ResponseType = RefineConfirmResp
	////MessageVerb = POST
	////GameUrlOpcode = api/gem/refine/confirm
	//message RefineConfirmReq // IRequest
	//{
	//  // 洗炼结果cid
	//  int32 cid = 1;
	//}
	//
	//message RefineConfirmResp // IResponse
	//{
	//  RoleItemChangeDto consumeGem = 1; // 本次洗炼所消耗的宝石
	//  RoleItemChangeDto gainGem = 2; // 本次洗炼所获得的宝石
	//}
	// 获取预设页数的所有宝石请求
	//ResponseType = GetWearGemsListResp
	//MessageVerb = POST
	//GameUrlOpcode = api/gem/wear/list
	[ProtoContract]
	public partial class GetWearGemsListReq: IRequest
	{
		public string url => "api/gem/wear/list";

	}

	[ProtoContract]
	public partial class GetWearGemsListResp: IResponse
	{
		[ProtoMember(1)]
		public List<PresetGems> partInfos = new List<PresetGems>();

		[ProtoMember(2)]
		public List<int> allLockedGems = new List<int>();

		[ProtoMember(3)]
		public int preset { get; set; }

	}

	// 宝石加锁操作请求
	//ResponseType = OperateOkResp
	//MessageVerb = POST
	//GameUrlOpcode = api/gem/operate
	[ProtoContract]
	public partial class OperateGemReq: IRequest
	{
		public string url => "api/gem/operate";

		// 宝石配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 操作类型 1:上锁；2：解锁
		[ProtoMember(2)]
		public int operateType { get; set; }

	}

	// 操作成功返回
	[ProtoContract]
	public partial class OperateOkResp: IResponse
	{
		[ProtoMember(1)]
		public bool ok { get; set; }

	}

	//// 卸下指定页面的所有宝石请求
	////ResponseType = UnloadAllGemResp
	////MessageVerb = POST
	////GameUrlOpcode = api/gem/unload_all
	//message UnloadAllGemReq // IRequest
	//{
	//  // 指定的预设页数 有效值: [0,2]
	//  int32 presetNum = 1;
	//}
	//
	//// 卸下指定页面的所有宝石返回
	//message UnloadAllGemResp // IResponse
	//{
	//  // 受影响的预设页信息
	//  repeated PartWearGems partInfos = 1;
	//}
	//指定位置镶嵌宝石请求
	//ResponseType = InlayGemResp
	//MessageVerb = POST
	//GameUrlOpcode = api/gem/inlay
	[ProtoContract]
	public partial class InlayGemReq: IRequest
	{
		public string url => "api/gem/inlay";

		// 镶嵌宝石所在的预设页数 有效值: [0,2]
		[ProtoMember(2)]
		public int presetNum { get; set; }

		// 宝石的配置id
		[ProtoMember(3)]
		public int gemCid { get; set; }

		// 宝石槽的索引 有效值: [0,4]
		[ProtoMember(4)]
		public int slotIndex { get; set; }

	}

	[ProtoContract]
	public partial class InlayGemResp: IResponse
	{
		// 受到影响的预设页信息
		[ProtoMember(1)]
		public PresetGems partInfo { get; set; }

	}

	//装备宝石卸下请求
	//ResponseType = UnloadGemResp
	//MessageVerb = POST
	//GameUrlOpcode = api/gem/unload
	[ProtoContract]
	public partial class UnloadGemReq: IRequest
	{
		public string url => "api/gem/unload";

		// 镶嵌宝石的配置id
		[ProtoMember(1)]
		public int gemCid { get; set; }

		// 镶嵌宝石所在的预设页数 有效值: [0,2]
		[ProtoMember(2)]
		public int presetNum { get; set; }

		// 宝石槽的索引 有效值: [0,4]
		[ProtoMember(3)]
		public int slotIndex { get; set; }

	}

	[ProtoContract]
	public partial class UnloadGemResp: IResponse
	{
		// 受到影响的预设页信息
		[ProtoMember(1)]
		public PresetGems presetInfo { get; set; }

	}

	// 保存宝石预设请求
	//ResponseType = SaveGemPresetResp
	//MessageVerb = POST
	//GameUrlOpcode = api/gem/save_preset
	[ProtoContract]
	public partial class SaveGemPresetReq: IRequest
	{
		public string url => "api/gem/save_preset";

		// 需要保存的装备预设页数
		[ProtoMember(1)]
		public int preset { get; set; }

	}

	// 保存宝石预设响应
	[ProtoContract]
	public partial class SaveGemPresetResp: IResponse
	{
	}

	// 获取玩家礼包商城信息请求
	//ResponseType = UserGiftInfoResp
	//MessageVerb = POST
	//GameUrlOpcode = api/gift/info
	[ProtoContract]
	public partial class UserGiftInfoReq: IRequest
	{
		public string url => "api/gift/info";

	}

	// 获取玩家礼包商城信息的响应
	[ProtoContract]
	public partial class UserGiftInfoResp: IResponse
	{
		[ProtoMember(1)]
		public List<GiftGoodsInfoDto> goodsInfos = new List<GiftGoodsInfoDto>();

	}

	[ProtoContract]
	public partial class GiftGoodsInfoDto
	{
		[ProtoMember(1)]
		public int cid { get; set; }

		[ProtoMember(2)]
		public int purchasedCount { get; set; }

		//  int64 purchasedInPast = 3; // 历史已购买次数
	}

	// 玩家商城购买请求
	//ResponseType = GiftPurchaseResp
	//MessageVerb = POST
	//GameUrlOpcode = api/gift/purchase
	[ProtoContract]
	public partial class GiftPurchaseReq: IRequest
	{
		public string url => "api/gift/purchase";

		[ProtoMember(1)]
		public int cid { get; set; }

	}

	// 玩家商城购买响应
	[ProtoContract]
	public partial class GiftPurchaseResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> changes = new List<RoleItemChangeDto>();

	}

	// GM请求
	//ResponseType = GMRsp
	//MessageVerb = POST
	//GameUrlOpcode = api/gm/cmd
	[ProtoContract]
	public partial class GMReq: IRequest
	{
		public string url => "api/gm/cmd";

		[ProtoMember(1)]
		public List<string> cmd = new List<string>();

	}

	// GM响应
	[ProtoContract]
	public partial class GMRsp: IResponse
	{
	}

	// RoleItemDto represents information about a role item.
	// 背包物品信息
	[ProtoContract]
	public partial class RoleItemDto
	{
		// 物品id
		[ProtoMember(1)]
		public long id { get; set; }

		// 物品配置id
		[ProtoMember(2)]
		public int cid { get; set; }

		// 所属角色id
		[ProtoMember(3)]
		public long roleId { get; set; }

		// 物品数量
		[ProtoMember(4)]
		public long num { get; set; }

		// 物品类型 1=道具 2=货币 3=掉落（暂时不用）
		[ProtoMember(5)]
		public int type { get; set; }

	}

	// RoleItemChangeDto represents changes in a role item.
	// 物品更改信息，在背包物品发生增减后返回
	[ProtoContract]
	public partial class RoleItemChangeDto
	{
		// 物品配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 所属角色id
		[ProtoMember(2)]
		public long roleId { get; set; }

		// 物品数量
		[ProtoMember(3)]
		public long num { get; set; }

		// 物品类型 1=道具 2=货币 3=掉落（暂时不用）
		[ProtoMember(4)]
		public int type { get; set; }

		// 物品增减标识，true为增加，false为减少
		[ProtoMember(5)]
		public bool inc { get; set; }

		[ProtoMember(6)]
		public long before { get; set; }

		[ProtoMember(7)]
		public long after { get; set; }

		[ProtoMember(8)]
		public EquipSnapshot equipSnapshot { get; set; }

	}

	[ProtoContract]
	public partial class EquipSnapshot
	{
		[ProtoMember(1)]
		public long id { get; set; }

		[ProtoMember(3)]
		public int forgeCid { get; set; }

		[ProtoMember(4)]
		public List<EquipAttrSnapshot> attrs = new List<EquipAttrSnapshot>();

	}

	[ProtoContract]
	public partial class EquipAttrSnapshot
	{
		[ProtoMember(1)]
		public int cid { get; set; }

		[ProtoMember(2)]
		public float value { get; set; }

		[ProtoMember(3)]
		public int eType { get; set; }

	}

	//UseItemReq represents the request structure for using an item. 消耗单个道具接口
	//ResponseType = UseItemResp
	//MessageVerb = POST
	//GameUrlOpcode = api/item/use
	[ProtoContract]
	public partial class UseItemReq: IRequest
	{
		public string url => "api/item/use";

		[ProtoMember(1)]
		public List<ChangeItemDto> items = new List<ChangeItemDto>();

	}

	//UseItemResp represents the response structure for using an item.
	[ProtoContract]
	public partial class UseItemResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> changes = new List<RoleItemChangeDto>();

	}

	//AddItemReq represents the request structure for adding an item. 添加单个道具接口
	//ResponseType = AddItemResp
	//MessageVerb = POST
	//GameUrlOpcode = api/item/add
	[ProtoContract]
	public partial class AddItemReq: IRequest
	{
		public string url => "api/item/add";

		[ProtoMember(1)]
		public List<ChangeItemDto> items = new List<ChangeItemDto>();

	}

	//AddItemResp represents the response structure for adding an item.
	[ProtoContract]
	public partial class AddItemResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> changes = new List<RoleItemChangeDto>();

	}

	// 按类型获取玩家背包物品请求
	//ResponseType = GetRoleItemsResp
	//MessageVerb = POST
	//GameUrlOpcode = api/item/list
	[ProtoContract]
	public partial class GetRoleItemsReq: IRequest
	{
		public string url => "api/item/list";

		// 物品类型 1=道具 2=货币
		[ProtoMember(1)]
		public int itemType { get; set; }

		// 物品子类型。不填查询所有子类型
		// itemType=1时，子类型为道具类型 subtype传Item_Desc#Type
		// itemType=2时，子类型为货币类型 subtype传Money_Desc#Id
		// itemType不能=3
		[ProtoMember(2)]
		public List<int> subtypes = new List<int>();

	}

	// 按类型获取玩家背包物品响应
	[ProtoContract]
	public partial class GetRoleItemsResp: IResponse
	{
		// 玩家背包中指定类型的物品信息
		[ProtoMember(1)]
		public List<RoleItemDto> items = new List<RoleItemDto>();

	}

	// 测试box
	//ResponseType = UseUserPickBoxResp
	//MessageVerb = POST
	//GameUrlOpcode = api/item/pick_test
	[ProtoContract]
	public partial class PickTestReq: IRequest
	{
		public string url => "api/item/pick_test";

		// 宝箱配置id
		[ProtoMember(1)]
		public int planCid { get; set; }

		// 自选索引，从0开始
		[ProtoMember(2)]
		public int pickNum { get; set; }

	}

	[ProtoContract]
	public partial class PickTestResp: IResponse
	{
		[ProtoMember(1)]
		public List<ChangeItemDto> result = new List<ChangeItemDto>();

	}

	[ProtoContract]
	public partial class ChangeItemDto
	{
		// 物品配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 物品类型 1=道具 2=货币 3=掉落
		[ProtoMember(2)]
		public int type { get; set; }

		// 物品数量
		[ProtoMember(3)]
		public long num { get; set; }

	}

	//请求用户登录注册
	//ResponseType = UserLoginResp
	//MessageVerb = POST
	//GameUrlOpcode = api/auth/login
	[ProtoContract]
	public partial class UserLoginReq: IRequest
	{
		public string url => "api/auth/login";

		// 账号登录时为账号；手机号登录时为手机号；微信登录时为code；邮箱登录时为邮箱；99SDK登录时为openid
		[ProtoMember(1)]
		public string account { get; set; }

		// 账号登录时为密码；手机号登录时为验证码；微信登录时为code；邮箱登录时为验证码；99SDK登录时为token
		[ProtoMember(2)]
		public string credential { get; set; }

		// 登录平台，区分大小写。LOCAL=账号密码登录；WECHAT=微信登录；99SDK=99SDK登录；PHONE=手机号登录；EMAIL=邮箱登录
		[ProtoMember(3)]
		public string platform { get; set; }

		// 用户的硬件信息
		[ProtoMember(4)]
		public DeviceInfo deviceInfo { get; set; }

		// 来源用户的广告信息
		[ProtoMember(5)]
		public AdsInfo adsInfo { get; set; }

		// token
		[ProtoMember(6)]
		public string token { get; set; }

	}

	// 响应用户登录
	[ProtoContract]
	public partial class UserLoginResp: IResponse
	{
		// 身份凭证，用于后续请求的身份认证。放在http header的Authorization字段中，格式为：Bearer token。（注意空格）
		[ProtoMember(1)]
		public string token { get; set; }

		// 身份凭证的过期时间，单位为毫秒
		[ProtoMember(2)]
		public long expire { get; set; }

		// 服务器的UTC时间戳, 单位毫秒
		[ProtoMember(3)]
		public long serverTime { get; set; }

	}

	[ProtoContract]
	public partial class DeviceInfo
	{
		// 用户微信的openid
		[ProtoMember(1)]
		public string distinctId { get; set; }

		// 操作系统 ios/android
		[ProtoMember(2)]
		public string os { get; set; }

		// 操作系统版本 例如：Android OS 9
		[ProtoMember(3)]
		public string osVersion { get; set; }

		// 客户端的版本号
		[ProtoMember(4)]
		public string appVersion { get; set; }

		// 设备型号 例如：iphone15、redmi14
		[ProtoMember(5)]
		public string deviceModel { get; set; }

		// 设备类型 例如：iphone、ipad、pc
		[ProtoMember(6)]
		public string deviceType { get; set; }

	}

	[ProtoContract]
	public partial class AdsInfo
	{
		// 用户归因到的广告商
		[ProtoMember(1)]
		public string userSource { get; set; }

		// 用户归因到的广告位
		[ProtoMember(2)]
		public string adName { get; set; }

	}

	//获取服务器列表
	//ResponseType = GetServerListRsp
	//MessageVerb = POST
	//GameUrlOpcode = api/server/list
	[ProtoContract]
	public partial class GetServerListReq: IRequest
	{
		public string url => "api/server/list";

	}

	[ProtoContract]
	public partial class ServerInfo
	{
		// 服务器名字
		[ProtoMember(1)]
		public string name { get; set; }

		// 网关地址
		[ProtoMember(2)]
		public string gateWay { get; set; }

		// 服务器状态
		[ProtoMember(3)]
		public int serverStatus { get; set; }

	}

	[ProtoContract]
	public partial class GetServerListRsp
	{
		// 服务器列表
		[ProtoMember(1)]
		public List<ServerInfo> serverList = new List<ServerInfo>();

	}

	//账号验证
	//ResponseType = AccountAuthRsp
	//MessageVerb = POST
	//GameUrlOpcode = api/auth/auth
	[ProtoContract]
	public partial class AccountAuthReq: IRequest
	{
		public string url => "api/auth/auth";

		// 账号登录时为账号；手机号登录时为手机号；微信登录时为code；邮箱登录时为邮箱；99SDK登录时为openid
		[ProtoMember(1)]
		public string account { get; set; }

		// 账号登录时为密码；手机号登录时为验证码；微信登录时为code；邮箱登录时为验证码；99SDK登录时为token
		[ProtoMember(2)]
		public string credential { get; set; }

		// 登录平台，区分大小写。LOCAL=账号密码登录；WECHAT=微信登录；99SDK=99SDK登录；PHONE=手机号登录；EMAIL=邮箱登录
		[ProtoMember(3)]
		public string platform { get; set; }

		// 用户的硬件信息
		[ProtoMember(4)]
		public DeviceInfo deviceInfo { get; set; }

		// 来源用户的广告信息
		[ProtoMember(5)]
		public AdsInfo adsInfo { get; set; }

	}

	// 响应账号验证
	[ProtoContract]
	public partial class AccountAuthRsp: IResponse
	{
		// 身份凭证，用于后续请求的身份认证。放在http header的Authorization字段中，格式为：Bearer token。（注意空格）
		[ProtoMember(1)]
		public string token { get; set; }

		// 身份凭证的过期时间，单位为毫秒
		[ProtoMember(2)]
		public long expire { get; set; }

		// 服务器的UTC时间戳, 单位毫秒
		[ProtoMember(3)]
		public long serverTime { get; set; }

	}

	[ProtoContract]
	public partial class MailDto
	{
		// 邮件id
		[ProtoMember(1)]
		public long id { get; set; }

		// 邮件标题
		[ProtoMember(2)]
		public string title { get; set; }

		// 邮件内容
		[ProtoMember(3)]
		public string content { get; set; }

		// 邮件状态 0=未读 1=已读未领取 2=已读已领取
		[ProtoMember(5)]
		public int status { get; set; }

		// 邮件创建时间 utc时间戳 单位毫秒
		[ProtoMember(6)]
		public long createTime { get; set; }

		// 附件
		[ProtoMember(7)]
		public List<ChangeItemDto> attachments = new List<ChangeItemDto>();

		// 邮件配置id
		[ProtoMember(8)]
		public int cid { get; set; }

		// 邮件到期时间 utc时间戳 单位毫秒
		[ProtoMember(9)]
		public long expiredTime { get; set; }

	}

	// 邮件列表请求
	//ResponseType = GetMailListResp
	//MessageVerb = POST
	//GameUrlOpcode = api/mail/list
	[ProtoContract]
	public partial class GetMailListReq: IRequest
	{
		public string url => "api/mail/list";

	}

	// 邮件列表响应
	[ProtoContract]
	public partial class GetMailListResp: IResponse
	{
		// 邮件列表
		[ProtoMember(1)]
		public List<MailDto> mails = new List<MailDto>();

	}

	// 邮件操作请求
	//ResponseType = MailOperateResp
	//MessageVerb = POST
	//GameUrlOpcode = api/mail/operate
	[ProtoContract]
	public partial class MailOperateReq: IRequest
	{
		public string url => "api/mail/operate";

		// 邮件id
		[ProtoMember(1)]
		public List<long> id = new List<long>();

		// 操作类型 1=已读操作 2=领取附件
		[ProtoMember(2)]
		public int operateType { get; set; }

	}

	[ProtoContract]
	public partial class MailOperateResp: IResponse
	{
		// 邮件附件内容 已读操作则返回空列表
		[ProtoMember(1)]
		public List<RoleItemChangeDto> attachments = new List<RoleItemChangeDto>();

	}

	//// 邮件批量操作请求
	////ResponseType = MailBatchOperateResp
	////MessageVerb = POST
	////GameUrlOpcode = api/mail/batch/operate
	//message MailBatchOperateReq // IRequest
	//{
	//  // 邮件id列表
	//  repeated int64 ids = 1;
	//  // 操作类型 1=已读操作 2=领取附件
	//  int32 operateType = 2;
	//}
	//
	//message MailBatchOperateResp // IResponse
	//{
	//  // 邮件附件内容 已读操作则返回空列表
	//  repeated RoleItemChangeDto attachments = 1;
	//}
	// 邮件操作请求
	//ResponseType = MailGenRsp
	//MessageVerb = POST
	//GameUrlOpcode = api/mail/gen
	[ProtoContract]
	public partial class MailGenReq: IRequest
	{
		public string url => "api/mail/gen";

		[ProtoMember(1)]
		public int cid { get; set; }

		[ProtoMember(2)]
		public CustomDefineParam param { get; set; }

	}

	[ProtoContract]
	public partial class MailGenRsp: IResponse
	{
		[ProtoMember(1)]
		public bool ok { get; set; }

	}

	[ProtoContract]
	public partial class CustomDefineParam
	{
		[ProtoMember(1)]
		public long sendingTimestamp { get; set; }

		[ProtoMember(2)]
		public string title { get; set; }

		[ProtoMember(3)]
		public string content { get; set; }

		[ProtoMember(4)]
		public int mailLifespan { get; set; }

		[ProtoMember(5)]
		public List<ChangeItemDto> rewards = new List<ChangeItemDto>();

	}

	// 发起购买下单请求
	//ResponseType = RechargeResp
	//MessageVerb = POST
	//GameUrlOpcode = api/payment/recharge
	[ProtoContract]
	public partial class RechargeReq: IRequest
	{
		public string url => "api/payment/recharge";

		// 充值配置id
		[ProtoMember(1)]
		public int goodsCid { get; set; }

		// 充值渠道 99充值: 99SDK
		[ProtoMember(2)]
		public string channel { get; set; }

		// 购买参数，暂时不使用
		[ProtoMember(3)]
		public string param { get; set; }

		// 充值场景
		[ProtoMember(4)]
		public RechargeScene scene { get; set; }

	}

	// 发起购买下单响应
	[ProtoContract]
	public partial class RechargeResp: IResponse
	{
		// 充值订单
		[ProtoMember(1)]
		public PaymentOrderDto order { get; set; }

	}

	// 查询订单状态
	//ResponseType = QueryPaymentOrderResp
	//MessageVerb = POST
	//GameUrlOpcode = api/payment/query
	[ProtoContract]
	public partial class QueryPaymentOrderReq: IRequest
	{
		public string url => "api/payment/query";

		// 充值订单id
		[ProtoMember(1)]
		public long orderId { get; set; }

	}

	// 查询订单状态响应
	[ProtoContract]
	public partial class QueryPaymentOrderResp: IResponse
	{
		// 充值订单
		[ProtoMember(1)]
		public PaymentOrderDto order { get; set; }

	}

	// 取消支付订单
	//ResponseType = ClosePaymentOrderResp
	//MessageVerb = POST
	//GameUrlOpcode = api/payment/close
	[ProtoContract]
	public partial class ClosePaymentOrderReq: IRequest
	{
		public string url => "api/payment/close";

		// 充值订单id
		[ProtoMember(1)]
		public long orderId { get; set; }

	}

	[ProtoContract]
	public partial class ClosePaymentOrderResp: IResponse
	{
		// 充值订单
		[ProtoMember(1)]
		public PaymentOrderDto order { get; set; }

	}

	// 服务器订单信息
	[ProtoContract]
	public partial class PaymentOrderDto
	{
		// 订单id
		[ProtoMember(1)]
		public long id { get; set; }

		// 充值配置id
		[ProtoMember(2)]
		public int cid { get; set; }

		// 创建时间
		[ProtoMember(3)]
		public long createTime { get; set; }

		// 订单状态 0:未支付 1:已支付 2:已到账
		[ProtoMember(4)]
		public int status { get; set; }

		// 玩家id
		[ProtoMember(5)]
		public long rid { get; set; }

		// 充值渠道 99SDK: 99充值
		[ProtoMember(6)]
		public string channel { get; set; }

		// 充值场景
		[ProtoMember(7)]
		public RechargeScene scene { get; set; }

		// 订单的奖励内容
		[ProtoMember(8)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	//排行榜玩家数据
	[ProtoContract]
	public partial class RankRoleDto
	{
		[ProtoMember(1)]
		public long id { get; set; }

		// 昵称
		[ProtoMember(2)]
		public string nickname { get; set; }

		// 头像
		[ProtoMember(3)]
		public string avatar { get; set; }

		// 玩家等级
		[ProtoMember(4)]
		public int level { get; set; }

		// 玩家排名
		[ProtoMember(5)]
		public int rank { get; set; }

	}

	// 获取排行榜列表
	//ResponseType = GetRankingListResp
	//MessageVerb = POST
	//GameUrlOpcode = api/ranking/list
	[ProtoContract]
	public partial class GetRankingListReq: IRequest
	{
		public string url => "api/ranking/list";

		//排行榜类型
		[ProtoMember(1)]
		public int type { get; set; }

		//排行榜涵盖的范围（EX: 1.全区服 2.单服 3.好友）
		[ProtoMember(2)]
		public int zoneType { get; set; }

		//排行榜起始位置
		[ProtoMember(3)]
		public long start { get; set; }

		//排行榜结束位置
		[ProtoMember(4)]
		public long end { get; set; }

	}

	// 排行榜列表响应
	[ProtoContract]
	public partial class GetRankingListResp: IResponse
	{
		// 排行榜列表列表
		[ProtoMember(1)]
		public List<RankRoleDto> rankRole = new List<RankRoleDto>();

	}

	// 兑换码使用请求
	//ResponseType = UseRedeemCodeResp
	//MessageVerb = POST
	//GameUrlOpcode = api/redeem_code/use
	[ProtoContract]
	public partial class UseRedeemCodeReq: IRequest
	{
		public string url => "api/redeem_code/use";

		// 兑换码
		[ProtoMember(1)]
		public string code { get; set; }

	}

	// 兑换码使用响应
	[ProtoContract]
	public partial class UseRedeemCodeResp: IResponse
	{
		// 获取的奖励
		[ProtoMember(1)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	// 获取玩家基础信息请求
	//ResponseType = GetRoleInfoResp
	//MessageVerb = POST
	//GameUrlOpcode = api/role/info
	[ProtoContract]
	public partial class GetRoleInfoReq: IRequest
	{
		public string url => "api/role/info";

	}

	// 获取玩家基础信息响应
	[ProtoContract]
	public partial class GetRoleInfoResp: IResponse
	{
		// 玩家基础信息
		[ProtoMember(1)]
		public RoleDto baseInfo { get; set; }

		// 体力恢复时间戳（毫秒）
		[ProtoMember(2)]
		public long lastStaminaRecoverTime { get; set; }

	}

	// 玩家体力值检查请求
	//ResponseType = CheckStaminaResp
	//MessageVerb = POST
	//GameUrlOpcode = api/role/check_stamina
	[ProtoContract]
	public partial class CheckStaminaReq: IRequest
	{
		public string url => "api/role/check_stamina";

	}

	// 玩家体力值检查响应
	[ProtoContract]
	public partial class CheckStaminaResp: IResponse
	{
		// 玩家所有体力资源信息
		[ProtoMember(1)]
		public RoleItemDto stamina { get; set; }

		// 体力恢复时间戳（毫秒）
		[ProtoMember(2)]
		public long lastStaminaRecoverTime { get; set; }

	}

	[ProtoContract]
	public partial class RoleDto
	{
		[ProtoMember(1)]
		public long id { get; set; }

		// 昵称
		[ProtoMember(2)]
		public string nickname { get; set; }

		// 头像
		[ProtoMember(3)]
		public string avatar { get; set; }

		// 玩家等级
		[ProtoMember(4)]
		public int level { get; set; }

		// 玩家本日扫荡剩余次数
		[ProtoMember(5)]
		public int leftSweepCount { get; set; }

		// 玩家当前使用的宝石预设页数（TODO 即将作废，使用gem.proto下的GetWearGemsListResp#preset）
		[ProtoMember(6)]
		public int gemPreset { get; set; }

		// 玩家当前已通过的最高关卡
		[ProtoMember(7)]
		public int maxCombatLevel { get; set; }

	}

	// 玩家账号删除请求
	//ResponseType = SelfDeletionResp
	//MessageVerb = POST
	//GameUrlOpcode = api/role/self_deletion
	[ProtoContract]
	public partial class SelfDeletionReq: IRequest
	{
		public string url => "api/role/self_deletion";

	}

	// 玩家账号删除响应
	[ProtoContract]
	public partial class SelfDeletionResp: IResponse
	{
	}

	// 心跳请求
	//ResponseType = HeartBeatResp
	//MessageVerb = POST
	//GameUrlOpcode = api/role/heartbeat
	[ProtoContract]
	public partial class HeartBeatReq: IRequest
	{
		public string url => "api/role/heartbeat";

	}

	[ProtoContract]
	public partial class HeartBeatResp: IResponse
	{
		[ProtoMember(1)]
		public long serverTime { get; set; }

	}

	// 问卷奖励领取请求
	//ResponseType = QuestionnaireRewardResp
	//MessageVerb = POST
	//GameUrlOpcode = api/role/questionnaire_reward
	[ProtoContract]
	public partial class QuestionnaireRewardReq: IRequest
	{
		public string url => "api/role/questionnaire_reward";

		// 问卷id
		[ProtoMember(1)]
		public string quizId { get; set; }

		// 操作 1:填完问卷 2：领取奖励
		[ProtoMember(2)]
		public int operation { get; set; }

	}

	// 问卷奖励领取响应
	[ProtoContract]
	public partial class QuestionnaireRewardResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	// 问卷状态查询接口
	//ResponseType = QuestionnaireStatusResp
	//MessageVerb = POST
	//GameUrlOpcode = api/role/questionnaire_status
	[ProtoContract]
	public partial class QuestionnaireStatusReq: IRequest
	{
		public string url => "api/role/questionnaire_status";

		// 问卷id
		[ProtoMember(1)]
		public List<string> quizIds = new List<string>();

	}

	// 问卷状态查询接口
	[ProtoContract]
	public partial class QuestionnaireStatusResp: IResponse
	{
		[ProtoMember(1)]
		public List<QuizStatusDto> statuses = new List<QuizStatusDto>();

	}

	[ProtoContract]
	public partial class QuizStatusDto
	{
		[ProtoMember(1)]
		public string quizId { get; set; }

		// 0:未完成 1:可领取 2：已领取
		[ProtoMember(2)]
		public int status { get; set; }

	}

	[ProtoContract]
	public partial class VipDto
	{
		[ProtoMember(1)]
		public PeriodicRewardDto vipWithRewardInfo { get; set; }

		[ProtoMember(2)]
		public PeriodicPrivilegeDto vipWithPrivilegeInfo { get; set; }

		[ProtoMember(3)]
		public PeriodicPrivilegeDto vipWithPrivilegeExpInfo { get; set; }

	}

	[ProtoContract]
	public partial class PeriodicRewardDto
	{
		// 过期时间戳，utc时间，单位毫秒
		[ProtoMember(1)]
		public long expireTime { get; set; }

		// 本日奖励是否已领取 true：已领取；false：未领取
		[ProtoMember(2)]
		public bool receivedToday { get; set; }

	}

	[ProtoContract]
	public partial class PeriodicPrivilegeDto
	{
		// 过期时间戳，utc时间，单位毫秒
		[ProtoMember(1)]
		public long expireTime { get; set; }

	}

	// 获取玩家月卡信息请求
	//ResponseType = GetVipInfoResp
	//MessageVerb = POST
	//GameUrlOpcode = api/role/vip/info
	[ProtoContract]
	public partial class GetVipInfoReq: IRequest
	{
		public string url => "api/role/vip/info";

	}

	// 获取玩家基础信息响应
	[ProtoContract]
	public partial class GetVipInfoResp: IResponse
	{
		// 玩家vip信息
		[ProtoMember(1)]
		public VipDto vipInfo { get; set; }

	}

	// 同步用户昵称请求
	//ResponseType = ChgNicknameResp
	//MessageVerb = POST
	//GameUrlOpcode = api/role/chg_nickname
	[ProtoContract]
	public partial class ChgNicknameReq: IRequest
	{
		public string url => "api/role/chg_nickname";

		// 用户昵称
		[ProtoMember(1)]
		public string nickname { get; set; }

	}

	// 同步用户昵称响应
	[ProtoContract]
	public partial class ChgNicknameResp: IResponse
	{
		[ProtoMember(1)]
		public bool success { get; set; }

	}

	// 同步用户头像请求
	//ResponseType = ChgAvatarResp
	//MessageVerb = POST
	//GameUrlOpcode = api/role/chg_avatar
	[ProtoContract]
	public partial class ChgAvatarReq: IRequest
	{
		public string url => "api/role/chg_avatar";

		// 用户头像
		[ProtoMember(1)]
		public string avatar { get; set; }

	}

	// 同步用户头像响应
	[ProtoContract]
	public partial class ChgAvatarResp: IResponse
	{
		[ProtoMember(1)]
		public bool success { get; set; }

	}

	// 获取玩家商城信息请求
	//ResponseType = UserShopInfoResp
	//MessageVerb = POST
	//GameUrlOpcode = api/shop/info
	[ProtoContract]
	public partial class UserShopInfoReq: IRequest
	{
		public string url => "api/shop/info";

	}

	// 获取玩家商城信息的响应
	[ProtoContract]
	public partial class UserShopInfoResp: IResponse
	{
		[ProtoMember(1)]
		public List<GoodsInfoDto> goodsInfos = new List<GoodsInfoDto>();

		[ProtoMember(2)]
		public List<GachaInfoDto> gachaInfos = new List<GachaInfoDto>();

	}

	[ProtoContract]
	public partial class GoodsInfoDto
	{
		[ProtoMember(1)]
		public int cid { get; set; }

		[ProtoMember(2)]
		public int purchasedCount { get; set; }

		[ProtoMember(3)]
		public long purchasedTotal { get; set; }

		//  int32 adsViewedToday = 2; // 今日已查看广告次数
	}

	[ProtoContract]
	public partial class GachaInfoDto
	{
		[ProtoMember(1)]
		public int cid { get; set; }

		[ProtoMember(2)]
		public long purchasedInPast { get; set; }

		//  int32 purchasedToday = 2; // 今日已购买次数
		//  int32 adsViewedToday = 2; // 今日已查看广告次数
	}

	// 玩家商城购买请求
	//ResponseType = ShopPurchaseResp
	//MessageVerb = POST
	//GameUrlOpcode = api/shop/purchase
	[ProtoContract]
	public partial class ShopPurchaseReq: IRequest
	{
		public string url => "api/shop/purchase";

		[ProtoMember(1)]
		public int cid { get; set; }

		//  bool byAds = 2; // 是否通过广告购买
	}

	// 玩家商城购买响应
	[ProtoContract]
	public partial class ShopPurchaseResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> changes = new List<RoleItemChangeDto>();

	}

	// 玩家宝石抽奖购买请求
	//ResponseType = GachaPurchaseResp
	//MessageVerb = POST
	//GameUrlOpcode = api/shop/gacha/purchase
	[ProtoContract]
	public partial class GachaPurchaseReq: IRequest
	{
		public string url => "api/shop/gacha/purchase";

		[ProtoMember(1)]
		public int cid { get; set; }

		[ProtoMember(2)]
		public int drawNum { get; set; }

		[ProtoMember(3)]
		public int drawType { get; set; }

	}

	// 玩家宝石抽奖响应
	[ProtoContract]
	public partial class GachaPurchaseResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> changes = new List<RoleItemChangeDto>();

	}

	// 玩家获取超值月卡内容物的请求
	//ResponseType = GainPeriodicRewardsResp
	//MessageVerb = POST
	//GameUrlOpcode = api/shop/periodic/rewards
	[ProtoContract]
	public partial class GainPeriodicRewardsReq: IRequest
	{
		public string url => "api/shop/periodic/rewards";

	}

	// 玩家获取超值月卡内容物的响应
	[ProtoContract]
	public partial class GainPeriodicRewardsResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> changes = new List<RoleItemChangeDto>();

	}

	//请求技能列表
	//ResponseType = GetSkillsResp
	//MessageVerb = POST
	//GameUrlOpcode = api/skill/list
	[ProtoContract]
	public partial class GetSkillsReq: IRequest
	{
		public string url => "api/skill/list";

	}

	// 响应技能列表
	[ProtoContract]
	public partial class GetSkillsResp: IResponse
	{
		// 技能列表
		[ProtoMember(1)]
		public List<int> skills = new List<int>();

	}

	//请求升级技能
	//ResponseType = UpgradeSkillResp
	//MessageVerb = POST
	//GameUrlOpcode = api/skill/upgrade
	[ProtoContract]
	public partial class UpgradeSkillReq: IRequest
	{
		public string url => "api/skill/upgrade";

		// 技能配置id
		[ProtoMember(1)]
		public int cid { get; set; }

	}

	// 响应升级技能
	[ProtoContract]
	public partial class UpgradeSkillResp: IResponse
	{
		// 升级消耗
		[ProtoMember(1)]
		public List<RoleItemChangeDto> costs = new List<RoleItemChangeDto>();

	}

	//请求任务列表
	//ResponseType = GetTaskResp
	//MessageVerb = POST
	//GameUrlOpcode = api/task/period/list
	[ProtoContract]
	public partial class GetTaskReq: IRequest
	{
		public string url => "api/task/period/list";

		// 1=日 2=周 4=新人
		[ProtoMember(1)]
		public TaskType taskType { get; set; }

	}

	// 响应任务列表
	[ProtoContract]
	public partial class GetTaskResp: IResponse
	{
		// 1=日 2=周 4=新人
		[ProtoMember(1)]
		public TaskType taskType { get; set; }

		// 任务列表
		[ProtoMember(2)]
		public List<TaskDto> tasks = new List<TaskDto>();

		// 活跃度进度
		[ProtoMember(3)]
		public List<TaskDto> activeTasks = new List<TaskDto>();

	}

	//请求领取奖励
	//ResponseType = RewardTaskResp
	//MessageVerb = POST
	//GameUrlOpcode = api/task/period/reward
	[ProtoContract]
	public partial class RewardTaskReq: IRequest
	{
		public string url => "api/task/period/reward";

		// 任务配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 领取类型 0=任务奖励 1=活跃度进度奖励
		[ProtoMember(2)]
		public RewardType rewardType { get; set; }

	}

	// 响应领取奖励
	[ProtoContract]
	public partial class RewardTaskResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	//请求批量领取奖励
	//ResponseType = BatchRewardTaskResp
	//MessageVerb = POST
	//GameUrlOpcode = api/task/period/reward/batch
	[ProtoContract]
	public partial class BatchRewardTaskReq: IRequest
	{
		public string url => "api/task/period/reward/batch";

		// 任务配置id
		[ProtoMember(1)]
		public List<int> cids = new List<int>();

		// 领取类型 0：任务奖励 1：活跃度进度奖励
		[ProtoMember(2)]
		public RewardType rewardType { get; set; }

	}

	// 响应领取奖励
	[ProtoContract]
	public partial class BatchRewardTaskResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	// 任务信息
	[ProtoContract]
	public partial class TaskDto
	{
		[ProtoMember(1)]
		public int cid { get; set; }

		[ProtoMember(2)]
		public int progress { get; set; }

		[ProtoMember(3)]
		public bool isReward { get; set; }

		// 更新时间戳（毫秒）
		[ProtoMember(4)]
		public long updateTime { get; set; }

	}

	//请求主线任务
	//ResponseType = GetMainTaskResp
	//MessageVerb = POST
	//GameUrlOpcode = api/task/main/list
	[ProtoContract]
	public partial class GetMainTaskReq: IRequest
	{
		public string url => "api/task/main/list";

	}

	// 响应主线任务
	[ProtoContract]
	public partial class GetMainTaskResp: IResponse
	{
		// 任务列表
		[ProtoMember(1)]
		public List<TaskDto> tasks = new List<TaskDto>();

	}

	//请求领取奖励
	//ResponseType = RewardMainTaskResp
	//MessageVerb = POST
	//GameUrlOpcode = api/task/main/reward
	[ProtoContract]
	public partial class RewardMainTaskReq: IRequest
	{
		public string url => "api/task/main/reward";

		[ProtoMember(1)]
		public int cid { get; set; }

	}

	// 响应领取奖励
	[ProtoContract]
	public partial class RewardMainTaskResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	//请求批量领取奖励
	//ResponseType = BatchRewardMainTaskResp
	//MessageVerb = POST
	//GameUrlOpcode = api/task/main/reward/batch
	[ProtoContract]
	public partial class BatchRewardMainTaskReq: IRequest
	{
		public string url => "api/task/main/reward/batch";

		[ProtoMember(1)]
		public List<int> cid = new List<int>();

	}

	// 响应领取奖励
	[ProtoContract]
	public partial class BatchRewardMainTaskResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> rewards = new List<RoleItemChangeDto>();

	}

	//请求UAV列表
	//ResponseType = GetUavResp
	//MessageVerb = POST
	//GameUrlOpcode = api/uav/list
	[ProtoContract]
	public partial class GetUavReq: IRequest
	{
		public string url => "api/uav/list";

	}

	// 响应UAV列表
	[ProtoContract]
	public partial class GetUavResp: IResponse
	{
		// UAV列表
		[ProtoMember(1)]
		public List<UavDto> list = new List<UavDto>();

		[ProtoMember(2)]
		public List<UavSkillSlotDto> skills = new List<UavSkillSlotDto>();

		[ProtoMember(3)]
		public List<UavFormationDto> formations = new List<UavFormationDto>();

		[ProtoMember(4)]
		public int preset { get; set; }

	}

	[ProtoContract]
	public partial class UavDto
	{
		// 配置id
		[ProtoMember(1)]
		public int cid { get; set; }

	}

	[ProtoContract]
	public partial class UavSkillSlotDto
	{
		// 预设套装id
		[ProtoMember(1)]
		public int presetId { get; set; }

		// 槽位id
		[ProtoMember(2)]
		public int slotId { get; set; }

		// 孔位id
		[ProtoMember(3)]
		public int posId { get; set; }

		// 技能配置id
		[ProtoMember(4)]
		public int skillCid { get; set; }

	}

	[ProtoContract]
	public partial class UavFormationDto
	{
		// 槽位id 1-5
		[ProtoMember(1)]
		public int slotId { get; set; }

		// UAV配置id 对应配置表uav_id
		[ProtoMember(2)]
		public int uavType { get; set; }

	}

	//请求激活UAV
	//ResponseType = ActivateUavResp
	//MessageVerb = POST
	//GameUrlOpcode = api/uav/activate
	[ProtoContract]
	public partial class ActivateUavReq: IRequest
	{
		public string url => "api/uav/activate";

		// UAV配置id
		[ProtoMember(1)]
		public int cid { get; set; }

	}

	// 响应激活UAV
	[ProtoContract]
	public partial class ActivateUavResp: IResponse
	{
		// 消耗
		[ProtoMember(1)]
		public List<RoleItemChangeDto> costs = new List<RoleItemChangeDto>();

		[ProtoMember(2)]
		public UavDto uav { get; set; }

	}

	//请求升级UAV
	//ResponseType = UpgradeUavResp
	//MessageVerb = POST
	//GameUrlOpcode = api/uav/upgrade
	[ProtoContract]
	public partial class UpgradeUavReq: IRequest
	{
		public string url => "api/uav/upgrade";

		// UAV配置id
		[ProtoMember(1)]
		public int cid { get; set; }

		// 消耗物品配置id
		[ProtoMember(2)]
		public int costItemCid { get; set; }

	}

	// 响应升级UAV
	[ProtoContract]
	public partial class UpgradeUavResp: IResponse
	{
		// 升级消耗
		[ProtoMember(1)]
		public List<RoleItemChangeDto> costs = new List<RoleItemChangeDto>();

	}

	// 请求更改UAV技能槽
	// ResponseType = ChangeSkillResp
	// MessageVerb = POST
	// GameUrlOpcode = api/uav/skill/change
	[ProtoContract]
	public partial class ChangeSkillReq: IRequest
	{
		public string url => "api/uav/skill/change";

		// 预设套装id 1-3
		[ProtoMember(1)]
		public int presetId { get; set; }

		// 槽位id 1-3
		[ProtoMember(2)]
		public int slotId { get; set; }

		// 孔位id 1-N
		[ProtoMember(3)]
		public int posId { get; set; }

		// 技能配置id
		[ProtoMember(4)]
		public int skillCid { get; set; }

		// 是否嵌入
		[ProtoMember(5)]
		public bool embed { get; set; }

	}

	// 响应更改UAV技能槽
	[ProtoContract]
	public partial class ChangeSkillResp: IResponse
	{
	}

	// 请求更改UAV编队
	// ResponseType = ChangeFormationResp
	// MessageVerb = POST
	// GameUrlOpcode = api/uav/formation/change
	[ProtoContract]
	public partial class ChangeFormationReq: IRequest
	{
		public string url => "api/uav/formation/change";

		// 槽位id 1-5
		[ProtoMember(1)]
		public int slotId { get; set; }

		// UAV配置id
		[ProtoMember(2)]
		public int uavCid { get; set; }

		// 是否嵌入
		[ProtoMember(3)]
		public bool embed { get; set; }

	}

	// 响应更改UAV编队
	[ProtoContract]
	public partial class ChangeFormationResp: IResponse
	{
	}

	// 保存UAV预设请求
	//ResponseType = SaveUavPresetResp
	//MessageVerb = POST
	//GameUrlOpcode = api/uav/save_preset
	[ProtoContract]
	public partial class SaveUavPresetReq: IRequest
	{
		public string url => "api/uav/save_preset";

		// 需要保存的装备预设页数
		[ProtoMember(1)]
		public int preset { get; set; }

	}

	// 保存UVA预设响应
	[ProtoContract]
	public partial class SaveUavPresetResp: IResponse
	{
	}

	// 分解UAV材料请求
	//ResponseType = ConvertUavMatResp
	//MessageVerb = POST
	//GameUrlOpcode = api/uav/convert
	[ProtoContract]
	public partial class ConvertUavMatReq: IRequest
	{
		public string url => "api/uav/convert";

		[ProtoMember(1)]
		public int cid { get; set; }

		[ProtoMember(2)]
		public int num { get; set; }

	}

	// 保存UVA预设响应
	[ProtoContract]
	public partial class ConvertUavMatResp: IResponse
	{
		[ProtoMember(1)]
		public List<RoleItemChangeDto> result = new List<RoleItemChangeDto>();

	}

}
