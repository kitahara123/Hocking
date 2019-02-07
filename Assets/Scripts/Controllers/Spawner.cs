﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : SpeedControl
{
    [SerializeField] private Creature rangedEnemyPrefab;
    [SerializeField] private Creature meleeEnemyPrefab;
    [SerializeField] private int rangedEnemyNumber = 5;
    [SerializeField] private int meleeEnemyNumber = 15;
    [SerializeField] private int enemiesPerLevel = 40;
    [SerializeField] private int spawnCD = 1;
    private int enemiesKilledOnLevel;
    private bool cooldown;

    private int rangedCounter;
    private int meleeCounter;

    private List<Creature> enemies;

    private void Start()
    {
        enemies = new List<Creature>();
    }

    private void Update()
    {
        if (speedModifier == 0 || cooldown || enemiesPerLevel <= enemiesKilledOnLevel) return;
        if (rangedEnemyNumber == 0 && meleeEnemyNumber == 0) return;

        if (meleeCounter < meleeEnemyNumber || rangedCounter < rangedEnemyNumber)
        {
            StartCoroutine(Spawn());
        }
    }

    private IEnumerator Spawn()
    {
        cooldown = true;

        float melees = 1;
        float ranges = 1;
        if (rangedEnemyNumber > 0) ranges = 0;
        if (meleeEnemyNumber > 0) melees = 0;
        if (meleeCounter > 0 && meleeEnemyNumber > 0)
        {
            melees = meleeCounter / meleeEnemyNumber;
        }

        if (rangedCounter > 0 && rangedEnemyNumber > 0)
        {
            ranges = rangedCounter / rangedEnemyNumber;
        }


        Creature newEnemy;
        if (melees < ranges)
        {
            newEnemy = Instantiate(meleeEnemyPrefab);
            meleeCounter++;
            newEnemy.OnDeath += (c) =>
            {
                meleeCounter--;
                enemies.Remove(c);
                enemiesKilledOnLevel++;
            };
        }
        else
        {
            newEnemy = Instantiate(rangedEnemyPrefab);
            rangedCounter++;
            newEnemy.OnDeath += (c) =>
            {
                rangedCounter--;
                enemies.Remove(c);
                enemiesKilledOnLevel++;
            };
        }


        enemies.Add(newEnemy);
        newEnemy.transform.position = transform.position;
        yield return new WaitForSeconds(spawnCD);
        cooldown = false;
    }
}