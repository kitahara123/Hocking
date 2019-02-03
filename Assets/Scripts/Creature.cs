using System;
using System.Collections;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] private int maxHP;
    [SerializeField] private int HP;
    [SerializeField] private Color hitColor;
    [SerializeField] private float damageDelay = 0.2f;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioSource soundSource;
    public event Action<Creature> OnDeath;
    private Renderer rend;

    private float lastHitTime;

    public bool Alive { get; private set; } = true;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    public void ChangeHealth(int value)
    {
        HP += value;
        if (HP > maxHP)
            HP = maxHP;
        else if (HP < 0)
        {
            HP = 0;
        }

        if (HP == 0)
        {
            StartCoroutine(Die());

        }
    }


    public void Hurt(int damage)
    {
        if (!Alive) return;
        if (Time.time < lastHitTime + damageDelay) return;
        lastHitTime = Time.time;

        ChangeHealth(-damage);
        StartCoroutine(ReactToHit() );

        if (gameObject.CompareTag("Enemy"))
        {
            soundSource.PlayOneShot(hitSound);
        }
    }


    private IEnumerator ReactToHit()
    {
        var colorTmp = rend.material.color;
        rend.material.color = hitColor;

        yield return new WaitForSeconds(0.1f);
        rend.material.color = colorTmp;
    }

    private IEnumerator Die()
    {
        Alive = false;
        transform.Rotate(-75, 0, 0);
        OnDeath?.Invoke(this);
        yield return new WaitForSeconds(1.5f);

        if (gameObject.CompareTag("Enemy")) Messenger.Broadcast(GameEvent.SCORE_EARNED);
        
        Destroy(gameObject);
    }
}