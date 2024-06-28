using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float rotationSpeed = 9f;
    private float gravity = -9.81f;
    private float yVelocity = 0.0f;
    private float yVelocityWhenGrounded = -0.1f;
    [SerializeField]
    private float jumpHeight = 1f;
    private float jumpTime = 0.5f;
    float initialJumpVelocity = 0f;
    float rotationY;
    CharacterController cc;

    // Number of allowed jumps (1 for single jump, 2 for double jump, etc.)
    [SerializeField]
    private int maxJumps = 2;
    private int remainingJumps;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        HandleJump();
        Cursor.lockState = CursorLockMode.Locked;
        remainingJumps = maxJumps; // Initialize remaining jumps
    }

    public void HandleJump()
    {
        float timeToApex = jumpTime / 2f;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * jumpHeight) / timeToApex;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // Reset jumps when grounded
        if (cc.isGrounded && yVelocity < 0.0f)
        {
            yVelocity = yVelocityWhenGrounded;
            remainingJumps = maxJumps; // Reset the number of jumps when grounded
        }
        else
        {
            // Apply gravity when not grounded
            yVelocity += gravity * Time.deltaTime;
        }

        // Handle jumping
        if (Input.GetButtonDown("Jump") && remainingJumps > 0)
        {
            yVelocity = initialJumpVelocity;
            remainingJumps--; // Decrease the number of jumps left
        }

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(hInput, 0, vInput);

        // Clamp the magnitude of the movement vector to 1.0f
        movement = Vector3.ClampMagnitude(movement, 1.0f);

        // Convert local movement to world coordinates
        movement = transform.TransformDirection(movement);
        movement *= speed;
        //set y velocity 
        movement.y = yVelocity;

        movement *= Time.deltaTime;
        cc.Move(movement);

        float deltaHoriz = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, deltaHoriz, 0);
    }

    // Respawn the player at a set position
    public void Respawn(Vector3 spawnPoint)
    {
        // Stop falling
        yVelocity = yVelocityWhenGrounded;
        // Set the player to a given position
        transform.position = spawnPoint;
        // Apply transform changes to the physics engine manually
        Physics.SyncTransforms();
    }
}
