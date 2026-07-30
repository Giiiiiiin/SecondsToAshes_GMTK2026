using UnityEngine;
using UnityEngine.Serialization;

public class SoundPlayer : MonoBehaviour
{
    [FormerlySerializedAs("_soundClip")] [SerializeField] private AudioClip soundClip;
    [FormerlySerializedAs("_soundChannel")] [SerializeField] private SoundChannel soundChannel;
    [SerializeField] private bool startOnAwake = false;

    private void Start()
    {
        if (startOnAwake)
            PlaySound();
    }
    
    public void PlaySound()
    {
        SoundSystem.Instance.PlayAudioClip(soundChannel, soundClip);
    }

    public void EditSound(AudioClip newClip)
    {
        if (newClip == null) return;
        soundClip = newClip;
    }
}