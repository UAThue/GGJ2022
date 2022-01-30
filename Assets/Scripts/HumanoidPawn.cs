using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidPawn : Pawn
{
    private Animator anim;

    public override void MoveForward(float moveDirection)
    {
        anim.SetFloat("Speed", moveDirection * moveSpeed);
    }

    public override void Rotate(float rotationDirection)
    {
        transform.Rotate(rotationDirection * Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void Carry()
    {
        // If not carrying anything
            // Get object in front of you
            // Pick it up
        // else, carrying something
            // Drop it
    }

    public void Yeet()
    {
        // If not carrying anything
            // Get object in front of you
            // Lock Movement!
            // Pick it up
            // Start Yeet arrow growth
        // else, carrying something
            // Yeet it
            // Unlock Movement
    }

    public override void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Update()
    {
    }
}
