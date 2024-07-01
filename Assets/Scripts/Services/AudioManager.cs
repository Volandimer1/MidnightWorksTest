using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSourceSFXPrefab;
    [SerializeField] private AudioSource _audioSourceMusic;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioClipsBundle _audiosBundle;

    [HideInInspector] public Dictionary<AudioClipsEnum, SoundClipVolumePair> EnumClipsDictionary = new Dictionary<AudioClipsEnum, SoundClipVolumePair>();
    [HideInInspector] public Dictionary<AudioClipsEnum, SoundClipVolumePair> EnumMusicDictionary = new Dictionary<AudioClipsEnum, SoundClipVolumePair>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (_audiosBundle != null)
        {
            LoadEnumClipsPairDictionaryFromBundle(_audiosBundle);
        }
    }

    public void LoadEnumClipsPairDictionaryFromBundle(AudioClipsBundle audiosBundle)
    {
        for (int i = 0; i < audiosBundle.Items.Count; i++)
        {
            SoundClipVolumePair pair = new SoundClipVolumePair();
            pair.Clip = audiosBundle.Items[i].Clip;
            pair.Volume = audiosBundle.Items[i].Volume;
            EnumClipsDictionary.Add(audiosBundle.Items[i].AudioClipsEnum, pair);
        }
    }

    public void LoadEnumMusicPairDictionaryFromBundle(AudioClipsBundle audiosBundle)
    {
        for (int i = 0; i < audiosBundle.Items.Count; i++)
        {
            SoundClipVolumePair pair = new SoundClipVolumePair();
            pair.Clip = audiosBundle.Items[i].Clip;
            pair.Volume = audiosBundle.Items[i].Volume;
            EnumMusicDictionary.Add(audiosBundle.Items[i].AudioClipsEnum, pair);
        }
    }

    public void ReleaseSoundClips()
    {
        EnumClipsDictionary.Clear();
    }

    public void ReleaseMusicClips()
    {
        EnumMusicDictionary.Clear();
    }

    public void StopMusic()
    {
        _audioSourceMusic.Stop();
    }

    public void PlayMusic(AudioClipsEnum audioEnum, bool isLoop)
    {
        if (EnumMusicDictionary.ContainsKey(audioEnum) == false)
        {
            Debug.LogWarning($"There was no such key {audioEnum} in EnumMusicDictionary");
            return;
        }

        _audioSourceMusic.Stop();
        _audioSourceMusic.loop = isLoop;
        _audioSourceMusic.clip = EnumMusicDictionary[audioEnum].Clip;
        _audioSourceMusic.volume = EnumMusicDictionary[audioEnum].Volume;
        _audioSourceMusic.Play();
    }

    public AudioSource PlaySFX(AudioClipsEnum audioEnum, Vector3 spawnPosition)
    {
        if (EnumClipsDictionary.ContainsKey(audioEnum) == false)
        {
            Debug.LogWarning($"There was no such key {audioEnum} in EnumClipsDictionary");
            return null;
        }

        AudioSource audioSource = Instantiate(_audioSourceSFXPrefab, spawnPosition, Quaternion.identity);
        audioSource.clip = EnumClipsDictionary[audioEnum].Clip;
        audioSource.volume = EnumClipsDictionary[audioEnum].Volume;
        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
        return audioSource;
    }

    public void PlaySFXWithDelay(AudioClipsEnum audioEnum, Vector3 spawnPosition, float delayInSeconds)
    {
        if (EnumClipsDictionary.ContainsKey(audioEnum) == false)
        {
            Debug.LogWarning($"There was no such key {audioEnum} in EnumClipsDictionary");
            return;
        }

        StartCoroutine(InstantiateWithDelay(EnumClipsDictionary[audioEnum].Clip, EnumClipsDictionary[audioEnum].Volume, spawnPosition, delayInSeconds));
    }

    private IEnumerator InstantiateWithDelay(AudioClip audioClip, float volume, Vector3 spawnPosition, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds); // Wait for the specified delay

        AudioSource audioSource = Instantiate(_audioSourceSFXPrefab, spawnPosition, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    public void SetMusicVolume(float value)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 30f);
    }

    public void SetSFXVolume(float value)
    {
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 30f);
    }
}