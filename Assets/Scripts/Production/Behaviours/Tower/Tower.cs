using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all towers.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public abstract class Tower : MonoBehaviour
{
    private SphereCollider m_Collider;
    [SerializeField] private float m_TowerRange;
    [SerializeField] protected Transform m_FirePoint;
    protected List<Enemy> m_Targets = new List<Enemy>();
    //Next time tower will fire.
    protected float m_FireTime;
    //Wait time between each fire.
    [SerializeField] protected float m_CooldownTime;

    private void Awake()
    {
        m_Collider = GetComponent<SphereCollider>();
        m_Collider.radius = m_TowerRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        m_Targets.Add(other.gameObject.GetComponent<Enemy>());
    }

    private void OnTriggerExit(Collider other)
    {
        m_Targets.Remove(other.gameObject.GetComponent<Enemy>());
    }

    private void Update()
    {
        if (m_Targets.Count > 0)
        {
            if (m_FireTime < Time.time)
            {
                if (!m_Targets[0].gameObject.activeSelf)
                {
                    m_Targets.RemoveAt(0);
                    m_FireTime = Time.time + m_CooldownTime;
                    return;
                }

                float distance = Vector3.Distance(m_Targets[0].transform.position, transform.position);

                if (distance - m_Targets[0].transform.localScale.x > m_TowerRange)
                {
                    m_Targets.RemoveAt(0);
                    return;
                }

                Fire();
                m_FireTime = Time.time + m_CooldownTime;
            }
        }
    }

    /// <summary>
    /// Needs to be overriden, Triggers when an enemy is in range of tower.
    /// </summary>
    public abstract void Fire();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_TowerRange);
    }
}
