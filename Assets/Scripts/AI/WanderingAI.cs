using UnityEngine;

[RequireComponent(typeof(Creature))]
public class WanderingAI : SpeedControl
{
    [SerializeField] private float obstacleRange = 5.0f;
    private Creature creature;

    private void Start()
    {
        creature = GetComponent<Creature>();
    }

    /// <summary>
    /// Разворачивается и идёт в рандомную сторону если впереди препятствие 
    /// </summary>
    private void Update()
    {
        if (!creature.Alive) return;
        transform.Translate(0, 0, speed * Time.deltaTime);

        var ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;
        if (speed > 0 && Physics.SphereCast(ray, 0.75f, out hit, obstacleRange))
        {
            if (hit.transform.CompareTag("Projectile")) return;
            var angle = Random.Range(-110, 110);
            transform.Rotate(0, angle, 0);
        }
    }
}