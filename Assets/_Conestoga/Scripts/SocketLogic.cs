using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
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

    int rightSocketsFilled = 0;
    int rightSocketsMax;
    [SerializeField] int generatorPuzzleConnectionAmount = 2;
    [SerializeField] int elevatorPuzzleConnectionAmount = 3;

    [SerializeField]
    public enum Puzzle
    {
        Generator,
        Elevator
    }
    public Puzzle activeConnectorPuzzle;

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
        activeConnectorPuzzle = Puzzle.Generator;
    }

    private void Update()
    {
        CheckSocketsCorrect();
    }

    // This is so so messy and needs to be redone to not have a bajillion variations on the same function. Use case switches or what not.
    private void AddSocketIn()
    {
        rightSocketsFilled++;
        CheckSocketsCorrect();
    }

    public static void AddSocketInside() => singleton?.AddSocketIn();

    void RemoveSocketOut()
    {
        print("remove socket");
        if (rightSocketsFilled > 0) rightSocketsFilled--;

        // If somehow this goes under 0, fix back to 0.
        if (rightSocketsFilled < 0) rightSocketsFilled = 0;
    }

    public static void RemoveSocket() => singleton?.RemoveSocketOut();

    public void ResetSocketCount()
    {
        rightSocketsFilled = 0;
    }

    public void ChangePuzzleType(string newState)
    {
        activeConnectorPuzzle = (Puzzle)Enum.Parse(typeof(Puzzle), newState);
    }

    public void CheckSocketsCorrect()
    {
        switch (activeConnectorPuzzle)
        {
            case Puzzle.Generator:
                rightSocketsMax = generatorPuzzleConnectionAmount;
                break;
            case Puzzle.Elevator:
                rightSocketsMax = elevatorPuzzleConnectionAmount;
                break;
        }

        Debug.Log("the max of the puzzle amount is " + rightSocketsMax + " and the current case is " + activeConnectorPuzzle);

        if (rightSocketsFilled == rightSocketsMax)
        {
            switch (activeConnectorPuzzle)
            {
                case Puzzle.Generator:
                    Debug.Log("do generator resolution things");
                    // Enable lights
                    roomLight1.enabled = true;
                    roomLight2.enabled = true;
                    roomLight3.enabled = true;
                    break;
                case Puzzle.Elevator:
                    Debug.Log("do elevator resoltuion things");
                    break;
            }

            Debug.Log("yay correct password");

            ResetSocketCount();

            //// Door Logic Active
            //door.SetActive(false);
            //vent.SetActive(false);
            //doorLock.SetActive(false);
            //boxes.SetActive(true);

        }
        else
        {
            Debug.Log("aww wrong match");
        }
    }

    public IEnumerator FadeSceneChange(string sceneName)
    {
        Fader.FadeOut();
        while (Fader.isFading) yield return null; // wait until fade is complete
        SceneManager.LoadScene(sceneName);
    }

}
