using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;
using System;

public class CountGUI : NetworkBehaviour
{
    private TextMeshProUGUI tmProElement;

    public NetworkVariable<int> count = new NetworkVariable<int>(0);

    public string itemName;

    void Start()
    {
        tmProElement = GetComponent<TextMeshProUGUI>();
        count.OnValueChanged += OnCountValueChanged;
    }

    public override void OnNetworkSpawn()
    {
        UpdateText();
    }

    public void UpdateCountBroadcast()
    {
        if (IsServer)
        {
            UpdateCount();
        }
        else
        {
            UpdateCountRpc();
        }
    }

    private void OnCountValueChanged(int previousValue, int newValue)
    {
        UpdateText();
    }

    [Rpc(SendTo.Server)]
    public void UpdateCountRpc()
    {
        UpdateCount();
    }

    public void UpdateCount()
    {
        count.Value++;
    }

    public void UpdateText()
    {
        tmProElement.text = itemName + ": " + count.Value;
    }
}
