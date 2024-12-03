using UnityEditor;
using UnityEngine;

public class AudioImport : AssetPostprocessor
{
	private void OnPreprocessAudio()
	{
		AudioImporter importer = assetImporter as AudioImporter;
		var setting = importer.GetOverrideSampleSettings("WebGL");
		setting.loadType = AudioClipLoadType.DecompressOnLoad;
		//importer.forceToMono = false;
		setting.quality = .8f;
		setting.compressionFormat = AudioCompressionFormat.AAC;
		setting.preloadAudioData = true;
		importer.SetOverrideSampleSettings("WebGL", setting);
	}
}
