using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform firePoint;

    [SerializeField] LineRenderer freezeLaser;
    [SerializeField] Bomb bomb;

    [SerializeField] Material freezeTower;
    [SerializeField] Material bombTower;

    float fireTime;
    List<Enemy> targets = new List<Enemy>();
    TileType towerType;

    private void OnTriggerEnter(Collider other)
    {
        targets.Add(other.gameObject.GetComponent<Enemy>());
    }

    private void OnTriggerExit(Collider other)
    {
        targets.Remove(other.gameObject.GetComponent<Enemy>());
    }

    private void Start()
    {
        towerType = GetComponentInParent<Tile>().type;
        MeshRenderer rend = firePoint.parent.GetComponent<MeshRenderer>();
        switch (towerType)
        {
            case TileType.TowerOne:
                rend.material = freezeTower;
                break;
            case TileType.TowerTwo:
                rend.material = bombTower;
                break;
        }
    }

    private void Update()
    {
        if (targets.Count > 0)
        {
            if (fireTime < Time.time)
            {
                switch (towerType)
                {
                    case TileType.TowerOne:
                        fireTime = Time.time + MapLoader.Instance.mapData.bombTowerCooldownTime;
                        break;
                    case TileType.TowerTwo:
                        fireTime = Time.time + MapLoader.Instance.mapData.freezeTowerCooldownTime;
                        break;
                }
                FireAtTarget();
            }
        }
    }

    private void FireAtTarget()
    {
        if (targets[0] == null)
        {
            targets.RemoveAt(0);
            fireTime = Time.time;
            return;
        }

        switch (towerType)
        {
            case TileType.TowerOne:
                FireBomb();
                break;
            case TileType.TowerTwo:
                FireFreeze();
                break;
        }
    }

    private void FireFreeze()
    {
        float damage = MapLoader.Instance.mapData.freezeTowerDamage;
        float slowAmount = MapLoader.Instance.mapData.freezeTowerSlowAmount;
        float slowDuration = MapLoader.Instance.mapData.freezeTowerSlowDuration;

        SpawnFreezeLaser(firePoint.position, targets[0].transform.position);

        if (targets[0].DealDamage(damage, StatusAilments.Slowed, slowAmount, slowDuration) == 0)
        {
            targets.RemoveAt(0);
        }
    }

    private void SpawnFreezeLaser(Vector3 firePoint, Vector3 enemyLocation)
    {
        LineRenderer lr = Instantiate(freezeLaser);
        lr.SetPositions(new Vector3[] { firePoint, enemyLocation });
    }

    private void FireBomb()
    {
        Bomb bombProjectile = Instantiate(bomb, firePoint.position, Quaternion.identity);
        bombProjectile.SetTarget(targets[0].transform);
    }
}
