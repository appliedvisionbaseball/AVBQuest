using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHandler : MonoBehaviour
{
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private Vector3 defaultScale;

    private Grabbable grabbable;
    public GameObject batPrefab;

    void Start()
    {
        // Store the object's initial transform values
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        defaultScale = transform.localScale;
    }

    public void ResetTransform()
    {
        StartCoroutine(WaitForGravity());
    }

    IEnumerator WaitForGravity() {
        yield return new WaitForSeconds(2f);

       GameObject newBat = Instantiate(batPrefab,defaultPosition,defaultRotation);
        newBat.transform.SetParent(this.transform.parent.transform);

        this.gameObject.SetActive(false);
    }
}
