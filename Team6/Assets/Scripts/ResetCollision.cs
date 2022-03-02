using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class ResetCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Reset();
    }
	
	public void Reset()
    {
        SceneManager.LoadScene("SampleWholePlane");
    }
}
