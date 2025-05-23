using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInventory : MonoBehaviour
{
    [Header("Quick Slot References")] 
    public ItemSlot[] slots;
    public Transform slotPanel;

    private ItemSlot selectedItem;
    private int selectedItemIndex = -1;
    public BuffDuration speedBuffDuration;
    public BuffDuration jumpBuffDuration;
    
    private PlayerController controller;
    private PlayerCondition condition;
    
    void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();

        if (slotPanel != null)
        {
            slots = new ItemSlot[slotPanel.childCount];
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
                if (slots[i] != null)
                {
                    slots[i].index = i;
                    slots[i].inventory = this;
                    slots[i].Clear();
                }
            }
        }
    }

    public void AddItem(ItemData data)
    {
        Debug.Log("UIInventory AddItem Called: " + data.displayName);
        if (data == null) return;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }
    }

    public void UpdateUI()
    {
        Debug.Log("UI Updated!");
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                slots[i].Set();
            }
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].item == data)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void SelectItem(int index)
    {
        if (index < 0 || index >= slots.Length) return;

        if (selectedItemIndex == index) return;
        
        if (selectedItem != null)
        {
            selectedItem.isSelected = false;
            selectedItem.Set();
        }
        
        selectedItemIndex = index;
        selectedItem = slots[index];
        
        if (selectedItem != null)
        {
            selectedItem.isSelected = true;
            selectedItem.Set();
        }
    }
    
    public void UseSelectedItem()
    {
        if (selectedItem == null) return;
        
        ItemData itemToUse = selectedItem.item;

        switch (itemToUse.type)
        {
            case ItemType.Consumable:
                UseConsumableItem(itemToUse);
                RemoveSelectedItem(selectedItemIndex);
                break;
            case ItemType.Equipable:
                if (selectedItem.isSelected)
                {
                    UnEquip(selectedItemIndex);
                }
                else
                {
                    {
                        Equip(selectedItemIndex);
                    }
                }

                break;
            default:
                break;
        }

        UpdateUI();
    }

    private void UseConsumableItem(ItemData consumableItem)
    {
        for (int i = 0; i < consumableItem.consumables.Length; i++)
        {
            switch (consumableItem.consumables[i].type)
            {
                case ConsumableType.Health:
                    condition.Heal(consumableItem.consumables[i].value);
                    break;
                case ConsumableType.Stamina:
                    condition.stamina.Add(consumableItem.consumables[i].value);
                    break;
                case ConsumableType.Buff:
                    foreach (ItemDataBuff buff in consumableItem.buffs)
                    {
                        controller.ApplyBuff(buff);
                        if (buff.type == BuffType.Speed)
                        {
                            speedBuffDuration.StartBuff(buff.duration);
                        }
                        else if (buff.type == BuffType.Jump)
                        {
                            jumpBuffDuration.StartBuff(buff.duration);
                        }
                    }
                    break;
            }
        }
    }

    public void RemoveSelectedItem(int slotIndex)
    {
        ItemSlot slot = slots[slotIndex];
        if (slot == null || slot.item == null) return;
        
        slot.quantity--;

        if (slot.quantity <= 0)
        {
            if (slot.isSelected)
            {
                UnEquip(slotIndex);
            }
            slot.Clear();
            if (selectedItemIndex == slotIndex)
            {
                selectedItemIndex = -1;
                selectedItem = null;
            }
        }
        UpdateUI();
    }
    
    public void Equip(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;
        
        slots[slotIndex].isSelected = true;
        UpdateUI();
    }

    public void UnEquip(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;
        
        slots[slotIndex].isSelected = false;
        UpdateUI();
    }
}
