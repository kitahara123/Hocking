using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : SpeedControl
{
    private int direction = 1;
    
    private void Update()
    {
        transform.Translate(0,0, direction * speed * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<PlayerCharacter>();
            player.Hurt(100);
            Destroy(gameObject);
            return;
        }
        direction = -direction;
    }

}

