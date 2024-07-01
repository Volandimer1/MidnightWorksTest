using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioClipsBundle : ScriptableObject
{
    public List<EnumSoundClipTriples> Items;
}

[System.Serializable]
public class EnumSoundClipTriples
{
    public AudioClipsEnum AudioClipsEnum;
    [Range(0, 1)] public float Volume = 1f;
    public AudioClip Clip;
}

[System.Serializable]
public class SoundClipVolumePair
{
    [Range(0, 1)] public float Volume = 1f;
    public AudioClip Clip;
}