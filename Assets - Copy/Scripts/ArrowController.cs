using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    float origY;
    void Start()
    {
        origY = transform.position.y;
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, origY + Mathf.PingPong(Time.time * 2, 1), transform.position.z);
    }
}