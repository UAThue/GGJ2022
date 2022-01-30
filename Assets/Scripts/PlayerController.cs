using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public string verticalAxis = "Vertical";
    public string horizontalAxis = "Horizontal";
    public string AButton = "Fire1";
    public string BButton = "Fire2";
    public string XButton = "Fire3";
    public string YButton = "Fire4";


    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        pawn.MoveForward(Input.GetAxis(verticalAxis));
        pawn.Rotate(Input.GetAxis(horizontalAxis));
        GameManager.instance.ShowRules(Input.GetButton(YButton));

    }
}
