using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public enum SoundType { Ai, Player}
public struct AudioRequest 
{
    public AudioClip clip;
    public int priority;
    public Vector3 pos;
    public float volume;
    public SoundType soundType;

    public AudioRequest(AudioClip clip, int priority, Vector3 position, float volume = 1f, SoundType type = SoundType.Ai)
    {
        this.clip = clip;
        this.priority = priority;
        this.pos = position;
        this.volume = volume;
        this.soundType = type;
    }
}


public class AudioManager : MonoBehaviour
{

    public static int MaxAiSounds = 8;


    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private AudioClip baseMusicClip;
    [SerializeField] private AudioClip battleMusicClip;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private int initialPoolSize = 8;

    private readonly List<AudioSource> _aiSources = new();
    private readonly List<(AudioSource source, int priority)> _activeAiSounds = new();
    private void Awake()
    {
        if(GameServices.AudioManager != this)
            GameServices.AudioManager = this;

        for (int i = 0; i < initialPoolSize; i++) 
        {
            var src = Instantiate(audioSourcePrefab, transform);
            src.playOnAwake = false;
            _aiSources.Add(src);
        }
    }
    private void OnDestroy()
    {
        if(GameServices.AudioManager == this)
            GameServices.AudioManager = null;
    }
    private void Update()
    {
        for (int i = _activeAiSounds.Count - 1; i >= 0; i--) 
        {
            if (!_activeAiSounds[i].source.isPlaying) 
            {
                _activeAiSounds.RemoveAt(i);
            }
        }
        if (transitionMusicDown) 
        {
            musicSource.volume = Mathf.Max(0f, musicSource.volume - musicTransitionSpeed * Time.deltaTime);

            if (musicSource.volume <= 0f) 
            {
                transitionMusicDown = false;
                transitionMusicUp = true;
                musicSource.clip = pendingClip;
            }
        }
        if (transitionMusicUp) 
        {
            musicSource.volume = Mathf.Min(1f, musicSource.volume + musicTransitionSpeed * Time.deltaTime);
            if (musicSource.volume >= 1f) 
            {
                transitionMusicUp = false;
            }
        }
    }
    [SerializeField] private float musicTransitionSpeed = 1f;
    bool transitionMusicDown = false;
    bool transitionMusicUp = false;
    AudioClip pendingClip = null;
    public void PlayMusicClip(AudioClip clip) 
    {
        pendingClip = clip;

        if (!musicSource.isPlaying)
        {
            musicSource.volume = 0f;
            musicSource.clip = pendingClip;
            musicSource.Play();
            pendingClip = null;
            transitionMusicDown = false;
            transitionMusicUp = true;
            return;
        }

        transitionMusicDown = true;
        transitionMusicUp = false;
    }
    public void SendRequest(AudioRequest r) 
    {
        if (r.clip == null) return;

        if (_activeAiSounds.Count < MaxAiSounds) 
        {
            PlayNewAiSound(r);
            return;
        }

        int lowestIndex = -1;
        int lowestPriority = int.MaxValue;

        for (int i = 0; i < _activeAiSounds.Count; i++) 
        {
            if (_activeAiSounds[i].priority < lowestPriority) 
            {
                lowestPriority = _activeAiSounds[i].priority;
                lowestIndex = i;
            }
        }
        if (r.priority <= lowestPriority)
            return;

        var entry = _activeAiSounds[lowestIndex];
        entry.source.Stop();
        _activeAiSounds.RemoveAt(lowestIndex);

        PlayAiSoundOnSource(entry.source, r);
    }
    private void PlayNewAiSound(AudioRequest r) 
    {
        AudioSource src = GetFreeAiSource();
        PlayAiSoundOnSource(src, r);
    }
    private void PlayAiSoundOnSource(AudioSource src, AudioRequest r) 
    {
        src.transform.position = r.pos;
        src.clip = r.clip;
        src.volume = r.volume;
        src.Play();

        _activeAiSounds.Add((src, r.priority));
    }
    private AudioSource GetFreeAiSource() 
    {
        foreach (var src in _aiSources) 
        {
            if (!src.isPlaying)
                return src;
        }
        var newSrc = Instantiate(audioSourcePrefab, transform);
        newSrc.playOnAwake = false;
        _aiSources.Add(newSrc);
        return newSrc;
    }
}
