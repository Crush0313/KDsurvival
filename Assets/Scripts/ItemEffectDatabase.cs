using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class ItemEffect
{
    public string itemName;
    [Tooltip("HP, SP, DP, HUNGRY, THIRSTY, SATISFY")]
    public string[] Part;
    public int[] num;

}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    StatusController theStatus;
    [SerializeField]
    WeaponManager theWeaponManager;
    [SerializeField]
    ItemEffect[] itemEffects;
    const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";


    private void Start()
    {

        theWeaponManager = FindAnyObjectByType<WeaponManager>();
    }
    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment) //장비
        {
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
        else if (_item.itemType == Item.ItemType.Used) //소모품인가
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].itemName == _item.itemName) //맞는 이름 찾기
                {
                    for (int j = 0; j < itemEffects[i].Part.Length; j++)
                    {
                        switch (itemEffects[i].Part[j])
                        {
                            case HP:
                                theStatus.IncreaseHP(itemEffects[i].num[j]);
                                break;
                            case SP:
                                theStatus.IncreaseSP(itemEffects[i].num[j]);
                                break;
                            case DP:
                                theStatus.IncreaseDP(itemEffects[i].num[j]);
                                break;
                            case HUNGRY:
                                theStatus.IncreaseHungry(itemEffects[i].num[j]);
                                break;
                            case THIRSTY:
                                theStatus.IncreaseThirsty(itemEffects[i].num[j]);
                                break;
                            case SATISFY:
                                break;
                            default:
                                Debug.Log("잘못된 status 부위");
                                break;

                        }
                    }
                    return; //이름 찾고 효과 썼으면, 반복문 끝냄
                }
            }
            Debug.Log("일치 아이템 없음");
        }
    }
}
