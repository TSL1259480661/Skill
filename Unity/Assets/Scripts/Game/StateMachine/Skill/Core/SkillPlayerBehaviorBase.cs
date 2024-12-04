using System;
using System.Collections.Generic;

namespace Skills.Core
{
	public class SkillPlayerBaseData//技能具体行为的相关数据
	{
		public double startTime;//技能具体行为开始的时间
		public double endTime = -1f;//技能具体行为结束的时间
		public int startFrame = -1;//技能开始的帧
		public int endFrame = -1;//技能结束的帧

		public List<SkillPlayerBaseData> triggerList;//技能具体行为数据的列表
	}

	public partial class SkillPlayerBase<T_SkillContext, T_ParamContext>
	{
		public abstract class SkillPlayerBehaviorBase : IObjectPoolItem
		{
			abstract protected void Start();//开启当前技能具体行为
			abstract protected void FrameUpdate(float deltaTime);//技能具体行为的每一帧刷新
			abstract protected void End();//结束当前技能具体行为

			public SkillPlayerBaseData data { private set; get; }//技能具体行为的数据

			public bool done { protected set; get; }//技能结束的标识
			public float startTime;//技能具体行为的开始时间
			public float endTime;//技能具体行为的结束时间

			protected T_SkillContext skillContext;//技能具体行为所持有的组件

			private Action<SkillPlayerBehaviorBase> onTriggerList;//未知的回调：TODO

			protected void TriggerList()//执行未知的回调
			{
				onTriggerList?.Invoke(this);
			}

			protected T_ParamContext paramContext;//技能具体行为所持有的交互数据
			protected SkillPlayerBehaviorBase fromBehavior;//从那个技能行为过来的（待定）
			private int startFrame;//技能具体行为开始的帧
			private int endFrame;//技能具体行为结束的帧
			/// <summary>
			/// 技能具体行为的初始化
			/// </summary>
			/// <param name="data">技能具体行为的数据</param>
			/// <param name="skillContext">技能具体行为所持有的组件数据</param>
			/// <param name="paramContext">技能具体行为所持有的交互数据</param>
			/// <param name="fromBehavior">同40行</param>
			/// <param name="onTriggerList">某个不知名回调</param>
			public void Init(SkillPlayerBaseData data, T_SkillContext skillContext, T_ParamContext paramContext, SkillPlayerBehaviorBase fromBehavior, Action<SkillPlayerBehaviorBase> onTriggerList)
			{
				this.data = data;
				this.skillContext = skillContext;
				this.fromBehavior = fromBehavior;
				this.paramContext = paramContext;
				this.onTriggerList = onTriggerList;

				startTime = Convert.ToSingle(data.startTime);
				endTime = Convert.ToSingle(data.endTime);
				startFrame = data.startFrame;
				endFrame = data.endFrame;
			}

			protected int frameCount { get; private set; }//当前技能已经运行的帧
			private bool started;//技能具体行为是否开始
			/// <summary>
			/// 技能具体行为的刷新
			/// </summary>
			/// <param name="skillDuration">技能的持续时间</param>
			public void Update(float skillDuration)
			{
				if (!started && ((startFrame < 0 && skillDuration >= startTime) || (startFrame >= 0 && frameCount >= startFrame)))
				{
					started = true;
					startFrame = frameCount;
					Start();
				}

				if (started && !done)
				{
					FrameUpdate(skillDuration - startTime);
				}

				if (endFrame >= 0 && frameCount - startFrame >= endFrame)
				{
					if (started && !done)
					{
						done = true;
						End();
					}
				}
				else if (endTime >= 0f && skillDuration >= endTime)
				{
					if (started && !done)
					{
						done = true;
						End();
					}
				}

				frameCount++;
			}

			public virtual bool CheckDone()
			{
				return started && done;//若技能开始并且结束，则为代表当前技能具体行为彻底结束
			}

			public virtual void OnReuse()
			{
				started = false;
				done = false;

				frameCount = 0;
			}

			public virtual void OnRecycle()
			{
				this.data = null;
				this.skillContext = null;
				this.fromBehavior = null;
				this.paramContext = default;
				this.onTriggerList = null;

				startTime = 0f;
				endTime = 0f;
				startFrame = 0;
			}
		}
	}
}
