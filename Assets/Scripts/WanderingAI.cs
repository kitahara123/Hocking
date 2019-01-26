using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : SpeedControl
{
    [SerializeField] private float obstacleRange = 5.0f;
    private bool alive;

    [SerializeField] private Fireball fireballPrefab;
    private Fireball fireball;

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
        if (!Physics.SphereCast(ray, 0.75f, out hit)) return;

        var hitObject = hit.transform.gameObject;
        if (hitObject.GetComponent<PlayerCharacter>())
        {
            if (fireball == null)
            {
                fireball = Instantiate(fireballPrefab);
                fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                fireball.transform.rotation = transform.rotation;
            }
        }
        else if (hit.distance < obstacleRange)
        {
            var angle = Random.Range(-110, 110);
            transform.Rotate(0, angle, 0);
        }
    }
    
    public void SetAlive(bool value) => alive = value;
}