using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    float range; //습득 가능 거리
    bool pickupActivated = false; //습득 가능여부
    RaycastHit hitinfo; //충돌체 정보 저장

    [SerializeField]
    LayerMask layerMask; //아이템 레이어에만 반응하도록

    [SerializeField]
    Text actionText;

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
            Debug.Log("신");
            canPickup();
        }
    }
    void canPickup()
    {
        if (pickupActivated)
        {
            if (hitinfo.transform != null)
            {
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
                //Destroy(hitinfo.transform.gameObject);
            }
        }
        else
            infoDisappear();
    }

    void InfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitinfo.transform.GetComponent<ItemPickUp>().item.itemName + "획득" + "<color=yellow>" + "E키" + "</color>";
    }
    void infoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
