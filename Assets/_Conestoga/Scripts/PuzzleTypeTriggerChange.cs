using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTypeTriggerChange : MonoBehaviour
{
    [SerializeField] SocketLogic socketLogic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) socketLogic.elevatorPuzzle = true;
    }
}
