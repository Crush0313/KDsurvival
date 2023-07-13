using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    float range; //���� ���� �Ÿ�
    bool pickupActivated = false; //���� ���ɿ���
    RaycastHit hitinfo; //�浹ü ���� ����

    [SerializeField]
    LayerMask layerMask; //������ ���̾�� �����ϵ���

    [SerializeField]
    Text actionText;
    [SerializeField]
    Inventory theInventory;

    void Update()
    {
        chkItem();
        TryAction();
    }
    void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            chkItem();
            canPickup();
        }
    }
    void canPickup()
    {
        if (pickupActivated)
        {
            if (hitinfo.transform != null)
            {
                theInventory.AcquireItem(hitinfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitinfo.transform.gameObject);
                infoDisappear();
            }
        }
    }
    
    void chkItem()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo,range,layerMask)) {
            if (hitinfo.transform.tag == "Item")
            {
                InfoAppear();
            }
        }
        else
            infoDisappear();
    }

    void InfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitinfo.transform.GetComponent<ItemPickUp>().item.itemName + "ȹ��" + "<color=yellow>" + "EŰ" + "</color>";
    }
    void infoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
