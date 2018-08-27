using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour {

    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Throwable")
        {
            gameManager.TriggerOutOfBounds();
        }
    }
}
