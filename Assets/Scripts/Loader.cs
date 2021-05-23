using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public Animator animator;
    public Vector3 position;
    public VectorValue playerStorage;

    public void LoadLevel(int sceneIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ObeliskWork"))
        {
            playerStorage.initialValue = position;
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
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