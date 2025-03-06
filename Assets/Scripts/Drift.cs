using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Drift : NetworkBehaviour
{
    public float speed = 5.0f;
    public enum DriftDirection
    {
        LEFT = -1,
        RIGHT = 1
    }
    public DriftDirection driftDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer)
        {
            return;
        }
        //transform.Translate(new Vector3(-1, 0, 0)); 
        transform.Translate(Vector3.right * Time.deltaTime * speed * (int)driftDirection); 

        if (transform.position.x < -80 || transform.position.x > 80)
        {
            for(int i  = 0; i < transform.childCount; i++)
            {
                NetworkObject player = transform.GetChild(i).GetComponent<NetworkObject>();
                player.TryRemoveParent(); // sets the parent of player to this
            }
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            NetworkObject player = collision.gameObject.GetComponent<NetworkObject>();
            player.TrySetParent(transform); // sets the parent of player to this
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            NetworkObject player = collision.gameObject.GetComponent<NetworkObject>();
            player.TryRemoveParent(); // sets the parent of player to this
        }
    }
}
