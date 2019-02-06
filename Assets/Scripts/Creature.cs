using System;
using System.Collections;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] public int maxHP;
    [SerializeField] public int HP;
    [SerializeField] private Color hitColor;
    [SerializeField] private float damageDelay = 0.2f;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioSource soundSource;
    public event Action<Creature> OnDeath;
    private Renderer rend;

    private float lastHitTime;

    public bool Alive { get; private set; } = true;

    protected virtual void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    public virtual void ChangeHealth(int value)
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
        StartCoroutine(ReactToHit());

        soundSource.PlayOneShot(hitSound);
    }


    private IEnumerator ReactToHit()
    {
        if (rend == null) yield break;
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

        if (CompareTag("Enemy"))
        {
            Messenger.Broadcast(GameEvent.SCORE_EARNED);
            Destroy(gameObject);
        }

        if (CompareTag("Player")) Messenger.Broadcast(GameEvent.LEVEL_FAILED);
    }

    public void UpdateData(int value, int value1)
    {
        HP = value;
        maxHP = value1;
    }
}