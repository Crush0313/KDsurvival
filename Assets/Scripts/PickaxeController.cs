using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : CloseWeaponController
{
    public static bool isActivate = false;//활성화 여부
    void Update()
    {
        if (isActivate)
            TryAttack();
    }
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing) //isSwing인 동안 계속 피격 체크
        {
            if (chkObj())
            {
                isSwing = false; //1회만 타격

            }
            yield return null;
        }
    }
    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
