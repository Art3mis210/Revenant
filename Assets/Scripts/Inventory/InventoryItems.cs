using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItems : ScriptableObject
{
    public string ItemName;
    public PlayerInventory.ItemType itemType;
    public int ID;
    public Sprite icon;
    public GameObject Prefab;
}

