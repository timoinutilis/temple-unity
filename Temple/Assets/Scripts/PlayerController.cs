using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Paused,
        Playing
    }

    public Camera playerCamera;
    public GameObject flashlight;
    public AudioSource audioSource;
    public Material lightBlockerMaterial;
    public float speed = 6.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float pauseRotationSpeed = 1.0f;
    public Transform grab;
    public float grabDistance = 2.0f;
    public float grabMaxDistance = 4.0f;
    public float grabForceFactor = 10.0f;

    private State state = State.Paused;
    private CharacterController characterController;
    private Vector3 move = Vector3.zero;
	private float rotationX = 0;
    private Rigidbody grabbed;
    private bool isIndoor = false;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // set feet on ground
        characterController.Move(new Vector3(0, -1, 0));

        SetState(State.Paused);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Paused:
                UpdatePaused();
                break;
            case State.Playing:
                UpdatePlaying();
                break;
        }
    }

    private void UpdatePaused()
    {
        transform.Rotate(new Vector3(0, pauseRotationSpeed * Time.deltaTime, 0));
        UpdateFlashlight();
    }

    private void UpdatePlaying()
    {
        if (characterController.isGrounded)
        {
            move = speed * transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            if (move.magnitude > 0)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
        else
        {
            move.y -= gravity * Time.deltaTime;
            //if (audioSource.isPlaying)
            //{
            //    audioSource.Pause();
            //}
        }

        characterController.Move(move * Time.deltaTime);

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        UpdateFlashlight();

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

    void UpdateFlashlight()
    {
        if (isIndoor)
        {
            if (RenderSettings.ambientIntensity > 0)
            {
                RenderSettings.ambientIntensity = Mathf.Max(0, RenderSettings.ambientIntensity - Time.deltaTime / 0.5f);
                lightBlockerMaterial.color = new Color(0, 0, 0, RenderSettings.ambientIntensity);
            }
            if (!flashlight.activeSelf)
            {
                flashlight.SetActive(true);
            }
        }
        else
        {
            if (RenderSettings.ambientIntensity < 1)
            {
                RenderSettings.ambientIntensity = Mathf.Min(1, RenderSettings.ambientIntensity + Time.deltaTime / 0.5f);
                lightBlockerMaterial.color = new Color(0, 0, 0, RenderSettings.ambientIntensity);
            }
            if (flashlight.activeSelf)
            {
                flashlight.SetActive(false);
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

    public void SetState(State value)
    {
        state = value;
        switch (state)
        {
            case State.Paused:
                break;
            case State.Playing:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isIndoor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isIndoor = false;
    }

    public void WriteGameState(GameState gameState)
    {
        gameState.playerPosition = characterController.transform.position;
        gameState.playerRotation = characterController.transform.rotation;
    }

    public void ReadGameState(GameState gameState)
    {
        characterController.enabled = false;
        characterController.transform.SetPositionAndRotation(gameState.playerPosition, gameState.playerRotation);
        characterController.enabled = true;
    }

}
