using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public float jumpForce;
    public float movementSpeed;
    public Transform feetBox;
    public LayerMask groundLayers;

    float _mx;
    bool _bJumpRequest;

    void Update()
    {
        _mx = Input.GetAxisRaw("Horizontal");
        bool bGrounded = Physics2D.OverlapCircle(feetBox.position, 0.5f, groundLayers);

        if (bGrounded)
        {

            if (Input.GetButtonDown("Jump"))
            {
                _bJumpRequest = true;
            }
        }


        if (Mathf.Abs(_mx) > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Sign(_mx), 1f, 1f); // flips the player if needed
        }
    }

    private void FixedUpdate()
    {
        // move (Mass = 1, JumpForce = 17, movementSpeed = 10) 
        Vector2 movement = new Vector2(_mx * movementSpeed, rb.velocity.y);
        rb.velocity = movement;

        /* For an ice level?  Mass = 1, JumpForce = 15, movementSpeed = 35 
        Vector2 movement = new Vector2(_mx * movementFSpeed, 0);
        rb.AddForce(movement);
        */

        // jump
        if (_bJumpRequest)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _bJumpRequest = false;
        }

    }
}
