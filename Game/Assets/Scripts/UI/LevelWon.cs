using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWon : MonoBehaviour
{
    public void Menu()
    {
        LoadLevel(0);
    }

    public void NextLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
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
        SceneManager.LoadScene(index);
    }
}
