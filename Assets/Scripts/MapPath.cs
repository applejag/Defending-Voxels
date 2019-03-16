using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapPath : MonoBehaviour
{
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Transform[] pathTransforms = transform.OfType<Transform>()
            .ToArray();

        var offset = new Vector3(0, 0.1f, 0);
        var c1 = Color.red;
        var c2 = new Color(1, 0, 0, 0.3f);
        for (var i = 0; i < pathTransforms.Length; i++)
        {
            Vector3 a = pathTransforms[i].position;
            Vector3 ay = a + offset;
            Gizmos.color = c1;
            Gizmos.DrawLine(a, ay);
            Gizmos.DrawSphere(ay, 0.02f);

            Gizmos.color = c2;
            if (i < pathTransforms.Length - 1)
            {
                Vector3 by = pathTransforms[i + 1].position + offset;

                Gizmos.DrawLine(ay, by);
            }
        }
    }

#endif
}