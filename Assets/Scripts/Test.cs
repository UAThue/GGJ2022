using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.eventManager.RegisterEvent("RunEventTest", this.RunEventTest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // NOTE: All triggerable functions must have a parameter of a payload
    void RunEventTest(HueEventSystem.MessagePayload input)
    {
        Debug.Log("Fart" + gameObject.name);
    }
}
