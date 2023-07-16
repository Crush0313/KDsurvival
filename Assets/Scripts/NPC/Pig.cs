using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SearchService;

public class Pig : MonoBehaviour
{
    [SerializeField] string animalName;
    [SerializeField] int hp;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    bool isAction;
    bool isWalking;
    bool isRunning;
    bool isDead;

    [SerializeField] float walkTime;
    [SerializeField] float runTime;
    float applySpeed;
    [SerializeField] float waitTime;
    float currentTime;
    Vector3 dir;

    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rigid;
    [SerializeField] BoxCollider boxCol;
    AudioSource theAudio;
    [SerializeField] AudioClip[] SE_pig_Normal;
    [SerializeField] AudioClip SE_pig_Hurt;
    [SerializeField] AudioClip SE_pig_Dead;
    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
        //액션 : 대기
        currentTime = waitTime;
        isAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            ElaspeTime();
            Move();
            Rotation();
        }
    }
    private void Move()
    {
        if (isWalking||isRunning)
        {
            rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        }
        
        }

    void Rotation()
    {
        if (isWalking||isRunning)
        {
            Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0,dir.y,0), 0.01f);
            rigid.MoveRotation(Quaternion.Euler(_rotation));
        }
    }
    public void Run(Vector3 _targetPos)
    {
        dir = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        applySpeed = runSpeed;
        anim.SetBool("Running", isRunning);
    }
    void ElaspeTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
                _Reset();
        }
    }
    void RandomAction()
    {

        PlayRandomSE();
        int _rand = Random.Range(0, 4); //0,1,2,3
        switch (_rand)
        {
            case 0:
                Wait();
                break;
            case 1:
                Eat();
                break;
            case 2:
                Peek();
                break;
            case 3:
                TryWalk();
                break;
            default:
                break;
        }
    }
    private void _Reset()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;
        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        dir.Set(0, Random.Range(0f, 360f), 0);
        applySpeed = walkSpeed;
        RandomAction();
    }
        
    public void Damage(int _dmg, Vector3 _targetPos)
    {
        if (!isDead)
        {

            hp -= _dmg;
            if (hp <= 0)
                Dead();
            else
            {
                anim.SetTrigger("Hurt");
                PlaySE(SE_pig_Hurt);
                Run(_targetPos);
            }
        }
    }
    void Dead()
    {

        anim.SetTrigger("Dead");
        PlaySE(SE_pig_Dead);
        isWalking = false;
        isRunning = false;
        isDead = true;

    }
    void Wait()
    {
        currentTime = waitTime;
        
    }
    void Eat()
    {
        currentTime = waitTime;
        anim.SetTrigger("Eat");
    }
void Peek()
    {
        currentTime = waitTime;
        anim.SetTrigger("Peek");
    }
void TryWalk()
    {
        currentTime = walkTime;
        isWalking = true;
        applySpeed = walkSpeed;
        anim.SetBool("Walking", isWalking);
    }

    void PlayRandomSE()
    {
        int _random = Random.Range(0, 3); //일상 사운드 3개
        PlaySE(SE_pig_Normal[_random]);
    }
    void PlaySE(AudioClip _clip)
    {
        theAudio.clip = _clip;
        theAudio.Play();
    }
}
