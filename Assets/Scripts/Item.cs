using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType = ItemType.Bomb;
    public string itemName = "Bomb";
    public string description = "Destroys nearby walls when used.";

    //Visual effects
    public float rotationSpeed = 60f;
    public float bobSpeed = 4f;
    public float bobHeight = 0.1f;

    protected Vector3 startPosition;

    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Items");

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }
    protected virtual void Start()
    {
        startPosition = transform.position;

        if (gameObject.name.Contains("Clone") || gameObject.name == "Item")
        {
            gameObject.name = itemName + "Item";
        }
    }

    protected virtual void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        Vector3 newPosition = startPosition;
        newPosition.y += Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = newPosition;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //Check if player picked up item
        if (other.CompareTag("Player"))
        {
            //Get player inventory
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                //Try to add item to inventory
                bool itemAdded = inventory.AddItem(itemName);

                //Only destroy the item if it was added
                if (itemAdded)
                {
                    Debug.Log("Player picked up " + itemName);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Item not picked up - inventory full");
                }
            }
        }
    }
}