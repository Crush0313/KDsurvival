using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;
    public Slot dragSlot;

    private void Start()
    {
        instance = this;
    }

    [SerializeField]
    Image imageItem;

    public void DragSetImg(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    } 
    public void SetColor(float Alpha)
    {
        Color color = imageItem.color;
        color.a=Alpha;
        imageItem.color = color;

    }


}
