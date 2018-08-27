using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.time = 0.2f;
        audioSource.Play();
    }
}
