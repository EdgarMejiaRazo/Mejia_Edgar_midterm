using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float jumpForce = 500.0f;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask whatIsGround;


    //Private Variables

    private Rigidbody2D rBody;
    private Animator anim;
    private bool isGrounded = false;
    private bool isDucking = false;
    private bool isFacingRight = true;
    //AudioSource jumpSound;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //jumpSound = GetComponent<AudioSource>();
    }

    //pHYSICS
    private void FixedUpdate()
    {
        float horiz = Input.GetAxis("Horizontal");
        isGrounded = GroundCheck();
        isDucking = false;

        //Jump
        if (isGrounded && Input.GetAxis("Jump") > 0)
        {
        //jumpSound.Play();
         rBody.AddForce(new Vector2(0.0f, jumpForce));
         isGrounded = false;
        }

        //Jump
        if (isGrounded && Input.GetAxis("Vertical") < 0)
        {
            //jumpSound.Play();

            isDucking = true;
        }


        rBody.velocity = new Vector2(horiz * speed, rBody.velocity.y);

        // Check if flip the player

        if ((isFacingRight && rBody.velocity.x < 0) || (!isFacingRight && rBody.velocity.x > 0))
        {
        Flip();
        }
        else if (!isFacingRight && rBody.velocity.x > 0)
        {
            Flip();
        }

        //cominucate with animator
        anim.SetFloat("xSpeed", Mathf.Abs(rBody.velocity.x));
        anim.SetFloat("ySpeed", rBody.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDucking", isDucking);

    }

    private bool GroundCheck()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, whatIsGround);

    }

    private void Flip()
    {
        Vector3 temp = transform.localScale;
        temp.x *= -1;
        transform.localScale = temp;

        isFacingRight = !isFacingRight;
    }


}
