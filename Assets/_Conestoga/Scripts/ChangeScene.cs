using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    private void Start() => Fader.FadeIn();

    public void ChangeToSpecificScene(string sceneName)
    {
        StartCoroutine(FadeSceneChange(sceneName));
        //SceneManager.LoadScene(sceneName);
    }

    public IEnumerator FadeSceneChange(string sceneName)
    {
        Fader.FadeOut();
        while (Fader.isFading) yield return null; // wait until fade is complete
        SceneManager.LoadScene(sceneName);
    }
    public void ChangeToSpecificSceneNonFade(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
