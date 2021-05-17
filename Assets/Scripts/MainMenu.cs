using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingSlider;

    public void PlayGame(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = progress;
            yield return null;
        }
    }
}
