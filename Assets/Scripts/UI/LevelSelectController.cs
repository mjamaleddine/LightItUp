using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{

    public GameObject loadingImage;
    public Slider loadingBar;

    private AsyncOperation _async;

    public void LoadSceneAsync(int level)
    {
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }

    IEnumerator LoadLevelWithBar(int level)
    {
        _async = Application.LoadLevelAsync(level);
        while (!_async.isDone)
        {
            loadingBar.value = _async.progress;
            yield return null;
        }
    }

    public void LoadScene(int level)
    {
        loadingImage.SetActive(true);
        Application.LoadLevel(level);
    }

}
