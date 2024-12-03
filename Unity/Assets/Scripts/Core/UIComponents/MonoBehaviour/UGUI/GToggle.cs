using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GToggle : Toggle, ISound
{
	[SerializeField]
	private int soundId;
	public int SoundId { get => soundId; set => soundId = value; }

	private AudioManager audioManager;

	/// <summary>
	/// 初始化音效
	/// </summary>
	/// <param name="audioManager"></param>
	public void InitSound(AudioManager audioManager)
	{
		this.audioManager = audioManager;
		this.onValueChanged.RemoveListener(OnClickToogle);
		this.onValueChanged.AddListener(OnClickToogle);
	}

	public void OnClickToogle(bool on)
	{
		if (on)
			PlaySound();
	}

	/// <summary>
	/// 播放音效
	/// </summary>
	public void PlaySound()
	{
	
	}
}
