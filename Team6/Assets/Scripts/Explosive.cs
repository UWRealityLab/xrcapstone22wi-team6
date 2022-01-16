using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "box")
        {
            this.gameObject.GetComponent<Rigidbody>()
                .AddExplosionForce(10, this.gameObject.transform.position, 5);
        }
    }
}