using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// It's called socket logic but it handles most of the game logic overall.
/// </summary>
public class SocketLogic : MonoBehaviour
{
    public static SocketLogic singleton = null;

    [Header("Enabling Systems")]
    [SerializeField] ParticleSystem[] particleSystems;
    [SerializeField] AudioSource[] fireSources;

    public enum GameDifficulty
    {
        Easy,
        Hard
    }
    [Header("Difficulty")]
    public GameDifficulty gameDifficulty;

    // These objects will be enabled depending on difficulty
    [Header("Difficulty Objects")]
    [SerializeField]
    GameObject[] easyObjects;
    [SerializeField]
    GameObject[] hardObjects;

    [Header("Elevator Items")]
    [SerializeField] PlayableDirector elevatorPlayableDirector;
    [SerializeField] MeshRenderer elevatorEmission;
    Material elevatorEmissionMaterial;
    [SerializeField] TextMeshPro countdownText;
    [SerializeField] AudioSource elevatorAudio;

    [Header("Puzzle Logics")]
    [Tooltip("Amount of sockets correctly needed for generator puzzle.")]
    [SerializeField] int generatorPuzzleConnectionAmount = 2;
    [Tooltip("Amount of sockets correctly needed for elevator puzzle.")]
    [SerializeField] int elevatorPuzzleConnectionAmount = 3;
    [SerializeField]
    public enum Puzzle
    {
        Generator,
        Elevator
    }
    public Puzzle activeConnectorPuzzle;
    int rightSocketsFilled = 0;
    int rightSocketsMax;
    PlayableDirector playableDirector;

    string activeScene;

    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioClip colourHint, keypadHint1, keypadhint2, gameWin;
    [SerializeField] GameObject playerXRInteractable;
    public Transform playerRespawn;
    public bool hintGiven;
    bool keypadHint1Given;
    bool keypadHint2Given;
    bool canEscapeNow;

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
        playableDirector = GetComponent<PlayableDirector>();
        // Set puzzle logic to generator since it's the first puzzle done.
        activeConnectorPuzzle = Puzzle.Generator;

        SetPuzzleSocketMaxAmount();
        DisableFires();

        elevatorEmissionMaterial = elevatorEmission.GetComponent<Renderer>().material;
        elevatorEmissionMaterial.EnableKeyword("_EMISSION");

