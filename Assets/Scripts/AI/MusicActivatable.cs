using UnityEngine;

public class MusicActivatable : MonoBehaviour, IActivatable
{
    [SerializeField] private AudioClip musicToPlay;
    public void Activate()
    {
        if(musicToPlay)
            GameServices.AudioManager.PlayMusicClip(musicToPlay);
    }
    public void Deactivate()
    {

    }
    public void Toggle()
    {

    }
}
