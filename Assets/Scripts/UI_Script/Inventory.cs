using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated=false;

    [SerializeField]
    GameObject go_InventoryBase;
    [SerializeField]
    GameObject go_SlotParent;

    Slot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotParent.GetComponentsInChildren<Slot>();    
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInv();

    }

    //인벤창 여닫기
    void TryOpenInv()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                OpenInv();
            }
            else
                CloseInv();
        }
    }
    void OpenInv()
    {
        go_InventoryBase.SetActive(true);
    }
    void CloseInv()
    {
        go_InventoryBase.SetActive(false);

    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        //장비는 중첩불가이니 바로 아래쪽으로 빠지고, 여기는 일단 장비가 아니어야 함
        if (_item.itemType != Item.ItemType.Equipment) 
        {
            //이미 있는(슬롯을 차지한) 아이템이면
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        Debug.Log("ㄹㄹ");
                        return;
                    }
                }
            }
        }

            //슬롯에 없는 아이템이면,혹은 장비타입이라면
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null) //빈 슬롯 찾음
                {
                    slots[i].AddItem(_item, _count);
                    Debug.Log(_item.itemName);
                    Debug.Log("ㄹ");
                    return;
                }
            }
        
    }
}
