using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RuleDuality", order = 1)]
public class RuleDuality : ScriptableObject
{
    // Duality is where it is one thing or the other
    [TextArea]public string oneThing;
    public string oneActionToInvoke; // Dangerous! Playing with Fire!!!
    [TextArea] public string theOther;
    public string otherActionToInvoke; // Dangerous! Playing with Fire!!!

}
