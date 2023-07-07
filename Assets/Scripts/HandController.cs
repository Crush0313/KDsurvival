using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField]
    Hand currentHand; //현재 장착된 hand타입 무기
    bool isAttack = false;
    bool isSwing = false;
    RaycastHit hitInfo;


    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }
    void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }
    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false; //재공격 가능
    }
    IEnumerator HitCoroutine()
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
    bool chkObj()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        else
            return false;
    }
}
