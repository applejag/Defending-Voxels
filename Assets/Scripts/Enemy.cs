using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PathAgent))]
public class Enemy : MonoBehaviour
{
    private static readonly HashSet<Enemy> _enemies = new HashSet<Enemy>();
    public static IReadOnlyCollection<Enemy> Enemies => _enemies;

    [NonSerialized]
    public PathAgent agent;

    public Vector3 centerOfMass;
    public int health = 5;

    [Space]
    public Renderer[] renders;

    [Space]
    public float damagedTime = 0.5f;
    public Color defaultColor = Color.white;
    public Color damagedColor = Color.red;
    public AnimationCurve damagedColorWeight = AnimationCurve.Linear(0, 1, 1, 0);

    private float damagedTimeLeft;

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.TransformPoint(centerOfMass);
        Gizmos.DrawRay(center + Vector3.forward * 0.25f, Vector3.back * 0.5f);
        Gizmos.DrawRay(center + Vector3.left * 0.25f, Vector3.right * 0.5f);
    }

#endif

    private void OnEnable()
    {
        _enemies.Add(this);
        agent = GetComponent<PathAgent>();

        if (!agent)
        {
            Debug.LogError($"Missing path agent for {name}. Disabling");
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _enemies.Remove(this);
    }

    private void Update()
    {
        if (damagedTimeLeft > 0)
        {
            float t = 1 - damagedTimeLeft / damagedTime;

            SetRendersColor(t < 1
                ? Color.Lerp(defaultColor, damagedColor, damagedColorWeight.Evaluate(t))
                : Color.Lerp(defaultColor, damagedColor, damagedColorWeight.Evaluate(1)));

            damagedTimeLeft -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        damagedTimeLeft = damagedTime;
    }

    private void SetRendersColor(Color color)
    {
        if (renders.Length == 0) return;

        Material material = renders.First().material;

        foreach (Renderer render in renders.Skip(1))
        {
            render.sharedMaterial = material;
        }

        material.color = color;
    }

}
