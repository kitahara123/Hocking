using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoObjectsPool<T> where T : MonoBehaviour
{
    public List<T> EnabledList { get; }

    private readonly Queue<T> disabledList;
    private readonly T prefab;

    public MonoObjectsPool(T prefab)
    {
        this.prefab = prefab;
        EnabledList = new List<T>();
        disabledList = new Queue<T>();
    }

    public T CreateInstance()
    {
        T obj;
        if (disabledList.Count > 0)
        {
            obj = disabledList.Dequeue();
            EnabledList.Add(obj);
            obj.gameObject.SetActive(true);
            return obj;
        }

        obj = Object.Instantiate(prefab);
        EnabledList.Add(obj);
        return obj;
    }

    public T CreateInstance(float duration)
    {
        var obj = CreateInstance();
        obj.StartCoroutine(DestroyWithDelay(duration, obj));
        return obj;
    }

    private IEnumerator DestroyWithDelay(float duration, T obj)
    {
        yield return new WaitForSeconds(duration);
        RemoveInstance(obj);
    }

    public T RemoveInstance(T insntace)
    {
        if (!EnabledList.Remove(insntace))
            return null;
        disabledList.Enqueue(insntace);
        insntace.gameObject.SetActive(false);
        return insntace;
    }

    public void RemoveAllInstance()
    {
        for (var i = EnabledList.Count - 1; i >= 0; i--)
        {
            RemoveInstance(EnabledList[i]);
        }
    }
}
