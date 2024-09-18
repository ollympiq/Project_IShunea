using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Image[] slotImages = new Image[2]; 
    private GameObject[] slots = new GameObject[2];
    private List<string> items = new List<string>();

    public bool AddItem(GameObject item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                item.SetActive(false);
                UpdateSlotUI(i, item);
                return true;
            }
        }
        return false;
    }

    public bool HasItem(string itemTag)
    {
        foreach (var slot in slots)
        {
            if (slot != null && slot.CompareTag(itemTag))
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(string itemTag)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].CompareTag(itemTag))
            {
                slots[i] = null;
                UpdateSlotUI(i, null);
                break;
            }
        }
    }

    private void UpdateSlotUI(int slotIndex, GameObject item)
    {
        if (slotIndex >= 0 && slotIndex < slotImages.Length)
        {
            if (item != null)
            {
                
                SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    slotImages[slotIndex].sprite = spriteRenderer.sprite;
                    slotImages[slotIndex].enabled = true;
                }
            }
            else
            {
                
                slotImages[slotIndex].sprite = null;
                slotImages[slotIndex].enabled = false;
            }
        }
    }
}
