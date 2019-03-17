using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathAgent))]
public class Enemy : MonoBehaviour
{
    private static readonly HashSet<Enemy> _enemies = new HashSet<Enemy>();
    public static IReadOnlyCollection<Enemy> Enemies => _enemies;

    [NonSerialized]
    public PathAgent agent;

    public int health = 5;

    private void Awake()
    {
        _enemies.Add(this);
        agent = GetComponent<PathAgent>();

        if (!agent)
        {
            Debug.LogError($"Missing path agent for {name}. Disabling");
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _enemies.Remove(this);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        print($"{name}: Ouch!");
    }
}
