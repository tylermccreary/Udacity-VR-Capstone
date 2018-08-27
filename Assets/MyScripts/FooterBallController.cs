using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooterBallController : MonoBehaviour {

    public Transform footerBall;

    // Update is called once per frame
    void Update () {
        footerBall.position = new Vector3(transform.position.x, footerBall.position.y, transform.position.z);
	}
}
