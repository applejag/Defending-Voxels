using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (enemyPrefabs.Length == 0)
            {
                Debug.LogWarning("No prefabs to spawn", this);
            }

            Vector3 position = FindObjectOfType<MapPath>().path[0];

            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], position, Quaternion.identity);
        }
    }
}
