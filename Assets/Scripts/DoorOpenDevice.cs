using UnityEngine;

public class DoorOpenDevice : BaseDevice
{
    [SerializeField] private Vector3 dPos;
    private bool open;

    public override void Operate()
    {
        if (open)
        {
            Vector3 pos = transform.position - dPos;
            transform.position = pos;
        }
        else
        {
            Vector3 pos = transform.position + dPos;
            transform.position = pos;
        }

        open = !open;
    }

    public void Activate()
    {
        if (open) return;
        var pos = transform.position + dPos;
        transform.position = pos;
        open = true;
    }
    public void Deactivate()
    {
        if (!open) return;
        var pos = transform.position - dPos;
        transform.position = pos;
        open = false;
    }

}