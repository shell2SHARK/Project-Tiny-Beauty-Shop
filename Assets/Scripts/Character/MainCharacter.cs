using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public float maxSpeed = 10; // max speed to walk
    private bool isFlip = false; // check if character should flip your animations
    private float xScale = 0; // value to use when flip the character 

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 charDirection; // receives the directions to walk

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        xScale = transform.localScale.x;
    }
    private void Update()
    {
        // process the game controllers
        InputProcess(); 
    }

    private void FixedUpdate()
    {
        // move the character
        Move(); 
    }

    private void InputProcess()
    {
        float verticalAxis = Input.GetAxisRaw("Vertical");
        float horizontalAxis = Input.GetAxisRaw("Horizontal");

        charDirection = new Vector2(horizontalAxis, verticalAxis).normalized;

        AnimationController(horizontalAxis, verticalAxis);
    }

    private void Move()
    {
        rb.velocity = new Vector2(charDirection.x, charDirection.y) * maxSpeed;  
    }

    private void AnimationController(float xAxis, float yAxis)
    {
        // Check if character is moving to start the animation
        if (xAxis != 0 || yAxis != 0)
        {
            anim.SetFloat("walk_weight", 1);
        }
        else
        {
            anim.SetFloat("walk_weight", 0);
        }

        // Check when the character should flip on X axis
        if(xAxis > 0)
        {
            anim.SetBool("side", true);

            if (!isFlip)
            {
                FlipCharacter(-xScale);
                isFlip = true;
            }
        }
        else if (xAxis < 0)
        {
            anim.SetBool("side", true);

            if (!isFlip)
            {
                FlipCharacter(xScale);
                isFlip = true;
            }
        }
        else
        {
            isFlip = false;
        }

        // Check which animation should be played based on front-back view
        if (yAxis > 0)
        {
            anim.SetBool("side", false);
            anim.SetBool("front", false);
            anim.SetBool("back", true);
        }
        else if (yAxis < 0)
        {
            anim.SetBool("side", false);
            anim.SetBool("front", true);
            anim.SetBool("back", false);
        }
        else
        {
            anim.SetBool("front", false);
            anim.SetBool("back", false);
        }
    }

    private void FlipCharacter(float xValue)
    {
        Vector2 flipScale = transform.localScale;
        flipScale.x = xValue;
        flipScale.y = transform.localScale.y;
        transform.localScale = flipScale;
    }
}
