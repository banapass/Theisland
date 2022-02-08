using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float checkDistance; // ������ üũ ��Ÿ�

    [SerializeField]
    private TextMeshProUGUI itemInfoText; // ������ �ؽ�Ʈ
    
    [SerializeField]
    LayerMask layerMask;
    RaycastHit hit;
    
    
    public Inventory inventory;
    private Slot[] slots;

    private bool isHit;
    public static string currentEquip;

    private void Start()
    {
        currentEquip = null;
    }
    // Update is called once per frame
    void Update()
    {
        CheckObj();
        TryAction();
        ChangeWeapon();

    }
    // ������Ʈ Ȯ�ο� ����
    // ������ ,�Ա� ,�� ,���� Ȯ��
    private void CheckObj()
    {
        Debug.DrawRay(transform.position, transform.forward * checkDistance, Color.black);
        if (Physics.Raycast(transform.position, transform.forward, out hit, checkDistance, layerMask))
        {
            if (hit.transform.tag == "Item")
            {
                ItemInfoEnable();
            }
            
            else
                ItemInfoDisable();

        }
        else
            ItemInfoDisable();

    }
    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
        if(Input.GetMouseButtonDown(1))
        {
            CheckWater();
        }
      
    }
  
    private void ItemInfoEnable()
    {
        isHit = true;
        itemInfoText.gameObject.SetActive(true);
        itemInfoText.text = hit.transform.GetComponent<ItemPickUp>().item.itemName + "  (E)";
    }
    private void ItemInfoDisable()
    {
        isHit = false;
        itemInfoText.gameObject.SetActive(false);
    }
    private void PickUp()
    {
        if(isHit)
        {
            if (hit.transform != null)
            {
                if (hit.transform.tag == "Item")
                {
                    Debug.Log(hit.transform.GetComponent<ItemPickUp>().item.itemName);
                    inventory.AcquireItem(hit.transform.GetComponent<ItemPickUp>().item);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    private void CheckWater()
    {

        if (hit.transform != null)
        {
            if (hit.transform.tag == "Water")
            {
                for (int i = 0; i < Inventory.slots.Length; i++)
                {
                    if (Inventory.slots[i].item != null)
                    {
                        if (Inventory.slots[i].item.itemName == "Empty Bottle")
                        {
                            //Debug.Log("Water");
                            //Inventory.slots[i].item.itemName = "Water Bottle";
                            //Inventory.slots[i].itemImage.sprite = Inventory.waterImages[1];
                            Inventory.slots[i].item = Inventory.slots[i].item.itemPrefab.GetComponent<Bottle>().waterBottle;
                            Inventory.slots[i].itemImage.sprite = Inventory.slots[i].item.itemImage;
                        }
                    }
                }
            }
        }
    }
    private void ChangeWeapon()
    {
        // ���
        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.inventoryActivated == false)
        {
            GetComponent<PickAxeController>().enabled = true;
            GetComponent<AxeController>().enabled = false;
            GetComponent<SwordController>().enabled = false;
            currentEquip = "PickAxe";
        }

        // ����
        if (Input.GetKeyDown(KeyCode.Alpha2) && Inventory.inventoryActivated == false)
        {
            GetComponent<PickAxeController>().enabled = false;
            GetComponent<AxeController>().enabled = true;
            GetComponent<SwordController>().enabled = false;
            currentEquip = "Axe";
        }

        // ��
        if (Input.GetKeyDown(KeyCode.Alpha3) && Inventory.inventoryActivated == false)
        {
            GetComponent<PickAxeController>().enabled = false;
            GetComponent<AxeController>().enabled = false;
            GetComponent<SwordController>().enabled = true;
            currentEquip = "Sword";
        }
        
    }




}
