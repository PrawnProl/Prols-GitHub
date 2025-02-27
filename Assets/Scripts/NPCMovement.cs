using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCMovement : MonoBehaviour
{
    private float speed = 1f;
    private Rigidbody2D myRigidBody;
    private Vector3 directionVector;
    private Transform myTransform;
    private Animator anim;

    private float changeDirectionTime = 2f;
    private float timer;

    void Start()
    {
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        myRigidBody = GetComponent<Rigidbody2D>();
        ChangeDirection();
        timer = changeDirectionTime;
    }

    void Update()
    {
        myRigidBody.linearVelocity = directionVector * speed; // Changed movement formula

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChangeDirection();
            timer = changeDirectionTime;
        }

        if (directionVector.magnitude > 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetFloat("LastInputX", directionVector.x);
            anim.SetFloat("LastInputY", directionVector.y);
        }
    }

    void ChangeDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                directionVector = Vector3.right;
                break;
            case 1:
                directionVector = Vector3.up;
                break;
            case 2:
                directionVector = Vector3.left;
                break;
            case 3:
                directionVector = Vector3.down;
                break;
        }
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        anim.SetFloat("MoveX", directionVector.x);
        anim.SetFloat("MoveY", directionVector.y);
    }
}
