using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	CharacterController characterController;
	
	public Camera playerCamera;
	public float speed = 6.0f;
	public float gravity = 20.0f;
	public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
	
	private Vector3 move = Vector3.zero;
	private float rotationX = 0;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
    	if (characterController.isGrounded)
    	{
    		move = speed * transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
    	}
    	else
    	{
    		move.y -= gravity * Time.deltaTime;
    	}

        characterController.Move(move * Time.deltaTime);

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
