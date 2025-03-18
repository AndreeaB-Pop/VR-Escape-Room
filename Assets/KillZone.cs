using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    [SerializeField] ChangeScene changeScene;

    // This is just to kill the player.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            print("kill da player");
            string activeScene = SceneManager.GetActiveScene().name;
            StartCoroutine(changeScene.FadeSceneChange(activeScene));
        }
        
    }
}
