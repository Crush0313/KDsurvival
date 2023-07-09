using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accuracy; //정확도
    public float fireRate; //연사속도
    public float reloadTime; //재장전 속도
    public int damage;

    public int reloadBulletCount; //총알 재장전 개수
    public int currentBulletCount;
    public int maxBulletCount; 
    public int carryBulletCount; //소지 총알

    public float retroActionForce; //반동세기
    public float retroActionFineSightForce;//정조준시 반동 세기

    public Vector3 fireSightOriginPos;
    public Animator anim;
    public ParticleSystem muzzleFlash;
    public AudioClip fireSound;

}
