using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseAndDisappear : MonoBehaviour
{
    public float lifespan = 1.0f;
    public float floatSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifespan);       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        transform.LookAt(Camera.main.transform.position);        
    }
}
