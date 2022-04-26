using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    
    public float MoveSpeed = 4f;
    public float JumpHeight = 2f;
    public float Gravity = -9.81f;
    private Vector3 Movement;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool Grounded;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {
        if (Grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        bool IsWalking = animator.GetBool("WalkForward");
        bool WalkBackward = animator.GetBool("WalkBackward");
        bool WalkLeft = animator.GetBool("WalkLeft");
        bool WalkRight = animator.GetBool("WalkRight");
        //WalkForwardLeft
        bool WalkForwardLeft = animator.GetBool("WalkForwardLeft");
        bool WalkForwardLeftForward = animator.GetBool("WalkForwardLeftForward");
        bool WalkForwardLeftLeft = animator.GetBool("WalkForwardLeftLeft");
        //WalkForwardRight
        bool WalkForwardRight = animator.GetBool("WalkForwardRight");
        //WalkBackwardLeft
        bool WalkBackwardLeft = animator.GetBool("WalkBackwardLeft");
        //WalkBackwardRight
        bool WalkBackwardRight = animator.GetBool("WalkBackwardRight");

        bool Jump = animator.GetBool("Jump");

        bool ForwardPressed = Input.GetKey("w");
        bool BackPressed = Input.GetKey("s");
        bool LeftPressed = Input.GetKey("a");
        bool RightPressed = Input.GetKey("d");
        bool ForwardLeftPressed = Input.GetKey("w") && Input.GetKey("a");
        bool ForwardRightPressed = Input.GetKey("w") && Input.GetKey("d");
        bool BackLeftPressed = Input.GetKey("s") && Input.GetKey("a");
        bool BackRightPressed = Input.GetKey("s") && Input.GetKey("d");

        bool JumpPressed = Input.GetKeyDown("space");

        if (!IsWalking && ForwardPressed)
        {
            animator.SetBool("WalkForward", true);
        }
        else if (IsWalking && !ForwardPressed)
        {
            animator.SetBool("WalkForward", false);
        }

        if (!WalkBackward && BackPressed)
        {
            animator.SetBool("WalkBackward", true);
        }
        else if (WalkBackward && !BackPressed)
        {
            animator.SetBool("WalkBackward", false);
        }

        if (!WalkLeft && LeftPressed)
        {
            animator.SetBool("WalkLeft", true);
        }
        else if (WalkLeft && !LeftPressed)
        {
            animator.SetBool("WalkLeft", false);
        }

        if (!WalkRight && RightPressed)
        {
            animator.SetBool("WalkRight", true);
        }
        else if (WalkRight && !RightPressed)
        {
            animator.SetBool("WalkRight", false);
        }
        
        //WalkForwardLeft
        if (!WalkForwardLeft && ForwardLeftPressed)
        {
            animator.SetBool("WalkForwardLeft", true);
        }
        else if (WalkForwardLeft && !ForwardLeftPressed && ForwardPressed)
        {
            animator.SetBool("WalkForwardLeft", false);
            animator.SetBool("WalkForward", true);
        }
        else if (WalkForwardLeft && !ForwardLeftPressed && LeftPressed)
        {
            animator.SetBool("WalkForwardLeft", false);
            animator.SetBool("WalkLeft", true);
        }

        //WalkForwardRight
        if (!WalkForwardRight && ForwardRightPressed)
        {
            animator.SetBool("WalkForwardRight", true);
        }
        else if (WalkForwardRight && !ForwardRightPressed && ForwardPressed)
        {
            animator.SetBool("WalkForwardRight", false);
            animator.SetBool("WalkForward", true);
        }
        else if (WalkForwardRight && !ForwardRightPressed && RightPressed)
        {
            animator.SetBool("WalkForwardRight", false);
            animator.SetBool("WalkRight", true);
        }
        //WalkBackwardsRight
        if (!WalkBackwardRight && BackRightPressed)
        {
            animator.SetBool("WalkBackwardRight", true);
        }
        else if (WalkBackwardRight && !BackRightPressed && BackPressed)
        {
            animator.SetBool("WalkBackwardRight", false);
            animator.SetBool("WalkBackward", true);
        }
        else if (WalkBackwardRight && !BackRightPressed && RightPressed)
        {
            animator.SetBool("WalkBackwardRight", false);
            animator.SetBool("WalkRight", true);
        }

        //WalkBackwardsLeft
        if (!WalkBackwardLeft && BackLeftPressed)
        {
            animator.SetBool("WalkBackwardLeft", true);
        }
        else if (WalkBackwardLeft && !BackLeftPressed && BackPressed)
        {
            animator.SetBool("WalkBackwardLeft", false);
            animator.SetBool("WalkBackward", true);
        }
        else if (WalkBackwardLeft && !BackLeftPressed && LeftPressed)
        {
            animator.SetBool("WalkBackwardLeft", false);
            animator.SetBool("WalkLeft", true);
        }

        //Jump
        if(!Jump && JumpPressed)
        {
            animator.SetBool("Jump", true);
        }
        else if (Jump && !JumpPressed)
        {
            animator.SetBool("Jump", false);
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = 0f;
        float z = Input.GetAxisRaw("Vertical");
        Movement = transform.right * x + transform.forward * z;

        controller.Move(Movement * MoveSpeed * Time.deltaTime);

        velocity.y += Gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && Grounded)
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down,out hit,2f))
        {
            Grounded = true;
        }else {
            Grounded = false;
        }
    }
}
