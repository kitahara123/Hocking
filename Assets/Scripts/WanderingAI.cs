using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 3.0f;
    [SerializeField] private float obstacleRange = 5.0f;
    private bool alive;
    private float speed;

    [SerializeField] private Fireball fireballPrefab;
    private Fireball fireball;

    private void Awake() => Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    private void OnDestroy() => Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);

    private void Start()
    {
        speed = baseSpeed * PlayerPrefs.GetFloat("Speed", 1);
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
    
    private void OnSpeedChanged(float value) => speed = baseSpeed * value;
    public void SetAlive(bool value) => alive = value;
}