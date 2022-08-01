using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject aimingCamera;


    public static Aiming instance;



    private void Awake()
    {
        instance = this;
    }

    public void SetAimingTarget(Transform target)
    {
        aimingCamera.GetComponent<CinemachineVirtualCamera>().Follow = target;
    }

    public void StartAiming()
    { 
        mainCamera.SetActive(false);
        aimingCamera.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(ChangeTimeScale(.4f, .5f));
    }

    public void StopAiming()
    {
        if (mainCamera.activeSelf)
            return;

        mainCamera.SetActive(true);
        aimingCamera.SetActive(false);

        StopAllCoroutines();
        StartCoroutine(ChangeTimeScale(1f, .5f));
    }

    // Slow down time when aiming
    private static IEnumerator ChangeTimeScale(float endValue, float duration)
    {
        float start = Time.timeScale;
        float t = 0;

        while (t <= duration)
        {
            
            t += Time.unscaledDeltaTime;
            t = Mathf.Min(t, duration);

            Time.timeScale = Mathf.Lerp(start, endValue, t / duration);

            yield return null;
        }
    }
}
