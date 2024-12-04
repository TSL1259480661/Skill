using App;
using System;
using System.Collections.Generic;

namespace Skills.Core
{
	public interface ISkillContext//技能所持有的组件数据
	{
		ISkillContext Clone();//克隆组件数据，因为存在多个技能行为列表{各个技能行为包含了技能行为的操作列表}
		void Recycle();
	}

	public interface IParamContext//技能所持有的交互数据
	{
		void OnSkillEnd();//技能结束后的回调
		void Recycle();
	}

	public abstract partial class SkillPlayerBase<T_SkillContext, T_ParamContext>//技能释放着本体
				where T_SkillContext : class, ISkillContext
				where T_ParamContext : class, IParamContext
	{
		protected abstract void OnSkillPlay(T_SkillContext context, T_ParamContext paramContext);//技能开始时的逻辑
		
		public interface ISkillFactory//技能工厂接口
		{
			public SkillPlayerBehaviorBase CreateBehavior(SkillPlayerBaseData data);//根据技能具体行为数据创建技能具体行为

			public void Recycle(SkillPlayerBaseData data, SkillPlayerBehaviorBase skillBase);//回收{参数：技能数据， 技能具体行为}
		}

		public class Skill : IObjectPoolItem
		{
			/// <summary>
			/// 技能具体行为列表
			/// </summary>
			public class RunningList : IObjectPoolItem
			{
				private static UObjectPool<Skill.RunningList> pool = new UObjectPool<Skill.RunningList>(200);

				public static RunningList Get()
				{
					return pool.Get();
				}

				public void Recycle()
				{
					pool.Recycle(this);
				}

				private List<SkillPlayerBehaviorBase> list = new List<SkillPlayerBehaviorBase>(20);//技能具体行为列表{上级为：技能行为{包含：技能主行为，一系列次级行为}}

				private bool started;//技能具体行为是否开始的标识
				private float duration;//技能具体行为持续的时间

				private ISkillFactory behaviorFactory;//技能具体行为生成工厂，外界传入

				/// <summary>
				/// 技能具体行为的初始化
				/// </summary>
				/// <param name="skillBehaviorBase">技能具体行为</param>
				/// <param name="SkillContext">技能具体行为所持有的组件信息</param>
				/// <param name="paramContext">技能具体行为所持有的交互信息</param>
				/// <param name="onTriggerList">（未知触发回调，怀疑行为结束的连续）</param>
				/// <param name="behaviorFactory">技能具体行为的工厂接口</param>
				public void Init(SkillPlayerBehaviorBase skillBehaviorBase, T_SkillContext SkillContext, T_ParamContext paramContext, Action<SkillPlayerBehaviorBase> onTriggerList, ISkillFactory behaviorFactory)
				{
					var mainData = skillBehaviorBase.data;

					this.behaviorFactory = behaviorFactory;

					if (mainData != null && mainData.triggerList != null && mainData.triggerList.Count > 0)
					{
						//根据外部传入的数据获取技能行为列表的数据并根据数据生成对应的技能具体行为{参靠{bullent,Audio等等}}
						for (int i = 0; i < mainData.triggerList.Count; i++)
						{
							SkillPlayerBaseData data = mainData.triggerList[i];
							SkillPlayerBehaviorBase behavior = behaviorFactory.CreateBehavior(data);
							behavior.Init(data, SkillContext, paramContext, skillBehaviorBase, onTriggerList);
							list.Add(behavior);
						}
					}
				}

				/// <summary>
				/// 技能具体行为列表的刷新
				/// </summary>
				/// <param name="deltaTime">间隔时间</param>
				public void Update(float deltaTime)
				{
					if (started)//前提，技能已经开始
					{
						if (list != null)//列表不为空
						{
							for (int i = 0; i < list.Count; i++)
							{
								list[i].Update(duration);//刷新技能具体行为，传递当前技能持续时间
							}
						}

						duration += deltaTime;//技能持续时间的累计
					}
				}

				public void Start()//技能的开始
				{
					started = true;//技能开始的标识
					duration = 0f;//技能的持续时间归零
				}

				public bool CheckPlayDone()//检测所有的技能具体行为是否结束
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (!list[i].CheckDone())
						{
							return false;
						}
					}
					return true;
				}

				public void OnRecycle()
				{
					for (int i = 0; i < list.Count; i++)
					{
						behaviorFactory.Recycle(list[i]?.data, list[i]);
					}
					list.Clear();
				}

				public void OnReuse()
				{
					started = false;
					duration = 0f;
				}
			}

			private static UObjectPool<Skill> skillPool = new UObjectPool<Skill>();
			public static Skill Get()
			{
				return skillPool.Get();
			}

			private List<RunningList> runningTriggeredList = new List<RunningList>(20);//技能行为列表{一个技能包含：主行为{多个技能具体行为}，次级行为{多个技能具体行为}}

