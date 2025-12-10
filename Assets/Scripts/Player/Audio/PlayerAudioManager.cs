using Unity.VisualScripting;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource weaponSfxAudioSource;

    public void PlayClip(AudioClip clip) 
    {
                weaponSfxAudioSource.PlayOneShot(clip);
        
    }
}
