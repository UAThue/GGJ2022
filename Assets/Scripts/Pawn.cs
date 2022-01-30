using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    // Start is called before the first frame update
    public abstract void Start();
    // Update is called once per frame
    public abstract void Update();

    public abstract void MoveForward(float moveDirection);
    public abstract void Rotate(float rotationDirection);
    public abstract void StartAction();
    public abstract void EndAction();
}
