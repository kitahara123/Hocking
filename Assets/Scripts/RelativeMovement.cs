using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotSpeed = 15.0f;
    [SerializeField] private float movementSpeed = 6.0f;
    [SerializeField] private float jumpForce = 15.0f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float maxVelocity = -18.0f;
    [SerializeField] private float minFall = -1.5f;

    private float vertSpeed;
    private CharacterController characterController;
    private ControllerColliderHit contact;

    private void Start()
    {
        vertSpeed = minFall;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        var movement = Vector3.zero;

        var horInput = Input.GetAxis("Horizontal");
        var vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            movement.x = horInput * movementSpeed;
            movement.z = vertInput * movementSpeed;
            movement = Vector3.ClampMagnitude(movement, movementSpeed);


            var tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            var direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        var check = (characterController.height + characterController.radius) / 1.9f;

        if (vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, check))
        {
            vertSpeed = Input.GetButtonDown("Jump") ? jumpForce : minFall;
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < maxVelocity) vertSpeed = maxVelocity;

            if (characterController.isGrounded)
            {
                if (Vector3.Dot(movement, contact.normal) < 0)
                    movement = contact.normal * movementSpeed;
                else
                    movement += contact.normal * movementSpeed;
            }
        }

        movement.y = vertSpeed;
        movement *= Time.deltaTime;
        characterController.Move(movement);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contact = hit;
    }
}