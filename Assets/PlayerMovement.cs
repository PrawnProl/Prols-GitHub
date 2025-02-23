using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 3f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 adjustedMovement = AdjustMovement(moveInput);

        if (moveInput != Vector2.zero)
        {
            // Always save last input direction, even if movement is blocked
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }

        if (adjustedMovement != Vector2.zero)
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
        return rb.Cast(direction, new RaycastHit2D[1], checkDistance) == 0;
    }
}
