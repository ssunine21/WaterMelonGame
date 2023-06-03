using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Init => _init;

	[SerializeField] private List<AudioList> audioLists;
	
	private static AudioManager _init;
	private void Awake()
	{
		if (_init == null)
		{
			_init = this;
		}
		else if (_init != this)
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);

		NewAudioSources();
	}

    private void Start()
    {
		SetOption();
    }

    private void NewAudioSources()
    {
		foreach(var audio in audioLists)
        {
			audio.SetAudioClip();
        }
    }

	public void Play(Definition.AudioType type)
    {
		foreach (var audio in audioLists)
		{
			if(audio.Type == type)
            {
				audio.Play();
				break;
            }
		}
	}

	public void Mute(bool flag)
    {
		foreach(var audio in audioLists)
        {
			audio.AudioSource.mute = flag;
        }
    }

	public void SetOption()
    {
		foreach (var audio in audioLists)
		{
			if (audio.Type == Definition.AudioType.Background)
				audio.AudioSource.volume = DataManager.init.gameData.isBGMVolum ? 1 : 0;
			else
				audio.AudioSource.volume = DataManager.init.gameData.isEffectVolum ? 1 : 0;
		}
	}

	[System.Serializable]
	public class AudioList
    {
		public Definition.AudioType Type => type;
		public AudioSource AudioSource => _audioSource;

		[SerializeField] private Definition.AudioType type;
		[SerializeField] private AudioClip _audioClip;

		private AudioSource _audioSource;

		public void SetAudioClip()
		{
			_audioSource = new GameObject("audioSource").AddComponent<AudioSource>();
			_audioSource.clip = _audioClip;
		}

		public void Play()
        {
			if (type == Definition.AudioType.Background)
			{
				_audioSource.loop = true;
				_audioSource.Play();
			}
            else
            {
				_audioSource.PlayOneShot(_audioClip);
            }
        }
    }
}
