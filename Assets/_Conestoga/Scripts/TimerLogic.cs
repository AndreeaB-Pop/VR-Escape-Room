using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerLogic : MonoBehaviour
{
    // [SerializeField] private AudioClip introClip, successClip, failureClip;
    [SerializeField] private float gameDuration = 5 * 60;
    [SerializeField] private TextMeshProUGUI timeLeftDisplay;

    private bool solved = false;

    // Calls a function that sets the solved variable to true, can be publically called.
    public void RoomSolved() => solved = true;

    public void InitiateTimer()
    {
        timeLeftDisplay.enabled = true;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        //AudioSource audioSource = GetComponent<AudioSource>();
        //audioSource.PlayOneShot(introClip);
        float endTime = Time.time + gameDuration;
        while (Time.time < endTime && !solved)
        {
            timeLeftDisplay.text = TimeSpan.FromSeconds(endTime - Time.time).ToString(@"mm\:ss");
            if (endTime - Time.time < 1 * 60) timeLeftDisplay.color = Color.red;
            yield return new WaitForSeconds(1);
        }
        //audioSource.PlayOneShot(solved ? successClip : failureClip);
        // can add another wait for seconds here and boot players out (even though that's jarring)
        Fader.FadeOut();
        while (Fader.isFading) yield return null;
        string activeScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(activeScene);
        //Application.Quit();
    }
}
