using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [Space(5)]
    public float maxSpeed = 10; // max speed to walk

    private bool isPaused = false;
    private bool isFlip = false; // check if character should flip your animations
    private float xScale = 0; // value to use when flip the character
                              
    private Rigidbody2D rb;
    private Animator anim;
    private EquipItens itensClass;
    private Vector2 charDirection; // receives the directions to walk

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        itensClass = GetComponent<EquipItens>();

        xScale = transform.localScale.x;
    }
    private void Update()
    {        
        if (!isPaused)
        {
            // process the game controllers
            InputProcess();         
        }       
    }

    private void FixedUpdate()
    {
        // move the character
        Move();       
    }

    private void InputProcess()
    {
        // receive the axis to move, this include joystick axis too
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
            anim.SetBool("front", false);
            anim.SetBool("back", false);
            itensClass.characterBody.ChangeBodySprite("Side");

            if (!isFlip)
            {
                FlipCharacter(-xScale);
                isFlip = true;
            }
        }
        else if (xAxis < 0)
        {
            anim.SetBool("side", true);
            anim.SetBool("front", false);
            anim.SetBool("back", false);
            itensClass.characterBody.ChangeBodySprite("Side");

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
            itensClass.characterBody.ChangeBodySprite("Back");
        }
        else if (yAxis < 0)
        {
            anim.SetBool("side", false);
            anim.SetBool("front", true);
            anim.SetBool("back", false);
            itensClass.characterBody.ChangeBodySprite("Front");
        }
    }

    private void FlipCharacter(float xValue)
    {
        Vector2 flipScale = transform.localScale;
        flipScale.x = xValue;
        flipScale.y = transform.localScale.y;
        transform.localScale = flipScale;
    }

    public void PausedGame()
    {
        // when the boolean invert your value, the state of the game change too
        isPaused = !isPaused;
        itensClass.characterBody.ChangeBodySprite("Front");

        anim.SetBool("side", false);
        anim.SetBool("front", true);
        anim.SetBool("back", false);
    }
}
