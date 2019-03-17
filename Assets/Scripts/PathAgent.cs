﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAgent : MonoBehaviour
{
    public float speed = 5;
    public float turnWait = 0.3f;

    private void Start()
    {
        var path = FindObjectOfType<MapPath>();
        if (!path)
        {
            Debug.LogWarning("No path found. Disabling");
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(FollowPath(path.path));
    }

    private IEnumerator FollowPath(Vector3[] path)
    {
        if (path is null)
            throw new ArgumentNullException(nameof(path));
        if (path.Length < 2)
        {
            Debug.LogWarning($"There were not enough path nodes. Actual: {path.Length}", this);
            yield break;
        }

        Vector3 pos = path[0];

        for (var i = 0; i < path.Length - 1; i++)
        {
            Vector3 target = path[i + 1];
            transform.forward = target - pos;
            

            while ((pos - target).sqrMagnitude > 0.01f)
            {
                pos = Vector3.MoveTowards(pos, target, speed * Time.deltaTime);
                transform.position = pos;
                yield return null;
            }

            transform.position = pos = target;
            yield return new WaitForSeconds(turnWait);
        }
    }
}
