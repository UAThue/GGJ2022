using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public string verticalAxis = "Vertical";
    public string horizontalAxis = "Horizontal";
    public string ActionButton = "Fire3";
    public string RulesButton = "Fire2";


    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        if (GameManager.instance == null || GameManager.instance.isCharacterControl) {
            pawn.MoveForward(Input.GetAxis(verticalAxis));
            pawn.Rotate(Input.GetAxis(horizontalAxis));
            if (GameManager.instance != null) {
                GameManager.instance.ShowRules(Input.GetButton(RulesButton));
            }
            // TODO: If pressed button, do appropriate action
            if (Input.GetButtonDown(ActionButton)) {
                pawn.StartAction();
            }
            else if (Input.GetButtonUp(ActionButton)) {
                pawn.EndAction();
            }
        } else {
            pawn.MoveForward(0);
            pawn.Rotate(0);
        }

    }
}
