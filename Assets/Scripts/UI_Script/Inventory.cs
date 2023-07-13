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

    //�κ�â ���ݱ�
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
        //���� ��ø�Ұ��̴� �ٷ� �Ʒ������� ������, ����� �ϴ� ��� �ƴϾ�� ��
        if (_item.itemType != Item.ItemType.Equipment) 
        {
            //�̹� �ִ�(������ ������) �������̸�
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        Debug.Log("����");
                        return;
                    }
                }
            }
        }

            //���Կ� ���� �������̸�,Ȥ�� ���Ÿ���̶��
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null) //�� ���� ã��
                {
                    slots[i].AddItem(_item, _count);
                    Debug.Log(_item.itemName);
                    Debug.Log("��");
                    return;
                }
            }
        
    }
}
