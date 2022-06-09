using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : Singleton<SoundPlayer>
{
	AudioSource[] audioSources = new AudioSource[80];
	Dictionary<string, AudioClip> soundCache = new Dictionary<string, AudioClip>();

	protected override void Init()
	{
		// AudioSource �߰�
		for (int i = 0; i < audioSources.Length; i++)
		{
			GameObject source = new GameObject("AudioSource");
			source.AddComponent<AudioSource>();
			source.transform.SetParent(transform);
			audioSources[i] = source.GetComponent<AudioSource>();
		}
	}


	public void PlaySound2D(string soundName)
	{
		AudioSource source = FindEmptyAudioSource();
		if (source == null)
			return;

		source.spatialBlend = 0.0f;
		source.volume = GameData.instance.sfxVolume;

		PlaySound(soundName, source);
	}


	public void PlaySound3D(string soundName, Vector3 position)
	{
		AudioSource source = FindEmptyAudioSource();
		if (source == null)
			return;

		source.spatialBlend = 1.0f;
		source.volume = GameData.instance.sfxVolume;
		source.transform.position = position;

		PlaySound(soundName, source);
	}


	private void PlaySound(string soundName, AudioSource source)
	{
		if (source == null || soundName.Length <= 0)
			return;

		// SoundCache���� ���带 ã��
		string path = $"Sounds/{soundName}";
		AudioClip clip = null;
		if (!soundCache.ContainsKey(soundName))
		{
			// - ���尡 ĳ�ÿ� ���ٸ� ĳ�ÿ� ���带 �ø�
			clip = Resources.Load<AudioClip>(path);
			if (clip != null)
				soundCache.Add(soundName, clip);
		}

		clip = soundCache[soundName];
		source.clip = clip;
		source.Play();
	}


	// ����ִ�(������� �ƴ�) AudioSource�� ã�� ��ȯ�ϴ� �Լ�
	private AudioSource FindEmptyAudioSource()
	{
		for (int i = 0; i < audioSources.Length; i++)
		{
			if (audioSources[i].isPlaying == false)
				return audioSources[i];
		}
		return null;
	}
}
