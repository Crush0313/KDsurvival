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
        if (_item.itemType == Item.ItemType.Equipment) //���
        {
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.weaponType, _item.itemName));
        }
        else if (_item.itemType == Item.ItemType.Used) //�Ҹ�ǰ�ΰ�
        {
            for (int i = 0; i < itemEffects.Length; i++)
            {
                if (itemEffects[i].itemName == _item.itemName) //�´� �̸� ã��
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
                                Debug.Log("�߸��� status ����");
                                break;

                        }
                    }
                    return; //�̸� ã�� ȿ�� ������, �ݺ��� ����
                }
            }
            Debug.Log("��ġ ������ ����");
        }
    }
}
