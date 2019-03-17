using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunProjectile : MonoBehaviour
{
    public float flashTime;
    public LineRenderer lineRenderer;

    [Space]

    public AnimationCurve color1Alpha = AnimationCurve.Linear(0, 1, 1, 0);
    public AnimationCurve color2Alpha = AnimationCurve.Linear(0, 0, 1, 1);

    [HideInInspector]
    public Vector3 positionOrigin;
    [HideInInspector]
    public Vector3 positionTo;

    private float timeAlive;

    private void Start()
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPositions(new[]
        {
            positionOrigin,
            positionTo
        });
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        float t = timeAlive / flashTime;

        var colorGradient = lineRenderer.colorGradient;
        var alphaKeys = colorGradient.alphaKeys;
        alphaKeys[0].alpha = color1Alpha.Evaluate(t);
        alphaKeys[1].alpha = color2Alpha.Evaluate(t);
        colorGradient.alphaKeys = alphaKeys;
        lineRenderer.colorGradient = colorGradient;

        if (timeAlive > flashTime)
        {
            Destroy(gameObject);
        }
    }
}
