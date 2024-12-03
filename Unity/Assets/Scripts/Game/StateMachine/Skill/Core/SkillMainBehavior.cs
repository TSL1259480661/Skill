namespace Skills.Core
{
	public partial class SkillPlayerBase<T_SkillContext, T_ParamContext>
	{
		public class SkillMainBehavior : SkillPlayerBehaviorBase
		{
			protected override void Start()
			{
				done = true;
			}

			protected override void End()
			{
			}

			protected override void FrameUpdate(float deltaTime)
			{
			}
		}
	}
}
