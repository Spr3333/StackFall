using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource audioSource;
    public bool sound = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void SoundOnOFF()
    {
        sound = !sound;
    }

    public void PlaySoundFX(AudioClip clip, float volume)
    {
        if (sound)
            audioSource.PlayOneShot(clip, volume);
    }
}
