using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    GameObject go_Base;

    [SerializeField]
    Text txt_ItemName;
    [SerializeField]
    Text txt_ItemDesc;
    [SerializeField]
    Text txt_ItemHowToUse;

public void showTooltip(Item _item,Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f, -go_Base.GetComponent<RectTransform>().rect.height, 0);
        go_Base.transform.position = _pos;

        txt_ItemName.text = _item.itemName;
        txt_ItemDesc.text = _item.itemDesc;

        if (_item.itemType == Item.ItemType.Equipment)
            txt_ItemHowToUse.text = "우클릭 - 장착";
        else if (_item.itemType == Item.ItemType.Used)
            txt_ItemHowToUse.text = "우클릭 - 먹기";
        else
            txt_ItemHowToUse.text = "";
    }
public void HideTooltip()
    {
        go_Base.SetActive(false);
    }
}
