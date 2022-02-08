using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject slotsParent;
    [SerializeField] private SlotToolTip toolTip;
    public static Sprite[] waterImages;
    public static Slot[] slots;
    [SerializeField] private GameObject flareGun;

    public static bool inventoryActivated;
    // Start is called before the first frame update
    void Start()
    {
        //waterImages = Resources.LoadAll<Sprite>("Image/");
        //waterImages[0] = Resources.Load<Sprite>("Image/Empty Bottle");
        //waterImages[1] = Resources.Load<Sprite>("Image/Water Bottle");
        slots = slotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab)) 
        {
            inventoryActivated = !inventoryActivated;
            if (inventoryActivated)
            {
                inventory.SetActive(true);
                MouseController.instance.MouseOn();
            }
            else
            {
                inventory.SetActive(false);
                MouseController.instance.MouseLock();
                toolTip.HideToolTip();
            }
            
        }
        //if (inventoryActivated == false)
        //{
        //    inventory.SetActive(false);
        //    MouseController.instance.MouseLock();
        //}
    }
    public void AcquireItem(Item item_, int count_ = 1)
    {
        //Debug.Log(item_.itemName);
        // 매개변수로 받아온 아이템의 타입이 장비가 아니고
        // 아이템의 이름이 같을시 카운트를 증가 시킴
        if (item_.itemType != Item.ItemType.Equip)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == item_.itemName)
                    {
                        slots[i].SetSlotCount(count_);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item_, count_);
                return;
            }
        }
        

    }
    
    
}
