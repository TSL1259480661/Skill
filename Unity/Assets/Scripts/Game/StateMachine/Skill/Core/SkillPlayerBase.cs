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

	public abstract partial class SkillPlayerBase<T_SkillContext, T_ParamContext>
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
				/// <param name="skillBehaviorBase"></param>
				/// <param name="SkillContext"></param>
				/// <param name="paramContext"></param>
				/// <param name="onTriggerList"></param>
				/// <param name="behaviorFactory"></param>
				public void Init(SkillPlayerBehaviorBase skillBehaviorBase, T_SkillContext SkillContext, T_ParamContext paramContext, Action<SkillPlayerBehaviorBase> onTriggerList, ISkillFactory behaviorFactory)
				{
					var mainData = skillBehaviorBase.data;

					this.behaviorFactory = behaviorFactory;

					if (mainData != null && mainData.triggerList != null && mainData.triggerList.Count > 0)
					{
						for (int i = 0; i < mainData.triggerList.Count; i++)
						{
							SkillPlayerBaseData data = mainData.triggerList[i];
							SkillPlayerBehaviorBase behavior = behaviorFactory.CreateBehavior(data);
							behavior.Init(data, SkillContext, paramContext, skillBehaviorBase, onTriggerList);
							list.Add(behavior);
						}
					}
				}

				public void Update(float deltaTime)
				{
					if (started)
					{
						if (list != null)
						{
							for (int i = 0; i < list.Count; i++)
							{
								list[i].Update(duration);
							}
						}

						duration += deltaTime;
					}
				}

				public void Start()
				{
					started = true;
					duration = 0f;
				}

				public bool CheckPlayDone()
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

			private List<RunningList> runningTriggeredList = new List<RunningList>(20);

			public void OnTriggerRunningList(SkillPlayerBehaviorBase skillBehaviorBase)
			{
				var runningList = RunningList.Get();
				runningList.Init(skillBehaviorBase, skillContext, paramContext, OnTriggerRunningList, skillBehaviorFactory);
				runningTriggeredList.Add(runningList);
				runningList.Start();
			}

			private static UDebugger debugger = new UDebugger("Skill");

			public T_SkillContext skillContext;

			private SkillMainBehavior skillMainBehavior = new SkillMainBehavior();
			public T_ParamContext paramContext;
			private ISkillFactory skillBehaviorFactory;

			public void Init(SkillPlayerBaseData skillPlayerBaseData, T_SkillContext skillContext, T_ParamContext paramContext, ISkillFactory skillBehaviorFactory)
			{
				this.skillContext = skillContext?.Clone() as T_SkillContext;
				this.paramContext = paramContext;
				this.skillBehaviorFactory = skillBehaviorFactory;

				skillMainBehavior.Init(skillPlayerBaseData, skillContext, paramContext, null, OnTriggerRunningList);

				OnTriggerRunningList(skillMainBehavior);
			}

			public void Update(float deltaTime)
			{
				for (int i = 0; i < runningTriggeredList.Count; i++)
				{
					runningTriggeredList[i].Update(deltaTime);
				}
			}

			public bool CheckPlayDone()
			{
				bool hasAllDone = true;

				if (runningTriggeredList.Count > 0)
				{
					for (int i = runningTriggeredList.Count - 1; i >= 0; i--)
					{
						if (!runningTriggeredList[i].CheckPlayDone())
						{
							hasAllDone = false;
						}
					}
				}

				return hasAllDone;
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

				paramContext?.OnSkillEnd();
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

		private T_SkillContext skillContext;
		public void Init(T_SkillContext skillContext, ISkillFactory skillBehaviorFactory)
		{
			this.skillContext = skillContext;
			this.skillBehaviorFactory = skillBehaviorFactory;
		}

		public bool isPlaying
		{
			get
			{
				return runningSkillList.Count > 0;
			}
		}

		protected List<Skill> runningSkillList = new List<Skill>(30);


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
