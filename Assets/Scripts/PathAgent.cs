using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAgent : MonoBehaviour
{
    public float speed = 5;
    public float turnWait = 0.3f;
    public Animator anim;
    private static readonly int Moving = Animator.StringToHash("Moving");
    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

    [NonSerialized]
    public float progress;

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

        progress = 0;
        Vector3 pos = path[0];
        if (anim) anim.SetFloat(MoveSpeed, speed);

        for (var i = 0; i < path.Length - 1; i++)
        {
            progress = (float) i / path.Length;

            Vector3 target = path[i + 1];
            transform.forward = target - pos;
            if (anim) anim.SetBool(Moving, true);

            float totalMagnitude = (pos - target).sqrMagnitude;
            float magnitude = totalMagnitude;
            while (magnitude > 0.01f)
            {
                progress = (i + magnitude / totalMagnitude) / path.Length;
                pos = Vector3.MoveTowards(pos, target, speed * Time.deltaTime);
                transform.position = pos;
                magnitude = (pos - target).sqrMagnitude;
                yield return null;
            }

            if (anim) anim.SetBool(Moving, false);
            transform.position = pos = target;
            yield return new WaitForSeconds(turnWait);
        }

        progress = 1;
    }
}
