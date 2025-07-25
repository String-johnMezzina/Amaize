using UnityEngine;

public class JumpItem : Item
{
    protected override void Awake()
    {
        itemType = ItemType.Jump;
        itemName = "Jump";
        description = "Jump over walls.";
    }

    protected override void Start()
    {
        base.Start();

        if (gameObject.name.Contains("Clone") || gameObject.name == "Item")
        {
            gameObject.name = "JumpItem";
        }
    }
}
