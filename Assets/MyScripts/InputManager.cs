using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public float throwSpeed = 1.0f;
    public float jumpSpeed = 1.0f;
    public float runningSpeed;
    public TutorialController tutorialController;
    private const string THROWABLE_TAG = "Throwable";
    public Transform rightControllerTransform;
    public Transform leftControllerTransform;
    public GameObject cameraRig;

    private SteamVR_Controller.Device rightController;
    private SteamVR_Controller.Device leftController;
    private bool leftGrip;
    private bool rightGrip;
    private bool playerMenu;
    private bool leftTouch;
    private bool rightTouch;
    private bool leftTrigger;
    private bool rightTrigger;
    private bool leftTouchPadPress;
    private bool rightTouchPadPress;
    private bool leftJumpable = false;
    private bool rightJumpable = false;
    private Rigidbody rigidRig;
    private bool grounded = false;

    private Vector3 prevPosLeft;
    private Vector3 prevParentPosLeft;
    private Vector3 prevDiffPosLeft;
    private Vector3 newPosLeft;
    private Vector3 newParentPosLeft;
    private Vector3 newDiffPosLeft;
    private Vector3 controllerVelocityLeft;

    private Vector3 prevPosRight;
    private Vector3 prevParentPosRight;
    private Vector3 prevDiffPosRight;
    private Vector3 newPosRight;
    private Vector3 newParentPosRight;
    private Vector3 newDiffPosRight;
    private Vector3 controllerVelocityRight;
    private float touchJumpStartLeft;
    private float touchJumpStartRight;
    private float touchJumpEndLeft;
    private float touchJumpEndRight;
    private bool jumpLeft;
    private bool jumpRight;
    private bool jumped = false;
    private bool grabbingTutorial = false;
    public GameManager gameManager;


    private GameObject grabObjectLeft;
    private GameObject grabObjectRight;

    void Awake()
    {
        rigidRig = cameraRig.transform.GetComponent<Rigidbody>();
    }

    void Start()
    {
        //left
        prevPosLeft = leftControllerTransform.position;
        prevParentPosLeft = cameraRig.transform.position;
        prevDiffPosLeft = prevParentPosLeft - prevPosLeft;
        //right
        prevPosRight = rightControllerTransform.position;
        prevParentPosRight = cameraRig.transform.position;
        prevDiffPosRight = prevParentPosRight - prevPosRight;
    }

    void Update()
    {
        if (!gameManager.menuScene)
        {
            //left
            newPosLeft = leftControllerTransform.position;
            newParentPosLeft = cameraRig.transform.position;
            newDiffPosLeft = newParentPosLeft - newPosLeft;
            controllerVelocityLeft = (newDiffPosLeft - prevDiffPosLeft) / Time.deltaTime;
            prevPosLeft = newPosLeft;
            prevParentPosLeft = newParentPosLeft;
            prevDiffPosLeft = newDiffPosLeft;

            //right
            newPosRight = rightControllerTransform.position;
            newParentPosRight = cameraRig.transform.position;
            newDiffPosRight = newParentPosRight - newPosRight;
            controllerVelocityRight = (newDiffPosRight - prevDiffPosRight) / Time.deltaTime;
            prevPosRight = newPosRight;
            prevParentPosRight = newParentPosRight;
            prevDiffPosRight = newDiffPosRight;

            if (leftController.GetAxis().x != 0 || leftController.GetAxis().y != 0)
            {
                if (!leftJumpable)
                {
                    touchJumpStartLeft = leftController.GetAxis().y;
                    leftJumpable = true;
                }
                else
                {
                    if (leftController.GetAxis().y < touchJumpStartLeft)
                    {
                        touchJumpStartLeft = leftController.GetAxis().y;
                    }
                    touchJumpEndLeft = leftController.GetAxis().y;
                }
            }
            else
            {
                if (leftJumpable)
                {
                    jumpLeft = true;
                    leftJumpable = false;
                }
            }

            if (rightController.GetAxis().x != 0 || rightController.GetAxis().y != 0)
            {
                if (!rightJumpable)
                {
                    touchJumpStartRight = rightController.GetAxis().y;
                    rightJumpable = true;
                }
                else
                {
                    if (rightController.GetAxis().y < touchJumpStartRight)
                    {
                        touchJumpStartRight = rightController.GetAxis().y;
                    }
                    touchJumpEndRight = rightController.GetAxis().y;
                }
            }
            else
            {
                if (rightJumpable)
                {
                    jumpRight = true;
                    rightJumpable = false;
                }
            }

            Jump();
            UpdateRunning();
        }
    }

    public void SetRightController(SteamVR_Controller.Device controller)
    {
        rightController = controller;
    }

    public void SetLeftController(SteamVR_Controller.Device controller)
    {
        leftController = controller;
    }

    public void LeftGripSqueeze()
    {

    }

    public void RightGripSqueeze()
    {
    }

    public void LeftAppMenuClick()
    {
        if (gameManager.menuScene)
        {
            gameManager.Resume();
            return;
        }
        gameManager.PauseGame();
    }

    public void RightAppMenuClick()
    {
        if (gameManager.menuScene)
        {
            gameManager.Resume();
            return;
        }
        gameManager.PauseGame();
    }

    private void GrabLeft()
    {
        if (grabObjectLeft != null && grabObjectLeft.tag == "Throwable")
        {
            grabObjectLeft.layer = LayerMask.NameToLayer("BallPossession");
            grabObjectLeft.transform.SetParent(leftControllerTransform.parent);
            grabObjectLeft.transform.GetComponent<Rigidbody>().isKinematic = true;
            leftController.TriggerHapticPulse(2000);
            if (tutorialController != null)
            {
                if (!grabbingTutorial)
                {
                    grabbingTutorial = true;
                    tutorialController.ValidateGrabbingGoThrowing();
                }
            }
        }
    }

    private void GrabRight()
    {
        if (grabObjectRight != null && grabObjectRight.tag == "Throwable")
        {
            grabObjectRight.layer = LayerMask.NameToLayer("BallPossession");
            grabObjectRight.transform.SetParent(rightControllerTransform.parent);
            grabObjectRight.transform.GetComponent<Rigidbody>().isKinematic = true;
            rightController.TriggerHapticPulse(2000);
            if (tutorialController != null)
            {
                if (!grabbingTutorial)
                {
                    grabbingTutorial = true;
                    tutorialController.ValidateGrabbingGoThrowing();
                }
            }
        }
    }

    private void ThrowLeft()
    {
        if (grabObjectLeft != null && grabObjectLeft.transform.parent == leftControllerTransform.parent)
        {
            grabObjectLeft.layer = LayerMask.NameToLayer("Ball");
            grabObjectLeft.transform.SetParent(null);
            Rigidbody rigid = grabObjectLeft.transform.GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.velocity = leftController.velocity * throwSpeed;
            rigid.angularVelocity = leftController.angularVelocity;
        }
    }

    private void ThrowRight()
    {
        if (grabObjectRight != null && grabObjectRight.transform.parent == rightControllerTransform.parent)
        {
            grabObjectRight.layer = LayerMask.NameToLayer("Ball");
            grabObjectRight.transform.SetParent(null);
            Rigidbody rigid = grabObjectRight.transform.GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.velocity = rightController.velocity * throwSpeed;
            rigid.angularVelocity = rightController.angularVelocity;
        }
    }

    public void LeftTriggerSqueeze()
    {
        GrabLeft();
    }

    public void RightTriggerSqueeze()
    {
        GrabRight();
    }

    public void LeftTriggerRelease()
    {
        ThrowLeft();
    }

    public void RightTriggerRelease()
    {
        ThrowRight();
    }

    public void LeftTouchPadPress()
    {
        leftTouchPadPress = true;
        newPosLeft = leftControllerTransform.position;
    }

    public void RightTouchPadPress()
    {
        rightTouchPadPress = true;
        newPosRight = rightControllerTransform.position;
    }

    public void LeftTouchPadPressUp(float pos)
    {
        touchJumpEndLeft = pos;
        leftTouchPadPress = false;
    }

    public void RightTouchPadPressUp(float pos)
    {
        touchJumpEndRight = pos;
        rightTouchPadPress = false;
    }

    public void RightOnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == THROWABLE_TAG)
        {
            grabObjectRight = other.gameObject;
        }
    }

    public void LeftOnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == THROWABLE_TAG)
        {
            grabObjectLeft = other.gameObject;
        }
    }

    public void RightOnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == THROWABLE_TAG)
        {
            grabObjectRight = null;
        }
    }

    public void LeftOnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == THROWABLE_TAG)
        {
            grabObjectLeft = null;
        }
    }

    private void UpdateRunning()
    {
        float speed = runningSpeed;
        if (!grounded)
        {
            speed /= 8;
        }
        float leftY = 0;
        float rightY = 0;
        if (leftTouchPadPress)
        {
            //left
            leftY = (cameraRig.transform.rotation.eulerAngles.y - leftControllerTransform.rotation.eulerAngles.y + 90) * Mathf.Deg2Rad;
        }

        //right
        if (rightTouchPadPress)
        {
            rightY = (cameraRig.transform.rotation.eulerAngles.y - rightControllerTransform.rotation.eulerAngles.y + 90) * Mathf.Deg2Rad;
        }

        //average
        if (leftTouchPadPress && rightTouchPadPress)
        {
            rigidRig.AddForce((Vector3.Magnitude(controllerVelocityLeft) + Vector3.Magnitude(controllerVelocityRight)) / 2 * speed * new Vector3(Mathf.Cos((leftY + rightY) / 2),
                0,
                Mathf.Sin((leftY + rightY) / 2)));
            return;
        }

        if (leftTouchPadPress)
        {
            rigidRig.AddForce(Vector3.Magnitude(controllerVelocityLeft) * speed * new Vector3(Mathf.Cos(leftY),
                0,
                Mathf.Sin(leftY)));
            return;
        }

        if (rightTouchPadPress)
        {
            rigidRig.AddForce(Vector3.Magnitude(controllerVelocityRight) * speed * new Vector3(Mathf.Cos(rightY),
                0,
                Mathf.Sin(rightY)));
            return;
        }
    }

    public void SetGrounded(bool grounded)
    {
        this.grounded = grounded;
    }

    void Jump()
    {
        float touchStart;
        float touchEnd;
        if ((jumpLeft && jumpRight && jumped) || (!jumpLeft && !jumpRight))
        {
            jumpLeft = false;
            jumpRight = false;
            return;
        }
        if (grounded)
        {
            if (jumpLeft)
            {
                touchStart = touchJumpStartLeft;
                touchEnd = touchJumpEndLeft;
            }
            else
            {
                touchStart = touchJumpStartRight;
                touchEnd = touchJumpEndRight;
            }
            if (touchEnd - touchStart > 0.2f)
            {
                if (!jumped)
                {
                    rigidRig.AddForce(Vector3.up * jumpSpeed * (touchEnd - touchStart));
                    jumped = true;
                }
            }
        }
        jumpLeft = false;
        jumpRight = false;
    }

    public void BackToGround()
    {
        jumped = false;
    }
}
