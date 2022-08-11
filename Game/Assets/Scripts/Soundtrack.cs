using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Soundtrack : MonoBehaviour
{
    public static Soundtrack instance;

    private AudioSource audioSource;

    private void Awake()
    {
        var tracks = FindObjectsOfType<Soundtrack>();

        if (tracks.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);

        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        VolumeChanged();
        audioSource.Play();
    }

    public void VolumeChanged()
    {
        audioSource.volume = Save.Instance.SoundtrackVolume() * 0.5f;
    }


}
