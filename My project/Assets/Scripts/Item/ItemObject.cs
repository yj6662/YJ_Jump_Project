using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.name}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        if (CharacterManager.Instance != null && CharacterManager.Instance.Player != null)
        {
            CharacterManager.Instance.Player.AddItemToInventory(data);
        }
        Destroy(gameObject);
    }
}
