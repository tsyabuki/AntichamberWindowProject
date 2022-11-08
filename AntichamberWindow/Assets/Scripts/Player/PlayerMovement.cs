using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity = -10;
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask groundCheckLayermask;
    [Space]
    [Header("References")]
    [SerializeField] private CharacterController cc;
    [SerializeField] private Transform groundCheck;

    private Vector3 velocity;
    private bool isGrounded = false;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();        
    }

    void OnDrawGizmosSelected()
    {
        // Draws the ground check ray
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        
        Vector3 direction = Vector3.down * groundCheckDistance;
        Gizmos.DrawRay(groundCheck.transform.position, direction);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(groundCheck.transform.position, Vector3.down, groundCheckDistance, groundCheckLayermask);

        if(isGrounded && velocity.y < 0f)
        {
            velocity.y = -10f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z);

        cc.Move(move * moveSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);
    }
}
