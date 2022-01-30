using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlaneMovement : MonoBehaviour
{
    // [SerializeField] private InputActionReference AccelerateActionReference;
    [SerializeField] private float velocityChangeMagtitude = 1.0f;
    [SerializeField] private float angleChangeMagtitude = 1.0f;

    private float maxAngle = 100f; 
    private float speed = 5f; 

    public GameObject rightController;  // the right controller, should be drag by the client
    public GameObject leftController;  // the left controller, should be drag by the client
    public GameObject gameBody;  // the left controller, should be drag by the client

    // private float period = 0.5f;
    // private float nextActionTime;

    private Rigidbody _body; // this should be the whole plane
    
    private Transform _leftTransform;
    private Transform _rightTransform;
    private Transform _bodyTransform;

    void Start()
    {
        // nextActionTime = 0.0f;
        _body = GetComponent<Rigidbody>();
        // jumpActionReference.action.performed += OnAccelerate;
        // jumpActionReference.action.performed += OnDecelerate;
        _bodyTransform = _body.transform;
        _rightTransform = rightController.GetComponent<Transform>();
        _leftTransform = leftController.GetComponent<Transform>();
        // _body.velocity = Vector3.forward * 5f;
    }

    // // increase the velocity magtitude
    // private void OnAccelerate(InputAction.CallbackContext obj)
    // {
    //     _body.velocity += Vector3.up * jumpForce;
    // }

    // // decrease the velocity magtitude
    // private void OnDecelerate(InputAction.CallbackContext obj)
    // {
    //     _body.velocity += Vector3.up * jumpForce;
    // }

    // when the joystick panel is horizontal and the ring is pointing forward => transform.x = 0
    // when the joystick panel is vertical and the ring is pointing down => transform.x = 90
    // we define the static limit from 320 to 350 or 90 to 220
    //  if larger than 220 and smaller than 320 -> go up
    //  if 350-360 or 0 to 90 -> go dowm
    void FixedUpdate()
    {
        // float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        // float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        float angleX = checkAngle(rotateVertically() + _bodyTransform.eulerAngles.x);
        float angleY = checkAngle(rotateHorizontally() + _bodyTransform.eulerAngles.y);
        
       
        // Rotate the cube by converting the angles into a quaternion.
        // Quaternion target = Quaternion.Euler(angleX, _bodyTransform.eulerAngles.y, _bodyTransform.eulerAngles.z);

        // Dampen towards the target rotation
        // _bodyTransform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
        // still some bugs for adding the rotation:
        // When you read the .eulerAngles property, Unity converts the Quaternion's internal representation of the rotation to Euler angles. Because, there is more than one way to represent any given rotation using Euler angles, the values you read back out may be quite different from the values you assigned. This can cause confusion if you are trying to gradually increment the values to produce animation.
        // https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html
        _bodyTransform.eulerAngles = new Vector3(angleX, angleY, _bodyTransform.eulerAngles.z);

        // transform.rotation *= Quaternion.AngleAxis(angleX, Vector3.right);
        
        // recalculate the velocity
        // _body.velocity = _bodyTransform.forward * _body.velocity.magnitude;
        // _body.AddForce(_bodyTransform.forward * 10.0f);
        _bodyTransform.position += _bodyTransform.forward * Time.deltaTime * speed;
    }


    private float checkAngle(float angel) {
        if (angel >= 360f) {
            return angel - 360f;
        }

        if (angel < 0f) {
            return angel + 360f;
        }

        return angel;
    }

    private float rotateHorizontally() 
    {
        float angle = checkAngle(_leftTransform.eulerAngles.z - _bodyTransform.eulerAngles.z);
        if (angle >= 10f && angle < 120f) {
            return - angle / maxAngle * angleChangeMagtitude;
        } 

        if (angle >= 250f && angle < 350f) {
            return - (angle - 360f) / maxAngle * angleChangeMagtitude;
        } 

        return 0.0f;
    }

    // this is for rotate up and down
    private float rotateVertically() {

        float angle = checkAngle(_leftTransform.eulerAngles.x - _bodyTransform.eulerAngles.x);
        // float angle =  _leftTransform.localEulerAngles.x;

        if ((angle >= 0f && angle < 90f) || angle > 350f) {

            // GoDown();
            if (angle < 100f) {
                angle += 360f;
            }

            return (angle - 350f) / maxAngle * angleChangeMagtitude;

        } 
        
        if (angle >= 220f && angle < 320f) {
            // GoUp();
            return (angle - 320f) / maxAngle * angleChangeMagtitude;
        }

        return 0f;
    }

    private void GoDown()
    {
        //_body.AddForce(Vector3.up*jumpForce);
        _body.velocity = Vector3.down * velocityChangeMagtitude;
    }

    private void GoUp()
    {
        //_body.AddForce(Vector3.up*jumpForce);
        _body.velocity = Vector3.up * velocityChangeMagtitude;
    }
}


