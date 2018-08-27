using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandInRange : MonoBehaviour {
    public bool leftHand;
    public AIDummyController aIDummyController;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Throwable")
        {
            if (leftHand)
            {
                aIDummyController.SetBallInRangeLeft(true);
            }
            else
            {
                aIDummyController.SetBallInRangeLeft(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Throwable")
        {
            if (leftHand)
            {
                aIDummyController.SetBallInRangeLeft(false);
            }
            else
            {
                aIDummyController.SetBallInRangeLeft(false);
            }
        }
    }
}
