using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float obstacleRange = 5.0f;
    private bool alive;

    private void Start()
    {
        alive = true;
    }

    private void Update()
    {
        if (!alive) return;
        transform.Translate(0, 0, speed * Time.deltaTime);

        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.75f, out hit, obstacleRange))
        {
            var angle = Random.Range(-110, 110);
            transform.Rotate(0, angle, 0);
        }
    }

    public void SetAlive(bool value)
    {
        alive = value;
    }
}