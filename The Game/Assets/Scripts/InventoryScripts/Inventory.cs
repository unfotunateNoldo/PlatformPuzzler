using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // singleton
    public static Inventory instance;

    public int capacity = 12;
    
    public Dictionary<string, List<Sprite>> items = new Dictionary<string, List<Sprite>>();

    public delegate void OnItemChanged();
    public OnItemChanged itemChangedCallback;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than once instance of inventory!");
            return;
        }
        instance = this;
    }

    public bool AddItem(string key, GameObject item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Inventory full");
            return false;
        }
        if (items.ContainsKey(key))
        {
            items[key].Add(item.GetComponent<SpriteRenderer>().sprite);
        } else {
            List<Sprite> l = new List<Sprite>();
            l.Add(item.GetComponent<SpriteRenderer>().sprite);
            items.Add(key, l);
        }
        if (itemChangedCallback != null) {
            Debug.Log("INvoking callback!");
            itemChangedCallback.Invoke();
        } else {
            Debug.Log("CALLBACK IS NULl!");
        }
        Debug.Log("ADDING ITEM AT " + Time.time);
        return true;
    }
    public void RemoveItem(string key)
    {
        items[key].RemoveAt(0);
        if (items[key].Count == 0)
            items.Remove(key);

        if (itemChangedCallback != null)
            itemChangedCallback.Invoke();
    }
    public List<Sprite> GetItems(string key)
    {
        if (items.ContainsKey(key)) {
            return items[key];
        }
        return null;
    }

    public string[] GetKeys()
    {
        return items.Keys.ToArray();
    }
}
