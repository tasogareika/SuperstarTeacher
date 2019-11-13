using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTurn : MonoBehaviour
{
    private Transform thisTransform;
    public float rotateSpeed;

    private void Start()
    {
        thisTransform = this.transform;
    }

    private void Update()
    {
        thisTransform.Rotate(0, 0, rotateSpeed);
    }
}
