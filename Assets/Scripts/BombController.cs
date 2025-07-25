using UnityEngine;

public class BombController : MonoBehaviour
{
    public float explosionRadius = 1.8f;
    public KeyCode useBombKey = KeyCode.B;    //Key to use the bomb

    private PlayerInventory inventory;

    void Start()
    {
        //Get reference to the inventory
        inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        //Check for bomb use 
        if (Input.GetKeyDown(useBombKey))
        {
            UseBomb();
        }
    }

    void UseBomb()
    {
        //check inv for bomb
        if (inventory != null && inventory.UseItem("Bomb"))
        {
            Debug.Log("Used a bomb!");

            //Find walls within explosion radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
            bool destroyedWall = false;

            foreach (var hitCollider in hitColliders)
            {
                //Check if it's a wall, if it is then destroy it
                if (hitCollider.CompareTag("Wall"))
                {
                    Debug.Log("Destroying wall: " + hitCollider.name);
                    Destroy(hitCollider.gameObject);
                    destroyedWall = true;
                }
            }
            if (!destroyedWall)
            {
                Debug.Log("No walls in range");
            }
        }
        else
        {
            Debug.Log("No bomb in inventory");
        }
    }
}