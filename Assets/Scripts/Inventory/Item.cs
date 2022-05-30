using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int ID;
    public int Quantity;
    public Text Name;
    public Text QuantityText;
    public Image ItemImage;
    public GameObject UseDropButton;
    public InventoryItems ItemReference;
    public delegate void Itemfunction();
    public Itemfunction UseItem;
    public GameObject ReferenceInScene;
    public void CreateItem(InventoryItems itemData)
    {
        this.ID = itemData.ID;
        Quantity = 1;
        if (ItemImage == null)
            ItemImage = transform.GetComponent<Image>();
        ItemImage.sprite = itemData.icon;
        QuantityText.text = "X" + Quantity.ToString();
        Name.text = itemData.ItemName;
        ItemReference = itemData;
        if (itemData.itemType == PlayerInventory.ItemType.Weapons)
        {
            ReferenceInScene = Instantiate(itemData.Prefab, PlayerWeapon.playerWeapon.WeaponParent);
            ReferenceInScene.SetActive(false);
        }
    }
    public void IncreaseQuantity()
    {
        Quantity++;
        QuantityText.text = "X" + Quantity.ToString();
    }
    void DecreaseQuantity()
    {
        Quantity--;
        QuantityText.text = "X" + Quantity.ToString();
    }
    public void OnOpenItem()
    {
        UseDropButton.SetActive(true);
        ItemImage.enabled = false;
    }
    public void OnCloseItem()
    {
        UseDropButton.SetActive(false);
        ItemImage.enabled = true;
    }
    public void OnDropItem()
    {
        if (Quantity == 1)
        {
            if (ItemReference.itemType == PlayerInventory.ItemType.Weapons)
            {
                if (PlayerWeapon.playerWeapon.CurrentWeapon == ReferenceInScene.GetComponent<Weapon>())
                {
                    PlayerWeapon.playerWeapon.DisableCurrentWeapon();
                }
                Destroy(ReferenceInScene);
            }
            Destroy(transform.gameObject);
        }
        else
        {
            DecreaseQuantity();
        }
        PlayerInventory.InventoryReference.Count[ID]--;
    }
    public void OnUseItem()
    {
        if (ItemReference.itemType == PlayerInventory.ItemType.Weapons)
        {
            if (!ReferenceInScene.activeInHierarchy)
            {
                PlayerWeapon.playerWeapon.DisableCurrentWeapon();
                PlayerWeapon.playerWeapon.CurrentWeapon = ReferenceInScene.GetComponent<Weapon>();
                ReferenceInScene.SetActive(true);
            }
            else
            {
                PlayerWeapon.playerWeapon.DisableCurrentWeapon();
            }
        }
        else
        {
            UseItem();
            OnDropItem();
        }
    }
}
