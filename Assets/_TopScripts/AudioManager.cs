using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("- - - Audio Source - - -")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("- - - Audio Clip - - -")]
    public AudioClip backgroundMusic;
    public AudioClip jumpSFX;
    public AudioClip slideSFX;

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
