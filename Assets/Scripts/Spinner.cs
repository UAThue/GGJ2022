using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Spinner : MonoBehaviour
{
    [Header("Parts")]
    public RectTransform oneDuality;
    public RectTransform twoDuality;
    public Text oneDualityTextBox;
    public Text twoDualityTextBox;
    public RectMask2D mask;

    [Header("Values")]
    public RuleDuality duality;
    public AnimationCurve spinCurve;
    private float target;
    public float minTarget = 20;
    public float maxTarget = 23;
    [SerializeField] private float distanceRemaining;
    public float maxSpeed = 100;
    public float currentSpeed = 0;
    private float targetDistance;
    public float cutoffDistance = 1;
    [Header("Events")]
    public UnityEvent OnSpinBegin;
    public UnityEvent OnSpinComplete;

    public void Awake()
    {

    }

    private void Start()
    {
        SetVisuals();
    }

    void SetVisuals()
    {
        oneDualityTextBox.text = duality.oneThing;
        twoDualityTextBox.text = duality.theOther;
    }

    public void SetDuality( RuleDuality input )
    {
        duality = input;
        SetVisuals();
    }

    public bool IsOneThing()
    {
        if (target % 2 == 0) return true;
        return false;
    }

    public bool IsTheOther()
    {
        return !IsOneThing();
    }

    public void Spin(float forcedTarget = -1)
    {
        // Choose a random spin if negative target
        if (forcedTarget < 0) {
            target = Mathf.Round(Random.Range(minTarget, maxTarget));
        } else {
            target = forcedTarget;
        }

        // Spin
        StartCoroutine("DoSpin");
    }

    IEnumerator DoSpin()
    {
        //Debug.Log(target);
        OnSpinBegin.Invoke();
        currentSpeed = maxSpeed;
        float dualitySize = mask.rectTransform.rect.height;
        targetDistance = target * dualitySize;
        distanceRemaining = targetDistance;

        yield return new WaitForSeconds(0.1f);

        while (distanceRemaining > 0) {
            // Move up
            oneDuality.anchoredPosition += Vector2.up * currentSpeed * Time.deltaTime;
            twoDuality.anchoredPosition += Vector2.up * currentSpeed * Time.deltaTime;
            distanceRemaining -= currentSpeed * Time.deltaTime;

            // if above the top, move to bottom of spinner
            if (oneDuality.anchoredPosition.y >= dualitySize) {
                oneDuality.anchoredPosition = new Vector3(0, -dualitySize, 0);
            }
            if (twoDuality.anchoredPosition.y >= dualitySize) {
                twoDuality.anchoredPosition = new Vector3(0, -dualitySize, 0);
            }

            // Slow down to match speed
            currentSpeed = maxSpeed * spinCurve.Evaluate(1 - ((float)distanceRemaining / (float)targetDistance));

            // if below cutoff, just move to goal
            if (distanceRemaining <= cutoffDistance) {
                oneDuality.anchoredPosition += Vector2.up * distanceRemaining;
                twoDuality.anchoredPosition += Vector2.up * distanceRemaining;
                distanceRemaining = 0;
                currentSpeed = 0;
            }

            yield return null;
        }

        OnSpinComplete.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
