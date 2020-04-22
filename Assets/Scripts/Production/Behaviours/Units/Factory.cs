using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    List<Wave> waves = new List<Wave>();
    Wave currentWave;

    public void CreateWaves(string waveData)
    {
        string[] wavesData = waveData.Split('\n');
        foreach (string wave in wavesData)
        {
            string[] unitData = wave.Split(' ');
            Wave newWave = new Wave();

            if (int.TryParse(unitData[0], out int smallUnitCount)) newWave.smallUnits = smallUnitCount;
            if (int.TryParse(unitData[1], out int largeUnitCount)) newWave.largeUnits = largeUnitCount;

            waves.Add(newWave);
        }
    }

    private void Start()
    {
        SpawnWave();
    }

    private void SpawnWave()
    {
        currentWave = waves[0];
        Enemy.enemiesRemaining = currentWave.TotalEnemies;
        StartCoroutine(SpawnUnits());
    }

    IEnumerator SpawnUnits()
    {

        for (int i = 0; i < currentWave.smallUnits; i++)
        {
            Enemy enemy = SpawnUnit();
            enemy.InitializeEnemy(UnitType.Standard);
            yield return new WaitForSeconds(MapLoader.Instance.mapData.standardSpawnInterval);
        }

        for (int i = 0; i < currentWave.largeUnits; i++)
        {
            Enemy enemy = SpawnUnit();
            enemy.InitializeEnemy(UnitType.Big);
            yield return new WaitForSeconds(MapLoader.Instance.mapData.largeSpawnInterval);
        }

        yield return new WaitWhile(Enemy.AreEnemiesAlive);
        yield return new WaitForSeconds(MapLoader.Instance.mapData.nextWaveInterval);
        waves.RemoveAt(0);

        if (waves.Count > 0)
        {
            SpawnWave();
        }
    }

    private Enemy SpawnUnit()
    {
        GameObject unit = Instantiate(MapLoader.Instance.mapData.unit, transform.position, Quaternion.identity);
        Enemy enemy = unit.GetComponentInChildren<Enemy>();
        return enemy;
    }
}
