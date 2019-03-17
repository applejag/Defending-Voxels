using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinSway : MonoBehaviour
{
    public float amplitude = 0.3f;
    public float zMult = 0.5f;
    public float xMult = 0.5f;

//#if UNITY_EDITOR
//    private void OnDrawGizmosSelected()
//    {
//        const float radius = 0.01f;
//        const int iterations = 20;

//        Vector3 start = transform.position + new Vector3(-0.5f, 1.612f, -0.5f);
//        Gizmos.color = Color.yellow;
//        Vector3 lastPoint = start + new Vector3(0, Mathf.Sin(0), 0);
//        Gizmos.DrawSphere(lastPoint, radius);
//        for (var i = 1; i <= iterations; i++)
//        {
//            float t = i / (float)iterations;
//            Vector3 point = start + new Vector3(t, 0, t) + Evaluate(t);
//            Gizmos.DrawLine(lastPoint, point);
//            Gizmos.DrawSphere(point, radius);
//            lastPoint = point;
//        }
//    }
//#endif

    private void Update()
    {
        transform.localPosition = Evaluate(Time.time);
    }

    private Vector3 Evaluate(float time)
    {
        Vector3 position = transform.position;
        return new Vector3(0, Mathf.Sin((time + position.x * xMult + position.z * zMult) * Mathf.PI) * amplitude, 0);
    }
}
