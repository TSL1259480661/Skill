using System;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "AnimationBehaviorAltas", menuName = "ScriptableObjects/AltasAnimation")]
[Serializable]
public class SpriteAnimationAltas : SpriteScriptableObject
{
	[SerializeField]
	private SpriteAtlas atlas;
	private Sprite[] spriteList;

	public override  Sprite[] sprites
	{
		get
		{
			if ((spriteList == null || spriteList!=null && (spriteList.Length == 0 || (spriteList.Length != 0 && spriteList[0] == null)))&& atlas != null)
			{
				spriteList = new Sprite[atlas.spriteCount];
				atlas.GetSprites(spriteList);
			    Sort();
			}
			return spriteList;
		}
	}

	private void Sort()
	{
		for(int i =0;i<spriteList.Length;i++)
		{
			for(int j = 0;j<spriteList.Length - i - 1; j++)
			{
				if (spriteList[j].name.CompareTo(spriteList[j+1].name) == 1)
				{
					var s = spriteList[j];
					spriteList[j] = spriteList[j + 1];
					spriteList[j + 1] = s;
				}
			}
		}
	}

	public SpriteAtlas spriteAtlas
	{
		set 
		{
			atlas = value;
		}
	}
}
