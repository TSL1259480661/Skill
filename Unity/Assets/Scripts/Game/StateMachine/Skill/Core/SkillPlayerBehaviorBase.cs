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
			abstract protected void Start();
			abstract protected void FrameUpdate(float deltaTime);
			abstract protected void End();

			public SkillPlayerBaseData data { private set; get; }

			public bool done { protected set; get; }
			public float startTime;
			public float endTime;

			protected T_SkillContext context;

			private Action<SkillPlayerBehaviorBase> onTriggerList;

			protected void TriggerList()
			{
				onTriggerList?.Invoke(this);
			}

			protected T_ParamContext paramContext;
			protected SkillPlayerBehaviorBase fromBehavior;
			private int startFrame;
			private int endFrame;
			public void Init(SkillPlayerBaseData data, T_SkillContext skillContext, T_ParamContext paramContext, SkillPlayerBehaviorBase fromBehavior, Action<SkillPlayerBehaviorBase> onTriggerList)
			{
				this.data = data;
				this.context = skillContext;
				this.fromBehavior = fromBehavior;
				this.paramContext = paramContext;
				this.onTriggerList = onTriggerList;

				startTime = Convert.ToSingle(data.startTime);
				endTime = Convert.ToSingle(data.endTime);
				startFrame = data.startFrame;
				endFrame = data.endFrame;
			}

			protected int frameCount { get; private set; }
			private bool started;
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
				return started && done;
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
				this.context = null;
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
