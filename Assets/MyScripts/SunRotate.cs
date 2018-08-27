using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotate : MonoBehaviour {

    public float speed;
    public Light light;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(1, 0, 0) * Time.deltaTime * speed);
        if ((transform.rotation.eulerAngles.x % 360 < 5) || transform.rotation.eulerAngles.x % 360 > 175)
        {
            light.color = Color.black;
        } else
        {
            light.color = Color.white;
        }
	}
}
