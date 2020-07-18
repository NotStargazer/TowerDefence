using UnityEngine;

/// <summary>
/// Abstract Tower, Fires lasers that slow the target.
/// </summary>
public class FreezeTower : Tower
{
    [SerializeField] private LineRenderer m_FreezeLaser;
    private LineRenderer m_Laser;

    [Header("Tower Settings")]
    [SerializeField] private float m_Damage = 10;
    [SerializeField] private float m_SlowDuration = 2f;
    [Range(0, 1)]
    [SerializeField] private float m_SlowAmount = 0.5f;

    public override void Fire()
    {
        FireFreezeLaser(m_FirePoint.position, m_Targets[0].transform.position);

        if (m_Targets.Count > 0)
        {
            if (m_Targets[0].DealDamage(m_Damage, StatusAilments.Slowed, m_SlowAmount, m_SlowDuration) == 0)
            {
                m_Targets.RemoveAt(0);
            }
        }

        m_FireTime = Time.time + m_CooldownTime;
    }

    /// <summary>
    /// Set the lasers position.
    /// </summary>
    private void FireFreezeLaser(Vector3 firePoint, Vector3 enemyLocation)
    {
        if (!m_Laser)
        {
            m_Laser = Instantiate(m_FreezeLaser);
        }
        else
        {
            m_Laser.gameObject.SetActive(false);
            m_Laser.gameObject.SetActive(true);
        }


        m_Laser.SetPositions(new Vector3[] { firePoint, enemyLocation });
    }
}
