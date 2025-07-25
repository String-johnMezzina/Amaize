using UnityEngine;

public class BombItem : Item
{
    protected override void Awake()
    {
        itemType = ItemType.Bomb;
        itemName = "Bomb";
        description = "Destroys nearby walls. Press B to activate.";
    }

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;

        if (gameObject.name.Contains("Clone") || gameObject.name == "Item")
        {
            gameObject.name = "BombItem";
        }
    }
}