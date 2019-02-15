using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Отталкивается от стен, при прикосновении к персонажу убивает его
/// </summary>
public class BackAndForth : SpeedControl
{
    [SerializeField] private int damage = 100;
    private int direction = 1;

    private void Update()
    {
        transform.Translate(0, 0, direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Creature>();
        if (player != null)
        {
            player.Hurt(damage);
            Destroy(gameObject);
            return;
        }

        direction = -direction;
    }
}