using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNum : MonoBehaviour
{
    private bool Activated;
    [SerializeField] private InputField inputField;
    [SerializeField] private Text previewText;
    [SerializeField] private Text inputText;
    [SerializeField] private GameObject inputObj;
    [SerializeField] private ActionController player;
    [SerializeField] private GameObject flareGun;
    int num;
    

    // Update is called once per frame
    void Update()
    {
        if (Activated)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                Ok();
            
            if (Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.Tab)||Input.GetKeyDown(KeyCode.I))
                Cancel();
            
        }
    }
    public void Call()
    {
        inputObj.SetActive(true);
        Activated = true;
        inputField.text = ""; // 텍스트 초기화
        DragSlot.instance.SetColor(0);
        previewText.text = DragSlot.instance.dragSlot.itemCount.ToString(); // 현재 가지고있는 아이템 개수

    }
    public void Ok()
    {
        
        if (inputText.text != "")
        {
            // 숫자체크
            
            if (CheckNumber(inputText.text))
            {
                
                num = int.Parse(inputText.text);
                if (num > DragSlot.instance.dragSlot.itemCount)
                    num = DragSlot.instance.dragSlot.itemCount;
            }
        }
        else
            num = int.Parse(previewText.text);

        StartCoroutine(DropCoroutine(num));
    }
    public void Cancel()
    {
        Activated = false;
        DragSlot.instance.dragSlot = null;
        inputObj.SetActive(false);
        
    }
    IEnumerator DropCoroutine(int num_)
    {
        for (int i = 0; i < num_; i++)
        {
            if (DragSlot.instance.dragSlot.item.itemName == "Flare Gun")
            {
                ActionController.currentEquip = null;
            }
            Instantiate(DragSlot.instance.dragSlot.item.itemPrefab, player.transform.position + player.transform.forward, Quaternion.identity);
            Inventory.inventoryActivated = false;
            DragSlot.instance.dragSlot.SetSlotCount(-1);
            yield return new WaitForSeconds(0.08f);
        }
        
        DragSlot.instance.dragSlot = null;
        inputObj.SetActive(false);
        Activated = false;
        
    }
    
    private bool CheckNumber(string check)
    {
        char[] tempCharArray = check.ToCharArray();
        bool isNumber = true;

        for(int i = 0; i < tempCharArray.Length;i++)
        {
            if (tempCharArray[i] >= 48 && tempCharArray[i] <= 57)
                continue;
            else
                isNumber = false;
        }
        return isNumber;
    }
}
