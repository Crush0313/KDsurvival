using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static bool isActivate= false;//Ȱ��ȭ ����

    [SerializeField]
    Hand currentHand; //���� ������ handŸ�� ����
    bool isAttack = false;
    bool isSwing = false;
    RaycastHit hitInfo;


    // Update is called once per frame
    void Update()
    {
        if (isActivate)
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
        isAttack = false; //����� ����
    }
    IEnumerator HitCoroutine()
    {
        while (isSwing) //isSwing�� ���� ��� �ǰ� üũ
        {
            if (chkObj())
            {
                isSwing = false; //1ȸ�� Ÿ��

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

    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentHand = _hand;
        WeaponManager.currentWeapon = currentHand.transform;
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero;
        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }
}
