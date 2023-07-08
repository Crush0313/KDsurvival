using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    Gun currentGun;
    float currentFieRate;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource=GetComponent<AudioSource>();
    }
    void GunFireRateCalc()
    {
        if (currentFieRate > 0)
            currentFieRate -= Time.deltaTime;
    }
    void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFieRate <= 0)
        {
            Fire();
        }
    }
    void Fire()
    {
        currentFieRate = currentGun.fireRate;
        Shoot();
    }
    void Shoot()
    {
        PlaySE(currentGun.fireSound);
        currentGun.muzzleFlash.Play();
    }
    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }
    void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
