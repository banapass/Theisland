using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item",menuName = "New Item/Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equip,
        Ingredient,
        Food
    }
    public string itemName;
    public ItemType itemType;
    public GameObject itemPrefab;
    public Sprite itemImage;
}
