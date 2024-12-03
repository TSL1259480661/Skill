
public interface ISound
{
	public int SoundId { get; set; }
	//public string SoundName { get; set; }

	public void InitSound(AudioManager audioManager);
	public void PlaySound();
}

/// <summary>
/// UI中可置灰的接口
/// </summary>
public interface IGray
{
	public bool IsGray { get; set; }
}
