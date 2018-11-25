using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInput : MonoBehaviour
{
    [SerializeField] private float speed = 6f;

    private void Update()
    {
        var deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        var deltaZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(deltaX, 0, deltaZ);
    }
}