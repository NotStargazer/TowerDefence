using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Transform target;
    [SerializeField] ParticleSystem explosion;
    public float projectileSpeed = 7;

    public void SetTarget(Transform setTarget)
    {
        target = setTarget;
    }

    private void Update()
    {
        if (target == null)
        {
            Explode();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * projectileSpeed);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        float explosionDamage = MapLoader.Instance.mapData.bombTowerExplosionDamage;
        float explosionRadius = MapLoader.Instance.mapData.bombTowerExplosionRadius;

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var enemy in hits)
        {
            Enemy target = enemy.transform.GetComponent<Enemy>();
            if (target != null)
            {
                target.DealDamage(explosionDamage);
            }
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
