using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class GameLogic_UI : MonoBehaviour
{

    private List<GameObject> spawnedObjects = new List<GameObject>();

    [SerializeField] private TextMeshProUGUI objectCounterText;
    [SerializeField] private GameObject despawnParticle;
    [SerializeField] private Transform moveableObject;
    [SerializeField] private XRJoystick joystick;

    private void Update()
    {
        moveableObject.localPosition = 0.20f * new Vector3(joystick.value.x, 0, joystick.value.y);
        UpdateObjectCounter();
    }

    public void CreateObject(GameObject prefab)
    {
        spawnedObjects.Add(Instantiate(prefab, Camera.main.transform.position + 2 * Camera.main.transform.forward, Quaternion.identity));
        UpdateObjectCounter();
    }
    public void DestroyObjectsNormally()
    {
        foreach (GameObject obj in spawnedObjects) Destroy(obj);
        spawnedObjects.Clear();
    }

    public void DestroyObjectsCoroutine()
    {
        StartCoroutine(DestroyObject());
    }

    private IEnumerator DestroyObject()
    {
        //need to swap to void if you just wanna do this
        //foreach (GameObject obj in spawnedObjects) Destroy(obj);
        //spawnedObjects.Clear();

        foreach (GameObject obj in spawnedObjects)
        {
            yield return new WaitForSeconds(1);
            Instantiate(despawnParticle, obj.transform.position, Quaternion.identity);
            Destroy(obj);
        }

        spawnedObjects.Clear();
        UpdateObjectCounter();
        UnityEngine.Debug.Log("All objects have been destroyed!");
    }

    private void UpdateObjectCounter()
    {
        objectCounterText.text = $"{spawnedObjects.Count} objects";
    }
}
