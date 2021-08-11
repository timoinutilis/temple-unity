using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    enum State
    {
        Idle,
        Walk,
        Turn,
        Look
    }


    public Transform headBone;
    public float speed = 6.0f;
    public float lookDistance = 20.0f;

    private CharacterController characterController;
    private Animator animator;
    private State state = State.Idle;
    private float timeCounter = 0;
    private float nextRotation = 0;
    private float lookRatio = 0;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        timeCounter = Random.Range(0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = Camera.main.transform.position - transform.position;
        float dist = diff.magnitude;
        float dot = Vector3.Dot(transform.forward, diff.normalized);
        bool isInView = dist <= lookDistance && dot > 0;
        if (state != State.Look)
        {
            lookRatio = Mathf.Max(0, lookRatio - Time.deltaTime);

            if (isInView)
            {
                state = State.Look;
                animator.SetFloat("speed", 0);
                animator.SetBool("isLooking", true);
            }
        }


        timeCounter -= Time.deltaTime;
        switch (state)
        {
            case State.Idle:
                if (timeCounter > 0)
                {
                }
                else
                {
                    state = State.Turn;
                    timeCounter = 1;
                    animator.SetFloat("speed", 0.25f);
                    nextRotation = Random.Range(-90, 90);
                }
                break;
            case State.Turn:
                if (timeCounter > 0)
                {
                    transform.Rotate(new Vector3(0, Time.deltaTime * nextRotation, 0));
                }
                else
                {
                    state = State.Walk;
                    animator.SetFloat("speed", speed);
                    timeCounter = Random.Range(5, 10);
                }
                break;
            case State.Walk:
                characterController.Move(transform.forward * speed * Time.deltaTime);
                if (timeCounter <= 0)
                {
                    state = State.Idle;
                    animator.SetFloat("speed", 0);
                    timeCounter = Random.Range(5, 10);
                }
                break;
            case State.Look:
                if (isInView)
                {
                    lookRatio = Mathf.Min(1, lookRatio + Time.deltaTime);
                }
                else
                {
                    state = State.Idle;
                    animator.SetFloat("speed", 0);
                    animator.SetBool("isLooking", false);
                    timeCounter = 1.0f;
                }
                break;
        }

        if (characterController.isGrounded)
        {

        }
        else
        {
            characterController.Move(Vector3.down * 20 * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        if (lookRatio > 0)
        {
            Quaternion q = Quaternion.LookRotation(Camera.main.transform.position - headBone.position);
            q *= Quaternion.Euler(Vector3.right * 90);
            headBone.rotation = Quaternion.Lerp(headBone.rotation, q, lookRatio * 0.8f);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("hit: " + hit.normal);
    }
}
