using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightController : MonoBehaviour
{
    public GameManager gameManager;
    public InputManager inputManager;
    public LineRenderer menuLineRenderer;
    public Color menuLineColor;
    public LayerMask rayLayerMask;

    private Rigidbody controllerRigid;
    private ButtonClickController button;

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
        menuLineRenderer.material.color = menuLineColor;
        inputManager.SetRightController(Controller);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.menuScene)
        {
            menuLineRenderer.gameObject.SetActive(true);
        } else
        {
            menuLineRenderer.gameObject.SetActive(false);
        }
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            inputManager.RightTriggerSqueeze();
        }
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            inputManager.RightTriggerRelease();
        }

        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip))
        {
            inputManager.RightGripSqueeze();
        }

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            inputManager.RightTouchPadPressUp(Controller.GetAxis().y);
        }

        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            inputManager.RightTouchPadPress();
        }

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            inputManager.RightAppMenuClick();
        }

        if (gameManager.menuScene)
        {
            menuLineRenderer.SetPosition(0, transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, rayLayerMask))
            {
                menuLineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, hit.point.z));
                ButtonClickController buttonClickController = hit.collider.gameObject.GetComponent<ButtonClickController>();
                if (buttonClickController != null)
                {
                    button = buttonClickController;
                    buttonClickController.Hover();
                    if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                    {
                        buttonClickController.Click();
                    }
                }
            }
            else
            {
                if (button != null)
                {
                    button.UnHover();
                }
                menuLineRenderer.SetPosition(1, transform.position + (10 * transform.forward));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        inputManager.RightOnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        inputManager.RightOnTriggerExit(other);
    }
}
