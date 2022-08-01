using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Tooltip("Amount of possible particles effects playing at the same time")]
    [SerializeField] private int poolSize = 5;

    [SerializeField] private AudioClip bounce;
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioClip buttonHover;
    [SerializeField] private AudioClip fanfare;
    [SerializeField] private AudioClip boost;

    private int index;
    private AudioSource[] audioSources;

    public static SoundManager instance;

    private void Awake()
    {
        if (FindObjectsOfType<SoundManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;

        for (int i = 0; i < poolSize - 1; i++)
            Instantiate(child, Vector3.zero, Quaternion.identity, transform);


        audioSources = GetComponentsInChildren<AudioSource>();
    }


    // Uses a pool of audio sources
    private AudioSource GetAudioSource()
    {
        ++index;
        index %= audioSources.Length;
        return audioSources[index];
    }

    public void PlayBounce(float speed)
    {
        speed = RemapSpeed(speed);

        AudioSource audioSource = GetAudioSource();

        audioSource.volume = speed * Save.Instance.data.sfxVolume;
        audioSource.PlayOneShot(bounce);
    }

    public void PlayButtonClicked()
    {
        Play(buttonClick);
    } 
    
    public void PlayFanfare()
    {
        Play(fanfare);
    }

    public void PlayButtonHover()
    {
        Play(buttonHover, 0.6f);
    }

    public void PlayBoost()
    {
        Play(boost, 0.05f);
    }

    public void Play(AudioClip clip, float volume = 1f)
    {
        AudioSource audioSource = GetAudioSource();

        audioSource.volume = Save.Instance.data.sfxVolume * volume;
        audioSource.PlayOneShot(clip);
    }

    

    private static float RemapSpeed(float speed)
    {
        return 2 / (1 + Mathf.Exp(-speed * 0.5f)) - 1; // sigmoid remapping to (0,1) for volume
    }
}
