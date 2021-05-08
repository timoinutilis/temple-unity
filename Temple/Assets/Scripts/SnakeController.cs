using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    enum State
    {
        Idle,
        Walk,
        Turn
    }


    public float speed = 6.0f;

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
        
        timeCounter -= Time.deltaTime;
        switch (state)
        {
            case State.Idle:
                if (timeCounter <= 0)
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
        }

        if (!characterController.isGrounded)
        {
            characterController.Move(Vector3.down * 20 * Time.deltaTime);
        }
    }
}
