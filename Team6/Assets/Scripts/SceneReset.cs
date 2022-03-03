using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneReset : MonoBehaviour
{
    [SerializeField] private InputActionReference resetBinding;
    [SerializeField] private InputActionReference skipBinding;
    private Rigidbody body;
    [SerializeField] public int tutorial_stage = -1; // stage -1 means we are not in tutorial
                                                      // stage 0 means we are at the seconnd stage of the tutorial

    private float nextActionTime = 0f;

    private void Awake()
    {
        body = GameObject.Find("XR Origin2").GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (resetBinding.action.IsPressed())
        {
            Reset();
        }

        if (skipBinding.action.IsPressed())
        {
            Skip();
        }

        if (tutorial_stage == 8) {
            tutorial_stage += 1;
            nextActionTime = Time.time + 5f;
        } else if (tutorial_stage == 9) {
            if (Time.time > nextActionTime)  {
                tutorial_stage = -1;
                SceneManager.LoadScene("SampleWholePlane");
            }
        }
    }

    public void Skip() {
        if (tutorial_stage <= 0 || tutorial_stage >= 8) {
            SceneManager.LoadScene("SampleWholePlane");
        } else {
            // we should set the position to the start of section 2
            SceneManager.LoadScene("Tutorial2");
        }
    }
    

    public void Reset()
    {
        // if (tutorial_stage == -1 || tutorial_stage == 0) {
        //     tutorial_stage = -1;
        //     SceneManager.LoadScene("SampleWholePlane");
        // } else {
        //     // we should set the position to the start of section 2
        //     SceneManager.LoadScene("Tutorial2");
        // }

        if (tutorial_stage == -1) {
            SceneManager.LoadScene("SampleWholePlane");
        } else if (tutorial_stage == 0 || tutorial_stage >= 8) {
            SceneManager.LoadScene("Tutorial2");
        } else {
            SceneManager.LoadScene("Tutorial");
        }
        
    }

    public void Tutor()
    {
        tutorial_stage = 1;
        SceneManager.LoadScene("Tutorial");
    }
}
