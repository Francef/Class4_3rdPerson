using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speed = 6f;
    float rotSpeed = 420f; // deg per sec
    [SerializeField]
    private CharacterController charController;
    float gravity = -9.81f;
    float yVelocity = 0.0f;
    float yVelocityWhenGrounded = -4.0f;
    float jumpHeight = 3.0f;
    float jumpTime = 0.5f;
    float initialJumpVelocity;
    int jumpsRemaining;

    private void Start()
    {
        // calculate gravity for our jump variables
        float timeToApex = jumpTime / 2.0f;
        gravity = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);

        // calculate the jump velocity needed to match up with jump variables
        initialJumpVelocity = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    // Update is called once per frame
    void Update()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizInput, 0, vertInput);

        // ensure diagonal movement doesn't exceed horiz/vert movement speed
        movement = Vector3.ClampMagnitude(movement, 1.0f);
        // convert from loval to world coordinates
        movement = transform.TransformDirection(movement);

        movement *= speed;              // based movement on speed

        // calculate downward velocity
        yVelocity += gravity * Time.deltaTime;

        // if we are on the ground and we are falling
        if(charController.isGrounded && yVelocity < 0.0)
        {
            // set y velocity to something constant
            yVelocity = yVelocityWhenGrounded;
        }

        if(Input.GetButtonDown("Jump"))
        {
            if (charController.isGrounded)
            {
                // player can make 2 jumps from ground
                jumpsRemaining = 2;
            }
            
            // player will need to come to ground before jumping again
            if(jumpsRemaining != 0)
            {
                yVelocity = initialJumpVelocity;
                jumpsRemaining -= 1;
            }  
        }

        movement.y = yVelocity;

        movement *= Time.deltaTime;     // base movement on time

        //transform.Translate(movement);
        charController.Move(movement);

        Vector3 rotation = Vector3.up * rotSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
        transform.Rotate(rotation);
    }

    // Respawn the player at set position
    public void Respawn(Vector3 spawnPoint)
    {
        // stop falling
        yVelocity = yVelocityWhenGrounded;
        // set the player to a given position
        transform.position = spawnPoint;
        // apply transform changes to the physics engine manually
        Physics.SyncTransforms();
    }
}
