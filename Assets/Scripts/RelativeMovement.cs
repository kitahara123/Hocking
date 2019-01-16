using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] private float speedModifier = 1;
    [SerializeField] private Transform target;
    [SerializeField] private float baseRotSpeed = 15.0f;
    [SerializeField] private float baseMovementSpeed = 6.0f;
    [SerializeField] private float baseJumpForce = 15.0f;
    [SerializeField] private float baseGravity = -9.8f;
    [SerializeField] private float maxVelocity = -18.0f;
    [SerializeField] private float baseMinFall = -1.5f;
    [SerializeField] private float pushForce = 3.0f;
    [SerializeField] private float offset = 1.3f;

    private float vertSpeed;
    private float movementSpeed;
    private float rotSpeed;
    private float jumpForce;
    private float gravity;
    private float minFall;
    private CharacterController characterController;
    private ControllerColliderHit contact;
    private Animator animator;

    [SerializeField] private float baseDeceleration = 20.0f;
    [SerializeField] private float targetBuffer = 1.5f;
    private Vector3 targetPos = Vector3.one;
    private float curSpeed = 0f;
    private float deceleration;

    private void Awake() => Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    private void OnDestroy() => Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);

    private void Start()
    {
        speedModifier = PlayerPrefs.GetFloat("Speed", speedModifier);
        OnSpeedChanged(speedModifier);

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void OnSpeedChanged(float value)
    {
        speedModifier = value;
        movementSpeed = baseMovementSpeed * speedModifier;
        rotSpeed = baseRotSpeed * speedModifier;
        jumpForce = baseJumpForce * speedModifier;
        gravity = baseGravity * speedModifier;
        minFall = baseMinFall * speedModifier;
        vertSpeed = minFall * speedModifier;
        deceleration = baseDeceleration * speedModifier;
    }

    private void Update()
    {
        var isometric = Managers.Managers.Settings.Isometric;
        var movement = isometric ? PointClickMovement() : WASDMovement();

        animator.speed = speedModifier;
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
                    movement = contact.normal * movementSpeed;
                else
                    movement += contact.normal * movementSpeed;
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

        return movement;
    }

    private Vector3 PointClickMovement()
    {
        var movement = Vector3.zero;
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit))
            {
                if (mouseHit.transform.gameObject.layer != LayerMask.NameToLayer("Ground")) return Vector3.zero;
                targetPos = mouseHit.point;
                curSpeed = movementSpeed;
            }
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