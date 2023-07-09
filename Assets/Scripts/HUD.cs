using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    [SerializeField]
    GunController theGunController;
    Gun currentGun; //ÇöÀç ÃÑ

    [SerializeField]
    GameObject go_BulletHUD;

    [SerializeField]
    Text[] text_Bullet;

    void chkBullet()
    {
        currentGun = theGunController.GetGun();
        text_Bullet[0].text=currentGun.carryBulletCount.ToString();
        text_Bullet[1].text=currentGun.reloadBulletCount.ToString();
        text_Bullet[2].text = currentGun.currentBulletCount.ToString();
    }
    void Update()
    {
        chkBullet();
    }
}
