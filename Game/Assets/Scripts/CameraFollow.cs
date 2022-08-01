using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        var p = target.position;
        p.z = -10;
        transform.position = p;
    }
}
