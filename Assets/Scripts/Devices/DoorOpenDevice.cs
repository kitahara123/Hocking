using System.Collections;
using UnityEngine;

/// <summary>
/// Девайс открывающий дверь
/// </summary>
public class DoorOpenDevice : BaseDevice
{
    [SerializeField] private Vector3 deltaPos;
    private bool opened;
    private Vector3 finalPos;
    private bool locked;
    
    public override void Operate()
    {
        if (opened)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    private IEnumerator Move()
    {
        while (Vector3.Distance(transform.position, finalPos) > 0.1f)
        {
            var v = Vector3.Slerp(transform.position, finalPos, 0.1f * speedModifier);
            transform.position = v;
            yield return null;
        }

        transform.position = finalPos;
        locked = false;
        opened = !opened;
    }

    public void Activate()
    {
        if (opened || locked) return;
        locked = true;
        finalPos = transform.position + deltaPos;
        StartCoroutine(Move());
    }

    public void Deactivate()
    {
        if (!opened || locked) return;
        locked = true;
        finalPos = transform.position - deltaPos;
        StartCoroutine(Move());
    }
}