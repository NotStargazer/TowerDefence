using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static int enemiesRemaining;

    public static bool AreEnemiesAlive()
    {
        return enemiesRemaining > 0;
    }

    float health;
    float maximumHealth;
    float movementSpeed;
    float maxMovementSpeed;


    IEnumerator<Vector2Int> path;
    public Vector3 nextTile;
    Direction movingDirection;
    Dictionary<StatusAilments, float> ailments = new Dictionary<StatusAilments, float>();

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextTile, Time.fixedDeltaTime * movementSpeed);

        if (Vector3.Distance(transform.position, nextTile) < 0.05f)
        {
            nextTile = GetNextTile();
            movingDirection = GetDirection();
        }
    }

    private void Update()
    {
        foreach (var ailment in ailments)
        {
            if (ailment.Value < Time.time)
            {
                switch (ailment.Key)
                {
                    case StatusAilments.Slowed:
                        movementSpeed = maxMovementSpeed;
                        break;
                }
                ailments.Remove(ailment.Key);
                return;
            }
        }
    }

    private Vector3 GetNextTile()
    {
        if (!path.MoveNext())
        {
            ReachEnd();
        }
        return new Vector3(path.Current.x, 0.5f, path.Current.y);
    }

    private void ReachEnd()
    {
        enemiesRemaining--;
        Destroy(transform.parent.gameObject);
    }

    private Direction GetDirection()
    {
        Vector3 difference = transform.position - nextTile;

        if (Mathf.RoundToInt(difference.x) > 0)
        {
            return Direction.Right;
        }

        if (Mathf.RoundToInt(difference.x) < 0)
        {
            return Direction.Left;
        }

        if (Mathf.RoundToInt(difference.z) > 0)
        {
            return Direction.Down;
        }

        if (Mathf.RoundToInt(difference.z) < 0)
        {
            return Direction.Up;
        }

        return Direction.Up;
    }

    public void InitializeEnemy(UnitType unit)
    {
        if (unit == UnitType.Standard)
        {
            maximumHealth = MapLoader.Instance.mapData.standardUnitHealth;
            maxMovementSpeed = MapLoader.Instance.mapData.standardUnitSpeed;
            health = maximumHealth;
            movementSpeed = maxMovementSpeed;
        }
        else
        {
            maximumHealth = MapLoader.Instance.mapData.largeUnitHealth;
            maxMovementSpeed = MapLoader.Instance.mapData.largeUnitSpeed;
            transform.parent.localScale *= 1.5f;
            health = maximumHealth;
            movementSpeed = maxMovementSpeed;
        }

        path = MapLoader.Instance.AIpath.GetEnumerator();
        nextTile = GetNextTile();
    }

    public float DealDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            Destroy(transform.parent.gameObject);
            enemiesRemaining--;
            return 0;
        }

        return health;
    }

    public float DealDamage(float damage, StatusAilments ailment, float effectPotency, float effectDuration)
    {
        health -= damage;

        if (health < 0)
        {
            Destroy(transform.parent.gameObject);
            enemiesRemaining--;
            return 0;
        }

        if (ailment == StatusAilments.Slowed)
        {
            Slow(effectPotency, effectDuration);
        }

        return health;
    }

    private void Slow(float effectPotency, float effectDuration)
    {
        movementSpeed = maxMovementSpeed * (1 - effectPotency);
        if (ailments.ContainsKey(StatusAilments.Slowed)) ailments[StatusAilments.Slowed] = Time.time + effectDuration;
        else ailments.Add(StatusAilments.Slowed, Time.time + effectDuration);
    }
}
