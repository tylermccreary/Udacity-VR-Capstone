using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTutorialCheckpoint : MonoBehaviour {

    public TutorialController tutorialController;
    public MeshRenderer renderer;
    public Material successMaterial;
    private bool runComplete = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!runComplete)
        {
            runComplete = true;
            tutorialController.ValidateRunningGoJumping();
            renderer.material = successMaterial;
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
