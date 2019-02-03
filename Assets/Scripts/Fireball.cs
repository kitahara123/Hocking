using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : SpeedControl
{

    [SerializeField] private int damage = 1;

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    private void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.Hurt(damage);
        }
        Destroy(gameObject);
    }
}
