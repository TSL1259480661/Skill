
using App;
using System.Collections.Generic;
using System.Numerics;

namespace Skills.Core
{

	public interface ISkillTargetBase//技能目标的接口
	{
		Vector3 GetPositon();//获取技能目标位置
		Vector3 GetForward();//获取技能目标的方向
	}

	public class SkillTargetListBase<T_SkillTarget> : IObjectPoolItem where T_SkillTarget : ISkillTargetBase
	{
		private static UObjectPool<SkillTargetListBase<T_SkillTarget>> pool = new UObjectPool<SkillTargetListBase<T_SkillTarget>>();

		public static SkillTargetListBase<T_SkillTarget> baseGet()//获取技能目标列表的基类
		{
			return pool.Get();
		}

		private static UDebugger debugger = new UDebugger("SkillTargetList");


		/// <summary>
		/// 技能目标对象列表数组，[int:目标类型][int：位置index]
		/// </summary>
		private List<T_SkillTarget>[] targetDic;

		public SkillTargetListBase()
		{
		}

		/// <summary>
		/// 预加载的预设大小
		/// </summary>
		private int capacity;
		public void Init(int typeCount,int capacity)
		{
			this.capacity = capacity;//记录
			if(targetDic == null || typeCount != targetDic.Length)
			{
				targetDic = new List<T_SkillTarget>[typeCount];//typeCount 可能恒定为3，因为敌人，自己，所有人
			}
		}

		/// <summary>
		/// 已经触发了的技能目标列表吗，一维的参数大概率是：类型，技能目标类型{自己，敌人，所有人}
		/// </summary>
		private List<List<T_SkillTarget>> triggeredTargetList = new List<List<T_SkillTarget>>(10);

		/// <summary>
		/// 技能目标的缓存列表
		/// </summary>
		private static List<List<T_SkillTarget>> cacheList = new List<List<T_SkillTarget>>(100);

		public void OnRecycle()
		{

		}

		public void OnReuse()
		{
			
		}
	}
}
