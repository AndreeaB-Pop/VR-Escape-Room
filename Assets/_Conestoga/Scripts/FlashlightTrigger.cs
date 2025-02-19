using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightTrigger : MonoBehaviour
{
    private Light flashLightBulb;

    // Start is called before the first frame update
    void Start()
    {
        flashLightBulb = GetComponentInChildren<Light>();
    }

    public void ToggleFlashlight()
    {
        flashLightBulb.enabled = !flashLightBulb.enabled;
    }
}
