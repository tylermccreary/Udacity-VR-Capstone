using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftController : MonoBehaviour
{
    public InputManager inputManager;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Start()
    {
        inputManager.SetLeftController(Controller);
    }

    void Update()
    {

        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            inputManager.LeftGripSqueeze();
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            inputManager.LeftTouchPadPressUp(Controller.GetAxis().y);
        }

        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            inputManager.LeftTouchPadPress();
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            inputManager.LeftTriggerSqueeze();
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            inputManager.LeftTriggerRelease();
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            inputManager.RightAppMenuClick();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        inputManager.LeftOnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        inputManager.LeftOnTriggerExit(other);
    }
}