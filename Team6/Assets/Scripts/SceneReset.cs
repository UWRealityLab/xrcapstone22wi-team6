using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneReset : MonoBehaviour
{
    [SerializeField] private InputActionReference actionReference;
    private Rigidbody body;
    [SerializeField] public int tutorial_stage = -1; // stage -1 means we are not in tutorial
                                                      // stage 0 means we are at the seconnd stage of the tutorial

    private void Awake()
    {
        body = GameObject.Find("XR Origin2").GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (actionReference.action.IsPressed())
        {
            Reset();
        }
    }
    

    public void Reset()
    {
        if (tutorial_stage == -1 || tutorial_stage == 0) {
            tutorial_stage = -1;
            SceneManager.LoadScene("SampleWholePlane");
        } else {
            // we should set the position to the start of section 2
            SceneManager.LoadScene("Tutorial2");
        }
        
    }

    public void Tutor()
    {
        tutorial_stage = 1;
        SceneManager.LoadScene("Tutorial");
    }
}
