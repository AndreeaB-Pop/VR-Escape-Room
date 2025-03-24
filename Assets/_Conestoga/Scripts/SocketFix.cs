using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketFix : MonoBehaviour
{
    private XRSocketInteractor _socket;

    private void Awake()
    {
        _socket = GetComponent<XRSocketInteractor>();
        _socket.selectEntered.AddListener(OnSelectEntered);
        _socket.selectExited.AddListener(OnSelectExited);
    }

    private void OnSelectEntered(SelectEnterEventArgs arg0)
    {
        var other = arg0.interactableObject.transform.gameObject;
        SocketCollisionsIgnored(other, true);
    }

    private void OnSelectExited(SelectExitEventArgs arg0)
    {
        var other = arg0.interactableObject.transform.gameObject;
        SocketCollisionsIgnored(other, false);
    }

    private void SocketCollisionsIgnored(GameObject other, bool flag)
    {
        var myColliders = GetComponentsInChildren<Collider>(true);
        var theirColliders = other.GetComponentsInChildren<Collider>(true);

        // overkill - all (A,B) pairs will be duplicated (B,A) - optimise?
        foreach (var cA in myColliders)
            foreach (var cB in theirColliders)
                Physics.IgnoreCollision(cA, cB, flag);

        // Debug.Log("other is " + other.name + " and socket collision is running. it is setting the collisions to " + flag + ". the colliders i have are " + myColliders.Length + " and the ones connected are " + theirColliders.Length);
    }
}
