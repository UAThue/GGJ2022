using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepData : MonoBehaviour
{
    public bool isBlack; // Black/White
    public bool isHipster; // Hipster/Normal
    public bool isFluffy; // Fluffy/Skinny

    public bool isInsidePen; // In Pen / Out of Pen
    public bool isNorthFarm; // North Half
    public bool isEastSide;  // East Half 

    [Header("GameObjects")]
    public GameObject hipsterGameObjects;
    public Renderer colorRenderer;
    public Material whiteSheepMaterial;
    public Material blackSheepMaterial;



    public void Awake()
    {
    }

    public void Start()
    {
        // Set visuals
        SetVisuals();

        // Saaaave the sheeep
        GameManager.instance.sheep.Add(this);
    }

    public bool MeetsRule ( Rule rule )
    {
        bool rightType = false;
        bool rightPlace = false;
        // FUDGE IT ALL! JUST HARDCODING THIS FOR NOW. 
        // NO ONE EVER DO THIS IN THE FUTURE.
        // STRING COMPARES ARE BAD, MMMKAY, but I have 2 hours to finish this.

        // If we are the right type (black/hip/yeet/north/in)
        if (rule.typeDuality.id == "black") {
            if (rule.typeDualityIsOneThing && isBlack) {
                rightType = true;
            }
            else if (!rule.typeDualityIsOneThing && !isBlack) {
                rightType = true;
            }
        }
        else if (rule.typeDuality.id == "hip") {
            if (rule.typeDualityIsOneThing && isHipster) {
                rightType = true;
            }
            else if (!rule.typeDualityIsOneThing && !isHipster) {
                rightType = true;
            }
        }
        // Now check if right place
        if (rule.placeDuality.id == "in") {
            if (rule.placeDualityIsOneThing && isInsidePen) {
                rightPlace = true;
            }
            else if (!rule.typeDualityIsOneThing && !isInsidePen) {
                rightPlace = true;
            }
        }
        else if (rule.placeDuality.id == "north") {
            if (rule.placeDualityIsOneThing && isNorthFarm) {
                rightPlace = true;
            }
            else if (!rule.placeDualityIsOneThing && !isNorthFarm) {
                rightPlace = true;
            }
        }
        else if (rule.placeDuality.id == "east") {
            if (rule.placeDualityIsOneThing && isEastSide) {
                rightPlace = true;
            }
            else if (!rule.placeDualityIsOneThing && !isEastSide) {
                rightPlace = true;
            }
        }

        // If both right, return true
        if (rightType && rightPlace) return true;

        // otherwise
        return false;
    }

    public void Update()
    {

        if (transform.position.z > 0) {
            isNorthFarm = true;
        } else if (transform.position.z <0) {
            isNorthFarm = false;
        }

        if (transform.position.x > 0) {
            isEastSide = true;
        }
        else if (transform.position.x < 0) {
            isEastSide = false;
        }

    }

    public void SetVisuals()
    {
        if (isBlack) {
            colorRenderer.material = blackSheepMaterial;
        } else {
            colorRenderer.material = whiteSheepMaterial;
        }

        if (isHipster) {
            hipsterGameObjects.SetActive(true);
        }
        else {
            hipsterGameObjects.SetActive(false);
        }

        if (isFluffy) {
            // TODO: Show fluffy stuff
        } else {
            // TODO: Show skinny stuff
        }
    }

}
