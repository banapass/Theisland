using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField] private GameObject toolTipPenal;
    [SerializeField] private TextMeshProUGUI itemName_Text;
    [SerializeField] private TextMeshProUGUI howToUse_Text;
    

    public void ShowToolTip(Item item_, Vector3 pos_)
    {
        toolTipPenal.SetActive(true);
        pos_ += new Vector3(toolTipPenal.GetComponent<RectTransform>().rect.width * 0.5f,
                            -toolTipPenal.GetComponent<RectTransform>().rect.height * 0.5f, 0);

        toolTipPenal.transform.position = pos_;

        itemName_Text.text = item_.itemName;

        // 타입별 텍스트
        if (item_.itemType == Item.ItemType.Equip && item_.itemName == "Water Bottle")
            howToUse_Text.text = "Right Click - Drink";
        else if (item_.itemType == Item.ItemType.Food)
            howToUse_Text.text = "Right Click - Eat";
        else if (item_.itemType == Item.ItemType.Equip && item_.itemName == "Flare Gun")
            howToUse_Text.text = "Right Click - Equip";
        else
            howToUse_Text.text = "";
    }
    public void HideToolTip()
    {
        toolTipPenal.SetActive(false);
    }
}
