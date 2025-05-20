using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipable
}

public enum ConsumableType
{
    Health,
    Stamina,
    Buff,
}

[System.Serializable]
public class ItemDataConsumable
{
    public  ConsumableType type;
    public float value;
}
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string name;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject prefab;
    
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Stackable")]
    public bool canStack;
    public int stackSize;
}
