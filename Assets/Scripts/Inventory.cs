using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        // accessing ItemCollect Script/Class, then the event += add a listener
        ItemCollect.ItemCollected += IncrementItem;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Veggietype is passed in by the invoke in Item collect
    void IncrementItem(Item.VegetableType veggieType)
    {
        CountGUI count = items[(int)veggieType].GetComponent<CountGUI>();
        count.UpdateCountBroadcast();
    }
}
