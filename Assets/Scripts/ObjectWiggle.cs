using UnityEngine;
using System.Collections;

public class ObjectWiggle : MonoBehaviour
{
    public float speed;
    public float maxRotation;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, maxRotation * Mathf.Sin(Time.time * speed));
    }
}