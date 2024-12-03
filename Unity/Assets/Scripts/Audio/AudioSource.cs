using App;
using System;
using System.Collections.Generic;
using System.IO;
using UIEngine;
using UnityEngine;

public class AudioSource : IObjectPoolItem
{
	private static UObjectPool<AudioSource> pool = new UObjectPool<AudioSource>(100, OnCreate);
	public static AudioSource Get()
	{
		return pool.Get();
	}
	public void Recycle()
	{
		pool.Recycle(this);
	}

	private static AudioSource OnCreate()
	{
		AudioSource source = new AudioSource();
		source.InitAudioSource(rootGo);
		return source;
	}

	private static GameObject rootGo;
	public static void PreInit(int count, GameObject go)
	{
		rootGo = go;
		pool.InCrease(count);
	}

	private static int _id;

	private UnityEngine.AudioSource audio;
	private IUILoadAsset loadAsset;
	private string assetPath;
	private bool isOneShot;
	private float volume;

	public Action<AudioSource> onRecycle;

	public int id { private set; get; }
	public void OnReuse()
	{
		id = ++_id;
		volume = 100;
		if (audio != null)
		{
			audio.enabled = true;
		}
	}

	public void OnRecycle()
	{
		onRecycle?.Invoke(this);
		onRecycle = null;

		Clear();

		//UnscaledTimer.Instance.Remove(this.completeTimeId);
		//completeTimeId = 0;

		bindTransform = null;

		if (audio != null)
		{
			audio.transform.position = Vector3.zero;
			audio.enabled = false;
		}

		this.assetPath = null;
		this.loadAsset = null;
		onPlayDone = null;
	}

	private string name;
	private Action<int> onPlayDone;
	public void Init(IUILoadAsset loadAsset, GameObject rootGo, bool isOneShot, bool loop, Action<int> onPlayDone = null)
	{
		this.loadAsset = loadAsset;
		this.isOneShot = isOneShot;
		this.onPlayDone = onPlayDone;

		InitAudioSource(rootGo);

		audio.loop = loop;
	}

	private void InitAudioSource(GameObject rootGo)
	{
		if (audio == null)
		{
			GameObject go = new GameObject();
			audio = go.AddComponent<UnityEngine.AudioSource>();
			audio.name = "AudioSource";

			if (rootGo != null && audio.transform.parent != rootGo.transform)
			{
#if UNITY_EDITOR
				audio.transform.SetParent(rootGo.transform);
				audio.transform.SetAsFirstSibling();
#endif
			}
		}
	}

	public void SetName(string name)
	{
#if UNITY_EDITOR
		this.name = name;
		audio.name = name;
#endif
	}

	public void AddName(string add)
	{
#if UNITY_EDITOR
		audio.name = name + add;
#endif
	}

	private AudioClip clip = null;
	private IUILoadAssetItem loadItem = null;

	public void Play(string assetPath, float spatialBlend = 0f, Transform bindTransform = null)
	{
#if UNITY_EDITOR
		AddName(id + "_" + Path.GetFileNameWithoutExtension(assetPath));
#endif

		if (!string.IsNullOrEmpty(assetPath) && assetPath != this.assetPath)
		{
			loadItem = loadAsset.LoadAsset(assetPath, UIAssetType.Resource, OnLoadDone);
		}
		else
		{
			DoPlay(clip);
		}

		this.assetPath = assetPath;

		audio.spatialBlend = spatialBlend;
		audio.rolloffMode = AudioRolloffMode.Linear;
		audio.minDistance = 0f;
		audio.maxDistance = 15f;
		SetVolumeSize(volume);

		Bind(bindTransform);
	}

	public void Play(AudioClip clip, bool loop, float spatialBlend = 0f, Transform bindTransform = null)
	{
		if (clip != null)
		{
			audio.loop = loop;
			DoPlay(clip);

			audio.spatialBlend = spatialBlend;
			audio.rolloffMode = AudioRolloffMode.Linear;
			audio.minDistance = 0f;
			audio.maxDistance = 15f;
			SetVolumeSize(volume);

			Bind(bindTransform);
		}
	}

	public void Clear()
	{
		clip?.UnloadAudioData();
		clip = null;

		loadItem?.Recycle();
		loadItem = null;
	}

	private Transform bindTransform;
	public void Bind(Transform bindTransform)
	{
		this.bindTransform = bindTransform;

		Update();
	}

	public void Update()
	{
		if (audio != null && bindTransform != null)
		{
			audio.transform.position = bindTransform.position;
		}
	}

	private void DoPlay(AudioClip clip)
	{
		if (clip == null) return;
		if (isOneShot)
		{
			this.audio.PlayOneShot(clip);
			//UnscaledTimer.Instance.Remove(this.completeTimeId);
			//this.completeTimeId = UnscaledTimer.Instance.Add(clip.length, 1, (timeID, paramList) =>
			//{
			//	if (isOneShot && timeID == this.completeTimeId)
			//	{
			//		onPlayDone?.Invoke(id);

			//		LoadAssetItemRecycle();
			//		this.Recycle();
			//	}
			//	else
			//	{
			//		Debug.LogError("异常情况");
			//	}

			//});
		}
		else
		{
			this.audio.clip = clip;
			this.audio.Play();
		}

	}

	public void Pause()
	{
		this.audio.Pause();
	}

	public void UnPause()
	{
		this.audio.UnPause();
	}

	public void Stop()
	{
		this.audio.Stop();
	}

	public void SetVolumeSize(float size)
	{
		volume = size;
		this.audio.volume = volume;
	}

	private void OnLoadDone(IUILoadAssetItem assetItem)
	{
		AudioClip clip = assetItem.content as AudioClip;
		this.clip = clip;
		DoPlay(clip);
	}

	public bool IsPlaying()
	{
		return audio.isPlaying;
	}
}
