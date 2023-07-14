
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    [SerializeField]
    int hp;
    int currentHp;

    [SerializeField]
    int sp;
    int currentSp;
    [SerializeField]
    int spIncreaseAmount;
        
    [SerializeField]
    int spRechargeTime;
    int currentSpRechargeTime;

    bool spUsed;


    [SerializeField]
    int dp;
    int currentDp;


    [SerializeField]
    int hungry;
    int currentHungry;

    [SerializeField]
    int hungryDecreaseTime;
    int currentHungryDecreaseTime;


    [SerializeField]
    int thirsty;
    int currentThirsty;

    [SerializeField]
    int thirstyDecreaseTime;
    int currentThirstyDecreaseTime;


    [SerializeField]
    int satisfy;
    int currentSatisfy;

    [SerializeField]
    Image[] image_Gauge;
    const int HP = 0
        , DP = 1
        , SP = 2
        , HUNGRY = 3
        , THIRSTY = 4
        , SATISFY = 5;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentSatisfy = satisfy;
        currentThirsty = thirsty;
    }

    // Update is called once per frame
    void Update()
    {
        Hungry();
        Thirsty();
        SPRechargeTime();
        RecoverSP();

        GaugeUpdate();
    }
    void Hungry()
    {
        if (currentHungry > 0) //������ 0 �̻��̸� -> ������ ����
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime) //��Ÿ�ӵɶ����� ++
                currentHungryDecreaseTime++;
            else //��Ÿ�� ����
            {
                currentHungryDecreaseTime = hungryDecreaseTime;
                currentHungry--;
            }
        }
    }
    void Thirsty()
    {
        if (currentSatisfy> 0) //������ 0 �̻��̸� -> ������ ����
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime) //��Ÿ�ӵɶ����� ++
                currentThirstyDecreaseTime++;
            else //��Ÿ�� ����
            {
                currentThirstyDecreaseTime = thirstyDecreaseTime;
                currentThirsty--;
            }
        }
    }
    void GaugeUpdate()
    {
        image_Gauge[HP].fillAmount = (float)currentHp / hp;
        image_Gauge[SP].fillAmount = (float)currentSp / sp;
        image_Gauge[DP].fillAmount = (float)currentDp / dp;
        image_Gauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        image_Gauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
        image_Gauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
    }
    public void DecreaseSP(int _count)
    {
        spUsed = true;

        currentSpRechargeTime = 0; //�� �ٽ� �ʱ�ȭ

        if (currentSp - _count > 0) //sp����
            currentSp -= _count;
        else
            currentSp = 0;
    }
    void SPRechargeTime() 
    {
        if (spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime) //�� ���
                currentSpRechargeTime++;
            else //�� ����
            {
                spUsed = false;  //ȸ�� ���� ����
            }

        }
    }
    void RecoverSP()
    {
        if (!spUsed && currentSp < sp) //sp ��� ���Ŀ��� ���ϰ�, ��ٷ��� �����ϵ��� ����� 
        {
            currentSp += spIncreaseAmount;
        }
    }
    public int GetCurrentSP()
    {
        return currentSp;
    }

    public void IncreaseSP(int _count)
    {

        if (currentSp + _count < sp)
            currentSp += _count;
        else
            currentSp = sp;

    }
    public void IncreaseHP(int _count)
    {

        if (currentHp + _count < hp) //sp����
            currentHp += _count;
        else
            currentHp = hp;

    }
    public void DecreaseHP(int _count)
    {
        if (currentDp > 0)
        {
            DecreaseDP(_count);
            return;
        }
        if (currentHp - _count > 0) //sp����
            currentHp -= _count;
        else
            currentHp = 0;

    }
    public void IncreaseDP(int _count)
    {

        if (currentDp + _count < dp) //sp����
            currentDp += _count;
        else
            currentDp = dp;

    }
    public void DecreaseDP(int _count)
    {

        if (currentDp - _count > 0) //sp����
            currentDp -= _count;
        else
            currentDp = 0;

    }
    public void IncreaseHungry(int _count)
    {

        if (currentHungry + _count < hungry) //sp����
            currentHungry += _count;
        else
            currentHungry = hungry;

    }
    public void DecreaseHungry(int _count)
    {
        if (currentHungry - _count > 0) //sp����
            currentHungry -= _count;
        else
            currentHungry = 0;

    }
    public void IncreaseThirsty(int _count)
    {

        if (currentThirsty + _count < thirsty) //sp����
            currentThirsty += _count;
        else
            currentThirsty = hungry;

    }
    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count > 0) //sp����
            currentThirsty -= _count;
        else
            currentThirsty = 0;

    }
}
