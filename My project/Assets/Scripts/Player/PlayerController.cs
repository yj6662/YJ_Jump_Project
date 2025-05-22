using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private float baseMoveSpeed;
    private Vector2 curMovementInput;
    public float jumpPower = 10f;
    private float baseJumpPower;
    public LayerMask groundLayerMask;
    public float maxSlopeAngle = 45f;
    public float slidePower = 1f;
    public float maxSpeed = 5f;
    public float acceleration = 20f;

    [Header("Look")] public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody _rigidbody;
    private UIInventory uiInventory;
    
    private Dictionary<BuffType, float> activeBuffs = new Dictionary<BuffType, float>();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        uiInventory = GetComponent<UIInventory>();
        baseMoveSpeed = moveSpeed;
        baseJumpPower = jumpPower;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canLook = true;
    }
    private void Update()
    {

        List<BuffType> expiredBuffs = new List<BuffType>();
        foreach (var buff in activeBuffs)
        {
            if (Time.time > buff.Value)
            {
                expiredBuffs.Add(buff.Key);
            }
        }

        foreach (var type in expiredBuffs)
        {
            activeBuffs.Remove(type);

            switch (type)
            {
                case BuffType.Speed:
                    moveSpeed = baseMoveSpeed;
                    break;
                case BuffType.Jump:
                    jumpPower = baseJumpPower;
                    break;
            }
        }
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
        horizontalMovement.Normalize();
        horizontalMovement.y = 0f;

        Vector3 targetHorizontalVelocity = horizontalMovement * moveSpeed;
        Vector3 currentHorizontalVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        
        Vector3 velocityChange = targetHorizontalVelocity - currentHorizontalVelocity;
        Vector3 movementForce = velocityChange * acceleration;
        _rigidbody.AddForce(movementForce, ForceMode.Force);
        
        if (horizontalMovement != Vector3.zero) 
        {
            if (currentHorizontalVelocity.magnitude > maxSpeed)
            {
                Vector3 clampedVelocity = currentHorizontalVelocity.normalized * maxSpeed;
                _rigidbody.velocity = new Vector3(clampedVelocity.x, _rigidbody.velocity.y, clampedVelocity.z);
            }
        }
        
        float yVelocity = _rigidbody.velocity.y;
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, yVelocity, _rigidbody.velocity.z);
        
        HandleSlopeMovement(horizontalMovement);
    }
    private void HandleSlopeMovement(Vector3 horizontalMovement)
    {
        RaycastHit hit;
        if (IsGrounded() && Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 0.2f, groundLayerMask))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            if (slopeAngle > maxSlopeAngle)
            {
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized;
                _rigidbody.AddForce(slideDirection * slidePower, ForceMode.Force);
                
                float speedModifier = Mathf.Clamp01(1f - (slopeAngle - maxSlopeAngle) / 10f);
                horizontalMovement *= speedModifier;
            }
        }
    }


    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0f, 0f);
        
        transform.Rotate(Vector3.up * mouseDelta.x * lookSensitivity);
    }
    bool IsGrounded()
    {
        RaycastHit hit;
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
    public void ApplyBuff(ItemDataBuff buff) // 버프 적용 함수
    {
        switch (buff.type)
        {
            case BuffType.Speed:
                moveSpeed = baseMoveSpeed * buff.value; // 이동 속도 증가
                break;
            case BuffType.Jump:
                jumpPower = baseJumpPower * buff.value; // 점프력 증가
                break;
        }

        activeBuffs[buff.type] = Time.time + buff.duration; // 버프 지속 시간 저장
        StartCoroutine(RevertBuff(buff.type, buff.duration)); // 버프 해제 코루틴 시작
    }
    private IEnumerator RevertBuff(BuffType type, float duration) // 버프 해제 코루틴
    {
        yield return new WaitForSeconds(duration); // duration 시간 동안 대기

        switch (type)
        {
            case BuffType.Speed:
                moveSpeed = baseMoveSpeed; // 이동 속도 원래대로
                break;
            case BuffType.Jump:
                jumpPower = baseJumpPower; // 점프력 원래대로
                break;
        }

        activeBuffs.Remove(type); // 활성화된 버프 목록에서 제거
    }
    public void OnQuickSlot1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && uiInventory != null)
        {
            uiInventory.SelectItem(0);
        }
    }
    public void OnQuickSlot2(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && uiInventory != null)
        {
            uiInventory.SelectItem(1);
        }
    }
    public void OnQuickSlot3(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && uiInventory != null)
        {
            uiInventory.SelectItem(2);
        }
    }
    public void OnQuickSlot4(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && uiInventory != null)
        {
            uiInventory.SelectItem(3);
        }
    }
    public void OnQuickSlot5(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && uiInventory != null)
        {
            uiInventory.SelectItem(4);
        }
    }
    public void OnQuickSlot6(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && uiInventory != null)
        {
            uiInventory.SelectItem(5);
        }
    }
    public void OnQuickSlot7(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && uiInventory != null)
        {
            uiInventory.SelectItem(6);
        }
    }
    public void OnQuickSlot8(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && uiInventory != null)
        {
            uiInventory.SelectItem(7);
        }
    }
    public void OnUseQuickSlotItem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && uiInventory != null)
        {
            uiInventory.UseSelectedItem();
        }
    }
}
