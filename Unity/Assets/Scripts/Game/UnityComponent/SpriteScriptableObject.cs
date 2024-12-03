using System;
using UnityEngine;

[Serializable]
public class SpriteScriptableObject : ScriptableObject
{
	public virtual Sprite[] sprites { get; set; }
}
