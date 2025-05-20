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
        Debug.Log("ItemObject OnInteract Called! Item: " + data.displayName);
        if (CharacterManager.Instance != null && CharacterManager.Instance.Player != null)
        {
            CharacterManager.Instance.Player.AddItemToInventory(data);
        }
        Destroy(gameObject);
    }
}
