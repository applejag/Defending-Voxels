using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 10;

    public float aimWait = 0.5f;
    public float reloadWait = 3f;

    public Transform aimPivot;
    public Vector3 aimOffset;
    public float aimSpeed = 10;
    public float aimThreshold = 5;

    private float reloadTimeLeft = 0;

    private Enemy lockedEnemy;

    private float sqrRange;


#if UNITY_EDITOR

    private void OnValidate()
    {
        sqrRange = range * range;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Vector3 position = transform.position;
        position.y = 0.1f;
        Gizmos.DrawWireSphere(position, range);

        if (aimPivot)
        {
            Gizmos.color = Color.red;
            Vector3 firePosition = aimPivot.TransformPoint(aimOffset);
            Gizmos.DrawRay(firePosition + Vector3.forward * 0.25f, Vector3.back * 0.5f);
            Gizmos.DrawRay(firePosition + Vector3.left * 0.25f, Vector3.right * 0.5f);

            if (lockedEnemy)
            {
                Gizmos.DrawLine(firePosition, lockedEnemy.transform.position);
            }
        }
    }

#endif

    private void OnEnable()
    {
        sqrRange = range * range;
    }

    private void Update()
    {
        if (reloadTimeLeft > 0)
            reloadWait -= Time.deltaTime;

        if (Enemy.Enemies.Count == 0)
        {
            print("no enemies???");
            return;
        }

        Vector3 pos = transform.position;
        var towerPos2 = new Vector2(pos.x, pos.z);

        // Enemy out of range. Get a new one
        if (lockedEnemy && !IsEnemyInRange(lockedEnemy, in towerPos2))
        {
            lockedEnemy = null;
        }

        // Get an enemy
        if (!lockedEnemy)
        {
            Enemy enemyInRange = FindFirstEnemyInRange(in towerPos2);

            if (enemyInRange)
            {
                LockOn(enemyInRange);
            }
        }

        if (lockedEnemy)
        {
            // Aiming
            Vector3 delta = lockedEnemy.transform.position - aimPivot.TransformPoint(aimOffset);
            Quaternion rotation = Quaternion.LookRotation(delta);
            aimPivot.rotation = Quaternion.RotateTowards(aimPivot.rotation, rotation, aimSpeed * Time.deltaTime);

            // Pew'ing
            float angle = Quaternion.Angle(rotation, aimPivot.rotation);

            if (reloadTimeLeft <= 0 && angle < aimThreshold)
            {
                reloadTimeLeft = reloadWait;
                print("PEW!");
            }
        }

    }

    private void LockOn(Enemy enemy)
    {
        lockedEnemy = enemy;
        reloadTimeLeft = Mathf.Max(reloadTimeLeft, aimWait);
    }

    private Enemy FindFirstEnemyInRange(in Vector2 towerPos2)
    {
        Enemy firstOrDefault = EnemiesInRange(towerPos2)
            .OrderBy(o => o.agent.progress)
            .FirstOrDefault();

        return firstOrDefault;
    }

    private bool IsEnemyInRange(Enemy enemy, in Vector2 towerPos2)
    {
        Vector3 ePos = enemy.transform.position;
        var enemyPos2 = new Vector2(ePos.x, ePos.z);
        return (enemyPos2 - towerPos2).sqrMagnitude <= sqrRange;
    }

    private IEnumerable<Enemy> EnemiesInRange(Vector2 towerPos2)
    {
        foreach (Enemy enemy in Enemy.Enemies)
        {
            if (IsEnemyInRange(enemy, in towerPos2))
                yield return enemy;
        }
    }
}