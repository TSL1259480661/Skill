using UnityEditor;

public class TextureImport : AssetPostprocessor
{
	private void OnPreprocessTexture()
	{
		TextureImporter importer = assetImporter as TextureImporter;
		if (importer.assetPath.Contains("Assets/Bundles/Effect"))
		{
			var setting = importer.GetPlatformTextureSettings("WebGL");
			setting.compressionQuality = 50;
			setting.format = TextureImporterFormat.ASTC_8x8;

			CheckNameSize(importer.assetPath, 512, setting);

			setting.textureCompression = TextureImporterCompression.CompressedLQ;
			setting.overridden = true;
			importer.SetPlatformTextureSettings(setting);
		}
		else if (importer.assetPath.Contains("Assets/Bundles/Units"))
		{
			var setting = importer.GetPlatformTextureSettings("WebGL");
			setting.compressionQuality = 50;
			setting.format = TextureImporterFormat.ASTC_8x8;
			CheckNameSize(importer.assetPath, 256, setting);
			setting.textureCompression = TextureImporterCompression.CompressedLQ;
			setting.overridden = true;
			importer.SetPlatformTextureSettings(setting);
		}
		else if (importer.assetPath.Contains("Assets/Bundles/Atlas") ||
				importer.assetPath.Contains("Assets/Bundles/Texture2D"))
		{
			var setting = importer.GetPlatformTextureSettings("WebGL");
			setting.compressionQuality = 50;
			setting.format = TextureImporterFormat.ASTC_6x6;
			CheckNameSize(importer.assetPath, 1024, setting);
			setting.textureCompression = TextureImporterCompression.CompressedLQ;
			setting.overridden = true;
			importer.SetPlatformTextureSettings(setting);
		}
	}

	private void CheckNameSize(string assetPath, int defaultSize, TextureImporterPlatformSettings settings)
	{
		//if (assetPath.Contains("512"))
		//{
		//	settings.maxTextureSize = 512;
		//}
		//else if (assetPath.Contains("256"))
		//{
		//	settings.maxTextureSize = 256;
		//}
		//else if (assetPath.Contains("128"))
		//{
		//	settings.maxTextureSize = 128;
		//}
		//else if (assetPath.Contains("1024"))
		//{
			settings.maxTextureSize = 1024;
		//}
		//else if (assetPath.Contains("2048"))
		//{
		//	settings.maxTextureSize = 2048;
		//}
		//else
		//{
		//	if (settings.maxTextureSize > defaultSize)
		//	{
		//		settings.maxTextureSize = defaultSize;
		//	}
		//}
	}
}
