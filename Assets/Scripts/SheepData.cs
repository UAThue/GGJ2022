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

    public void Start()
    {
        SetVisuals();
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
