using UnityEngine;
using UnityEngine.XR;
using OVR;

public class Flicker : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OVRManager.display.displayFrequency = 72.0f;
        XRSettings.eyeTextureResolutionScale = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
