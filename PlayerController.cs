using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRgbd;
    public Transform cameraTransform;

    [SerializeField]
    public float inputX, inputY, inputZ, inputSprint, inputLookX, inputLookY;
    [SerializeField]
    private int isGrounded = 0;

    public float maxSpeed = 10f;

    public float speedMultiplier = 3, sprintMultiplier = 1.2f, jumpMultiplier = 15f;

    public float maxHitDistance = 0.5f;
    private int layerMask = ~(1 << 7); //bit shift to ignore every layer except 7 and ~ to ignore only layer 7
    private RaycastHit hit;

    // Update is called once per frame
    void Update()//non physics
    {
        //reading inputs
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        inputY = Input.GetAxis("Jump");
        inputSprint = Input.GetAxis("Sprint");

        inputLookX = Input.GetAxis("Mouse X");
        inputLookY = Input.GetAxis("Mouse Y");

        //rotating camera
    }

    void FixedUpdate()//physics
    {
        //setting inputs
        //jump check
        Debug.DrawRay(playerRgbd.transform.position, playerRgbd.transform.TransformDirection(Vector3.down) * hit.distance, Color.blue);
        if (Physics.Raycast(playerRgbd.transform.position, playerRgbd.transform.TransformDirection(Vector3.down), out hit, maxHitDistance, layerMask))//if hit
        {
            isGrounded = 1;
        }

        //movement inputs
        Vector3 velocity = new Vector3(
            inputX * (speedMultiplier + inputSprint * sprintMultiplier), 
            playerRgbd.velocity.y + (inputY * isGrounded) * jumpMultiplier, 
            inputZ * (speedMultiplier + inputSprint * sprintMultiplier));

        isGrounded = 0;

        //moving player
        playerRgbd.transform.Rotate(cameraTransform.transform.forward, Space.Self);
        playerRgbd.velocity = velocity;
    }
}
