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

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        timeCounter = Random.Range(0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = 0.0f;
        timeCounter -= Time.deltaTime;
        switch (state)
        {
            case State.Idle:
                if (timeCounter > 0)
                {
                    dist = (transform.position - Camera.main.transform.position).magnitude;
                    if (dist <= lookDistance)
                    {
                        state = State.Look;
                        animator.SetBool("isLooking", true);
                    }
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
                dist = (transform.position - Camera.main.transform.position).magnitude;
                if (dist <= lookDistance)
                {
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

        if (!characterController.isGrounded)
        {
            characterController.Move(Vector3.down * 20 * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        switch (state)
        {
            case State.Look:
                Quaternion q = Quaternion.LookRotation(Camera.main.transform.position - headBone.position);
                q *= Quaternion.Euler(Vector3.right * 90);
                headBone.rotation = Quaternion.Lerp(headBone.rotation, q, 0.6f);
                break;
        }
    }

}
