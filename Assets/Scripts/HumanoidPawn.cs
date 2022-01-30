using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HumanoidPawn : Pawn
{
    private Animator anim;

    // Fuck it, hardcoding the action
    public bool isActionCarry;
    public bool canMove = true;
    public float yeetDistance;
    public Transform arrow;
    public GameObject carriedObject;
    public Transform carryPoint;
    public float dropForce = 150;
    public float yeetUp = 150;
   
  
    

    public override void StartAction()
    {
        if (isActionCarry) {
            Carry();
        } else {
            // Can't move when yeeting
            canMove = false;
            // Pick Something up
            Pickup();
            // Start our yeet at a large amount
            yeetDistance = 5;
        }
    }

    public override void EndAction()
    {
        if (isActionCarry) {
            // Nothing - carry is toggle
        }
        else {
            // Yeet it!
            Yeet();
            // End yeet
            yeetDistance = 0;
            // Can Move again
            canMove = true;
        }
    }

    public void Pickup()
    {
        // Get everything in front of us
        Collider[] hitObjects = Physics.OverlapSphere(transform.position + Vector3.up + transform.forward, 1);
        // Seek through for a sheep
        foreach (Collider hitObject in hitObjects) {
            SheepData sheep = hitObject.GetComponent<SheepData>();
            if (sheep != null) {
                // If we find one, pick it up
                carriedObject = sheep.gameObject;
                // Move it to the carrypoint position
                carriedObject.transform.position = carryPoint.position;
                // Child to our pawn
                carriedObject.transform.parent = carryPoint.transform;
                // If it has a rigidbody, turn it off
                Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.isKinematic = true;
                }
                break;
            }
        }
    }

    public void Yeet()
    {
        // Don't just drop, yeet!
        if (carriedObject != null) {
            // Drop it
            carriedObject.transform.parent = null;
            // If it has a rigidbody, turn it on
            Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.isKinematic = false;
                rb.AddForce(transform.forward * (dropForce * yeetDistance));
                rb.AddForce(Vector3.up * yeetUp);
            }
            carriedObject = null;
        }
    }

    public void Carry()
    {
        // Remember, we can  move while carrying
        canMove = true;
        
        // If we are not carrying anything, pick it up
        if (carriedObject == null) {
            Pickup();
        }
        else {
            Drop();
        }
    }

    public void Drop()
    {
        if (carriedObject != null) {
            // Drop it
            carriedObject.transform.parent = null;
            // If it has a rigidbody, turn it on
            Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.isKinematic = false;
                rb.AddForce(transform.forward * dropForce);
            }
            carriedObject = null;
        }
    }

    public override void MoveForward(float moveDirection)
    {
        if (canMove) {
            anim.SetFloat("Speed", moveDirection * moveSpeed);
        } else {
            anim.SetFloat("Speed", 0);
        }
    }

    public override void Rotate(float rotationDirection)
    {
        transform.Rotate(rotationDirection * Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public override void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Update()
    {
        // Set arrow based on yeet distance
        arrow.localScale = new Vector3(1, 1, yeetDistance);
    }

    public void OnAnimatorIK ()
    {
        if (carriedObject != null) {
            anim.SetIKPosition(AvatarIKGoal.RightHand, carryPoint.position + (transform.right * 0.25f));
            anim.SetIKPosition(AvatarIKGoal.LeftHand, carryPoint.position - (transform.right * 0.25f));
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        }
        else {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.0f);
        }
    }
}
