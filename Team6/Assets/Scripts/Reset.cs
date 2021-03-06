using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{ 
    private Vector3 start;
    private Rigidbody body;

    private void Awake()
    {
        start = transform.position;
        body = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        body.position = start;
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }


}
