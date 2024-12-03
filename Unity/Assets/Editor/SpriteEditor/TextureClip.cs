using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 用作裁剪图片，并使偏移保持不变
/// </summary>
public class TextureClip
{
	private static int allSprite = 0;
	private static int loadedSprite = 0;
	public static void SetTextureParam(Texture2D texture, TextureImporter textureImporter, string path, int type)
	{
		StartClip(texture, textureImporter, path, type);
	}

	private static void Refresh()
	{
		Debug.Log($"需要加载数{allSprite},已加载数{loadedSprite}");
	}

	private static void StartClip(Texture2D texture, TextureImporter textureImporter, string path, int type)
	{

		if (type == 1)
		{
			Refresh();
			allSprite++;
			textureImporter.isReadable = true;//设置成可读写
			textureImporter.SaveAndReimport();
			return;
		}
		if (type == 2)
		{
			int oldWidth = texture.width;
			int oldHeight = texture.height;
			int up, down, left, right;
			if(!Check(texture, out up, out down, out left, out right))
			{
				Debug.Log("大小边界为1024不裁剪");
				return;
			}
			Texture2D t = Copy(up, down, left, right, texture);
			Save(t, path);
			Revert(path, up, down, left, right, oldWidth, oldHeight);
			loadedSprite++;
			ShowProgres.ShowPro(allSprite,loadedSprite);
			Refresh();
		}
		if (type == 3)
		{
			EndClip(texture, AssetImporter.GetAtPath(path) as TextureImporter);
		}

	}

	private static void Revert(string path, int up, int down, int left, int right, int oldWidth, int oldHeight)
	{
		TextureImporter assetImporter = AssetImporter.GetAtPath(path) as TextureImporter;
		int newWidth = right - left + 1;
		int newHeight = up - down + 1;
		int offsetW = (oldWidth / 2) - (newWidth / 2) - left;
		int offsetH = (oldHeight / 2) - (newHeight / 2) - down;

		float w = assetImporter.spritePivot.x + 1.0f * offsetW / newWidth;
		float h = assetImporter.spritePivot.y + 1.0f * offsetH / newHeight;

		TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
		assetImporter.ReadTextureSettings(textureImporterSettings);
		textureImporterSettings.spriteAlignment = (int)SpriteAlignment.Custom;
		textureImporterSettings.spritePivot = new Vector2(w, h);
		assetImporter.SetTextureSettings(textureImporterSettings);
	}

	private static void Save(Texture2D t, string path)
	{
		byte[] data = t.EncodeToPNG();
		FileStream fileStream = File.Open(path, FileMode.OpenOrCreate);
		fileStream.Write(data, 0, data.Length);
		fileStream.Close();
	}

	private static Texture2D Copy(int up, int down, int left, int right, Texture2D t)
	{
		int width = right - left + 1;
		int height = up - down + 1;
		Texture2D texture = new Texture2D(width, height);
		texture.alphaIsTransparency = true;
		var colors = t.GetPixels(left, down, width, height);
		texture.SetPixels(colors);
		texture.Apply();
		return texture;
	}


	private static bool Check(Texture2D texture, out int up, out int down, out int left, out int right)
	{
		up = right = -1;
		left = texture.width + 1;
		down = texture.height + 1;
		float width = texture.width;
		float height = texture.height;
		bool f = false;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				var item = texture.GetPixel(i, j);
				if (item.a >= 0.1f)
				{
					f = true;
					up = Mathf.Max(up, j);
					down = Mathf.Min(down, j);
					left = Mathf.Min(left, i);
					right = Mathf.Max(right, i);
				}
			}
		}
		if (!f)
		{
			up = Mathf.Max(up, 0);
			right = Mathf.Max(right, 0);
			left = 0;
			down = 0;
		}
		bool f1 = IsClip(up - down + 1);
		bool f2 = IsClip(right - left + 1);
		if (f1 && f2) return false;
		return true;
	}
	private static bool IsClip(int l)
	{
		int n = 2;
		while (l>=n) n *= 2;
		return n == 1024;
	}

	private static void EndClip(Texture2D texture, TextureImporter textureImporter)
	{
		textureImporter.isReadable = false;
		textureImporter.SaveAndReimport();
	}
}
