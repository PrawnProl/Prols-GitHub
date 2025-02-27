using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    public LayerMask wallLayer; // Assign this in Unity to the "Walls" layer

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 adjustedMovement = AdjustMovement(moveInput);
        rb.linearVelocity = adjustedMovement * moveSpeed;

        if (adjustedMovement != Vector2.zero)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    private Vector2 AdjustMovement(Vector2 direction)
    {
        bool canMoveX = CanMove(new Vector2(direction.x, 0));
        bool canMoveY = CanMove(new Vector2(0, direction.y));

        return new Vector2(canMoveX ? direction.x : 0, canMoveY ? direction.y : 0);
    }

    private bool CanMove(Vector2 direction)
    {
        float checkDistance = 0.1f;
        Vector2 size = rb.GetComponent<Collider2D>().bounds.size * 0.9f; // Slightly smaller than player size

        RaycastHit2D hit = Physics2D.BoxCast(rb.position, size, 0, direction, checkDistance, LayerMask.GetMask("Walls"));
        return hit.collider == null; // Only allow movement if no wall is detected
    }



    void Update()
    {
        Vector2 adjustedMovement = AdjustMovement(moveInput);

        if (moveInput != Vector2.zero)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }

        //Only move if there is no wall in the way!
        if (adjustedMovement != Vector2.zero && CanMove(adjustedMovement))
        {
            rb.linearVelocity = adjustedMovement * moveSpeed;
            animator.SetBool("isWalking", true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
    }


}


