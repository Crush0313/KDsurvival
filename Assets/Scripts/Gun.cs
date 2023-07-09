using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accuracy; //��Ȯ��
    public float fireRate; //����ӵ�
    public float reloadTime; //������ �ӵ�
    public int damage;

    public int reloadBulletCount; //�Ѿ� ������ ����
    public int currentBulletCount;
    public int maxBulletCount; 
    public int carryBulletCount; //���� �Ѿ�

    public float retroActionForce; //�ݵ�����
    public float retroActionFineSightForce;//�����ؽ� �ݵ� ����

    public Vector3 fireSightOriginPos;
    public Animator anim;
    public ParticleSystem muzzleFlash;
    public AudioClip fireSound;

}
