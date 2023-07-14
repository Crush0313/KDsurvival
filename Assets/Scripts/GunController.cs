using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public static bool isActivate = true;//Ȱ��ȭ ����

    [SerializeField]
    Gun currentGun; //���� ��
    float currentFireRate; //����ӵ�

    //���º���
    bool isReload = false;
    [HideInInspector]
    public bool isFineSightMode = false;

    [SerializeField]
    Vector3 originPos;

    RaycastHit hitInfo;
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject hit_effect_prefab;
    CrossHair theCrossHair;
    AudioSource audioSource;


    public Gun GetGun()
    {
        return currentGun;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
        theCrossHair = FindAnyObjectByType<CrossHair>();
        originPos = Vector3.zero;

        WeaponManager.currentWeapon = currentGun.transform;
        WeaponManager.currentWeaponAnim = currentGun.anim;
    }

    void Update()
    {
        if (isActivate&&!Inventory.inventoryActivated)
            
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }

    void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }
    void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        theCrossHair.SightAni(isFineSightMode);
        currentGun.anim.SetBool("FineSight" ,isFineSightMode);
        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }
    public void CancelFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }
    IEnumerator FineSightActivateCoroutine()
    {
        int count = 0;
        while (currentGun.transform.localPosition!=currentGun.fireSightOriginPos)
        {
            count++;
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fireSightOriginPos, 0.3f);//����
            if (count > 30)
                break;
            yield return null;
        }
        currentGun.transform.localPosition = currentGun.fireSightOriginPos;
    }
    IEnumerator FineSightDeactivateCoroutine()
    {
        int count = 0;
        while (currentGun.transform.localPosition != originPos)
        {
            count++;
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.3f);//����
            if (count > 30)
                break;
            yield return null;
        }
        currentGun.transform.localPosition = originPos;
    }

    void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload )
        {
            Fire();
        }
    }
    //�߻� �� ���
    void Fire()
    {
        if (currentGun.currentBulletCount > 0)
        {
            Shoot();
            Hit();
        }
        else
        {
            StartCoroutine(Reload());

        }
    }
    //�߻� �� ���
    void Shoot()
    {
        theCrossHair.FireAni();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; //����ӵ� ����
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    void Hit()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward +
            new Vector3(Random.Range(-theCrossHair.getAccuracy() -currentGun.accuracy, theCrossHair.getAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrossHair.getAccuracy() -currentGun.accuracy, theCrossHair.getAccuracy() + currentGun.accuracy),
                        0)
            , out hitInfo, currentGun.range))
        {
            GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }

    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fireSightOriginPos.y, currentGun.fireSightOriginPos.z);

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)//�ݵ�����
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            while (currentGun.transform.localPosition != originPos)//�ݵ�����
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {

            currentGun.transform.localPosition = currentGun.fireSightOriginPos;
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)//�ݵ�����
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroRecoilBack, 0.4f);
                yield return null;
            }

            while (currentGun.transform.localPosition != currentGun.fireSightOriginPos)//�ݵ�����
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fireSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentGun.currentBulletCount < currentGun.reloadBulletCount && !isReload)
        {
            StartCoroutine(Reload());
        }

    }
    IEnumerator Reload()
    {
        CancelFineSight();
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");
            yield return new WaitForSeconds(currentGun.reloadTime);

            currentGun.carryBulletCount += currentGun.currentBulletCount; //��ź ���¿��� ������ �� ��ȯ
            currentGun.currentBulletCount = 0; //���������� ���� ��쿡 �տ������� ����Ǳ⿡ 0���� �ʱ�ȭ

            if (currentGun.carryBulletCount > currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }
            isReload = false;
        }
    }
    public void cancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }
    void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }
    public void GunChange(Gun _gun)
    {
        if (WeaponManager.currentWeapon != null)//���� ����־��� ��� ���� ���� ��Ȱ��ȭ
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.transform;
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
