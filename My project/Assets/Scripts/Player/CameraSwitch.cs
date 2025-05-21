using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;
    public Camera thirdPersonCamera;

    private bool isThirdPerson = false;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        if (thirdPersonCamera == null)
        {
            thirdPersonCamera = GetComponent<Camera>();
        }
        
        mainCamera.enabled = true;
        thirdPersonCamera.enabled = false;
    }

    public void OnSwitchCameraInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isThirdPerson = !isThirdPerson;
            
            mainCamera.gameObject.SetActive(!isThirdPerson);
            mainCamera.enabled = !isThirdPerson;
            thirdPersonCamera.gameObject.SetActive(isThirdPerson);
            thirdPersonCamera.enabled = isThirdPerson;
        }
    }
}
