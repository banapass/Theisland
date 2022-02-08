using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChange : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject flareGun;
    
    // Update is called once per frame
    private void Start()
    {
        SetColor(0);
    }
    void Update()
    {
        ImageChange();
    }
    private void SetColor(int alpha)
    {
        Color color = GetComponent<Image>().color;
        color.a = alpha;
        GetComponent<Image>().color = color;
    }
   
    private void ImageChange()
    {
        if (ActionController.currentEquip == "PickAxe")
            GetComponent<Image>().sprite = sprites[0];

        else if (ActionController.currentEquip == "Axe")
            GetComponent<Image>().sprite = sprites[1];

        else if (ActionController.currentEquip == "Sword")
            GetComponent<Image>().sprite = sprites[2];

        else if (ActionController.currentEquip == "Flare Gun")
        {
            GetComponent<Image>().sprite = sprites[3];
            flareGun.SetActive(true);
        }
        else if (ActionController.currentEquip == null)
            SetColor(0);
        if (ActionController.currentEquip != "Flare Gun")
        {
            flareGun.SetActive(false);
        }
        if (ActionController.currentEquip != null)
            SetColor(1);
    }
        
}
