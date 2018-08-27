using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTutorialCheckpoint : MonoBehaviour
{

    public TutorialController tutorialController;
    public MeshRenderer mRenderer;
    public Material successMaterial;
    private bool jumpComplete = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!jumpComplete)
        {
            jumpComplete = true;
            tutorialController.ValidateJumpingGoGrabbing();
            mRenderer.material = successMaterial;
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
