using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

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
    public int AddedItems;
    public int MaxItems = 20;
    public InventoryItems[] AllInventoryItems;
    public int[] Count;
    bool Loading;
    int Health=10;
    int MaxHealth=100;
    public bool Load;
    public enum ItemType
    {
        BuildingMaterials = 0,
        Weapons = 1,
        Essentials = 2
    }
    private void Start()
    {
        if(SceneManager.GetActiveScene().name=="Prologue" || Load==false )
        {
            AddedItems = 0;
            Count = new int[AllInventoryItems.Length];
        }
        else
        {
            AddedItems = 0;
            int[] LoadInventory;
            Count = new int[AllInventoryItems.Length];
            LoadInventory = PlayerDataSaver.LoadGame().quantity;
            Loading = true;
            for(int i=0;i< LoadInventory.Length; i++)
            {
                if(LoadInventory[i]>0)
                {
                    for(int j=0;j<LoadInventory[i];j++)
                    {
                        PickUpItem(null, AllInventoryItems[i]);
                    }
                }
            }
            Loading = false;
        }
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
    public void PickUpItem(PickableInventoryItem ItemtoPick,InventoryItems itemData)
    {
        if (AddedItems < MaxItems)
        {
            AddedItems++;
            for(int i=0;i<AllInventoryItems.Length;i++)
            {
                if(AllInventoryItems[i].ID==itemData.ID)
                {
                    Count[i]++;
                }
            }
            if (Loading == false)
            {
                PickIndicator.SetActive(false);
                PlayerAnimator.SetTrigger("Pick");
            }
            if (itemData.itemType == ItemType.BuildingMaterials)
                AddBuildingMaterials(ItemtoPick,itemData);
            else if (itemData.itemType == ItemType.Weapons)
                AddWeapons(ItemtoPick,itemData);
            else if (itemData.itemType == ItemType.Essentials)
                AddEssentials(ItemtoPick,itemData);
        }
        PlayerDataSaver.SaveGame(this);
    }
    void AddBuildingMaterials(PickableInventoryItem ItemtoPick, InventoryItems itemData)
    {
        if (ItemtoPick != null)
            ItemtoPick.transform.gameObject.SetActive(false);
        foreach (Item item in BuildingItems)
        {
            if(item.ID==itemData.ID)
            {
                item.IncreaseQuantity();
                return;
            }
        }

        GameObject newItem = Instantiate(ItemPrefab, BuildingMaterials.transform);
        newItem.GetComponent<Item>().CreateItem(ItemtoPick.itemData);
        BuildingItems.Add(newItem.GetComponent<Item>());
        
    }
    void AddWeapons(PickableInventoryItem ItemtoPick, InventoryItems itemData)
    {
        if (ItemtoPick != null)
            ItemtoPick.transform.gameObject.SetActive(false);
        foreach (Item item in WeaponItems)
        {
            if (item.ID ==itemData.ID)
            {
                item.IncreaseQuantity();
                return;
            }
        }

        GameObject newItem = Instantiate(ItemPrefab, Weapons.transform);
        newItem.GetComponent<Item>().CreateItem(itemData);
        BuildingItems.Add(newItem.GetComponent<Item>());
    }
    void AddEssentials(PickableInventoryItem ItemtoPick, InventoryItems itemData)
    {
        if (ItemtoPick != null)
            ItemtoPick.transform.gameObject.SetActive(false);
        foreach (Item item in EssentialItems)
        {
            if (item.ID == itemData.ID)
            {
                item.IncreaseQuantity();
                return;
            }
        }
        GameObject newItem = Instantiate(ItemPrefab, Essentials.transform);
        newItem.GetComponent<Item>().CreateItem(itemData);
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
        else
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
