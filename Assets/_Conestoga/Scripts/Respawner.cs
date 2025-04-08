using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public Transform playerPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPosition = other.GetComponent<Transform>();
            Debug.Log(playerPosition.name + " is located at " +  playerPosition.position);
            SocketLogic.RespawnPlayerPublic(playerPosition);
        }
    }
}
