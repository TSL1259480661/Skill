using ClientData;
using Skills;
using System;
using System.Collections.Generic;


public enum SkillType
{
	Attack = 7,//近战
	UItimate = 8,//大招
	RangedAttack = 9,//远程
	Dodge = 21,//闪避
	Block = 22,//格挡
}

public class SkillMainData : SkillBaseData
{
	public int skillId;//技能id
	public Skill_Desc_skill_active config;//技能表现相关的配置
	public int[] paramList;//技能表现数据后的参数列表
	public Dictionary<int, Asset_effect> effectList = new Dictionary<int, Asset_effect>(10);//特效列表
	public Dictionary<int, int[]> effectParams = new Dictionary<int, int[]>(10);//特效列表相关参数
	public Dictionary<int, int> audioDic = new Dictionary<int, int>(10);//音效列表
	public Dictionary<int, int> effectScaleRateDic;//特效缩放列表

	public List<int[]> interruptions = new List<int[]>();//当前技能能被某些技能打断
	public int cdTime;//技能cd
	public float durationTime;//持续时间
	public int triggerCdType;//技能释放后，技能cd的类型（1.释放就进入倒计时，2.释放完进入倒计时）
	public bool autoActive;//是否自动激活
	public int extraRepeat;//重复执行
	public int extraCount;//重复执行的次数

	public SkillMainData()
	{

	}

	public SkillMainData(int capacity)//构造函数，在初始化阶段就预生成对应数量的技能行为
	{
		triggerList = new List<Skills.Core.SkillPlayerBaseData>(capacity);
	}

	private Action<SkillMainData, int[], List<int[]>, Dictionary<int, Asset_effect>, Dictionary<int, int[]>, Dictionary<int, int>> initCallback;//初始化的回调，当数据层面准备好后，通过回调的形式创建

	public void Init(Action<SkillMainData, int[], List<int[]>, Dictionary<int, Asset_effect>, Dictionary<int, int[]>, Dictionary<int, int>> initCallback)
	{
		this.initCallback = initCallback;//记录外界的回调
	}
	public SkillMainData Create(int capacity = 5)//空的数据为何在赋值
	{
		SkillMainData skillMainData = new SkillMainData(capacity);
		skillMainData.skillId = skillId;//技能id
		skillMainData.config = config;//技能表现相关配置
		skillMainData.cdTime = cdTime;//技能的cd类型（两种：点击技能立刻计时，技能释放完后再计时）
		skillMainData.damage = damage;//伤害值
		skillMainData.durationTime = durationTime;//技能持续时间
		skillMainData.skillType = skillType;//技能的类型
		skillMainData.movable = movable;//该技能是否可以再移动的情况下释放
		skillMainData.parentSkillId = parentSkillId;//父技能的id
		skillMainData.triggerCdType = triggerCdType;//技能的cd类型
		skillMainData.autoActive = autoActive;//技能是否自动释放
		skillMainData.extraRepeat = extraRepeat; //技能是否重复
		skillMainData.extraCount = extraCount;//技能重复的释放次数
		skillMainData.damage = damage;//技能的伤害
		skillMainData.weight = weight;//技能的权重
		skillMainData.maxHpPct = maxHpPct;//技能释放的血量限制
		skillMainData.playImmediately = playImmediately;//满足某条件后立即释放一次技能
		skillMainData.playDirectly = playDirectly;//技能的释放是否需要打断为前提

		skillMainData.Init(initCallback);
		initCallback?.Invoke(skillMainData,paramList,interruptions,effectList,effectParams,audioDic);
		return skillMainData;
	}

	public float checkRadius;//伤害检测半径
	public SkillType skillType;//技能类型

	public bool movable;//定义当前技能是否可以再移动的情况释放
	public int parentSkillId;//夫技能id
	public int damage;//技能伤害
	public int weight;//技能权重
	public float maxHpPct;//血量限制(例如：血量低于多少才可以释放)
	public bool playImmediately;//满足某个条件后立刻释放
	public bool playDirectly;//该技能是否需要被打断才能释放，参考普攻三连击

	public void Add(SkillBaseData skillBaseData)
	{
		triggerList.Add(skillBaseData);//添加技能具体行为数据
	}
}

