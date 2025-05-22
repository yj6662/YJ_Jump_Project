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

public enum BuffType
{
    Speed,
    Jump,
}

[System.Serializable]
public class ItemDataConsumable
{
    public  ConsumableType type;
    public float value;
}

[System.Serializable]
public class ItemDataBuff
{
    public BuffType type;
    public float value;
    public float duration;
}
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject prefab;
    
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
    
    [Header("Buff")]
    public ItemDataBuff[] buffs;

    [Header("Stackable")]
    public bool canStack;
    public int stackSize;
}
