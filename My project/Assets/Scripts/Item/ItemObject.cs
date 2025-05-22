using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string prompt = "`E`키를 눌러 상호작용<br>";
        string str = $"{data.name}\n{data.description}";
        return prompt + str;
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
