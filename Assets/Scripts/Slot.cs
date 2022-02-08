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
       
        // 사이즈 
        sizeX = baseRect.rect.width * baseRect.localScale.x;
        sizeY = baseRect.rect.height * baseRect.localScale.y;

       // Debug.Log(sizeX+","+sizeY);
        //Debug.Log(baseRect.position.x + "," + baseRect.position.y);
        // 위치
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
    // RayCast로 아이템을 매개변수로 받아옴 타입이 장비가 아닐 시  카운트 증가 및 이미지 활성화 
    // 장비 일 시 이미지 비활성화 텍스트 초기화 
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
    // 카운트 증가 및 아이템 갯수가 0일시 슬롯 초기화
    public void SetSlotCount(int count_)
    {
        itemCount += count_;
        textCount.text = itemCount.ToString();
        
        if (itemCount <= 0)
            ClearSlot();
            
    }
    // 아이템 카운트 0일시 슬롯 초기화 및 이미지 비활성화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        textCount.text = "0";
        countImage.SetActive(false);
    }
    // 마우스 클릭시
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
                        Debug.Log(item.itemName + "소모");
                        SetSlotCount(-1);
                    }
                    
                }
            }
        }
    }
    // 드래그 시작
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != null)
        {
            DragSlot.instance.dragSlot = this; // 드래그 슬롯에 처음 드래그한 위치에 아이템을 넣어줌
            DragSlot.instance.DragSetImage(itemImage); // 드래그 슬롯에 해당 슬롯에 이미지를 복사
           // DragSlot.instance.transform.position = eventData.position;
        }
    }
    // 드래그 중
    public void OnDrag(PointerEventData eventData)
    {

        if (item != null)
        {
            
            DragSlot.instance.transform.position = eventData.position; // 드래그 시 마우스따라 이동 
            //Debug.Log("test : " + DragSlot.instance.transform.position);// 드래그 위치 확인용
        }
    }
    // 드래그 끝
    public void OnEndDrag(PointerEventData eventData)
    {
        // 아이템 드롭 범위
        if (DragSlot.instance.transform.position.x < minX
         || DragSlot.instance.transform.position.x > maxX
         || DragSlot.instance.transform.position.y < minY
         || DragSlot.instance.transform.position.y > maxY)
        {
            // 아이템 버리기
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
    // 슬롯 교환
    private void ChangeSlot()
    {
        // 이 위치에 있는 아이템과 개수를 temp에 임시 저장
        Item tempItem = item;
        int tempCount = itemCount;

        // 드롭한 슬롯 위치에 드래그한 아이템,개수를 넣어줌
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (tempItem != null)
        {

            // 드롭한 위치에 다른 아이템이 존재 할 시 드래그 슬롯에 미리 저장해둔 아이템을 넣어줌
            // dragSlot에 AddItem을 할 시 처음 슬롯에 들어가는 이유는 OnBegin에서 this를 넣어서
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
