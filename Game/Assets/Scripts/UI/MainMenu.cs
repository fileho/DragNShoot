using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Save data;
    private int maxLevel;

    private List<Button> levels;

    // Start is called before the first frame update
    void Start()
    {
        data = Save.Instance;

        EnableLevels();
    }

    private void EnableLevels()
    {
        Transform level = transform.Find("Levels");
        if (level == null)
            return;
        levels = new List<Button>();
        maxLevel = data.GetMaxLevelComplete();

        for (int i = 0; i < level.childCount; i++)
        {
            levels.Add(level.GetChild(i).GetComponent<Button>());
        }

        for (int i = 0; i < levels.Count; i++)
        {
            int local = i + 1;
            levels[i].onClick.AddListener(delegate { LoadLevel(local); });

            if (i > maxLevel)
            {
                levels[i].interactable = false;
                levels[i].GetComponent<EventTrigger>().enabled = false;
            }

        }
    }

    public void Menu()
    {
        LoadLevel(0);
    }

    public void Play()
    {
        LoadLevel(maxLevel + 1);
    }

    public void ResetData()
    {
        SoundManager.instance.PlayButtonClicked();
        data.ResetSave();
        EnableLevels();
    }

    public void Quit()
    {
        SoundManager.instance.PlayButtonClicked();
        Application.Quit();
    }

    public void ButtonHover()
    {
        SoundManager.instance.PlayButtonHover();
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
