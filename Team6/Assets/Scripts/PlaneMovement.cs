using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlaneMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference accelerateActionReference;
    [SerializeField] private float velocityChangeMagtitude = 1.0f;
    [SerializeField] private float angleChangeMagtitude = 0.5f;
    [SerializeField] private InputActionReference actionReference;

    [SerializeField] public int coins;
    [SerializeField] public int coins_total;

    private float maxAngle = 100f; 
    private float speed;

    public GameObject panel;
    public GameObject rightController;  // the right controller, should be drag by the client
    public GameObject leftController;  // the left controller, should be drag by the client
    public GameObject gameBody;  // the left controller, should be drag by the client

    private Rigidbody _body; // this should be the whole plane
    
    private Transform _leftTransform;
    private Transform _rightTransform;
    private Transform _bodyTransform;
    private Text _text;

    private SceneReset sceneReset;

    void Start()
    {
        speed = 5f;
        _body = GetComponent<Rigidbody>();
        _bodyTransform = _body.transform;
        _rightTransform = rightController.GetComponent<Transform>();
        _leftTransform = leftController.GetComponent<Transform>();
        _text = panel.GetComponent<Text>();

        sceneReset = GameObject.Find("SceneManager").GetComponent<SceneReset>();
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
            if (sceneReset.tutorial_stage == 1) {
                sceneReset.tutorial_stage += 1;
            } else {
                Vector2 speedChange = accelerateActionReference.action.ReadValue<Vector2>();
                if (speedChange[1] > 0.1 && speed <= 39.5f) {
                    if (sceneReset.tutorial_stage == 2) {
                        sceneReset.tutorial_stage += 1;
                    }
                    speed += 0.05f;
                } else if (speedChange[1] < -0.1 && speed > 1.05f) {
                    if (sceneReset.tutorial_stage == 3) {
                        sceneReset.tutorial_stage += 1;
                    }
                    speed -= 0.05f;
                }

                // float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
                // float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
                float angleX = checkAngle(rotateVertically() + _bodyTransform.eulerAngles.x);
                float angleY = checkAngle(rotateHorizontally()/3 + _bodyTransform.eulerAngles.y);
                // The side way should lean the way my controller leans regardless of where it was
                float angleZ = checkAngle(rotateSideway() + _bodyTransform.eulerAngles.z);
                if (angleZ > 30f && angleZ < 180f)
                {
                    angleZ = 30f;
                }
                else if (angleZ < 330f && angleZ >= 180f)
                {
                    angleZ = 330f;
                }

                // Dampen towards the target rotation
                // _bodyTransform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
                // still some bugs for adding the rotation:
                // When you read the .eulerAngles property, Unity converts the Quaternion's internal representation of the rotation to Euler angles. Because, there is more than one way to represent any given rotation using Euler angles, the values you read back out may be quite different from the values you assigned. This can cause confusion if you are trying to gradually increment the values to produce animation.
                // https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html
                _bodyTransform.eulerAngles = new Vector3(angleX, angleY, angleZ);
            }
        } else {
            // we will reset the rotation in Z to 0 gradually
            if (_bodyTransform.eulerAngles.z > 1f && _bodyTransform.eulerAngles.z < 359f) {
                float angleZ = _bodyTransform.eulerAngles.z - 0.5f;
                if (_bodyTransform.eulerAngles.z > 180f) {
                    angleZ = _bodyTransform.eulerAngles.z + 0.5f;
                }

                _bodyTransform.eulerAngles = new Vector3(_bodyTransform.eulerAngles.x, _bodyTransform.eulerAngles.y, angleZ);
            } else {
                 _bodyTransform.eulerAngles = new Vector3(_bodyTransform.eulerAngles.x, _bodyTransform.eulerAngles.y, 0f);
            }
        }

        _bodyTransform.position += _bodyTransform.forward * Time.deltaTime * speed;

        if (sceneReset.tutorial_stage == -1) {
            _text.text = "           Speed: " + speed;
            _text.text += "\n          Coins you have: " + coins;
            //_text.text += "\nRotations(x, y, z): \n"
              //  + _bodyTransform.eulerAngles.x + "??, "
              //  + _bodyTransform.eulerAngles.y + "??, "
              //  + _bodyTransform.eulerAngles.z + "??";
            _text.text += "\n           Press \"A\" to Reset";
        } else if (sceneReset.tutorial_stage == 0) {
            _text.text = "Congratulations! Now try to collect all the coins!\n";
            _text.text += "Coins you have: " + coins + "\n Total coins to collect: " + coins_total;
            _text.text += "\nSpeed: " + speed;
            //_text.text += "\nRotations(x, y, z): \n"
              //  + _bodyTransform.eulerAngles.x + "??, "
              //  + _bodyTransform.eulerAngles.y + "??, "
              //  + _bodyTransform.eulerAngles.z + "??";
            _text.text += "\nPress \"A\" to reset!\n";
            _text.text += "Press \"B\" to enter the exploration mode!";
        } else if (sceneReset.tutorial_stage == 1) {
            _text.text = "Please play our game seated. \n";
            _text.text += "Hold the controllers so the Trigger buttons face forward\n";
            _text.text += "Speed: " + speed; 
            _text.text += "\nStage 1: Enter Control Mode\n";
            _text.text += "\t Press and hold the Trigger buttons for both hands to enter control mode.\n";
            _text.text += "\t The Trigger buttons are used to Enter Control Mode.\n";
            _text.text += "\t You must hold the Trigger buttons if you want to change\n";
            _text.text += "\t the movements of the plane";
            _text.text += "\nPress \"A\" to reset the tutorial!\n";
            _text.text += "Press \"B\" to enter the explore phase of the tutorial!";
        } else if (sceneReset.tutorial_stage == 2) {
            _text.text = "Speed: " + speed;
            _text.text += "\nCongratulations! You are now in Stage 2\n";
            _text.text += "Stage 2: increase the speed. \n";
            _text.text += "\t Moving the joystick of the controllers forward while holding the Trigger buttons.\n";
            _text.text += "\t This will increase the speed of the plane.\n";
            _text.text += "\nPress \"A\" to reset the tutorial!\n";
            _text.text += "Press \"B\" to enter the explore phase of the tutorial!";
        } else if (sceneReset.tutorial_stage == 3) {
            _text.text = "Speed: " + speed;
            _text.text += "\nCongratulations! You are now in Stage 3\n";
            _text.text += "Stage 3: decrease the speed. \n";
            _text.text += "\t Moving the joystick of the controllers backward while holding the Trigger buttons.\n";
            _text.text += "\t This will decrease the speed of the plane.\n";
            _text.text += "\nPress \"A\" to reset the tutorial!\n";
            _text.text += "Press \"B\" to enter the explore phase of the tutorial!";
        } else if (sceneReset.tutorial_stage == 4) {
            _text.text = "Speed: " + speed;
            _text.text += "\nCongratulations! You are now in Stage 4\n";
            _text.text += "Stage 4: turning upward. \n";
            _text.text += "\t Turn the controllers upward while holding the Trigger buttons.\n";
            _text.text += "\t This will also turn your plane upward.\n";
            _text.text += "\nPress \"A\" to reset the tutorial!\n";
            _text.text += "Press \"B\" to enter the explore phase of the tutorial!";
        } else if (sceneReset.tutorial_stage == 5) {
            _text.text = "Speed: " + speed;
            _text.text += "\nCongratulations! You are now in Stage 5\n";
            _text.text += "Stage 5: turning downward. \n";
            _text.text += "\t Turn the controllers downward while holding the Trigger buttons.\n";
            _text.text += "\t This will also turn your plane downward.\n";
             _text.text += "\nPress \"A\" to reset the tutorial!\n";
            _text.text += "Press \"B\" to enter the explore phase of the tutorial!";
        } else if (sceneReset.tutorial_stage == 6) {
            _text.text = "Speed: " + speed;
            _text.text += "\nCongratulations! You are now in Stage 6\n";
            _text.text += "Stage 6: turning left. \n";
            _text.text += "\t Turn the controllers leftward while holding the Trigger buttons.\n";
            _text.text += "\t This will also turn your plane to the left.\n";
             _text.text += "\nPress \"A\" to reset the tutorial!\n";
            _text.text += "Press \"B\" to enter the explore phase of the tutorial!";
        } else if (sceneReset.tutorial_stage == 7) {
            _text.text = "Speed: " + speed;
            _text.text += "\nCongratulations! You are now in Stage 7\n";
            _text.text += "Stage 7: turning right. \n";
            _text.text += "\t Turn the controllers rightward while holding the Grip buttons.\n";
            _text.text += "\t This will also turn your plane to the right.\n";
            _text.text += "\nPress \"A\" to reset the tutorial!\n";
            _text.text += "Press \"B\" to enter the explore phase of the tutorial!";
        } else if (sceneReset.tutorial_stage == 8 || sceneReset.tutorial_stage == 9) {
            _text.text = "Speed: " + speed;
            _text.text += "\nCongratulations! You have collected all of the coins!\n";
            _text.text += "You will jump to the exploration mode shortly\n";
        }

        if (sceneReset.tutorial_stage == 0 && coins == coins_total) {
            sceneReset.tutorial_stage = 8;
        }

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
            if (sceneReset.tutorial_stage == 6) {
                sceneReset.tutorial_stage  += 1;
            }
            return -angle / maxAngle * angleChangeMagtitude * velocityChangeMagtitude * 2;
        } 

        if (angle >= 255f && angle < 355f) {
            if (sceneReset.tutorial_stage == 7) {
                sceneReset.Skip();
            }
            return -(angle - 360f) / maxAngle * angleChangeMagtitude * velocityChangeMagtitude * 2;
        } 

        return 0.0f;
    }

    // calculate the rotation angle roll axis
    private float rotateSideway()
    {
        // we only change of both are in the same angle range
        float angle = checkAngle((_leftTransform.eulerAngles.z + _rightTransform.eulerAngles.z) / 2
                                    - _bodyTransform.eulerAngles.z);
        if (angle >= 0f && angle < 105f)
        {
            // The 45f represents something very different than angleChange. Instead it's the maxinum
            // of angle for the plane sideways.
            // The current game is not so smooth here and I am think some
            // function is needed so that it's not continous propotional but curved.
            return angle * angle / maxAngle / maxAngle * 2;
        }

        if (angle >= 255f && angle < 359f)
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
            if (sceneReset.tutorial_stage == 5) {
                sceneReset.tutorial_stage  += 1;
            }
            if (angle < 100f) {
                angle += 360f;
            }

            return (angle - 350f) / maxAngle * angleChangeMagtitude * velocityChangeMagtitude;

        } 
        
        if (angle >= 220f && angle < 320f) {
            if (sceneReset.tutorial_stage == 4) {
                sceneReset.tutorial_stage  += 1;
            }
            return (angle - 320f) / maxAngle * angleChangeMagtitude * velocityChangeMagtitude;
        }

        return 0f;
    }
}


