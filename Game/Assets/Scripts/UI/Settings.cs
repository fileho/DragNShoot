using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickedSound;

    private Text aimMode;

    private UIManager uiManager;


    // Start is called before the first frame update
    void Start()
    {
        aimMode = transform.Find("Buttons").Find("AimMode").GetComponentInChildren<Text>();
        AimMode(true);

        var data = Save.Instance.data;

        Slider soundtrack = transform.Find("Soundtrack").GetComponent<Slider>();
        soundtrack.value = data.soundtrackVolume;
        soundtrack.onValueChanged.AddListener(SoundtrackChanged);
        Slider sfx = transform.Find("Sfx").GetComponent<Slider>();
        sfx.value = data.sfxVolume;
        sfx.onValueChanged.AddListener(SfxChanged);

        uiManager = GetComponentInParent<UIManager>();
    }


    public void AimModePressed()
    {
        SoundManager.instance.Play(buttonClickedSound);
        AimMode(false);
    }

    private void AimMode(bool init)
    {
        if (!init)
            Save.Instance.data.stickToBall = !Save.Instance.data.stickToBall;
        string text1 = "Stick to Ball";
        string text2 = "Free Aim";
        aimMode.text = Save.Instance.data.stickToBall ? text1 : text2;
    }

    public void Menu()
    {
        LoadLevel(0);
    }

    public void Restart()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void ClosePressed()
    {
        SoundManager.instance.Play(buttonClickedSound);
        uiManager.ShowSettings();
    }

    private void SfxChanged(float value)
    {
        Save.Instance.data.sfxVolume = value;
    }

    private void SoundtrackChanged(float value)
    {
        Save.Instance.data.soundtrackVolume = value;
        Soundtrack.instance.VolumeChanged();
    }

    private void LoadLevel(int level)
    {
        SoundManager.instance.PlayButtonClicked();
        StartCoroutine(LoadScene(level));
    }

    private static IEnumerator LoadScene(int index)
    {
        float duration = 0.25f;
        Fade.instance.FadeOut(duration);

        yield return new WaitForSeconds(duration);
        SceneManager.LoadSceneAsync(index);
    }
}
