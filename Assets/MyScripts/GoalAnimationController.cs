using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalAnimationController : MonoBehaviour {

    public Animator animator;

    private void Start()
    {
        animator.SetTrigger("Begin");
    }
}
