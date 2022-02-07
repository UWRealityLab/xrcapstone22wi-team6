using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlaneMovement : MonoBehaviour
{
    // [SerializeField] private InputActionReference AccelerateActionReference;
    [SerializeField] private float velocityChangeMagtitude = 1.0f;
    [SerializeField] private float angleChangeMagtitude = 0.5f;
    [SerializeField] private InputActionReference actionReference;

    private float maxAngle = 100f; 
    private float speed = 5f;

    public GameObject panel;
    public GameObject rightController;  // the right controller, should be drag by the client
    public GameObject leftController;  // the left controller, should be drag by the client
    public GameObject gameBody;  // the left controller, should be drag by the client

    private Rigidbody _body; // this should be the whole plane
    
    private Transform _leftTransform;
    private Transform _rightTransform;
    private Transform _bodyTransform;
    private Text _text;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _bodyTransform = _body.transform;
        _rightTransform = rightController.GetComponent<Transform>();
        _leftTransform = leftController.GetComponent<Transform>();
        _text = panel.GetComponent<Text>();
    }

    // when the joystick panel is horizontal and the ring is pointing forward => transform.x = 0
    // when the joystick panel is vertical and the ring is pointing down => transform.x = 90
    // we define the static limit from 320 to 350 or 90 to 220
    //  if larger than 220 and smaller than 320 -> go up
    //  if 350-360 or 0 to 90 -> go dowm
    void FixedUpdate()
    {
        // if the button is pressed, we will change the rotations of the plane
        if (actionReference.action.IsPressed())
        {
            // float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
            // float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
            float angleX = checkAngle(rotateVertically() + _bodyTransform.eulerAngles.x);
            float angleY = checkAngle(rotateHorizontally() + _bodyTransform.eulerAngles.y);
            // The side way should lean the way my controller leans regardless of where it was
            float angleZ = checkAngle(rotateSideway() + _bodyTransform.eulerAngles.z);


            // Rotate the cube by converting the angles into a quaternion.
            // Quaternion target = Quaternion.Euler(angleX, _bodyTransform.eulerAngles.y, _bodyTransform.eulerAngles.z);

            // Dampen towards the target rotation
            // _bodyTransform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
            // still some bugs for adding the rotation:
            // When you read the .eulerAngles property, Unity converts the Quaternion's internal representation of the rotation to Euler angles. Because, there is more than one way to represent any given rotation using Euler angles, the values you read back out may be quite different from the values you assigned. This can cause confusion if you are trying to gradually increment the values to produce animation.
            // https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html
            _bodyTransform.eulerAngles = new Vector3(angleX, angleY, angleZ);

            // transform.rotation *= Quaternion.AngleAxis(angleX, Vector3.right);

            // recalculate the velocity
            // _body.velocity = _bodyTransform.forward * _body.velocity.magnitude;
            // _body.AddForce(_bodyTransform.forward * 10.0f);
        } else {
            // we will reset the rotation in Z to 0 gradually
            if (_bodyTransform.eulerAngles.z > 1f && _bodyTransform.eulerAngles.z < 359f) {
                float angleZ = _bodyTransform.eulerAngles.z - 1f;
                if (_bodyTransform.eulerAngles.z > 180f) {
                    angleZ = _bodyTransform.eulerAngles.z + 1f;
                }

                _bodyTransform.eulerAngles = new Vector3(_bodyTransform.eulerAngles.x, _bodyTransform.eulerAngles.y, angleZ);
            } else {
                 _bodyTransform.eulerAngles = new Vector3(_bodyTransform.eulerAngles.x, _bodyTransform.eulerAngles.y, 0f);
            }
        }
        _bodyTransform.position += _bodyTransform.forward * Time.deltaTime * speed;
        _text.text = "Speed: " + speed;
        _text.text += "\nRotations(x, y, z): \n"
            + _bodyTransform.eulerAngles.x + "°, "
            + _bodyTransform.eulerAngles.y + "°, "
            + _bodyTransform.eulerAngles.z + "°";
        _text.text += "\nPress \"A\" to reset the game!";
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

    // calculate the rotation angle Yaw axis
    private float rotateHorizontally() 
    {   
        float angle = checkAngle(_bodyTransform.eulerAngles.z);
        if (angle >= 5f && angle < 105f) {
            return -angle / maxAngle * angleChangeMagtitude;
        } 

        if (angle >= 255f && angle < 355f) {
            return -(angle - 360f) / maxAngle * angleChangeMagtitude;
        } 

        return 0.0f;
    }

    // calculate the rotation angle roll axis
    private float rotateSideway()
    {
        // we only change of both are in the same angle range
        float angle = checkAngle((_leftTransform.eulerAngles.z + _rightTransform.eulerAngles.z) / 2
                                    - _bodyTransform.eulerAngles.z);
        if (angle >= 5f && angle < 105f)
        {
            // The 45f represents something very different than angleChange. Instead it's the maxinum
            // of angle for the plane sideways.
            // The current game is not so smooth here and I am think some
            // function is needed so that it's not continous propotional but curved.
            return angle * angle / maxAngle / maxAngle * 2;
        }

        if (angle >= 255f && angle < 355f)
        {
            return (angle - 360f) * (360f - angle) / maxAngle / maxAngle * 2;
        }

        return 0.0f;
    }

    // this is for rotate up and down
    // calculate the rotation angle pitch axis
    private float rotateVertically() {

        float angle = checkAngle((_leftTransform.eulerAngles.x + _rightTransform.eulerAngles.x) / 2
                                    - _bodyTransform.eulerAngles.x);

        if ((angle >= 0f && angle < 90f) || angle > 350f) {

            if (angle < 100f) {
                angle += 360f;
            }

            return (angle - 350f) / maxAngle * angleChangeMagtitude;

        } 
        
        if (angle >= 220f && angle < 320f) {
            return (angle - 320f) / maxAngle * angleChangeMagtitude;
        }

        return 0f;
    }
}


