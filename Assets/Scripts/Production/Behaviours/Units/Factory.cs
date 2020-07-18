using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

/// <summary>
/// Factory creates units and stores wave and enemy data.
/// </summary>
public class Factory : MonoBehaviour
{
    private static int s_EnemiesRemaining;

    private Queue<Wave> m_Waves = new Queue<Wave>();
    private Wave m_CurrentWave;
    private GameObjectPool m_EnemyPool;
    private Player m_Player;

    private MapSettings m_MapData;
    private IEnumerable<Vector2Int> m_UnitPath;

    /// <summary>
    /// Instantiate dependencies.
    /// </summary>
    internal void Construct(MapSettings mapData, Player playerData, IEnumerable<Vector2Int> unitPath)
    {
        m_MapData = mapData;
        m_Player = playerData;
        m_UnitPath = unitPath;
    }

    /// <summary>
    /// Create the wave data, determines how many of each unit should spawn on each wave.
    /// </summary>
    public void CreateWaves(string waveData)
    {
        string[] wavesData = waveData.Split('\n');
        foreach (string wave in wavesData)
        {
            string[] unitData = wave.Split(' ');
            Wave newWave = new Wave();

            if (int.TryParse(unitData[0], out int smallUnitCount))
            {
                newWave.SmallUnits = smallUnitCount;
            }
            if (int.TryParse(unitData[1], out int largeUnitCount))
            {
                newWave.LargeUnits = largeUnitCount;
            }

            m_Waves.Enqueue(newWave);
        }
    }

    private void Start()
    {
        m_EnemyPool = new GameObjectPool(10, m_MapData.Unit, 1, transform);

        SpawnWave();
    }

    /// <summary>
    /// Sets the current wave the first wave in the queue.
    /// </summary>
    private void SpawnWave()
    {
        m_CurrentWave = m_Waves.Dequeue();
        s_EnemiesRemaining = m_CurrentWave.TotalEnemies;
        StartCoroutine(SpawnUnits());
    }

    /// <summary>
    /// Loops through all the enemies in the wave and spawns them. Cooroutine to add delay.
    /// </summary>
    IEnumerator SpawnUnits()
    {
        yield return new WaitForSeconds(m_MapData.NextWaveInterval);

        for (int i = 0; i < m_CurrentWave.SmallUnits; i++)
        {
            Enemy enemy = SpawnUnit();
            enemy.ConstructEnemy(UnitType.Standard, m_UnitPath.GetEnumerator());
            yield return new WaitForSeconds(m_MapData.StandardSpawnInterval);
        }

        for (int i = 0; i < m_CurrentWave.LargeUnits; i++)
        {
            Enemy enemy = SpawnUnit();
            enemy.ConstructEnemy(UnitType.Big, m_UnitPath.GetEnumerator());
            yield return new WaitForSeconds(m_MapData.LargeSpawnInterval);
        }
    }

    /// <summary>
    /// Creates the instance data for the enemy.
    /// </summary>
    private Enemy SpawnUnit()
    {
        GameObject unit = m_EnemyPool.Rent(true);
        unit.transform.position = transform.position;
        Enemy enemy = unit.GetComponent<Enemy>();
        enemy.EnemyKilled += EnemyDestroyed;
        return enemy;
    }

    /// <summary>
    /// Event invoked, reduces player health when enemy reaches the end.
    /// </summary>
    public void EnemyDestroyed(bool reachedEnd, int damage = 0)
    {
        s_EnemiesRemaining--;
        m_Player.ReduceHealth(damage);

        CheckNextWave();
    }

    /// <summary>
    /// Check to see if enemies are remaining, if not, spawns the next wave.
    /// </summary>
    private void CheckNextWave()
    {
        if (s_EnemiesRemaining <= 0)
        {
            if (m_Waves.Count > 0)
            {
                SpawnWave();
            }
        }
    }
}
