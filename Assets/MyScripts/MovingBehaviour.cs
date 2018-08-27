using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingBehaviour : MonoBehaviour {

    private Transform target;

    public abstract void Move();
    public abstract void FindTarget();
}
