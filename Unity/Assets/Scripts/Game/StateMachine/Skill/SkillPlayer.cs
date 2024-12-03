using App;
using Skills.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

	
}
