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
    public void CreateItem(InventoryItems itemData)
    {
        this.ID = itemData.ID;
        Quantity = 1;
        if (ItemImage == null)
            ItemImage = transform.GetComponent<Image>();
        ItemImage.sprite = itemData.icon;
        QuantityText.text = "X" + Quantity.ToString();
        Name.text = itemData.ItemName;
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
            Destroy(this);
        }
        else
            DecreaseQuantity();
    }
}
