using UnityEngine;

public enum SoundChannel
{
    Sfx,
    Music,
}

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudioClip(SoundChannel soundChannel, AudioClip clip)
    {
        AudioSource selectedSource;
        if (soundChannel == SoundChannel.Music)
        {
            selectedSource = musicSource;
        } else {
            selectedSource = sfxSource;
        }
        
        selectedSource.clip = clip;
        selectedSource.Play();
    }
}