using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameManager gameManager;
    public Material goalMaterial;
    public Material startMaterial;
    public MeshRenderer lightMeshRenderer;
    public SphereCollider sCollider;

    public TutorialController tutorial;
    private bool tutorialDone = false;
    public string team;
    public AudioSource audioSource;

    public LightGoal lightGoal;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Throwable")
        {
            lightMeshRenderer.material = goalMaterial;

            if (tutorial != null && !tutorialDone)
            {
                tutorialDone = true;
                StartCoroutine(tutorial.ValidateThrowingGoHitting());
            } else if (gameManager != null)
            {
                sCollider.enabled = false;
                gameManager.TriggerGoal(this);
                if (audioSource != null)
                {
                    audioSource.Play();
                }
                lightGoal.Light();
            }
        }
    }

    public void ResetGoal()
    {
        lightMeshRenderer.material = startMaterial;
        sCollider.enabled = true;
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        lightGoal.Reset();
    }
}
