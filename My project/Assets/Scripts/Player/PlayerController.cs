using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 curMovementInput;
    public float jumpPower = 10f;
    public LayerMask groundLayerMask;
    public float maxSlopeAngle = 45f;

    [Header("Look")] public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody _rigidbody;
    //private UIInventory _uiInventory;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_uiInventory = GetComponent<UIInventory>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canLook = true;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        Vector3 horizontalMovement = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        
        // 대각선 이동 시 속도가 빨라지는 것을 방지
        if (curMovementInput.magnitude > 1) 
        {
            horizontalMovement.Normalize(); 
        }

        float yVelocity = _rigidbody.velocity.y; 
        
        RaycastHit hit;
        bool grounded = TryGetGroundHit(out hit);

        if (grounded)
        {
            float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            
            if (slopeAngle > maxSlopeAngle)
            {
                horizontalMovement = Vector3.zero;
                // TODO: 필요하다면 미끄러지는 힘을 주거나 다른 경사 처리 로직 추가
            }
        }
        

        Vector3 finalVelocity = horizontalMovement * moveSpeed;
        finalVelocity.y = yVelocity;

        _rigidbody.velocity = finalVelocity;
    }


    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0f, 0f);
        
        transform.eulerAngles += new Vector3(0f, mouseDelta.x * lookSensitivity, 0f);
    }
    bool IsGrounded()
    {
        RaycastHit hit; // hitInfo는 필요 없지만 TryGetGroundHit의 out 키워드를 위해 선언
        return TryGetGroundHit(out hit);
    }

    private bool TryGetGroundHit(out RaycastHit hitInfo)
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };
        
        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], out hitInfo, 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        
        hitInfo = default;
        return false;
    }
}
