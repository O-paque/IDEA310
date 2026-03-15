using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private Camera _camera;
    public float speed = 5.0f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        _characterController.Move(GetMoveDirection());

        var mouseInputX = Input.GetAxis("Mouse X");
        var mouseInputY = Input.GetAxis("Mouse Y");

        xRotation += mouseInputX;
        yRotation -= mouseInputY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(new Vector3(0f, xRotation, 0f));
        _camera.transform.rotation = Quaternion.Euler(new Vector3(yRotation, xRotation, 0f));
    }

    private Vector3 GetMoveDirection()
    {
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");

        var forwardDirection = transform.forward * verticalInput * Time.deltaTime * speed;
        var sideDirection = transform.right * horizontalInput * Time.deltaTime * speed;

        return forwardDirection + sideDirection;
    }
}
