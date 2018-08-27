using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHand : MonoBehaviour {

    public bool leftHand;
    public AIDummyController aIDummyController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Throwable")
        {
            if (leftHand)
            {
                aIDummyController.GrabLeft();
            }
            else
            {
                aIDummyController.GrabRight();
            }
        }
    }
}
