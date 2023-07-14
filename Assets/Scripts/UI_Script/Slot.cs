using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler      
{
    public Item item;
    public int itemCount;
    public Image itemImg;


    [SerializeField]
    Text text_Count;
    [SerializeField]
    GameObject go_CountImage;

    ItemEffectDatabase theItemEffectDatabase;

    private void Start()
    {
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
    }
    void SetColor(float _alpha)
    {
        Color color = itemImg.color;
        color.a = _alpha;
        itemImg.color = color;
    }

    public void AddItem(Item _item, int _count = 1) 
    {
        item = _item;
        itemCount = _count;

        itemImg.sprite = item.itemImage;
        SetColor(1);

        //개수 이미지,텍스트 조정
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


    public void OnPointerClick(PointerEventData eventData) //클릭
    {
        if(eventData.button == PointerEventData.InputButton.Right) //우클릭
        {
            if (item != null)
            {

                
               
                    theItemEffectDatabase.UseItem(item);
                if(item.itemType == Item.ItemType.Used)
                    SetSlotCount(-1); //소모품이면 사용 후 차감
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImg(itemImg);
        }
    }

    public void OnDrag(PointerEventData eventData) //슬롯에서 시작된 드래그가 끝
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnDrop(PointerEventData eventData) //슬롯에서 드래그가 끝남, 이때 드래그핸들러가 달린 곳에서 드래그가 시작되었어야 함
    {
        if (DragSlot.instance.dragSlot != null) //빈 슬롯으로부터는(드래그슬롯 비어있을 때) 교체 안함
        {
            ChangeSlot();
        }
    }

    void ChangeSlot()
    {
        //B' = B
        Item _tempItem = item;
        int _tempCount = itemCount;

        //B = A'
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        //A = B'
        if (_tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempCount);
        }
        else
        {
            DragSlot.instance.dragSlot.clearSlot();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null; //드래그 슬롯 비우기
    }
}
