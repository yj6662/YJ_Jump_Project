using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("Detection")] 
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance = 1.5f;
    public LayerMask layerMask;

    [Header("UI")] 
    public TextMeshProUGUI promptText;
    
    private GameObject curInteractGameObject;
    private IInteractable curInteractable;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            enabled = false;
            return;
        }
        
        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = curInteractGameObject.GetComponent<IInteractable>();

                    if (curInteractable != null)
                    {
                        SetPromptText();
                    }
                    else
                    {
                        ClearInteractionTarget();
                    }
                }
            }
            else
            {
                ClearInteractionTarget();
            }
        }
    }
    private void SetPromptText()
    {
        if (promptText != null)
        {
            promptText.text = curInteractable.GetInteractPrompt();
            promptText.gameObject.SetActive(true);
        }
    }

    private void ClearInteractionTarget()
    {
        curInteractGameObject = null;
        curInteractable = null;
        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            Debug.Log("Interact!");
            curInteractable.OnInteract();
            ClearInteractionTarget();
        }
    }
}
