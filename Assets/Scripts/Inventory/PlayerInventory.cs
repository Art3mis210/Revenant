using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject BuildingMaterials;
    public GameObject Weapons;
    public GameObject Essentials;
    public GameObject ItemPrefab;

    public List<Item> BuildingItems;
    public List<Item> WeaponItems;
    public List<Item> EssentialItems;


    private GameObject PickIndicator;
    private Animator PlayerAnimator;
    public GameObject KeyboardPickIndicator;
    public GameObject MobilePickIndicator;

    public enum ItemType
    {
        BuildingMaterials = 0,
        Weapons = 1,
        Essentials = 2
    }
    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        BuildingItems = new List<Item>();
        WeaponItems = new List<Item>();
        EssentialItems = new List<Item>();
        InventoryReference = this;
        #if MOBILE_INPUT
                PickIndicator=MobilePickIndicator;
        #else
                PickIndicator = KeyboardPickIndicator;
        #endif
    }
    private void Update()
    {
        Inventory.SetActive(Input.GetKey(KeyCode.Tab));
    }
    public static PlayerInventory InventoryReference
    { 
        get; 
        set; 
    }
    public void InventoryItemInRange(bool Status)
    {
        if(Status==true)
        {
            PickIndicator.SetActive(true);
        }
        else
        {
            PickIndicator.SetActive(false);
        }
    }
    public void PickUpItem(PickableInventoryItem ItemtoPick)
    {
        PlayerAnimator.SetTrigger("Pick");
        if (ItemtoPick.itemData.itemType == ItemType.BuildingMaterials)
            AddBuildingMaterials(ItemtoPick);
        else if (ItemtoPick.itemData.itemType == ItemType.Weapons)
            AddWeapons(ItemtoPick);
        else if (ItemtoPick.itemData.itemType == ItemType.Essentials)
            AddEssentials(ItemtoPick);
    }
    void AddBuildingMaterials(PickableInventoryItem ItemtoPick)
    {
        ItemtoPick.transform.gameObject.SetActive(false);
        foreach (Item item in BuildingItems)
        {
            if(item.ID==ItemtoPick.itemData.ID)
            {
                item.IncreaseQuantity();
                return;
            }
        }

        GameObject newItem = Instantiate(ItemPrefab, BuildingMaterials.transform);
        newItem.GetComponent<Item>().CreateItem(ItemtoPick.itemData);
        BuildingItems.Add(newItem.GetComponent<Item>());
        PickIndicator.SetActive(false);
    }
    void AddWeapons(PickableInventoryItem ItemtoPick)
    {
        ItemtoPick.transform.gameObject.SetActive(false);
        foreach (Item item in WeaponItems)
        {
            if (item.ID == ItemtoPick.itemData.ID)
            {
                item.IncreaseQuantity();
                return;
            }
        }

        GameObject newItem = Instantiate(ItemPrefab, Weapons.transform);
        newItem.GetComponent<Item>().CreateItem(ItemtoPick.itemData);
        BuildingItems.Add(newItem.GetComponent<Item>());
        PickIndicator.SetActive(false);
    }
    void AddEssentials(PickableInventoryItem ItemtoPick)
    {
        ItemtoPick.transform.gameObject.SetActive(false);
        foreach (Item item in EssentialItems)
        {
            if (item.ID == ItemtoPick.itemData.ID)
            {
                item.IncreaseQuantity();
                return;
            }
        }
        GameObject newItem = Instantiate(ItemPrefab, Essentials.transform);
        newItem.GetComponent<Item>().CreateItem(ItemtoPick.itemData);
        EssentialItems.Add(newItem.GetComponent<Item>());
        PickIndicator.SetActive(false);
    }
}
