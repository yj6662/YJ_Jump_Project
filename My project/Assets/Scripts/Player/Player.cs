using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public UIInventory uiInventory;

    private void Awake()
    {
        if (CharacterManager.Instance != null)
        {
            CharacterManager.Instance.Player = this;
        }
        
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        uiInventory = GetComponent<UIInventory>();
    }

    public void AddItemToInventory(ItemData data)
    {
        Debug.Log("Player AddItemToInventory Called: " + data.displayName);
        if (uiInventory != null)
        {
            uiInventory.AddItem(data);
        }
        else
        {
            Debug.Log("UIInventory is null!");
        }
    }
}
