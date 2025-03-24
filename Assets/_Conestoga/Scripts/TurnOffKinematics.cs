using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffKinematics : MonoBehaviour
{
    [SerializeField] Rigidbody[] rigidbodies;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    public void TurnOffTheKinematics()
    {
        foreach(var rigidbody in rigidbodies)
            rigidbody.isKinematic = false;
    }
}
