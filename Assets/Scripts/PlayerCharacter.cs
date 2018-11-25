using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    [SerializeField] private int health = 5;
    [SerializeField] private int maxHealth = 10;

    private void Start()
    {
        health = maxHealth;
    }

    public void Hurt(int damage)
    {
        health -= damage;
        Debug.Log("hp " + health);
    }
}
