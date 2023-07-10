using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName;

    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;


    public float range; //공격 범위
    public int damage; //공격력
    public float workSpeed; //작업 속도
    public float attackDelay; //공격딜레이
    public float attackDelayA; //공격 활성화 시점(대미지 적용)
    public float attackDelayB; //공격 활성화 시점

    public Animator anim;
}
