using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour {

    public InputManager inputManager;

    private void OnTriggerStay(Collider other)
    {
        inputManager.SetGrounded(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        inputManager.BackToGround();
    }

    private void OnTriggerExit(Collider other)
    {
        inputManager.SetGrounded(false);
    }
}
