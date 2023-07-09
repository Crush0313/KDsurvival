using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    [SerializeField]
    GameObject CrosshairHUD;
    float gunAccuracy;
    [SerializeField]
    GunController theGunController;

    public void walkAni(bool _flag)
    {
        WeaponManager.currentWeaponAnim.SetBool("Walk", _flag); //���� �ִ�
        anim.SetBool("Walk", _flag); //���ڼ� �ִ�
    }
    public void runAni(bool _flag)
    {
        anim.SetBool("Run", _flag);
        WeaponManager.currentWeaponAnim.SetBool("Run", _flag);
    }

    public void jumpAni(bool _flag)
    {
        anim.SetBool("Run", _flag); //���ڼ��� �޸��� �ִ϶� ������, ���� �޸��� ����� �̻��
    }
    public void crouchAni(bool _flag)
    {
        anim.SetBool("Crouch", _flag);
    }
    public void SightAni(bool _flag)
    {
        anim.SetBool("FineSight", _flag);
    }
    public void FireAni()
    {
        anim.SetTrigger("Fire");
    }

    public float getAccuracy()
    {
        if (anim.GetBool("Walk"))
            gunAccuracy = 0.06f;
        else if (anim.GetBool("Crouch"))
            gunAccuracy = 0.015f;
        else if (theGunController.GetFineSightMode())
            gunAccuracy = 0.001f;
        else
            gunAccuracy = 0.035f;

        return
            gunAccuracy;
    }
}
