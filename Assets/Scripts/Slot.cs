using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler
{
    public Item item;
    public int itemCount;
    public Image itemImage;
    private float sizeX;
    private float sizeY;
    [SerializeField] private TextMeshProUGUI textCount;
    [SerializeField] private GameObject countImage;
    [SerializeField] private GameObject player;
    [SerializeField] private InputNum inputNum;
    [SerializeField] private SlotToolTip toolTip;
    private RectTransform baseRect;

    float minX, maxX, minY, maxY;
    private bool isShow = true;

    private void Start()
    {
        baseRect = transform.parent.GetComponent<RectTransform>();
        Debug.Log(baseRect.rect.size);
       
        // ������ 
        sizeX = baseRect.rect.width * baseRect.localScale.x;
        sizeY = baseRect.rect.height * baseRect.localScale.y;

       // Debug.Log(sizeX+","+sizeY);
        //Debug.Log(baseRect.position.x + "," + baseRect.position.y);
        // ��ġ
        minX = baseRect.position.x - (sizeX / 2);
        maxX = baseRect.position.x + (sizeX / 2);
        minY = baseRect.position.y - (sizeY / 2);
        maxY = baseRect.position.y + (sizeY / 2);
    }
    private void Update()
    {
        
    }
    private void SetColor(float alpha_)
    {
        Color color = itemImage.color;
        color.a = alpha_;
        itemImage.color = color;
    }
    // RayCast�� �������� �Ű������� �޾ƿ� Ÿ���� ��� �ƴ� ��  ī��Ʈ ���� �� �̹��� Ȱ��ȭ 
    // ��� �� �� �̹��� ��Ȱ��ȭ �ؽ�Ʈ �ʱ�ȭ 
    public void AddItem(Item item_ ,int count_ = 1)
    {
        item = item_;
        itemCount = count_;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equip)
        {
            countImage.SetActive(true);
            textCount.text = itemCount.ToString();
        }
        else
        {
            textCount.text = "0";
            countImage.SetActive(false);
        }
        SetColor(1);
    }
    // ī��Ʈ ���� �� ������ ������ 0�Ͻ� ���� �ʱ�ȭ
    public void SetSlotCount(int count_)
    {
        itemCount += count_;
        textCount.text = itemCount.ToString();
        
        if (itemCount <= 0)
            ClearSlot();
            
    }
    // ������ ī��Ʈ 0�Ͻ� ���� �ʱ�ȭ �� �̹��� ��Ȱ��ȭ
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        textCount.text = "0";
        countImage.SetActive(false);
    }
    // ���콺 Ŭ����
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                if(item.itemType == Item.ItemType.Equip)
                {
                    if (item.itemName == "Water Bottle")
                    {
                        //item.itemName = "Empty Bottle";
                        //itemImage.sprite = Inventory.waterImages[0];
                        item = item.itemPrefab.GetComponent<Bottle>().emptyBottle;
                        itemImage.sprite = item.itemImage;
                        StatusManager.currentThirsty += 40;
                        if (StatusManager.currentThirsty > 100)
                            StatusManager.currentThirsty = 100;
                    }
                    else if (item.itemName == "Flare Gun")
                    {
                        ActionController.currentEquip = "Flare Gun";
                    }
                }
                else
                {
                    if (item.itemName == "Meat")
                    {
                        StatusManager.currentHungry += 35;
                        if (StatusManager.currentHungry > 100)
                            StatusManager.currentHungry = 100;
                        Debug.Log(item.itemName + "�Ҹ�");
                        SetSlotCount(-1);
                    }
                    
                }
            }
        }
    }
    // �巡�� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instance.dragSlot = this; // �巡�� ���Կ� ó�� �巡���� ��ġ�� �������� �־���
            DragSlot.instance.DragSetImage(itemImage); // �巡�� ���Կ� �ش� ���Կ� �̹����� ����
           // DragSlot.instance.transform.position = eventData.position;
        }
    }
    // �巡�� ��
    public void OnDrag(PointerEventData eventData)
    {

        if (item != null)
        {
            
            DragSlot.instance.transform.position = eventData.position; // �巡�� �� ���콺���� �̵� 
            //Debug.Log("test : " + DragSlot.instance.transform.position);// �巡�� ��ġ Ȯ�ο�
        }
    }
    // �巡�� ��
    public void OnEndDrag(PointerEventData eventData)
    {
        // ������ ��� ����
        if (DragSlot.instance.transform.position.x < minX
         || DragSlot.instance.transform.position.x > maxX
         || DragSlot.instance.transform.position.y < minY
         || DragSlot.instance.transform.position.y > maxY)
        {
            // ������ ������
            if (DragSlot.instance.dragSlot != null)
                inputNum.Call();
            //Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, player.transform.position + player.transform.forward, Quaternion.identity);
            //DragSlot.instance.dragSlot.SetSlotCount(-1);
        }
        else
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
       
        
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null)
            ChangeSlot();
    }
    // ���� ��ȯ
    private void ChangeSlot()
    {
        // �� ��ġ�� �ִ� �����۰� ������ temp�� �ӽ� ����
        Item tempItem = item;
        int tempCount = itemCount;

        // ����� ���� ��ġ�� �巡���� ������,������ �־���
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (tempItem != null)
        {

            // ����� ��ġ�� �ٸ� �������� ���� �� �� �巡�� ���Կ� �̸� �����ص� �������� �־���
            // dragSlot�� AddItem�� �� �� ó�� ���Կ� ���� ������ OnBegin���� this�� �־
            DragSlot.instance.dragSlot.AddItem(tempItem, tempCount);
        }
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
        {
            isShow = true;
            toolTip.ShowToolTip(item, transform.position);
            Debug.Log(isShow);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isShow = false;
        Debug.Log(isShow);
        toolTip.HideToolTip();
    }
}
