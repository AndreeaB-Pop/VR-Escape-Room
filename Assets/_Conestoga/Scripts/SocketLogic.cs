using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SocketLogic : MonoBehaviour
{
    public static SocketLogic singleton = null;

    ChangeScene changeScene;

    [Header("Door Spawns")]
    public GameObject door, vent, doorLock, boxes;

    [Header("Lights")]
    public Light roomLight1, roomLight2, roomLight3;

    public bool elevatorPuzzle;

    int rightSocketsFilled = 0;
    [SerializeField] int rightSocketsMax = 2;
    [SerializeField] int elevatorConnectionEscape = 3;

    [SerializeField] bool correctSocket = false;

    string activeScene;

    // Set singleton
    private void Awake()
    {
        if (singleton)
        {
            Debug.LogWarning("Already have a Socket Logic singleton");
            Destroy(this);  // already have one
            return;
        }
        // we're the first (and only!)
        singleton = this;
    }

    private void Start()
    {
        activeScene = SceneManager.GetActiveScene().name;
        changeScene = GetComponent<ChangeScene>();
    }

    // This is so so messy and needs to be redone to not have a bajillion variations on the same function. Use case switches or what not.
    public void AddSocketIn()
    {
        print("add one socket inside");
        rightSocketsFilled++;
        if (!elevatorPuzzle)
        {
            CheckSocketsCorrect();
        }
        else
        {
            CheckSocketsElevator();
        }
    }

    public static void AddSocketInside() => singleton?.AddSocketIn();

    public void RemoveSocketOut()
    {
        print("remove socket");
        if (rightSocketsFilled > 0) rightSocketsFilled--;
    }

    public static void RemoveSocket() => singleton?.RemoveSocketOut();

    public void CheckSocketsCorrect()
    {
        if (rightSocketsFilled == rightSocketsMax)
        {
            Debug.Log("yay correct password");

            // Door Logic Active
            door.SetActive(false);
            vent.SetActive(false);
            doorLock.SetActive(false);
            boxes.SetActive(true);

            // Lights on
            roomLight1.enabled = true;
            roomLight2.enabled = true;
            roomLight3.enabled = true;
            rightSocketsFilled = 0;
        }
        else
        {
            Debug.Log("aww wrong match");
        }
    }

    public void CheckSocketsElevator()
    {
        if (rightSocketsFilled == elevatorConnectionEscape)
        {
            Debug.Log("yay correct combo for elevator");
            
            StartCoroutine(FadeSceneChange(activeScene));
        }
        else
        {
            Debug.Log("aww wrong match for elevator");
        }
    }

    public IEnumerator FadeSceneChange(string sceneName)
    {
        Fader.FadeOut();
        while (Fader.isFading) yield return null; // wait until fade is complete
        SceneManager.LoadScene(sceneName);
    }

}
