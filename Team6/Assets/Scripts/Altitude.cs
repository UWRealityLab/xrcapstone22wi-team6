using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Altitude : MonoBehaviour
{
    [SerializeField] private InputActionReference jumpActionReference;
    [SerializeField] private float jumpForce = 1.0f;

    private float period = 0.5f;
    private float nextActionTime;

    private Rigidbody _body;

    void Start()
    {
        nextActionTime = 0.0f;
        _body = GetComponent <Rigidbody>();
        jumpActionReference.action.performed += OnJump;
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        //_body.AddForce(Vector3.up*jumpForce);
        _body.velocity += Vector3.up * jumpForce;
    }

    // Change the numbers here to make it feel smoother
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            Debug.Log(_body.velocity.magnitude);
            if (_body.velocity.magnitude > 0.5f)
            {
                // _body.velocity *= (_body.velocity.magnitude - 0.5f) / _body.velocity.magnitude;
                _body.velocity *= _body.velocity.magnitude * 4 / 5;
            }
            else if (_body.velocity.magnitude > 0)
            {
                _body.velocity = Vector3.zero;
            }
        }
        
    }
}


