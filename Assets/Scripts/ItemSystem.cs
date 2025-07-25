using UnityEngine;
using System.Collections.Generic;

public enum ItemType
{
    Bomb,
    Jump,
    
}
public class ItemSystem : MonoBehaviour
{
    [System.Serializable]
    public class ItemPrefabMapping
    {
        public ItemType itemType;
        public GameObject prefab;
        [Range(0, 100)]
        public int weight = 100; //chance of spawning (higher = more common)
    }

    public List<ItemPrefabMapping> itemPrefabs = new List<ItemPrefabMapping>();

    //% Chance of spawning an item in each cell 
    [Range(0, 100)]
    public int itemSpawnChance = 18;

    //Height above ground to spawn items
    public float itemHeight = 0.4f;

    private static ItemSystem _instance;
    public static ItemSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindFirstObjectByType<ItemSystem>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject("ItemSystem");
                    _instance = obj.AddComponent<ItemSystem>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    //Try to spawn an item at a given position
    public GameObject TrySpawnItem(Vector3 position, Transform parent = null)
    {
        //Check if we should spawn an item based on chance
        if (Random.Range(0, 100) >= itemSpawnChance)
        {
            return null; //No item spawn
        }

        //Select which item to spawn based on weights
        GameObject prefab = SelectRandomItemPrefab();
        if (prefab == null)
        {
            Debug.LogWarning("No item prefabs available to spawn");
            return null;
        }

        //Adjust height
        position.y = itemHeight;

        //Spawn item
        GameObject item = Instantiate(prefab, position, Quaternion.identity, parent);

        //log
        Debug.Log("Spawned item: " + item.name + " at " + position);

        return item;
    }

    //Select random item 
    private GameObject SelectRandomItemPrefab()
    {
        if (itemPrefabs.Count == 0)
            return null;

        //Calculate total weight
        int totalWeight = 0;
        foreach (var item in itemPrefabs)
        {
            totalWeight += item.weight;
        }

        //No valid items
        if (totalWeight <= 0)
            return null;

        //Random selection based on weight
        int randomValue = Random.Range(0, totalWeight);
        int weightSum = 0;

        foreach (var item in itemPrefabs)
        {
            weightSum += item.weight;
            if (randomValue < weightSum)
            {
                return item.prefab;
            }
        }
        return itemPrefabs[0].prefab;
    }
}
