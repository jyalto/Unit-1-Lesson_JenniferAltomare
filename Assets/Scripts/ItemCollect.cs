using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ItemCollect : NetworkBehaviour
{
    // Creates a CollectItem type that can be called
    public delegate void CollectItem(Item.VegetableType item);

    // Declare an event of type CollectItem
    public static event CollectItem ItemCollected;

    // Dictionary where the key value pairs are the Vegetable Type and the count
    private Dictionary<Item.VegetableType, int> inventory = new Dictionary<Item.VegetableType, int>();

    private Collider collidingItem;

    void Start()
    {
        // populate inventory dictionary with vegetable types and their counts
        // System.Enum.GetValues looks at the code to find enum values (reflection)
        foreach (Item.VegetableType type in System.Enum.GetValues(typeof(Item.VegetableType)))
        {
            inventory.Add(type, 0);
        }
    }
    void Update()
    {
        if (collidingItem != null && Input.GetKeyDown(KeyCode.Space))
        {
            Item item = collidingItem.gameObject.GetComponent<Item>();
            AddItemToInventory(item);

            // When an item is collected, broadcast to event
            ItemCollected?.Invoke(item.typeOfVeggie);
            PrintInventory();
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (!IsLocalPlayer)
        {
            return;
        }

        if (collider.CompareTag("Item"))
        {
            collidingItem = collider;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (!IsLocalPlayer)
        {
            return;
        }
        if (collider.CompareTag("Item"))
        {
            collidingItem = null;
        }
    }

    private void AddItemToInventory(Item item)
    {
        inventory[item.typeOfVeggie]++;
    }

    private void PrintInventory()
    {
        string output = "";
        foreach (KeyValuePair<Item.VegetableType, int> pair in inventory)
        {
            output += string.Format("{0}: {1}; ", pair.Key, pair.Value);
        }
        Debug.Log(output);
    }
}
