using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float maxZ = 16.0f;
    [SerializeField] private float minZ = -16.0f;

    private int direction = 1;

    private void Update()
    {
        transform.Translate(0,0, direction * speed * Time.deltaTime);

        bool bounced = false;
        if (transform.position.z > maxZ || transform.position.z < minZ)
        {
            direction = -direction;
            bounced = true;
        }

        if (bounced)
        {
            transform.Translate(0, 0, direction * speed * Time.deltaTime);
        }
    }
}

