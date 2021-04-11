using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	CharacterController characterController;
	
	public Camera playerCamera;
    public GameObject flashlight;
    public Transform autoLightRay;
	public float speed = 6.0f;
	public float gravity = 20.0f;
	public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public Transform grab;
    public float grabDistance = 2.0f;
    public float grabMaxDistance = 4.0f;
    public float grabForceFactor = 10.0f;

    private Vector3 move = Vector3.zero;
	private float rotationX = 0;
    private Rigidbody grabbed;

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

        Ray groundRay = new Ray(autoLightRay.position, autoLightRay.forward);
        RaycastHit groundHit;

        if (Physics.Raycast(groundRay, out groundHit, 5.0f, 1 << 6))
        {
            if (!flashlight.activeSelf)
            {
                flashlight.SetActive(true);
            }
        }
        else if (flashlight.activeSelf)
        {
            flashlight.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(grab.position, grab.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, grabDistance, 1 << 7))
            {
                grabbed = hit.rigidbody;
                //grabbed.isKinematic = true;
                Debug.Log("Clicked on " + hit.transform.name);
            }
            else
            {
                Debug.Log("Nothing hit");
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (grabbed != null)
            {
                //grabbed.isKinematic = false;
                grabbed = null;
            }
        }
    }

    void FixedUpdate()
    {
        if (grabbed != null)
        {
            if (Vector3.Distance(grab.position, grabbed.position) <= grabMaxDistance)
            {
                grabbed.AddForce(move * grabForceFactor);
            }
        }
    }
    
}
