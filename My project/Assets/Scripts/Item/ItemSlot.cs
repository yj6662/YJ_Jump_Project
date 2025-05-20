using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [Header("Item Data")] public ItemData item;
    
    [Header("UI References")]
    public UIInventory inventory;

    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;
    
    [Header("Slot State")]
    public int index;
    public bool isSelected;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        if (outline != null)
        {
            outline.enabled = isSelected;
        }
    }

    public void Set()
    {
        if (item == null)
        {
            Clear();
            return;
        }
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.gameObject.SetActive(true);
        quantityText.text = quantity.ToString();

        if (outline != null)
        {
            outline.enabled = isSelected;
        }
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.gameObject.SetActive(false);
        outline.enabled = false;
        isSelected = false;
    }
}
