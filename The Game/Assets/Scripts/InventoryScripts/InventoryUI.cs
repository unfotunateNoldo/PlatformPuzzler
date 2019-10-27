using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public GameObject inventoryUI;

    public List<Transform> slots = new List<Transform>();

    void Start()
    {
        Inventory.instance.itemChangedCallback += UpdateUI;

        foreach (Transform t in inventoryUI.transform.GetChild(0)) {
            slots.Add(t);
        }
        Debug.Log("This many slots: " + slots.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    void UpdateUI()
    {
        Debug.Log("UPDATING UI");
        string[] keys = Inventory.instance.GetKeys();
        for (int i = 0; i < slots.Count; i++) {
            if (i < keys.Length) {
                List<Sprite> items = Inventory.instance.GetItems(keys[i]);
                // items.Count
                // set the current slot to the current item
                slots[i].GetChild(0).gameObject.SetActive(true);
                slots[i].GetChild(0).GetComponent<Image>().sprite = items[0];
                // show the number, if > 1
                if (items.Count > 1) {
                    slots[i].GetChild(1).gameObject.SetActive(true);
                    slots[i].GetChild(1).GetComponent<Text>().text = items.Count + "";
                } else {
                    slots[i].GetChild(1).gameObject.SetActive(false);
                }
            } else {
                // for the rest of the slots, clear them
                slots[i].GetChild(0).gameObject.SetActive(false);
                slots[i].GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
