using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class CoinCollector : MonoBehaviour
{

    private PlaneMovement planeMovement;
    
    void Start()
    {
        planeMovement = GameObject.Find("XR Origin2").GetComponent<PlaneMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        planeMovement.coins += 1;
        Destroy(this.gameObject);
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * 50 * Time.deltaTime, Space.Self);
    }
}
