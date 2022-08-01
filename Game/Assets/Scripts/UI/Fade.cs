using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private Image image;

    public static Fade instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        image = GetComponentInChildren<Image>();
        FadeIn(0.25f);
    }
    

    public void FadeIn(float duration)
    {
        StartCoroutine(FadeAnimation(duration, true));
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(FadeAnimation(duration, false));
    }

    private IEnumerator FadeAnimation(float duration, bool isIn)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;

            float t = isIn ? 1 - time / duration : time / duration;
            t = Mathf.Clamp01(t);
            float a = Tweens.Smoothstep(t);

            image.color = new Color(0, 0, 0, a);
            yield return null;
        }

    }
}