			public void OnTriggerRunningList(SkillPlayerBehaviorBase skillBehaviorBase)//根据当前技能具体，获取技能具体行为列表，并进行初始化
			{
				var runningList = RunningList.Get();
				runningList.Init(skillBehaviorBase, skillContext, paramContext, OnTriggerRunningList, skillBehaviorFactory);
				runningTriggeredList.Add(runningList);
				runningList.Start();//技能具体行为开始运转
			}

			private static UDebugger debugger = new UDebugger("Skill");

			public T_SkillContext skillContext;//技能所持有的组件数据
			public T_ParamContext paramContext;//急哦能所持有的交互信息
			private SkillMainBehavior skillMainBehavior = new SkillMainBehavior();//技能具体行为的载体，不具备其他意义，单纯为传递数据
			private ISkillFactory skillBehaviorFactory;//技能工厂的接口

			/// <summary>
			/// 技能初始化
			/// </summary>
			/// <param name="skillPlayerBaseData">技能所持有的技能具体行为数据</param>
			/// <param name="skillContext">技能所持有的组件数据</param>
			/// <param name="paramContext">技能所持有的交互数据</param>
			/// <param name="skillBehaviorFactory">技能具体行为生成工厂</param>
			public void Init(SkillPlayerBaseData skillPlayerBaseData, T_SkillContext skillContext, T_ParamContext paramContext, ISkillFactory skillBehaviorFactory)
			{
				this.skillContext = skillContext?.Clone() as T_SkillContext;//克隆一份数据,防止多个技能具体行为产生冲突
				this.paramContext = paramContext;//技能具体行为的交互数据
				this.skillBehaviorFactory = skillBehaviorFactory;

				skillMainBehavior.Init(skillPlayerBaseData, skillContext, paramContext, null, OnTriggerRunningList);//初始化数据载体

				OnTriggerRunningList(skillMainBehavior);//解析数据并运行
			}

			/// <summary>
			/// 技能刷新
			/// </summary>
			/// <param name="deltaTime">刷新间隔</param>
			public void Update(float deltaTime)
			{
				for (int i = 0; i < runningTriggeredList.Count; i++)
				{
					runningTriggeredList[i].Update(deltaTime);
				}
			}

			public bool CheckPlayDone()//检测所有的技能行为是否完成（主行为，次级行为）
			{

				if (runningTriggeredList.Count > 0)
				{
					for (int i = runningTriggeredList.Count - 1; i >= 0; i--)
					{
						if (!runningTriggeredList[i].CheckPlayDone())
						{
							return false;
						}
					}
				}
				return true;
			}

			public void Recycle()
			{
				skillPool.Recycle(this);
			}

			public void OnRecycle()
			{
				if (runningTriggeredList.Count > 0)
				{
					for (int i = 0; i < runningTriggeredList.Count; i++)
					{
						runningTriggeredList[i].Recycle();
					}
					runningTriggeredList.Clear();
				}

				skillContext?.Recycle();
				skillContext = null;

				paramContext?.OnSkillEnd();//技能结束后，运行的逻辑
				paramContext?.Recycle();
				paramContext = null;
			}

			public void OnReuse()
			{
			}

			public override string ToString()
			{
				return $"Skill: {paramContext}";
			}
		}

		private static UDebugger debugger = new UDebugger("SkillPlayer");

		private ISkillFactory skillBehaviorFactory;

		private T_SkillContext skillContext;//技能释放者所持有的组件数据
		public void Init(T_SkillContext skillContext, ISkillFactory skillBehaviorFactory)
		{
			this.skillContext = skillContext;
			this.skillBehaviorFactory = skillBehaviorFactory;
		}

		public bool isPlaying//判定技能处于运行中
		{
			get
			{
				return runningSkillList.Count > 0;
			}
		}

		protected List<Skill> runningSkillList = new List<Skill>(30);//一个技能释放着，拥有多个技能

		public void Play(SkillMainData skillData,T_ParamContext paramContext)
		{
			Skill skill = Skill.Get();//获取技能
			skill.Init(skillData, skillContext,paramContext,skillBehaviorFactory);//初始化技能相关参数
			OnSkillPlay(skill.skillContext,skill.paramContext);//技能执行开始时，执行的逻辑
			runningSkillList.Add(skill);//将技能添加进技能列表
		}

		private void Update(float deltaTime)//刷新
		{
			if(runningSkillList.Count > 0)
			{
				for(int i=runningSkillList.Count - 1; i >= 0; i--)//倒着遍历是为了在删除执行完的技能行为时不影响列表
				{
					var skill = runningSkillList[i];//获取当前遍历技能
					skill.Update(deltaTime);//刷新当前技能行为
					if (skill.CheckPlayDone())//检测技能是否执行完成
					{
						debugger.LogFormat("Play Done:{0}",skill);
						skill.Recycle();//回收技能
						runningSkillList.RemoveAt(i);//在列表删除当前技能
					}
				}
			}
		}

		public void Clear()
		{
			for (int i = 0; i < runningSkillList.Count; i++)
			{
				runningSkillList[i].Recycle();
			}

			runningSkillList.Clear();
		}
	}
}
