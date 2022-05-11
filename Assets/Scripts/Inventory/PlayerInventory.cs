using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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

    int Health=10;
    int MaxHealth=100;

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
        if(PlayerController.Player.CurrentInput==PlayerController.InputType.Mobile)
                PickIndicator=MobilePickIndicator;
        else
                PickIndicator = KeyboardPickIndicator;
    }
    void Update()
    {
        if(CrossPlatformInputManager.GetButtonDown("Inventory"))
        {
            Inventory.SetActive(!Inventory.activeInHierarchy);
        }
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
        PickIndicator.SetActive(false);
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
        AssignItemFunction(newItem.GetComponent<Item>());
    }
    void AssignItemFunction(Item item)
    {
        if (item.ID == 0)
            item.UseItem = Medkit;
        else if (item.ID == 1)
            item.UseItem = Bandage;
        else if (item.ID == 2)
            item.UseItem = EquipWeapon;
    }
    void Medkit()
    {
        Debug.Log("Health increased to Max");
        Health = MaxHealth;
    }
    void Bandage()
    {
        Debug.Log("Health Increase by 25");
        if (MaxHealth - Health > 25)
        {
            Health += 25;
        }
        else
            Health = MaxHealth;
    }
    void EquipWeapon()
    {
        PlayerWeapon.playerWeapon.DisableCurrentWeapon();
        Debug.Log("ChangeWeapon");
       // PlayerWeapon.playerWeapon.CurrentWeapon
    }

}
