using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : SpeedControl
{
    [SerializeField] private Creature rangedEnemyPrefab;
    [SerializeField] private Creature meleeEnemyPrefab;
    [SerializeField] private int rangedEnemyNumber = 5;
    [SerializeField] private int meleeEnemyNumber = 15;
    [SerializeField] private int enemiesPerLevel = 40;
    [SerializeField] private float spawnCD = 1;
    [SerializeField] private float calibrateSpeedDependence = 1.9f;
    private int enemiesKilledOnLevel;
    private bool cooldown;

    private int rangedCounter;
    private int meleeCounter;

    private MonoObjectsPool<Creature> rangesPool;
    private MonoObjectsPool<Creature> meleesPool;
    private HashSet<Creature> enemies;

    private void Start()
    {
        enemies = new HashSet<Creature>();
        rangesPool = new MonoObjectsPool<Creature>(rangedEnemyPrefab);
        meleesPool = new MonoObjectsPool<Creature>(meleeEnemyPrefab);

        StartCoroutine(StartSpawner());
    }

    private IEnumerator StartSpawner()
    {
        if (rangedEnemyNumber == 0 && meleeEnemyNumber == 0) yield break;

        while (enemiesPerLevel > enemiesKilledOnLevel)
        {
            if (meleeCounter < meleeEnemyNumber || rangedCounter < rangedEnemyNumber)
            {
                Spawn();
            }

            // Кд должно быть обратно пропорционально скорости
            var wait = speedModifier == 0
                ? spawnCD
                : spawnCD * (calibrateSpeedDependence - Mathf.Log(speedModifier * speedModifier));

            yield return new WaitForSeconds(wait);
        }
    }

    // Спавнит тех тот тип противников которых меньше
    private void Spawn()
    {
        if (speedModifier == 0) return;

        float melees = 1;
        float ranges = 1;

        if (meleeEnemyNumber > 0)
        {
            melees = (float) meleeCounter / meleeEnemyNumber;
        }

        if (rangedEnemyNumber > 0)
        {
            ranges = (float) rangedCounter / rangedEnemyNumber;
        }

        Creature newEnemy;
        if (melees < ranges)
        {
            newEnemy = meleesPool.CreateInstance();
            meleeCounter++;
            
            if (!enemies.Contains(newEnemy))
                newEnemy.OnDeath += (c) =>
                {
                    meleeCounter--;
                    meleesPool.RemoveInstance(c);
                    enemiesKilledOnLevel++;
                    Messenger.Broadcast(GameEvent.SCORE_EARNED);
                };
        }
        else
        {
            newEnemy = rangesPool.CreateInstance();
            rangedCounter++;
            
            if (!enemies.Contains(newEnemy))
                newEnemy.OnDeath += (c) =>
                {
                    rangedCounter--;
                    rangesPool.RemoveInstance(c);
                    enemiesKilledOnLevel++;
                    Messenger.Broadcast(GameEvent.SCORE_EARNED);
                };
        }

        newEnemy.Reset();
        enemies.Add(newEnemy);
        newEnemy.transform.position = transform.position;
    }
}