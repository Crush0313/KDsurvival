using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon;//무기 중복 교체 실행 방지

    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    [SerializeField]
    GunController theGunController;
    [SerializeField]
    HandController theHandController;

    [SerializeField]
    float changeWeaponDelayTime;
    [SerializeField]
    float changeWeaponEndDelayTime;

    [SerializeField]
    Gun[] guns;
    [SerializeField]
    Hand[] hands;


    Dictionary<string,Gun> gunDict = new Dictionary<string, Gun>();
    Dictionary<string, Hand> handDict = new Dictionary<string, Hand>();
    [SerializeField]
    string currentWeaponType;

    
    // Start is called before the first frame update
    void Start()
    {
        //딕셔너리 자동생성
        for (int i = 0; i < guns.Length; i++)
        {

            gunDict.Add(guns[i].gunName, guns[i]);

        }
        for (int i = 0; i < hands.Length; i++)
        {

            handDict.Add(hands[i].handName, hands[i]);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            ;
        }
    }
    public IEnumerator  ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("WeaponOut");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        cancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);
        currentWeaponType = _type;
        isChangeWeapon = false;
    }
    void cancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.cancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
        }
    }
    void WeaponChange(string _type, string _name)
    {
        if (_type == "GUN")
        {
            theGunController.GunChange(gunDict[_name]);
        }
        else if (_type == "HAND")
        {
            theHandController.HandChange(handDict[_name]);

        }
    }
}
