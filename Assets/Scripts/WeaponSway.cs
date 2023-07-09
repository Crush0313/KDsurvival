using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    Vector3 originPos;
    Vector3 currenPos;

    [SerializeField]
    Vector3 LimitPos;
    [SerializeField]
    Vector3 fineSightLimitPos;
    [SerializeField]
    Vector3 smoothSway;

    [SerializeField]
    GunController theGunController;

    // Start is called before the first frame update
    void Start()
    {
        originPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        TrySway();
    }
    void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
        {
            Swaing();
        }
        else
            BackToOriginPos();
    }
    void Swaing() //중력가속도를 견디며 따라오는 것 처럼, 마우스로 시선 움직이면 그 반대 방향으로 총 움직임
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if (!theGunController.isFineSightMode)
        {
            currenPos.Set(
                Mathf.Clamp(Mathf.Lerp(currenPos.x, -_moveX, smoothSway.x),-LimitPos.x, LimitPos.x),
                Mathf.Clamp(Mathf.Lerp(currenPos.y, -_moveY, smoothSway.y),-LimitPos.y, LimitPos.y),
                originPos.z
                );
        }
        else
        {
            currenPos.Set(
                Mathf.Clamp(Mathf.Lerp(currenPos.x, -_moveX, smoothSway.x), -fineSightLimitPos.x, fineSightLimitPos.x),
                Mathf.Clamp(Mathf.Lerp(currenPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y),
                originPos.z
                );
        }
        transform.localPosition = currenPos;

    }
    void BackToOriginPos()
    {
        currenPos = Vector3.Lerp(currenPos, originPos, smoothSway.x);
        transform.localPosition = currenPos;
    }
}
