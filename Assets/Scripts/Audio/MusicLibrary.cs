using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip clip;
    [Range(0f, 10f)]
    public float volume;
}

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] tracks;

    public MusicTrack GetTrack(string trackName)
    {
        foreach (var track in tracks)
        {
            if (track.trackName == trackName)
            {
                return track;
            }
        }
        return default;
    }

    public AudioClip GetClipFromName(string trackName)
    {
        return GetTrack(trackName).clip;
    }
}