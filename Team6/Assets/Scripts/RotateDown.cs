using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDown : MonoBehaviour
{
    [SerializeField] private float jumpForce = 1.0f;

    private Rigidbody _body;
    private Transform _transform;

    public GameObject rightController;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _transform = rightController.GetComponent<Transform>();
    }

    private void GoDown()
    {
        //_body.AddForce(Vector3.up*jumpForce);
        _body.velocity = Vector3.down * jumpForce;
    }

    private void GoUp()
    {
        //_body.AddForce(Vector3.up*jumpForce);
        _body.velocity = Vector3.up * jumpForce;
    }

    // when the joystick panel is horizontal and the ring is pointing forward => transform.x = 0
    // when the joystick panel is vertical and the ring is pointing down => transform.x = 90
    // we define the static limit from 320 to 350 or 90 to 200
    //  if larger than 200 and smaller than 320 -> go up
    //  if 350-360 or 0 to 90 -> go dowm
    void Update()
    {
        if ((_transform.eulerAngles.x >= 0f && _transform.eulerAngles.x < 90f)
            || _transform.eulerAngles.x > 350f)
        {
            GoDown();
        }
        else if (_transform.eulerAngles.x >= 200f && _transform.eulerAngles.x < 320f) {
            GoUp();
        } else
        {
            _body.velocity = Vector3.zero; // show be deleted latter
        }
    }
}


