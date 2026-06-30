using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos, float volume = 1f)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, volume);
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        SoundEffect sfx = sfxLibrary.GetSoundEffect(soundName);
        if (sfx.clips != null && sfx.clips.Length > 0)
        {
            AudioClip clip = sfx.clips[Random.Range(0, sfx.clips.Length)];
            PlaySound3D(clip, pos, sfx.volume);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] Sound '{soundName}' tidak ditemukan di SoundLibrary.");
        }
    }

    public void PlaySound2D(string soundName)
    {
        SoundEffect sfx = sfxLibrary.GetSoundEffect(soundName);
        if (sfx.clips != null && sfx.clips.Length > 0)
        {
            AudioClip clip = sfx.clips[Random.Range(0, sfx.clips.Length)];
            sfx2DSource.PlayOneShot(clip, sfx.volume);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] Sound '{soundName}' tidak ditemukan di SoundLibrary.");
        }
    }
}