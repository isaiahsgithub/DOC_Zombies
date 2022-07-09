using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Air Movement Settings")]
    [SerializeField] private float jumpSpeed = 7f;
    [SerializeField] private bool enableAirMovement = true;

    [Header("Player Ground Movement Settings")]
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] public float mouseSensitivity = 300f;
    [SerializeField] private Transform camera;

    [Header("Grounding Factors")]
    [SerializeField] private float gravityFactor = 1f;
    [SerializeField] private Transform footPos;
    [SerializeField] private LayerMask groundLayers;

    private float verticalRotation = 0f;
    private float verticalSpeed = 0f;

    public bool isGrounded = false;
    private float distanceToGround = 0.2f;

    private CharacterController controller;
    private Animator animator;

    private void Awake()
    {
        //Gets controller from player before game starts
        controller = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        checkIfRun();
        //Checks our player position - specifically if we are grounded
        RaycastHit collision;
        if(Physics.Raycast(footPos.position, Vector3.down, out collision, distanceToGround, groundLayers))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        //If we are in the air, set the vertical speed
        if (!isGrounded)
        {
            verticalSpeed += gravityFactor * -9.81f * Time.deltaTime;
        }
        //If we aren't grounded, reset vertical speed and end jumping animation (if playing)
        else
        {
            animator.SetBool("isOnGround", true);
            verticalSpeed = 0f;
        }

        //Adjust the rotations based on the mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        verticalRotation -= mouseY;
        //Can only look up/down to a maximum of 90 degrees (so you don't break your neck looking)
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        camera.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);

        Vector3 x = Vector3.zero;
        Vector3 y = Vector3.zero;
        Vector3 z = Vector3.zero;

        //Handle jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            animator.SetBool("isOnGround", false);
            animator.SetTrigger("jumped");
            verticalSpeed = jumpSpeed;
            isGrounded = false;
            y = transform.up * verticalSpeed;
        }
        else if (!isGrounded)
        {
            y = transform.up * verticalSpeed;
        }


        //Handle walking/running
        if (isGrounded || enableAirMovement)
        {
            x = transform.right * Input.GetAxis("Horizontal") * movementSpeed;
            z = transform.forward * Input.GetAxis("Vertical") * movementSpeed;
        }

        //Handle motion
        Vector3 movement = (x + y + z) * Time.deltaTime;
        if(movement != new Vector3(0, 0, 0) && isGrounded)
        {
            animator.SetFloat("Speed" ,movementSpeed);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
        controller.Move(movement);
    }

    //If you hold down shift, your speed increases by 1.5f (3.0f -> 4.5f)
    private void checkIfRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed = 4.5f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = 3f;
        }
    }

}
