using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private int hits = 0;

    public static UIManager Instance;

    private Text text;
    private GameObject gameWon;
    private GameObject settings;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        text = transform.Find("Hits").GetComponent<Text>();
        gameWon = transform.Find("GameWon").gameObject;
        settings = transform.Find("Settings").gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Ball.instance.IsActive())
            {
                Ball.instance.StopAiming();
            }
            else
            {
                ShowSettings();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void AddStroke()
    {
        ++hits;
        text.text = "Hits: " + hits;
    }

    public void ShowSettings()
    {
        SoundManager.instance.PlayButtonClicked();
        bool active = settings.activeSelf;
        Ball.instance.StopAiming();

        if (!active)
            Ball.instance.SetInteractable(1);
        else
            StartCoroutine(UnlockControls(.1f));

        Tooltip.instance.Hide();

        settings.SetActive(!active);
    }

    public void ButtonHover()
    {
        SoundManager.instance.PlayButtonHover();
    }

    public void SettingHover()
    {
        ButtonHover();
        Ball.instance.SetInteractable(1);
        Ball.instance.StopAiming();
    }

    public void SettingEndHover()
    {
        Ball.instance.StopAiming();
        Ball.instance.SetInteractable(-1);
    }

    public void DrawWin()
    {
        StartCoroutine(ShowGameWon());
    }

    private IEnumerator ShowGameWon()
    {
        SoundManager.instance.PlayFanfare();

        int level = SceneManager.GetActiveScene().buildIndex - 1;

        gameWon.SetActive(true);
        gameWon.transform.Find("Hits").GetComponent<Text>().text = "Hits: " + hits;
        if (!Save.Instance.FinishLevel(level, hits))
        {
            gameWon.transform.Find("HighScore").GetComponent<Text>().text =
                "HighScore: " + Save.Instance.GetHighScore(level);
        }


        const float maxTime = 0.4f;

        var cg = gameWon.GetComponent<CanvasGroup>();

        float time = 0f;
        while (time < maxTime)
        {
            cg.alpha = time / maxTime;
            time += Time.deltaTime;
            yield return null;
        }

        cg.interactable = true;
    }

    private IEnumerator UnlockControls(float duration)
    {
        yield return new WaitForSeconds(duration);
        Ball.instance.SetInteractable(-1);
    }

}
