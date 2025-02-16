using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Class level variables
    private Rigidbody rbPlayer;
    private Vector3 direction = Vector3.zero;

    [SerializeField]
    private float forceMultiplier = 1.0f;

    [SerializeField]
    private ForceMode forceMode;
    public GameObject spawnPoint;

    // Dictionary where the key value pairs are the Vegetable Type and the count
    private Dictionary<Item.VegetableType, int> inventory = new Dictionary<Item.VegetableType, int>();



    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();

        // populate inventory dictionary with vegetable types and their counts
        // System.Enum.GetValues looks at the code to find enum values (reflection)
        foreach (Item.VegetableType type in System.Enum.GetValues(typeof(Item.VegetableType)))
        {
            inventory.Add(type, 0);
        }
    }

    void Update()
    {
        // local variables - they're local to Update, but not to other methods
        float horizontalVelocity = Input.GetAxis("Horizontal");
        float verticalVelocity = Input.GetAxis("Vertical");

        direction = new Vector3(horizontalVelocity, 0, verticalVelocity);
    }

    // Fixed Update is called once per frame. along with the Unity's Physics Engine
    void FixedUpdate()
    {
        rbPlayer.AddForce(direction * forceMultiplier, forceMode);

        // rbPlayer.AddForce(new Vector3(horizontalVelocity, 0f, verticalVelocity), ForceMode.Impulse);

        // Vector3 force = new Vector3 (horizontalVelocity, verticalVelocity, 0f);
        // rbPlayer.AddForce(force, ForceMode.Impulse);

        if (transform.position.z > 38)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 38);
        }
        else if (transform.position.z < -38)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -38);
        }
    }
    private void Respawn()
    {
        rbPlayer.MovePosition(spawnPoint.transform.position);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Item"))
        {
            Item item = collider.gameObject.GetComponent<Item>();   
            AddItemToInventory(item);
            PrintInventory();
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
 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hazard"))
        {
            Respawn();
        }
    }
}
