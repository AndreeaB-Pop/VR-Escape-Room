using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    [SerializeField] GameObject sceneChanger;
    ChangeScene changeScene;

    private void Start()
    {
        sceneChanger = GetComponent<GameObject>();
        if (sceneChanger != null ) changeScene = sceneChanger.GetComponent<ChangeScene>();
    }

    // This is just to kill the player.
    private void OnTriggerEnter(Collider other)
    {
        string activeScene = SceneManager.GetActiveScene().name;
        StartCoroutine(changeScene.FadeSceneChange(activeScene));
    }
}
