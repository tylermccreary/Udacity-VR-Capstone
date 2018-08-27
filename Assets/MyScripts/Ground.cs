using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Throwable")
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
