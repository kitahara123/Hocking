using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : SpeedControl
{
    [SerializeField] private Transform target;
    [SerializeField] private float baseRotSpeed = 15.0f;

    [SerializeField] private float baseJumpForce = 15.0f;
    [SerializeField] private float baseGravity = -9.8f;
    [SerializeField] private float maxVelocity = -18.0f;
    [SerializeField] private float baseMinFall = -1.5f;
    [SerializeField] private float pushForce = 3.0f;
    [SerializeField] private float offset = 1.3f;

    private float vertSpeed;
    private float rotSpeed;
    private float jumpForce;
    private float gravity;
    private float minFall;
    private CharacterController characterController;
    private ControllerColliderHit contact;
    private Animator animator;

    [SerializeField] private float baseDeceleration = 20.0f;
    [SerializeField] private float targetBuffer = 1.5f;
    public static Vector3 targetPos = Vector3.one;
    private float curSpeed = 0f;
    private float deceleration;
    private bool isometric;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isometric = Managers.Managers.Settings.Isometric;
        Messenger<bool>.AddListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Messenger<bool>.RemoveListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
    }

    private void OnIsometricEnabled(bool value) => isometric = value;

    protected override void OnSpeedChanged(float value)
    {
        base.OnSpeedChanged(value);
        rotSpeed = baseRotSpeed * speedModifier;
        jumpForce = baseJumpForce * speedModifier;
        gravity = baseGravity * speedModifier;
        minFall = baseMinFall * speedModifier;
        vertSpeed = minFall * speedModifier;
        deceleration = baseDeceleration * speedModifier;
    }

    private void Update()
    {
        var movement = isometric ? PointClickMovement() : WASDMovement();

        animator.SetFloat("Speed", movement.sqrMagnitude);

        var check = (characterController.height + characterController.radius) / 1.9f;

        if (vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, check))
        {
            if (Input.GetButtonDown("Jump") && !isometric)
                vertSpeed = jumpForce;
            else
            {
                vertSpeed = minFall;
                animator.SetBool("Jumping", false);
            }
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;

            if (vertSpeed < maxVelocity) vertSpeed = maxVelocity;

            if (contact != null)
            {
                animator.SetBool("Jumping", true);
            }

            if (characterController.isGrounded)
            {
                if (Vector3.Dot(movement, contact.normal) < 0)
                    movement = contact.normal * speed;
                else
                    movement += contact.normal * speed;
            }
        }

        movement.y = vertSpeed;
        movement *= Time.deltaTime;
        characterController.Move(movement);
    }

    private Vector3 WASDMovement()
    {
        var movement = Vector3.zero;
        var horInput = Input.GetAxis("Horizontal");
        var vertInput = Input.GetAxis("Vertical");


        if (horInput != 0 || vertInput != 0)
        {
            movement.x = horInput * speed;
            movement.z = vertInput * speed;
            movement = Vector3.ClampMagnitude(movement, speed);

            var tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            var direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        return movement;
    }

    private Vector3 PointClickMovement()
    {
        var movement = Vector3.zero;
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit))
            {
                targetPos = mouseHit.transform.gameObject.layer == LayerMask.NameToLayer("Ground")
                    ? mouseHit.point
                    : mouseHit.transform.gameObject.GetComponent<Collider>().ClosestPoint(transform.position);
                curSpeed = speed;
            }
        }

        if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit))
            {
                targetPos = mouseHit.transform.gameObject.layer == LayerMask.NameToLayer("Ground")
                    ? mouseHit.point
                    : new Vector3(mouseHit.point.x, transform.position.y - characterController.height / 2f,
                        mouseHit.point.z);
            }

            curSpeed = 0;
        }

        if (targetPos != Vector3.one && Vector3.Distance(targetPos, transform.position) > offset && speedModifier > 0)
        {
            if (Vector3.Distance(targetPos, transform.position) > targetBuffer)
            {
                var adjustedPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
                var targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            }

            movement = curSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);

            if (Vector3.Distance(targetPos, transform.position) < targetBuffer)
            {
                curSpeed -= deceleration * Time.deltaTime;
                if (curSpeed <= 0)
                {
                    targetPos = Vector3.one;
                }
            }
        }

        return movement;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contact = hit;

        var body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }
}