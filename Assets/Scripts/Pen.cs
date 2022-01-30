using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        SheepData data = other.GetComponent<SheepData>();
        if (data != null) {
            data.isInsidePen = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        SheepData data = other.GetComponent<SheepData>();
        if (data != null) {
            data.isInsidePen = false;
        }
    }

}
