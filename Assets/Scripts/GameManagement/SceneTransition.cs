using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void LoadNextScene(string sceneName) => StartCoroutine(LoadSpecifiedScene(sceneName));

    IEnumerator LoadSpecifiedScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitForSeconds(1);
        while (!asyncLoad.isDone) { yield return null; }
    }
}
