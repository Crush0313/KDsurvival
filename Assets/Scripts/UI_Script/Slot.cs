using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public int itemCount;
    public Image itemImg;

    [SerializeField]
    Text text_Count;
    [SerializeField]
    GameObject go_CountImage;

    void SetColor(float _alpha)
    {
        Color color = itemImg.color;
        color.a = _alpha;
        itemImg.color = color;
    }

    public void AddItem(Item _item, int _count = 1) 
    {
        Debug.Log("일단 애드");
        item = _item;
        itemCount = _count;

        itemImg.sprite = item.itemImage;

        if (item.itemType == Item.ItemType.Equipment)
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
        else
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        SetColor(1);
    }

    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            clearSlot();
    }
    void clearSlot()
    {
        item = null;

        itemImg.sprite = null;
        SetColor(0);

        itemCount = 0;
        text_Count.text = "0";

        go_CountImage.SetActive(false);
    }
}
