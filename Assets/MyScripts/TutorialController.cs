using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {
    public GameObject runningTutorial;
    public GameObject jumpingTutorial;
    public GameObject ball;
    public GameObject goal;
    public GameObject enemy;

    public AudioSource audioSource;
    public AudioClip welcomeClip;
    public AudioClip runningIntroClip;
    public AudioClip runningDirectionsClip;
    public AudioClip jumpingIntroClip;
    public AudioClip jumpingDirectionsClip;
    public AudioClip grabbingIntroClip;
    public AudioClip grabbingDirectionsClip;
    public AudioClip throwingIntroClip;
    public AudioClip throwingDirectionsClip;
    public AudioClip endClip;
    

	// Use this for initialization
	void Start () {
        StartCoroutine(BeginRunning());
	}

    IEnumerator BeginRunning()
    {
        yield return new WaitForSeconds(2);
        audioSource.clip = welcomeClip;
        audioSource.Play();
        yield return new WaitForSeconds(5f);
        audioSource.clip = runningIntroClip;
        audioSource.Play();
        runningTutorial.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        audioSource.clip = runningDirectionsClip;
        audioSource.Play();
    }

    public void ValidateRunningGoJumping()
    {
        audioSource.clip = jumpingDirectionsClip;
        audioSource.Play();
        jumpingTutorial.SetActive(true);
    }

    public void ValidateJumpingGoGrabbing()
    {
        audioSource.clip = grabbingDirectionsClip;
        audioSource.Play();
        ball.SetActive(true);
    }

    public void ValidateGrabbingGoThrowing()
    {
        audioSource.clip = throwingDirectionsClip;
        audioSource.Play();
        goal.SetActive(true);
    }

    public IEnumerator ValidateThrowingGoHitting()
    {
        audioSource.clip = endClip;
        audioSource.Play();
        goal.SetActive(true);
        yield return new WaitForSeconds(5f);

        SteamVR_LoadLevel.Begin("VikingMenu");
    }
}
