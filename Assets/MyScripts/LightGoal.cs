using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGoal : MonoBehaviour {

    public int speed;
    public Light light;
    private bool up = true;
	
	// Update is called once per frame
	void Update () {
		if (up)
        {
            light.intensity = light.intensity + Time.deltaTime * speed;
        } else
        {
            light.intensity = light.intensity - Time.deltaTime * speed;
        }

        if (light.intensity < 1)
        {
            up = true;
        }
        if (light.intensity > 8)
        {
            up = false;
        }
	}

    public void Light()
    {
        light.intensity = 1;
        light.gameObject.SetActive(true);
    }

    public void Reset()
    {
        light.gameObject.SetActive(false);
    }
}
