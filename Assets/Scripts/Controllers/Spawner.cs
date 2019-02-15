using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using UnityEngine;

/// <summary>
/// Спавнит заданные префабы
/// </summary>
public class Spawner : SpeedControl
{
    [SerializeField] private SpawnableObject[] prefList;
    [SerializeField] private int enemiesPerLevel = 40;
    [SerializeField] private float spawnCD = 1;
    [SerializeField] private float calibrateSpeedDependence = 1.9f;
    private int enemiesKilledOnLevel;
    private bool cooldown;

    private void Start() => StartCoroutine(StartSpawner());

    private IEnumerator StartSpawner()
    {
        if (prefList.Length == 0) yield break;
        if (prefList.Count(e => e.Quantity > 0) == 0) yield break;

        while (enemiesPerLevel > enemiesKilledOnLevel)
        {
            if (speedModifier == 0) yield return new WaitUntil( () => speedModifier > 0);

            var spawnIndex = -1;
            float rate = 1;
            for (var i = 0; i < prefList.Length; i++)
            {
                var pref = prefList[i];
                if (pref.Quantity == 0 || pref.CountActiveInstances >= pref.Quantity) continue;
                
                var tmpRate = (float) pref.CountActiveInstances / pref.Quantity;
                if (rate > tmpRate)
                {
                    spawnIndex = i;
                    rate = tmpRate;
                }
            }

            Spawn(spawnIndex);

            // Кд должно быть обратно пропорционально скорости
            var wait = spawnCD * (calibrateSpeedDependence - Mathf.Log(speedModifier * speedModifier));

            yield return new WaitForSeconds(wait);
        }
    }

    private void Spawn(int spawnIndex)
    {
        if (spawnIndex < 0) return;

        var prefab = prefList[spawnIndex];
        var newEnemy = prefab.AddInstance();

        newEnemy.OnDeath += NewEnemyOnOnDeath;
        newEnemy.transform.position = transform.position;
    }

    private void NewEnemyOnOnDeath(Creature obj)
    {
        enemiesKilledOnLevel++;
        obj.OnDeath -= NewEnemyOnOnDeath;
    }
}