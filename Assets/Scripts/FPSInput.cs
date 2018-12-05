using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 6.0f;
    [SerializeField] private float gravity = -9.8f;
    private float speed;
    
    private CharacterController characterController;
    
    private void Awake() => Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    private void OnDestroy() => Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);


    private void Start()
    {
        speed = baseSpeed * PlayerPrefs.GetFloat("Speed", 1);
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
    private void OnSpeedChanged(float value) => speed = baseSpeed * value;
}