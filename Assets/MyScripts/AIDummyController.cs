using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDummyController : MonoBehaviour
{
    public GameManager gameManager;
    public float moveSpeed;
    public Player player;
    public Ball ball;
    public enum StateEnum { Idle, Attacking, Grabbing, Releasing, Moving, Stunned, Passing };
    public Collider leftHandTrigger;
    public Collider rightHandTrigger;
    public GameObject head;
    private bool leftHandPossession;
    public float rotationSpeed;
    public float runSpeed;
    public float releaseSpeed;
    public InputManager inputManager;
    public Rigidbody rigid;
    public Material bluePlayerMaterial;
    public Material orangePlayerMaterial;
    public Animator animator;
    public float rangeMultiplier;

    public Transform goalL;
    public Transform goalC;
    public Transform goalR;
    private Vector3 releasePoint;
    private bool releaseFound = false;

    private StateEnum state;
    private float stunnedTime;
    private bool ballInRangeLeft;
    private bool ballInRangeRight;
    private GameObject target;

    private GameObject closestPlayer;
    private float lastGrab = 0.0f;
    private float releaseRange;


    private void Start()
    {
        stunnedTime = 0.0f;
        target = ball.gameObject;
        state = StateEnum.Moving;
        Material playerMaterial;
        if (player.team == "orange")
        {
            playerMaterial = orangePlayerMaterial;
        }
        else
        {
            playerMaterial = bluePlayerMaterial;
        }
        leftHandTrigger.gameObject.GetComponent<Renderer>().material = playerMaterial;
        rightHandTrigger.gameObject.GetComponent<Renderer>().material = playerMaterial;
        head.GetComponent<Renderer>().material = playerMaterial;
        releaseRange = Random.Range(20, 30) * rangeMultiplier;
    }

    private void Update()
    {
        if (state == StateEnum.Passing)
        {
            Vector3 targetDir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
            float step = rotationSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            if (Vector3.Angle((target.transform.position - transform.position), transform.forward) > 0.5f)
            {
                return;
            }
            if (!releaseFound)
            {
                FindReleasePoint();
            }
            ball.transform.SetParent(null);
            Rigidbody rigid = ball.transform.GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            Rigidbody rigidController;
            if (leftHandPossession)
            {
                rigidController = rightHandTrigger.gameObject.GetComponent<Rigidbody>();
            }
            else
            {
                rigidController = leftHandTrigger.gameObject.GetComponent<Rigidbody>();
            }
            rigid.velocity = (target.transform.position - releasePoint) * inputManager.throwSpeed * Time.deltaTime * releaseSpeed * 5;
            state = StateEnum.Moving;
            releaseFound = false;
            ball.gameObject.layer = LayerMask.NameToLayer("Ball");
            state = StateEnum.Moving;

        }
        if (state != StateEnum.Moving)
        {
            animator.SetBool("moving", false);
        }
        if (state == StateEnum.Moving)
        {
            animator.SetBool("moving", false);
            if (ball.transform.parent == leftHandTrigger.transform || ball.transform.parent == rightHandTrigger.transform)
            {

                if (Vector3.Distance(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), transform.position) < releaseRange)
                {
                    releaseRange = Random.Range(20, 50) * rangeMultiplier;
                    Shoot();
                }
                else
                {
                    //Rotate
                    Vector3 targetDir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
                    float step = rotationSpeed * Time.deltaTime;
                    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDir);

                    //Run
                    rigid.AddForce(transform.forward * runSpeed * Time.deltaTime);
                }
            }
            else
            {
                animator.SetBool("moving", true);
                if (ball.transform.parent != null)
                {
                    Player ballPlayer = ball.transform.parent.parent.gameObject.GetComponent<Player>();
                    if (ballPlayer != null)
                    {
                        if (ballPlayer.team == player.team)
                        {
                            target = ball.gameObject;
                            rigid.AddForce(runSpeed / Vector3.Distance(transform.position, target.transform.position) * Time.deltaTime * Vector3.Normalize(new Vector3(transform.position.x - target.transform.position.x, 0, transform.position.z - target.transform.position.z)));
                        }
                    }
                }
                if (ballInRangeLeft || ballInRangeRight)
                {
                    state = StateEnum.Grabbing;
                }

                target = ball.gameObject;
                Vector3 targetDir = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
                float step = rotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
                rigid.AddForce(transform.forward * runSpeed * Time.deltaTime);
            }
        }
        else if (state == StateEnum.Grabbing)
        {

            if (ball.transform.parent != null && ball.transform.parent.parent.gameObject.GetComponent<Player>().team == player.team && ball.transform.parent.parent.gameObject != gameObject)
            {
                state = StateEnum.Moving;
                return;
            }

            if (ball.transform.parent != leftHandTrigger.transform && ball.transform.parent != rightHandTrigger.transform)
            {
                rigid.AddForce(transform.forward * runSpeed * Time.deltaTime);
                if (ballInRangeLeft)
                {
                    leftHandTrigger.transform.position = leftHandTrigger.transform.position + Vector3.Normalize(ball.transform.position - leftHandTrigger.transform.position) * releaseSpeed * Time.deltaTime / rangeMultiplier * 2;
                }
                else if (ballInRangeRight)
                {
                    rightHandTrigger.transform.position = rightHandTrigger.transform.position + Vector3.Normalize(ball.transform.position - rightHandTrigger.transform.position) * releaseSpeed * Time.deltaTime / rangeMultiplier * 2;
                }
                else
                {
                    state = StateEnum.Moving;
                }
            }
            else
            {
                state = StateEnum.Moving;
                PickGoal();
            }
        }
        else if (state == StateEnum.Releasing)
        {
            if (!releaseFound)
            {
                FindReleasePoint();
            }
            else
            {
                if (ball.transform.parent == leftHandTrigger.transform || ball.transform.parent == rightHandTrigger.transform)
                {

                    if (Vector3.Distance(releasePoint, ball.transform.parent.position) < 0.1f ||
                        (Vector3.Distance(ball.transform.parent.position, transform.position) > 1.0f))
                    {
                        ball.transform.SetParent(null);
                        Rigidbody rigid = ball.transform.GetComponent<Rigidbody>();
                        rigid.isKinematic = false;
                        Rigidbody rigidController;
                        if (leftHandPossession)
                        {
                            rigidController = rightHandTrigger.gameObject.GetComponent<Rigidbody>();
                        }
                        else
                        {
                            rigidController = leftHandTrigger.gameObject.GetComponent<Rigidbody>();
                        }
                        rigid.velocity = Vector3.Normalize(target.transform.position - releasePoint + new Vector3(0, 10, 0)) * inputManager.throwSpeed * Time.deltaTime * releaseSpeed * Random.Range(80, 150);
                        state = StateEnum.Moving;
                        releaseFound = false;
                        ball.gameObject.layer = LayerMask.NameToLayer("Ball");
                    }
                    else
                    {
                        if (leftHandPossession)
                        {
                            leftHandTrigger.transform.position = leftHandTrigger.transform.position + Vector3.Normalize(target.transform.position - leftHandTrigger.transform.position) * releaseSpeed * Time.deltaTime;
                        }
                        else
                        {
                            rightHandTrigger.transform.position = rightHandTrigger.transform.position + Vector3.Normalize(target.transform.position - rightHandTrigger.transform.position) * releaseSpeed * Time.deltaTime;
                        }
                    }
                }
                else
                {
                    state = StateEnum.Moving;
                }
            }
        }
    }

    public void GrabRight()
    {
        if (ball.transform.parent != leftHandTrigger.transform)
        {
            ball.gameObject.layer = LayerMask.NameToLayer("BallPossession");
            ball.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            ball.transform.parent = rightHandTrigger.transform;
            leftHandPossession = false;
            lastGrab = Time.time;
        }
    }

    public void GrabLeft()
    {
        if (ball.transform.parent != rightHandTrigger.transform)
        {
            ball.gameObject.layer = LayerMask.NameToLayer("BallPossession");
            ball.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            ball.transform.parent = leftHandTrigger.transform;
            leftHandPossession = true;
        }
    }

    public void SetBallInRangeLeft(bool inRange)
    {
        ballInRangeLeft = inRange;
    }

    public void SetBallInRangeRight(bool inRange)
    {
        ballInRangeRight = inRange;
    }

    public void GetStunned()
    {
        state = StateEnum.Stunned;
    }

    public void Shoot()
    {
        state = StateEnum.Releasing;
    }

    public void FindReleasePoint()
    {
        if (ball.transform.parent == leftHandTrigger.transform || ball.transform.parent == rightHandTrigger.transform)
        {
            releasePoint = ball.transform.parent.position + Vector3.Normalize((new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z) - ball.transform.parent.position));
            releaseFound = true;
        }
        else
        {
            state = StateEnum.Moving;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        Player otherPlayer = col.gameObject.GetComponentInChildren<Player>();
        if (otherPlayer != null && otherPlayer.team != player.team)
        {
            if (closestPlayer == null)
            {
                closestPlayer = col.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (closestPlayer == col.gameObject)
        {
            closestPlayer = null;
        }
    }

    private void PickGoal()
    {
        int goalPick = Random.Range(0, 2);
        switch (goalPick)
        {
            case 0:
                target = goalC.gameObject;
                break;
            case 1:
                target = goalL.gameObject;
                break;
            default:
                target = goalC.gameObject;
                break;
        }
    }

    public void PassToPlayer(GameObject openPlayer)
    {
        target = openPlayer;
        state = StateEnum.Passing;
    }
}
