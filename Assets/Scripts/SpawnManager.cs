using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnManager : NetworkBehaviour
{
    public GameObject[] lilyPads;

    public float spawnRate = 2.0f;

    // OnNetworkSpawn is the network equivalent of start
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        InvokeRepeating("SpawnLilyPad", 2.0f, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnLilyPad()
    {
        foreach (GameObject lilyPad in lilyPads) 
        {
            NetworkObject lilyPadObject = Instantiate(lilyPad).GetComponent<NetworkObject>();
            lilyPadObject.Spawn();
        }
    }
}
