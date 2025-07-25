using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //inv capacity
    public int maxItems = 4;

    //inv array
    private string[] inventorySlots = new string[4];

    private InventoryUI uiManager;


    //UI for full inventory
    public GameObject inventoryFullMessage;
    private float messageTimer = 0f;
    private float messageDisplayTime = 2.0f;

    void Start()
    {
        uiManager = Object.FindAnyObjectByType<InventoryUI>();

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = "";
        }

        if (inventoryFullMessage != null)
        {
            inventoryFullMessage.SetActive(false);
        }
    }

    void Update()
    {

        //full inv message timer
        if (messageTimer > 0)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0 && inventoryFullMessage != null)
            {
                inventoryFullMessage.SetActive(false);
            }
        }
    }

    //add item to first slot
    public bool AddItem(string itemName)
    {
        //find first inv slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (string.IsNullOrEmpty(inventorySlots[i]))
            {
                inventorySlots[i] = itemName;
                Debug.Log("Added " + itemName + " to inventory slot " + i);
                return true;
            }
        }

        //if no slots found, inv full
        Debug.Log("Inventory full! Cannot add " + itemName);
        ShowInventoryFullMessage();
        return false;
    }

    //Use item type
    public bool UseItem(string itemName)
    {
        //last in first out
        for (int i = inventorySlots.Length - 1; i >= 0; i--)
        {
            if (inventorySlots[i] == itemName)
            {
                //remove item
                inventorySlots[i] = "";
                Debug.Log("Used " + itemName + " from slot " + i);
                return true;
            }
        }
        Debug.Log("No " + itemName + " in inventory!");
        return false;
    }

    //Get all items in the inv
    public string[] GetAllItems()
    {
        return inventorySlots;
    }

    //count items of each type
    public int GetItemCount(string itemName)
    {
        int count = 0;

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == itemName)
            {
                count++;
            }
        }

        return count;
    }

    //display inv
    private void ShowInventoryFullMessage()
    {
        if (inventoryFullMessage != null)
        {
            inventoryFullMessage.SetActive(true);
            messageTimer = messageDisplayTime;
        }
    }
}