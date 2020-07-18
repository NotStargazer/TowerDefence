using Tools;
using UnityEngine;
using System;

/// <summary>
/// Bomb object used by Bomb Tower when it fires.
/// </summary>
public class Bomb : MonoBehaviour
{
    public event Action TargetDestroyed;

    private Transform m_Target;
    [SerializeField] private ParticleSystem m_Explosion;

    [SerializeField] private float m_ExplosionRadius = 2;
    [SerializeField] private float m_ExplosionDamage = 10;
    [SerializeField] private float m_ProjectileSpeed = 7;

    private static GameObjectPool s_ParticalPool;

    private void Start()
    {
        if (s_ParticalPool == null)
        {
            s_ParticalPool = new GameObjectPool(10, m_Explosion.gameObject);
        }
    }

    /// <summary>
    /// Set the bombs target.
    /// </summary>
    public void SetTarget(Transform setTarget)
    {
        m_Target = setTarget;
    }

    private void Update()
    {
        if (!m_Target.gameObject.activeSelf)
        {
            Explode();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime * m_ProjectileSpeed);
        if (Vector3.Distance(transform.position, m_Target.position) < 0.1f)
        {
            Explode();
        }
    }

    /// <summary>
    /// Create an explosion that will call DealDamage to all IDamagable in the explosion radius
    /// </summary>
    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, m_ExplosionRadius);

        foreach (var enemy in hits)
        {
            if (enemy is IDamagable)
            {
                ((IDamagable)enemy).DealDamage(m_ExplosionDamage);
            }
        }

        var explosion = s_ParticalPool.Rent(true);
        explosion.transform.position = transform.position;
        gameObject.SetActive(false);
    }
}
