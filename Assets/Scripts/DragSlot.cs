using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;
    public Slot dragSlot;

    [SerializeField] private Image itemImage;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    
    public void DragSetImage(Image image)
    {
        itemImage.sprite = image.sprite;
        SetColor(1);
    }
    public void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }
}
