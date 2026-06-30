using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] clips;
    [Range(0f, 10f)]
    public float volume;
}

public class SoundLibrary : MonoBehaviour
{
    public SoundEffect[] soundEffects;

    public SoundEffect GetSoundEffect(string name)
    {
        foreach (var soundEffect in soundEffects)
        {
            if (soundEffect.groupID == name)
            {
                return soundEffect;
            }
        }
        return default;
    }

    public AudioClip GetClipFromName(string name)
    {
        SoundEffect sfx = GetSoundEffect(name);
        if (sfx.clips != null && sfx.clips.Length > 0)
        {
            return sfx.clips[Random.Range(0, sfx.clips.Length)];
        }
        return null;
    }
}