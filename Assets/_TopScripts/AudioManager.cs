using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("- - - Audio Source - - -")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("- - - Audio Clip - - -")]
    public AudioClip backgroundMusic;
    public AudioClip jumpSFX;
    public AudioClip slideSFX;
    public AudioClip swichtLaneSFX;
    public AudioClip dieSFX;
    public AudioClip afterJumpSFX;
    public AudioClip breakWallSFX;

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
    }

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
