using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSMovement : SpeedControl
{
    [SerializeField] private float gravity = -9.8f;
    
    private CharacterController characterController;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        var deltaZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        var movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);
        movement.y = gravity * 5 * Time.deltaTime;
        movement = transform.TransformDirection(movement);
        characterController.Move(movement);
    }
}