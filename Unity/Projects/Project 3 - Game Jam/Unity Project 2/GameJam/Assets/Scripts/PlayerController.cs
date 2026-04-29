using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float mouseSensitivity = 2f;

    private float verticalVelocity;
    private float xRotation;
    private float yRotation;

    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
        HandlePlatformMotion();
    }

    private void HandleLook()
    {
        float mouseInputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseInputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation += mouseInputX;
        yRotation -= mouseInputY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(0f, xRotation, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        Vector3 move = (transform.right * horizontal + transform.forward * vertical) * currentSpeed;

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        characterController.Move(move * Time.deltaTime);
    }

    private void HandlePlatformMotion()
    {
        if (currentPlatform != null)
        {
            Vector3 platformDelta = currentPlatform.position - lastPlatformPosition;

            if (platformDelta != Vector3.zero)
            {
                characterController.Move(platformDelta);
            }

            lastPlatformPosition = currentPlatform.position;
        }
    }

    // private void OnControllerColliderHit(ControllerColliderHit hit)
    // {
    //     if (hit.collider.CompareTag("Platform"))
    //     {
    //         if (Vector3.Dot(hit.normal, Vector3.up) > 0.5f)
    //         {
    //             if (currentPlatform != hit.collider.transform)
    //             {
    //                 currentPlatform = hit.collider.transform;
    //                 lastPlatformPosition = currentPlatform.position;
    //             }
    //         }
    //     }
    // }

    // private void LateUpdate()
    // {
    //     if (!characterController.isGrounded)
    //     {
    //         currentPlatform = null;
    //     }
    // }
}