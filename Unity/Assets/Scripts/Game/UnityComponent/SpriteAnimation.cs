using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationBehavior", menuName = "ScriptableObjects/SpriteAnimation")]
[Serializable]
public class SpriteAnimation :  SpriteScriptableObject
{
	[SerializeField]
	private Sprite[] spriteList;
	public override Sprite[] sprites { get => spriteList; set { spriteList = value; } }
}

