using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCharacterController : MonoBehaviour
{
    public static TankCharacterController Instance;

    [Header("Setup")]
    public GameObject m_CameraObject;
    public GameObject m_VisualElements;
    public GameObject m_HoldableObject;
    public CapsuleCollider m_Collider;

    [Header("Movement")]
    public float m_MovementSpeed = 10f;
    public float m_MovementSpeedCrouch = 5f;
    public float m_Acceleration = 1f;
    public float m_VisualRotationSpeed = 20f;

    [Header("Camera")]
    public float m_CameraSensitivity = 5f;
    public float m_CameraSmoothing = 2f;
    public float m_CameraZoomSensitivity = 2f;
    public float m_CameraZoomSmoothing = 2f;
    public float m_MinZoom = 1f;
    public float m_MaxZoom = 5f;
    public float m_CameraMaxPitch = 89f;
    public float m_CameraMinPitch = 25;

    [Header("Misc")]
    public float m_KeyMovement = -0.2f;

    private Holdable m_Holdable;

    private bool m_Crouched;
    private Quaternion m_CurrentVisualRotationTarget;
    private Vector3 m_InputMovementVector;
    private Vector3 m_LookDirection;
    private Vector2 m_CameraAngle = new Vector2(45f, 0f);
    private Vector2 m_CameraTargetAngle;
    private Quaternion m_CameraRotation;
    private Vector3 m_CameraVector;
    private float m_CameraZoom;
    private float m_CameraTargetZoom;
    private Quaternion m_CurrentVisualRotation;
    private bool m_Alive = true;

    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private float m_LastOnGround;

    public Holdable Holdable
        => m_Holdable;

    public bool IsHolding
        => this.Holdable != null;

    public float MovementSpeed
        => m_Crouched ? m_MovementSpeedCrouch : m_MovementSpeed;

    public bool Alive
        => m_Alive;

    public bool IsGrounded
    {
        get
        {
            RaycastHit hit;
            if (Physics.Raycast(m_Collider.transform.position - new Vector3(0f, m_Collider.height * 0.4f, 0f), -Vector3.up, out hit, m_Collider.height * 0.2f))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool IsFalling
        => Time.time - m_LastOnGround > 0.15f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.DestroyImmediate(gameObject);
        }
    }
    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            GameObject.DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;

        m_Rigidbody = this.GetComponent<Rigidbody>();
        m_CameraZoom = (m_MinZoom + m_MaxZoom) * 0.35f;
        m_CameraTargetZoom = m_CameraZoom;
        m_CameraTargetAngle = m_CameraAngle;

        m_Animator = this.GetComponentInChildren<Animator>();

        GameManager.Instance.LockCursor();
    }

    private void OnEnable()
    {
        Instance = this;
    }

    private void Update()
    {
        this.ProcessInput();
        this.ProcessCamera();
        this.ProcessVisualElements();
        this.UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (this.IsGrounded)
        {
            m_LastOnGround = Time.time;
        }

        this.ProcessMovement();
    }

    public void Kill()
    {
        m_Alive = false;

        this.Drop();

        SoundManager.Instance?.SetMusicType(MusicType.Death);
    }

    public void Pickup(Holdable holdable)
    {
        this.Drop();

        m_Holdable = holdable;
        m_Holdable.transform.SetParent(this.m_HoldableObject.transform);
        m_Holdable.transform.position = m_HoldableObject.transform.position;
        m_Holdable.Freeze();
    }

    public void Drop()
    {
        if (this.Holdable == null)
        {
            return;
        }

        this.Holdable.transform.SetParent(transform.root);
        this.Holdable.Unfreeze();

        var fwd = m_LookDirection.normalized;
        var pos = m_HoldableObject.transform.position + fwd * 0.8f;

        RaycastHit hit;
        if (Physics.Raycast(m_HoldableObject.transform.position, fwd, out hit, 0.8f))
        {
            pos = hit.point;
        }

        this.Holdable.transform.position = pos;

        m_Holdable = null;
    }

    public void Revive()
    {
        m_Alive = true;
    }

    private void UpdateAnimator()
    {
        var visualRotationVector = m_CurrentVisualRotation * Vector3.forward;
        var forwardSpeed = m_Rigidbody.velocity.magnitude * Vector3.Dot(m_Rigidbody.velocity.normalized, visualRotationVector);

        m_Animator.SetFloat("ForwardSpeed", Mathf.Clamp(forwardSpeed / this.MovementSpeed, 0f, 1f));
        m_Animator.SetBool("Crouched", m_Crouched);
        m_Animator.SetBool("Alive", this.Alive);
        m_Animator.SetBool("Falling", this.IsFalling);
    }

    private void ProcessInput()
    {
        if (GameManager.Instance.Paused)
        {
            return;
        }

        m_Crouched = Input.GetButton("Crouch");

        var forward = Input.GetAxis("Vertical");
        var right = Input.GetAxis("Horizontal");

        if (GameManager.Instance.IsGameOver)
        {
            forward = 0f;
            right = 0f;
        }

        var cameraRotation2D = Quaternion.Euler(new Vector3(0f, m_CameraAngle.y, 0f));
        var forwardVector = cameraRotation2D * Vector3.forward;
        var rightVector = cameraRotation2D * Vector3.right;

        m_InputMovementVector = forwardVector * forward + rightVector * right;
        m_InputMovementVector = m_InputMovementVector.normalized;
        m_InputMovementVector.Scale(new Vector3(this.MovementSpeed, this.MovementSpeed, this.MovementSpeed));

        if (m_InputMovementVector.magnitude > 0.01f)
        {
            m_LookDirection = m_InputMovementVector.normalized;
        }

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");
        var mouseScroll = -Input.GetAxis("Mouse ScrollWheel");

        if (GameManager.Instance.IsGameOver)
        {
            mouseX = 0f;
            mouseY = 0f;
            mouseScroll = 0f;
        }

        m_CameraTargetAngle.x -= mouseY * m_CameraSensitivity;
        m_CameraTargetAngle.y += mouseX * m_CameraSensitivity;
        m_CameraTargetZoom = Mathf.Clamp(m_CameraTargetZoom + mouseScroll * m_CameraZoomSensitivity, m_MinZoom, m_MaxZoom);
        m_CameraZoom = Mathf.Lerp(m_CameraZoom, m_CameraTargetZoom, Time.deltaTime * m_CameraZoomSmoothing);

        m_CameraTargetAngle.x = Mathf.Clamp(m_CameraTargetAngle.x, m_CameraMinPitch, m_CameraMaxPitch);
        m_CameraAngle = Vector2.Lerp(m_CameraAngle, m_CameraTargetAngle, Time.deltaTime * m_CameraSmoothing);

        if (!this.Alive)
        {
            m_Crouched = false;
            m_InputMovementVector = Vector3.zero;
        }
    }

    private void ProcessCamera()
    {
        m_CameraRotation = Quaternion.Euler(new Vector3(m_CameraAngle.x, m_CameraAngle.y, 0f));
        m_CameraVector = m_CameraRotation * Vector3.forward;

        m_CameraObject.transform.localPosition = -m_CameraVector * m_CameraZoom;
        m_CameraObject.transform.rotation = m_CameraRotation;
    }

    private void ProcessVisualElements()
    {
        var direction = m_Rigidbody.velocity;
        direction.y = 0f;
        direction = direction.normalized;

        if (direction.magnitude > 0f)
        {
            m_CurrentVisualRotationTarget = Quaternion.LookRotation(direction);
        }

        if (m_CurrentVisualRotation == m_CurrentVisualRotationTarget)
        {
            return;
        }

        var degreeDiff = Quaternion.Angle(m_CurrentVisualRotationTarget, m_CurrentVisualRotation);

        m_CurrentVisualRotation = Quaternion.RotateTowards(m_CurrentVisualRotation, m_CurrentVisualRotationTarget, degreeDiff * Time.deltaTime * m_VisualRotationSpeed);
        m_VisualElements.transform.rotation = m_CurrentVisualRotation;
    }

    private void ProcessMovement()
    {
        var velocity = m_Rigidbody.velocity;
        var diff = m_InputMovementVector - velocity;

        var adjustAmount = Mathf.Min(diff.magnitude, m_Acceleration);
        var adjustVelocity = diff.normalized * adjustAmount;
        adjustVelocity.y = 0f;

        m_Rigidbody.AddForce(adjustVelocity, ForceMode.VelocityChange);
    }
}
