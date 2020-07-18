using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base unit class, moves to the players base.
/// </summary>
public class Enemy : MonoBehaviour, IDamagable
{
    public event Action<bool, int> EnemyKilled;

    [Header("Unit Settings")]
    [SerializeField] private float m_StandardUnitHealth = 100;
    [SerializeField] private float m_StandardUnitSpeed = 2;
    [SerializeField] private int m_StandardUnitDamage = 1;
    [Space]
    [SerializeField] private float m_LargeUnitHealth = 500;
    [SerializeField] private float m_LargeUnitSpeed = 1;
    [SerializeField] private int m_LargeUnitDamage = 5;

    private float m_Health;
    private float m_MaximumHealth;
    private float m_MovementSpeed;
    private float m_MaxMovementSpeed;

    private IEnumerator<Vector2Int> m_Path;
    private Vector3 m_NextTile;
    private Dictionary<StatusAilments, float> m_Ailments = new Dictionary<StatusAilments, float>();
    private UnitType m_Type;

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_NextTile, Time.fixedDeltaTime * m_MovementSpeed);

        if (Vector3.Distance(transform.position, m_NextTile) < 0.05f)
        {
            m_NextTile = GetNextTile();
        }
    }

    private void Update()
    {
        foreach (var ailment in m_Ailments)
        {
            if (ailment.Value < Time.time)
            {
                switch (ailment.Key)
                {
                    case StatusAilments.Slowed:
                        m_MovementSpeed = m_MaxMovementSpeed;
                        break;
                }
                m_Ailments.Remove(ailment.Key);
                return;
            }
        }
    }

    /// <summary>
    /// Gets the next tile in the path.
    /// </summary>
    private Vector3 GetNextTile()
    {
        if (!m_Path.MoveNext())
        {
            ReachEnd();
        }
        return new Vector3(m_Path.Current.x, 0.5f, m_Path.Current.y);
    }

    /// <summary>
    /// Once there is no more tiles to travel to in the path the enemy invoke the killed event and disable itself.
    /// </summary>
    private void ReachEnd()
    {
        int damageToDealEnemy = 0;
        switch (m_Type)
        {
            case UnitType.Standard:
                damageToDealEnemy = m_StandardUnitDamage;
                break;
            case UnitType.Big:
                damageToDealEnemy = m_LargeUnitDamage;
                break;
        }
        if (EnemyKilled != null)
        {
            EnemyKilled.Invoke(false, damageToDealEnemy);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Construct enemy and instantiate dependencies.
    /// </summary>
    public void ConstructEnemy(UnitType unit, IEnumerator<Vector2Int> aiPath)
    {
        m_Type = unit;
        if (m_Type == UnitType.Standard)
        {
            m_MaximumHealth = m_StandardUnitHealth;
            m_MaxMovementSpeed = m_StandardUnitSpeed;
            transform.localScale = Vector3.one;
        }
        else
        {
            m_MaximumHealth = m_LargeUnitHealth;
            m_MaxMovementSpeed = m_LargeUnitSpeed;
            transform.localScale = Vector3.one * 1.5f;
        }

        m_Health = m_MaximumHealth;
        m_MovementSpeed = m_MaxMovementSpeed;

        m_Path = aiPath;
        m_NextTile = GetNextTile();
    }

    /// <summary>
    /// Deals damage to the enemy. If the health reaches 0, the enemy invoke the killed event and disable itself.
    /// </summary>
    /// <returns>The remaining health of the enemy.</returns>
    public float DealDamage(float damage)
    {
        m_Health -= damage;

        if (m_Health < 0)
        {
            EnemyKilled?.Invoke(false, 0);
            gameObject.SetActive(false);
            return 0;
        }

        return m_Health;
    }

    /// <summary>
    /// Deals damage to the enemy and applies a status ailment. If the health reaches 0, the enemy invoke the killed event and disable itself.
    /// </summary>
    /// <param name="ailment">Ailment type.</param>
    /// <param name="effectPotency">The effectiveness of the Ailment. (Slow: 0 to 1)</param>
    /// <param name="effectDuration">The duration of the Ailment.</param>
    /// <returns>The remaining health of the enemy.</returns>
    public float DealDamage(float damage, StatusAilments ailment, float effectPotency, float effectDuration)
    {
        if (ailment == StatusAilments.Slowed)
        {
            Slow(effectPotency, effectDuration);
        }

        return DealDamage(damage);
    }

    /// <summary>
    /// Slows the enemies movement speed based on effect potency, 0 to 1 (0% to 100%)
    /// </summary>
    private void Slow(float effectPotency, float effectDuration)
    {
        m_MovementSpeed = m_MaxMovementSpeed * (1 - effectPotency);
        if (m_Ailments.ContainsKey(StatusAilments.Slowed)) m_Ailments[StatusAilments.Slowed] = Time.time + effectDuration;
        else m_Ailments.Add(StatusAilments.Slowed, Time.time + effectDuration);
    }
}
