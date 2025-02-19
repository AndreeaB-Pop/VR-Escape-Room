using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameLogic_Coroutines : MonoBehaviour
{
    [SerializeField] private AudioClip introClip, successClip, failureClip;
    [SerializeField] private float gameDuration = 5 * 60;
    [SerializeField] private TextMeshProUGUI timeLeftDisplay;

    private bool solved = false;

    // Calls a function that sets the solved variable to true, can be publically called.
    public void RoomSolved() => solved = true;

    IEnumerator Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(introClip);
        Fader.FadeIn(1);
        float endTime = Time.time + gameDuration;
        while (Time.time < endTime && !solved)
        {
            timeLeftDisplay.text = TimeSpan.FromSeconds(endTime - Time.time).ToString(@"mm\:ss");
            if (endTime - Time.time < 1 * 60) timeLeftDisplay.color = Color.red;
            yield return new WaitForSeconds(1);
        }
        audioSource.PlayOneShot(solved ? successClip : failureClip);
        // can add another wait for seconds here and boot players out (even though that's jarring)
        Fader.FadeOut();
        while (Fader.isFading) yield return null;
        print("quit game here");
        Application.Quit();
    }
}
