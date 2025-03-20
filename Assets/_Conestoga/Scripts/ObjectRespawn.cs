using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawn : MonoBehaviour
{
    Vector3 originalPosition;
    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
    }

    /// <summary>
    /// When entering the respawn bounding box, reset this item back to its original position.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Object Respawn"))
        {
            transform.SetPositionAndRotation(originalPosition, originalRotation);
        }
    }
}
