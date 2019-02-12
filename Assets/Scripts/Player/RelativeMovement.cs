using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 6;
    [SerializeField] private float rotSpeed = 15.0f;

    [SerializeField] private float jumpForce = 15.0f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float maxVelocity = -18.0f;
    [SerializeField] private float minFall = -1.5f;
    [SerializeField] private float pushForce = 3.0f;
    [SerializeField] private float offset = 1.3f;
    [SerializeField] private float deceleration = 20.0f;
    [SerializeField] private float targetBuffer = 1.5f;

    private float vertSpeed;
    private CharacterController characterController;
    private ControllerColliderHit contact;
    private Animator animator;
    private float curSpeed = 0f;
    private bool isometric;
    private bool paused = false;

    public static Vector3 targetPos = Vector3.one;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isometric = Managers.Managers.Settings.Isometric;
        Messenger<bool>.AddListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }


    private void OnDestroy()
    {
        Messenger<bool>.RemoveListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    private void OnSpeedChanged(float value)
    {
        paused = value == 0;
    }

    private void OnIsometricEnabled(bool value) => isometric = value;

    private void Update()
    {
        if (paused)
        {
            animator.SetFloat("Speed", 0);
            return;
        }

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

        if (targetPos != Vector3.one && Vector3.Distance(targetPos, transform.position) > offset)
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