        // Activate all objects pertaining to a certain difficulty
        switch (gameDifficulty)
        {
            case GameDifficulty.Easy:
                foreach (var gameObject in easyObjects)
                    gameObject.SetActive(true);
                foreach (var gameObject in hardObjects)
                    gameObject.SetActive(false);
                break;
            case GameDifficulty.Hard:
                foreach (var gameObject in easyObjects)
                    gameObject.SetActive(false);
                foreach (var gameObject in hardObjects)
                    gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Change the difficulty of the game.
    /// </summary>
    /// <param name="newState">Difficulty, *MUST* match the difficulty enum parameter.</param>
    public void ChangeDifficultyType(string newState)
    {
        gameDifficulty = (GameDifficulty)Enum.Parse(typeof(GameDifficulty), newState);
        StartCoroutine(ChangePuzzleType());
    }

    IEnumerator ChangePuzzleType()
    {
        Fader.FadeOut();
        while (Fader.isFading) yield return null; // wait until fade is complete
        // Activate all objects pertaining to a certain difficulty
        switch (gameDifficulty)
        {
            case GameDifficulty.Easy:
                foreach (var gameObject in easyObjects)
                    gameObject.SetActive(true);
                foreach (var gameObject in hardObjects)
                    gameObject.SetActive(false);
                break;
            case GameDifficulty.Hard:
                foreach (var gameObject in easyObjects)
                    gameObject.SetActive(false);
                foreach (var gameObject in hardObjects)
                    gameObject.SetActive(true);
                break;
        }
        ResetSocketCount();
        Fader.FadeIn();
    }

    #region Socket_Logic
    /// <summary>
    /// Add a socket counter in, and subsequently check if the max amount is hit to resolve the puzzle.
    /// </summary>
    private void AddSocketIn()
    {
        rightSocketsFilled++;
        switch (activeConnectorPuzzle)
        {
            case Puzzle.Generator:
                CheckSocketsCorrect();
                break;
            case Puzzle.Elevator:
                CheckSocketsCorrect();
                break;
        }
    }

    public static void AddSocketInside() => singleton?.AddSocketIn();

    public void HintTick()
    {
        Debug.Log("add a tick to giving hint. will only trigger if correct slots are zero and it hits max");
    }

    /// <summary>
    /// Remove active socket when socket is removed. Ensure it doesn't go under zero at any point.
    /// </summary>
    void RemoveSocketOut()
    {
        print("remove socket");
        if (rightSocketsFilled > 0) rightSocketsFilled--;

        // If somehow this goes under 0, fix back to 0.
        if (rightSocketsFilled < 0) rightSocketsFilled = 0;
    }

    public static void RemoveSocket() => singleton?.RemoveSocketOut();

    /// <summary>
    /// Reset socket count to zero.
    /// </summary>
    public void ResetSocketCount()
    {
        rightSocketsFilled = 0;
    }

    /// <summary>
    /// Change puzzle type to a different puzzle, adjust max amount when changing puzzle type too.
    /// </summary>
    /// <param name="newState">Name of the puzzle, *MUST* match the enum name of the puzzle.</param>
    public void ChangePuzzleType(string newState)
    {
        activeConnectorPuzzle = (Puzzle)Enum.Parse(typeof(Puzzle), newState);
        ResetSocketCount();
        SetPuzzleSocketMaxAmount();
    }

    void SetPuzzleSocketMaxAmount()
    {
        // Change the amount of max sockets needed based on the puzzle count.
        switch (activeConnectorPuzzle)
        {
            case Puzzle.Generator:
                rightSocketsMax = generatorPuzzleConnectionAmount;
                break;
            case Puzzle.Elevator:
                rightSocketsMax = elevatorPuzzleConnectionAmount;
                break;
        }

        // Debug.Log("The max of the puzzle sockets is " + rightSocketsMax + " and the current puzzle state is " + activeConnectorPuzzle + ".");
    }

    public void CheckSocketsCorrect()
    {
        // If the current number of sockets filled matches the max variable, resolve the puzzle based on which it is, otherwise do nothing.
        if (rightSocketsFilled == rightSocketsMax)
        {
            // Check resolution based off puzzle, then reset socket count.
            switch (activeConnectorPuzzle)
            {
                case Puzzle.Generator:
                    Debug.Log("do generator resolution things");
                    playableDirector.Play();
                    break;
                case Puzzle.Elevator:
                    elevatorEmissionMaterial.SetColor("_EmissionColor", Color.white);
                    canEscapeNow = true;
                    elevatorPlayableDirector.Play();
                    Debug.Log("do elevator resolution things");
                    //StartCoroutine(FadeSceneChangeWithTimer("AlphaLayout", 5));
                    
                    break;
            }
            ResetSocketCount();
        }
        else
        {
            // do nothing
        }
    }

    public void EscapeTheArea()
    {
        if (canEscapeNow)
        {
            elevatorAudio.Play();
            StartCoroutine(CountdownToStart());
        } else
        {
            // Debug.Log("nuh uh, the puzzle isn't done yet.");
        }
    }

    IEnumerator CountdownToStart()
    {
        float currentTime = 6;
        while (currentTime > 0)
        {
            // Update the countdown display every frame
            countdownText.text = currentTime.ToString("0");            // Wait for a second
            yield return new WaitForSeconds(1f);            // Decrease the current time
            currentTime--;
        }        // When the countdown is over, 
        // set the text to indicate the end
        countdownText.text = "1";
        elevatorPlayableDirector.Play();
        playerAudio.clip = gameWin;
        playerAudio.Play();
    }

    public void EndTheGame()
    {
        // Use the singleton instance of Fader to call the non-static Fade method
        if (Fader.singleton != null)
        {
            StartCoroutine(Fader.singleton.Fade(Color.clear, Color.white, 5f));
            Application.Quit();
        }
        else
        {
            Debug.LogError("Fader singleton instance is not set.");
        }
    }

    public void GivePlayerHintColour()
    {
        if (!hintGiven) {
            playerAudio.clip = colourHint;
            playerAudio.Play();
            hintGiven = true;
        }
    }

    public void GivePlayerKeypadHint()
    {
        // lazy way to go about it, could be cleaned up
        if (!keypadHint1Given)
        {
            playerAudio.clip = keypadHint1;
            playerAudio.Play();
            keypadHint1Given = true;
            return;
        }
        else if (keypadHint1Given && !keypadHint2Given)
        {
            playerAudio.clip = keypadhint2;
            playerAudio.Play();
            keypadHint2Given = true;
            return;
        } else
        {
            // just play the audio again loool
            playerAudio.Play();
        }
    }
    #endregion

    #region Fire_Logic
    /// <summary>
    /// Disable fires at the beginning of the game.
    /// </summary>
    public void DisableFires()
    {
        foreach (var particleSystem in particleSystems)
            particleSystem.Stop();
    }

    /// <summary>
    /// Enable the fire particle effects + the audio associated with them.
    /// </summary>
    public void EnableFires()
    {
        foreach (var particleSystem in particleSystems)
        {
            particleSystem.Play();
        }
        foreach (var fireSource in fireSources)
        {
            fireSource.Play();
        }
    }
    #endregion

    #region Player_Respawn
    // This is such a dumb way to respawn, but if it works it works ...
    public void RespawnThePlayer()
    {
        StartCoroutine(RespawnPlayer());
    }

    public IEnumerator FadeSceneChangeWithTimer(string sceneName, float time = 3)
    {
        Fader.FadeOut(time);
        while (Fader.isFading) yield return null; // wait until fade is complete
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator RespawnPlayer()
    {
        Fader.FadeOut(1f);
        while (Fader.isFading) yield return null;

        // Fix for CS0200: Instead of assigning the transform directly, update the position and rotation.
        playerXRInteractable.transform.position = playerRespawn.position;
        playerXRInteractable.transform.rotation = playerRespawn.rotation;

        Debug.Log("The player position AFTER being updated is " + playerXRInteractable.transform.position +
                  " and the respawn position is " + playerRespawn.position);
        Fader.FadeIn();
    }
    #endregion
}
