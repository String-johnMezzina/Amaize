using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    //UI elements
    public GameObject inventoryPanel;
    public Image[] itemSlots = new Image[4]; //array 4 inv slots
    private PlayerInventory playerInventory;

    //item sprites
    public Sprite bombSprite;
    public Sprite jumpSprite;

    void Start()
    {
        playerInventory = Object.FindFirstObjectByType<PlayerInventory>();
        if (playerInventory == null)
        {
            Debug.LogError("Player Inventory not found!");
        }

        UpdateInventoryUI();
    }

    void Update()
    {
        if (playerInventory != null)
        {
            UpdateInventoryUI();
        }
    }

    void UpdateInventoryUI()
    {
        if (playerInventory == null) return;

        //Get current inventory 
        string[] items = playerInventory.GetAllItems();

        //Update each slot
        for (int i = 0; i < itemSlots.Length; i++)
        {
            //Skip null slots
            if (itemSlots[i] == null) continue;

            //Clear the slot
            itemSlots[i].enabled = true; 

            if (i < items.Length && !string.IsNullOrEmpty(items[i]))
            {
                //set sprite for item slot
                switch (items[i])
                {
                    case "Bomb":
                        if (bombSprite != null)
                        {
                            itemSlots[i].sprite = bombSprite;
                            itemSlots[i].color = Color.white; 
                        }
                        break;

                    case "Jump":
                        if (jumpSprite != null)
                        {
                            itemSlots[i].sprite = jumpSprite;
                            itemSlots[i].color = Color.white; 
                        }
                        break;

                    default:
                        itemSlots[i].sprite = null;
                        itemSlots[i].color = new Color(1, 1, 1, 0.2f); 
                        break;
                }
            }
            
            //slot is empty
            else
            {
                itemSlots[i].sprite = null;
                itemSlots[i].color = new Color(1, 1, 1, 0.2f); 
            }
        }
    }
}