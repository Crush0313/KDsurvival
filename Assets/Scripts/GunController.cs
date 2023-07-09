using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    Gun currentGun;
    float currentFireRate;
    AudioSource audioSource;

    bool isReload = false;
    bool isFineSightMode = false;

    [SerializeField]
    Vector3 originPos;

    // Start is called before the first frame update
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
        //originPos = transform.localPosition;
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
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
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fireSightOriginPos, 0.3f);//보간
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
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.3f);//보간
            if (count > 30)
                break;
            yield return null;
        }
        currentGun.transform.localPosition = originPos;
    }

    void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }
    void Fire()
    {
        if (currentGun.currentBulletCount > 0)
        {
            Shoot();

        }
        else
        {
            StartCoroutine(Reload());

        }
    }
    void Shoot()
    {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; //연사속도 재계산
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fireSightOriginPos.y, currentGun.fireSightOriginPos.z);

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;
            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)//반동시작
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            while (currentGun.transform.localPosition != originPos)//반동시작
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {

            currentGun.transform.localPosition = currentGun.fireSightOriginPos;
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)//반동시작
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroRecoilBack, 0.4f);
                yield return null;
            }

            while (currentGun.transform.localPosition != currentGun.fireSightOriginPos)//반동시작
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

            currentGun.carryBulletCount += currentGun.currentBulletCount; //잔탄 상태에서 재장전 시 반환
            currentGun.currentBulletCount = 0; //장전수보다 작을 경우에 합연산으로 적용되기에 0으로 초기화

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

    void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
