using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneReset : MonoBehaviour
{
    [SerializeField] private InputActionReference actionReference;

    private void FixedUpdate()
    {
        if (actionReference.action.IsPressed())
        {
            Reset();
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene("SampleWholePlane");
    }
}
