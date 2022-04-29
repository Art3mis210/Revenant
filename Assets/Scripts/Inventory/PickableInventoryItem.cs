using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PickableInventoryItem : MonoBehaviour
{
    public bool InInventory;
    public GameObject MeshObject;
    public InventoryItems itemData;
    
    private void Start()
    {
        MeshObject = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(!InInventory)
        {
            if(MeshObject.activeInHierarchy)
                MeshObject.transform.Rotate(0, 0, 60 * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            if(!InInventory)
                PlayerInventory.InventoryReference.InventoryItemInRange(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Pick"))
            {
                PlayerInventory.InventoryReference.PickUpItem(this);
                InInventory = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerInventory.InventoryReference.InventoryItemInRange(false);
        }
    }
}
