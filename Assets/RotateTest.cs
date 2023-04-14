using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    Quaternion ogRotationQ;

    private void Start () {
        ogRotationQ = transform.rotation;
    }

    void Update()
    {
        transform.rotation = ogRotationQ;
    }
}
