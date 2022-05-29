using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int[] quantity;
    public PlayerData(PlayerInventory inventory)
    {
        quantity = new int[inventory.Count.Length];
        for(int i=0;i<quantity.Length;i++)
        {
            quantity[i] = inventory.Count[i];
        }
    }
}
