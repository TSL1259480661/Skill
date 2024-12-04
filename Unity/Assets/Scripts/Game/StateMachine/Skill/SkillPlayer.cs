using Skills.Core;

namespace Skills
{
	public enum TargetEnum
	{
		Self,
		Enemy,
		Ally,
		TriggerBehavior
	}

	public class SkillBaseData : SkillPlayerBaseData
	{
		public TargetEnum fromTargetEnum;
		public TargetEnum toTargetEnum;
	}

	//public interface ISkillTarget : ISkillTargetBase
}
